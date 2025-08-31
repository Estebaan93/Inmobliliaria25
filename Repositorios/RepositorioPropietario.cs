using Inmobiliaria25.Db;
using MySql.Data.MySqlClient;
using Inmobiliaria25.Models;

namespace Inmobiliaria25.Repositorios
{
  public class RepositorioPropietario
  {
    private readonly DataContext _context;

    public RepositorioPropietario(DataContext context)
    {
      _context = context;
    }

    // SOLO ACTIVOS 
    public List<Propietarios> ObtenerActivos()
    {
      var lista = new List<Propietarios>();
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = "SELECT idPropietario, apellido, nombre, dni, telefono, correo, estado FROM Propietario WHERE estado = 1";
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

    //  SOLO DADOS DE BAJA 
    public List<Propietarios> ObtenerDadosDeBaja()
    {
      var lista = new List<Propietarios>();
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = "SELECT idPropietario, apellido, nombre, dni, telefono, correo, estado FROM Propietario WHERE estado = 0";
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

    //  BUSCAR POR ID 
    public Propietarios? ObtenerPorId(int id)
    {
      Propietarios? p = null;
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = @"SELECT idPropietario, dni, apellido, nombre, telefono, correo, estado
                               FROM Propietario WHERE idPropietario=@id";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          using (var reader = cmd.ExecuteReader())
          {
            if (reader.Read())
            {
              p = new Propietarios
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
      return p;
    }

    //  VALIDAR DNI 
    public bool ExisteDni(string dni, int? excluirId = null)
    {
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = "SELECT COUNT(*) FROM Propietario WHERE dni=@dni";
        if (excluirId.HasValue)
        {
          sql += " AND idPropietario != @id";
        }

        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@dni", dni);
          if (excluirId.HasValue)
            cmd.Parameters.AddWithValue("@id", excluirId.Value);

          var count = Convert.ToInt32(cmd.ExecuteScalar());
          return count > 0;
        }
      }
    }

    //  ALTA 
    public int Alta(Propietarios p)
    {
      int res = -1;
      if (ExisteDni(p.dni))
        throw new Exception("El DNI ya está registrado. Revise la tabla de propietarios.");

      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sqlInsert = @"INSERT INTO Propietario (apellido, nombre, dni, telefono, correo, estado)
                                     VALUES (@apellido, @nombre, @dni, @telefono, @correo, 1)";
        using (var cmd = new MySqlCommand(sqlInsert, conn))
        {
          cmd.Parameters.AddWithValue("@apellido", p.apellido);
          cmd.Parameters.AddWithValue("@nombre", p.nombre);
          cmd.Parameters.AddWithValue("@dni", p.dni);
          cmd.Parameters.AddWithValue("@telefono", p.telefono);
          cmd.Parameters.AddWithValue("@correo", p.correo);
          res = cmd.ExecuteNonQuery();
        }
      }
      return res;
    }

    //  MODIFICAR (NO toca estado) 
    public int Modificar(Propietarios p)
    {
      int res = -1;
      if (ExisteDni(p.dni, p.idPropietario))
        throw new Exception("El DNI ya está registrado en otro propietario.");

      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = @"UPDATE Propietario SET
                               apellido=@apellido, nombre=@nombre, dni=@dni,
                               telefono=@telefono, correo=@correo
                               WHERE idPropietario=@id";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@apellido", p.apellido);
          cmd.Parameters.AddWithValue("@nombre", p.nombre);
          cmd.Parameters.AddWithValue("@dni", p.dni);
          cmd.Parameters.AddWithValue("@telefono", p.telefono);
          cmd.Parameters.AddWithValue("@correo", p.correo);
          cmd.Parameters.AddWithValue("@id", p.idPropietario);
          res = cmd.ExecuteNonQuery();
        }
      }
      return res;
    }

    // BAJA  
    public int Baja(int id)
    {
      int res = -1;
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = "UPDATE Propietario SET estado = 0 WHERE idPropietario = @id";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          res = cmd.ExecuteNonQuery();
        }
      }
      return res;
    }
    // buscar x dni , en todos los buscador me trae datos poniendo una letra, numero, pero los de estado 1
    public List<Propietarios> BuscarPorDni(string dni)
    {
      var lista = new List<Propietarios>();
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = @"SELECT idPropietario, apellido, nombre, dni, telefono, correo, estado
                       FROM Propietario
                       WHERE dni LIKE @dni AND estado = 1";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@dni", dni + "%");
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
      }
      return lista;
    }

    // buscar x apellido
    public List<Propietarios> BuscarPorApellido(string apellido)
    {
      var lista = new List<Propietarios>();
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = @"SELECT idPropietario, apellido, nombre, dni, telefono, correo, estado
                       FROM propietario
                       WHERE apellido LIKE @apellido AND estado = 1";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@apellido", apellido + "%");
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
      }
      return lista;
    }

    // buscar x mail
    public List<Propietarios> BuscarPorEmail(string email)
    {
      var lista = new List<Propietarios>();
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = @"SELECT idPropietario, apellido, nombre, dni, telefono, correo, estado
                       FROM Propietario
                       WHERE correo LIKE @correo AND estado = 1";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@correo", email + "%");
          using (var reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              lista.Add(new Propietarios
              {
                idPropietario = reader.GetInt32("idInquilino"),
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
      }
      return lista;
    }
    // Reactivar propietario existente (UPDATE)
    public int Reactivar(Propietarios p)
    {
      int res = -1;
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = "UPDATE Propietario SET estado = 1 WHERE idPropietario=@id";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@id", p.idPropietario);
          res = cmd.ExecuteNonQuery();
        }
      }
      return res;
    }
  }
}
