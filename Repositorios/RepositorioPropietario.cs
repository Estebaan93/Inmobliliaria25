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

        //Para la paginacion
    public int ContarActivos()
    {
      using var conn = _context.GetConnection();
      conn.Open();
      const string sql = "SELECT COUNT(*) FROM Propietario WHERE estado=1";
      using var cmd = new MySqlCommand(sql, conn);
      return Convert.ToInt32(cmd.ExecuteScalar());
    }
    public List<Propietarios> ObtenerActivosPaginado(int page, int pageSize)
    {
      var lista = new List<Propietarios>();
      int offset = (page - 1) * pageSize;

      using var conn = _context.GetConnection();
      conn.Open();
      const string sql = @"
        SELECT idPropietario, dni, apellido, nombre, telefono, correo, estado
        FROM Propietario
        WHERE estado = 1
        ORDER BY apellido, nombre
        LIMIT @limit OFFSET @offset;";

      using var cmd = new MySqlCommand(sql, conn);
      cmd.Parameters.AddWithValue("@limit", pageSize);
      cmd.Parameters.AddWithValue("@offset", offset);

      using var reader = cmd.ExecuteReader();
          while (reader.Read())
    {
        lista.Add(new Propietarios
        {
              IdPropietario = reader.GetInt32("idPropietario"),
              Apellido = reader.GetString("apellido"),
              Nombre = reader.GetString("nombre"),
              Dni = reader.GetString("dni"),
              Telefono = reader.GetString("telefono"),
              Correo = reader.GetString("correo"),
              Estado = reader.GetBoolean("estado")
        });
    }
    return lista;

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
              IdPropietario = reader.GetInt32("idPropietario"),
              Apellido = reader.GetString("apellido"),
              Nombre = reader.GetString("nombre"),
              Dni = reader.GetString("dni"),
              Telefono = reader.GetString("telefono"),
              Correo = reader.GetString("correo"),
              Estado = reader.GetBoolean("estado")
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
              IdPropietario = reader.GetInt32("idPropietario"),
              Apellido = reader.GetString("apellido"),
              Nombre = reader.GetString("nombre"),
              Dni = reader.GetString("dni"),
              Telefono = reader.GetString("telefono"),
              Correo = reader.GetString("correo"),
              Estado = reader.GetBoolean("estado")
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
                IdPropietario = reader.GetInt32("idPropietario"),
                Dni = reader.GetString("dni"),
                Apellido = reader.GetString("apellido"),
                Nombre = reader.GetString("nombre"),
                Telefono = reader.GetString("telefono"),
                Correo = reader.GetString("correo"),
                Estado = reader.GetBoolean("estado")
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
      if (ExisteDni(p.Dni))
        throw new Exception("El DNI ya está registrado. Revise la tabla de propietarios.");

      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sqlInsert = @"INSERT INTO Propietario (apellido, nombre, dni, telefono, correo, estado)
                                     VALUES (@apellido, @nombre, @dni, @telefono, @correo, 1)";
        using (var cmd = new MySqlCommand(sqlInsert, conn))
        {
          cmd.Parameters.AddWithValue("@apellido", p.Apellido);
          cmd.Parameters.AddWithValue("@nombre", p.Nombre);
          cmd.Parameters.AddWithValue("@dni", p.Dni);
          cmd.Parameters.AddWithValue("@telefono", p.Telefono);
          cmd.Parameters.AddWithValue("@correo", p.Correo);
          res = cmd.ExecuteNonQuery();
        }
      }
      return res;
    }

    //  MODIFICAR (NO toca estado) 
    public int Modificar(Propietarios p)
    {
      int res = -1;
      if (ExisteDni(p.Dni, p.IdPropietario))
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
          cmd.Parameters.AddWithValue("@apellido", p.Apellido);
          cmd.Parameters.AddWithValue("@nombre", p.Nombre);
          cmd.Parameters.AddWithValue("@dni", p.Dni);
          cmd.Parameters.AddWithValue("@telefono", p.Telefono);
          cmd.Parameters.AddWithValue("@correo", p.Correo);
          cmd.Parameters.AddWithValue("@id", p.IdPropietario);
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
                IdPropietario = reader.GetInt32("idPropietario"),
                Apellido = reader.GetString("apellido"),
                Nombre = reader.GetString("nombre"),
                Dni = reader.GetString("dni"),
                Telefono = reader.GetString("telefono"),
                Correo = reader.GetString("correo"),
                Estado = reader.GetBoolean("estado")
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
                IdPropietario = reader.GetInt32("idPropietario"),
                Apellido = reader.GetString("apellido"),
                Nombre = reader.GetString("nombre"),
                Dni = reader.GetString("dni"),
                Telefono = reader.GetString("telefono"),
                Correo = reader.GetString("correo"),
                Estado = reader.GetBoolean("estado")
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
                IdPropietario = reader.GetInt32("idInquilino"),
                Apellido = reader.GetString("apellido"),
                Nombre = reader.GetString("nombre"),
                Dni = reader.GetString("dni"),
                Telefono = reader.GetString("telefono"),
                Correo = reader.GetString("correo"),
                Estado = reader.GetBoolean("estado")
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
          cmd.Parameters.AddWithValue("@id", p.IdPropietario);
          res = cmd.ExecuteNonQuery();
        }
      }
      return res;
    }
  }
}
