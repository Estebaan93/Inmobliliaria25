using Inmobiliaria25.Db;
using Inmobiliaria25.Models;
using MySql.Data.MySqlClient;

namespace Inmobiliaria25.Repositorios
{
  public class RepositorioLogin
  {
    private readonly DataContext _context;

    public RepositorioLogin(DataContext context)
    {
      _context = context;
    }

    public Usuario? Verificar(LoginViewModel model)
    {
      Usuario? usuario = null;

      using var conn = _context.GetConnection();
      conn.Open();

      string sql = "SELECT * FROM usuario WHERE email = @Email";
      using var cmd = new MySqlCommand(sql, conn);
      cmd.Parameters.AddWithValue("@Email", model.Email);

      using var reader = cmd.ExecuteReader();
      if (reader.Read())
      {
        string passwordHash = reader.GetString("password");

        if (BCrypt.Net.BCrypt.Verify(model.Password, passwordHash))
        {
          usuario = new Usuario
          {
            IdUsuario = reader.GetInt32("idUsuario"),
            Nombre = reader.GetString("nombre"),
            Apellido = reader.GetString("apellido"),
            Email = reader.GetString("email"),
            Rol = reader.GetString("rol"),
            Avatar = reader.IsDBNull(reader.GetOrdinal("avatar")) ? null : reader.GetString("avatar"),
            Estado = reader.GetBoolean("estado")
          };
        }
      }

      return usuario;
    }
  }
}