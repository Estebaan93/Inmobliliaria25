using Inmobiliaria25.Repositorios;
using Inmobiliaria25.Db;
using MySql.Data.MySqlClient;
using inmobiliaria25.Models;

namespace Inmobiliaria25.Repositorios
{
    public class RepositorioPropietario
    {
        private readonly DataContext _context;

        public RepositorioPropietario(DataContext context)
        {
            _context = context;
        }

        // Listar
        public List<Propietarios> ObtenerTodos()
        {
            var lista = new List<Propietarios>();
            using (var conn = _context.GetConnection())
            {
                conn.Open();
                string sql = "SELECT idPropietario, apellido, nombre, dni, telefono, correo, estado FROM Propietario";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Propietarios
                        {
                            idPropietario = reader.GetInt32("idPropietario"),
                            apellido = reader.GetString("apellido"),
                            nombre = reader.GetString("nombre"),
                            dni = reader.GetString("dni"),
                           telefono = reader.GetString("telefono"),
                            correo = reader.GetString("correo"),
                            estado = reader.GetBoolean("estado")
                        });
                    }
                }
            }
            return lista;
        }
    public Propietarios? ObtenerPorId(int id)
    {
      Propietarios? i = null;
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = @"SELECT idPropietario, dni, apellido, nombre, telefono, correo, estado
                               FROM Inquilino WHERE idInquilino=@id";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          using (var reader = cmd.ExecuteReader())
          {
            if (reader.Read())
            {
              i = new Propietarios
              {
                idPropietario = reader.GetInt32("idPropietario"),
                dni = reader.GetString("dni"),
                apellido = reader.GetString("apellido"),
                nombre = reader.GetString("nombre"),
                telefono = reader.GetString("telefono"),
                correo = reader.GetString("correo"),
                estado = reader.GetBoolean("estado")
              };
            }
          }
        }
      }
      return i;
    }

    // Alta
    public int Alta(Propietarios p)
    {
      int res = -1;
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = @"INSERT INTO Propietario (apellido, nombre, dni, telefono, correo,estado)
                               VALUES (@apellido, @nombre, @dni, @telefono, @correo, @estado)";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@apellido", p.apellido);
          cmd.Parameters.AddWithValue("@nombre", p.nombre);
          cmd.Parameters.AddWithValue("@dni", p.dni);
          cmd.Parameters.AddWithValue("@telefono", p.telefono);
          cmd.Parameters.AddWithValue("@correo", p.correo);
          cmd.Parameters.AddWithValue("@estado", p.estado);
          res = cmd.ExecuteNonQuery();
        }
      }
      return res;
    }

        // Modificar
        public int Modificar(Propietarios p)
        {
            int res = -1;
            using (var conn = _context.GetConnection())
            {
                conn.Open();
                string sql = @"UPDATE Propietario SET
                               apellido=@apellido, nombre=@nombre, dni=@dni,
                               telefono=@telefono, correo=@correo, estado=@estado
                               WHERE idPropietario=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@apellido", p.apellido);
                    cmd.Parameters.AddWithValue("@nombre", p.nombre);
                    cmd.Parameters.AddWithValue("@dni", p.dni);
                    cmd.Parameters.AddWithValue("@telefono", p.telefono);
                    cmd.Parameters.AddWithValue("@correo", p.correo);
                    cmd.Parameters.AddWithValue("@estado", p.estado);
                    cmd.Parameters.AddWithValue("@id", p.idPropietario);
                    res = cmd.ExecuteNonQuery();
                }
            }
            return res;
        }

        // Baja
        public int Baja(int id)
        {
            int res = -1;
            using (var conn = _context.GetConnection())
            {
                conn.Open();
                string sql = "DELETE FROM Propietario WHERE idPropietario=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    res = cmd.ExecuteNonQuery();
                }
            }
            return res;
        }
    }
}