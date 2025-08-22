using Inmobiliaria25.Repositorios;
using inmobiliaria25.Models;
using Inmobiliaria25.Db;
using MySql.Data.MySqlClient;

namespace Inmobiliaria25.Repositorios
{
  public class RepositorioInquilino
  {
    private readonly DataContext _context;

    public RepositorioInquilino(DataContext context)
    {
      _context = context;
    }

    // listo
    public List<Inquilinos> ObtenerTodos()
    {
      var lista = new List<Inquilinos>();
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = "SELECT idInquilino, dni, apellido, nombre, telefono, correo, estado FROM Inquilino";
        using (var cmd = new MySqlCommand(sql, conn))
        using (var reader = cmd.ExecuteReader())
        {
          while (reader.Read())
          {
            lista.Add(new Inquilinos
            {
              idInquilino = reader.GetInt32("idInquilino"),
              dni = reader.GetString("dni"),
              apellido = reader.GetString("apellido"),
              nombre = reader.GetString("nombre"),
              telefono = reader.GetString("telefono"),
              correo = reader.GetString("correo"),
              estado = reader.GetBoolean("estado")
            });
        }
      }
    }
            return lista;
        }

        // busco id
        public Inquilinos? ObtenerPorId(int id)
    {
      Inquilinos? i = null;
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = @"SELECT idInquilino, dni, apellido, nombre, telefono, correo, estado
                               FROM Inquilino WHERE idInquilino=@id";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          using (var reader = cmd.ExecuteReader())
          {
            if (reader.Read())
            {
              i = new Inquilinos
              {
                idInquilino = reader.GetInt32("idInquilino"),
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

    // creo
    public int Alta(Inquilinos i)
    {
      int res = -1;
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = @"INSERT INTO Inquilino (dni, apellido, nombre, telefono, correo, estado)
                               VALUES (@dni, @apellido, @nombre, @telefono, @correo, @estado)";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@dni", i.dni);
          cmd.Parameters.AddWithValue("@apellido", i.apellido);
          cmd.Parameters.AddWithValue("@nombre", i.nombre);
          cmd.Parameters.AddWithValue("@telefono", i.telefono);
          cmd.Parameters.AddWithValue("@correo", i.correo);
          cmd.Parameters.AddWithValue("@estado", i.estado);
          res = cmd.ExecuteNonQuery();
        }
      }
      return res;
    }

    // modifico
    public int Modificar(Inquilinos i)
    {
      int res = -1;
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = @"UPDATE Inquilino SET
                               dni=@dni, apellido=@apellido, nombre=@nombre,
                               telefono=@telefono,cCorreo=@correo, estado=@estado
                               WHERE idInquilino=@id";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@dni", i.dni);
          cmd.Parameters.AddWithValue("@apellido", i.apellido);
          cmd.Parameters.AddWithValue("@nombre", i.nombre);
          cmd.Parameters.AddWithValue("@telefono", i.telefono);
          cmd.Parameters.AddWithValue("@correo", i.correo);
          cmd.Parameters.AddWithValue("@estado", i.estado);
          cmd.Parameters.AddWithValue("@id", i.idInquilino);
          res = cmd.ExecuteNonQuery();
        }
      }
      return res;
    }

    // elimino
    public int Baja(int id)
    {
      int res = -1;
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = "DELETE FROM Inquilino WHERE idInquilino=@id";
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