//Repositorios/RepositorioUsuario.cs
using Inmobiliaria25.Db;
using MySql.Data.MySqlClient;
using Inmobiliaria25.Models;

namespace Inmobiliaria25.Repositorios
{
  public class RepositorioUsuario
  {
    private readonly DataContext _context;

    public RepositorioUsuario(DataContext context)
    {
      _context = context;
    }


    public int Guardar(Usuario usuario)
    {
      int idCreado = -1;

      using var conn = _context.GetConnection();
      conn.Open();

      //Definimos un avatar por defecto
      string avatar = string.IsNullOrEmpty(usuario.Avatar)
        ? "/img/avatars/default.jpg"
        : usuario.Avatar;

      string sql = @"
                INSERT INTO usuario (email, password, rol, avatar, nombre, apellido, estado)
                VALUES (@EmailUsuario, @PasswordUsuario, @RolUsuario, @AvatarUsuario, 
                        @NombreUsuario, @ApellidoUsuario, @EstadoUsuario);
                SELECT LAST_INSERT_ID();";

      using var cmd = new MySqlCommand(sql, conn);
      cmd.Parameters.AddWithValue("@EmailUsuario", usuario.Email);
      cmd.Parameters.AddWithValue("@PasswordUsuario", Encriptar(usuario.Password));
      cmd.Parameters.AddWithValue("@RolUsuario", usuario.Rol);
      cmd.Parameters.AddWithValue("@AvatarUsuario", avatar);
      cmd.Parameters.AddWithValue("@NombreUsuario", usuario.Nombre);
      cmd.Parameters.AddWithValue("@ApellidoUsuario", usuario.Apellido);
      cmd.Parameters.AddWithValue("@EstadoUsuario", true);

      idCreado = Convert.ToInt32(cmd.ExecuteScalar());

      return idCreado;
    }

    // encriptar
    public string Encriptar(string pass)
    {
      int workFactor = 12;
      string salt = BCrypt.Net.BCrypt.GenerateSalt(workFactor);
      return BCrypt.Net.BCrypt.HashPassword(pass, salt);
    }

    // listar activos
    public List<Usuario> Listar()
    {
      var lista = new List<Usuario>();

      using var conn = _context.GetConnection();
      conn.Open();

      string sql = "SELECT * FROM usuario WHERE estado = true";
      using var cmd = new MySqlCommand(sql, conn);
      using var reader = cmd.ExecuteReader();

      while (reader.Read())
      {
        lista.Add(MapearUsuario(reader));
      }

      return lista;
    }

    // dados de baja
    public List<Usuario> ListarDadosDeBaja()
    {
      var lista = new List<Usuario>();

      using var conn = _context.GetConnection();
      conn.Open();

      string sql = "SELECT * FROM usuario WHERE estado = false";
      using var cmd = new MySqlCommand(sql, conn);
      using var reader = cmd.ExecuteReader();

      while (reader.Read())
      {
        lista.Add(MapearUsuario(reader));
      }

      return lista;
    }

    // obtener x id
    public Usuario? Obtener(int id)
    {
      Usuario? usuario = null;

      using var conn = _context.GetConnection();
      conn.Open();

      string sql = "SELECT * FROM usuario WHERE idUsuario=@id AND estado=true";
      using var cmd = new MySqlCommand(sql, conn);
      cmd.Parameters.AddWithValue("@id", id);

      using var reader = cmd.ExecuteReader();
      if (reader.Read())
      {
        usuario = MapearUsuario(reader);
      }

      return usuario;
    }

    // Actualizar un usuario (opcionalmente con nueva password)
    public int Actualizar(Usuario usuario, string? nuevaPassword = null)
    {
      using var conn = _context.GetConnection();
      conn.Open();

      //Definimos avatar por defecto si no se proporciona uno
      string avatar = string.IsNullOrEmpty(usuario.Avatar)
        ? "/img/avatars/default.jpg"
        : usuario.Avatar;

      string sql = @"UPDATE usuario 
                           SET email=@EmailUsuario, rol=@RolUsuario, avatar=@AvatarUsuario, 
                               nombre=@NombreUsuario, apellido=@ApellidoUsuario";

      if (!string.IsNullOrWhiteSpace(nuevaPassword))
      {
        sql += ", password=@PasswordUsuario";
      }

      sql += " WHERE idUsuario=@IdUsuario";

      using var cmd = new MySqlCommand(sql, conn);
      cmd.Parameters.AddWithValue("@EmailUsuario", usuario.Email);
      if (!string.IsNullOrWhiteSpace(nuevaPassword))
        cmd.Parameters.AddWithValue("@PasswordUsuario", Encriptar(nuevaPassword));

      cmd.Parameters.AddWithValue("@RolUsuario", usuario.Rol);
      cmd.Parameters.AddWithValue("@AvatarUsuario", avatar);
      cmd.Parameters.AddWithValue("@NombreUsuario", usuario.Nombre);
      cmd.Parameters.AddWithValue("@ApellidoUsuario", usuario.Apellido);
      cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);

      return cmd.ExecuteNonQuery();
    }

    // baja logica
    public int Eliminar(int id)
    {
      return CambiarEstado(id, false);
    }

    //  alta
    public int Alta(int id)
    {
      return CambiarEstado(id, true);
    }

    private int CambiarEstado(int id, bool estado)
    {
      using var conn = _context.GetConnection();
      conn.Open();

      string sql = "UPDATE usuario SET estado=@estado WHERE idUsuario=@id";
      using var cmd = new MySqlCommand(sql, conn);
      cmd.Parameters.AddWithValue("@estado", estado);
      cmd.Parameters.AddWithValue("@id", id);

      return cmd.ExecuteNonQuery();
    }

    //  Mapear DataReader a Usuario
    private Usuario MapearUsuario(MySqlDataReader reader)
    {
      return new Usuario
      {
        IdUsuario = reader.GetInt32("idUsuario"),
        Email = reader.GetString("email"),
        Password = reader.GetString("password"),
        Rol = reader.GetString("rol"),
        Avatar = reader.IsDBNull(reader.GetOrdinal("avatar")) ? null : reader.GetString("avatar"),
        Nombre = reader.GetString("nombre"),
        Apellido = reader.GetString("apellido"),
        Estado = reader.GetBoolean("estado")
      };
    }
  }
}
