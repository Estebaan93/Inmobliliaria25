using Inmobiliaria25.Db;
using MySql.Data.MySqlClient;
using Inmobiliaria25.Models;

namespace Inmobiliaria25.Repositorios
{
  public class RepositorioInquilino
  {
    //declaro var privada es como el puente para conectar la BD en todos los metodos
    private readonly DataContext _context;

    public RepositorioInquilino(DataContext context)
    {
      _context = context;
    }

      //Para la paginacion
    public int ContarActivos()
{
    using var conn = _context.GetConnection();
    conn.Open();
    const string sql = "SELECT COUNT(*) FROM Inquilino WHERE estado = 1";
    using var cmd = new MySqlCommand(sql, conn);
    return Convert.ToInt32(cmd.ExecuteScalar());
}
public List<Inquilinos> ObtenerActivosPaginado(int page, int pageSize)
{
    var lista = new List<Inquilinos>();
    int offset = (page - 1) * pageSize;

    using var conn = _context.GetConnection();
    conn.Open();
    const string sql = @"
        SELECT idInquilino, apellido, nombre, dni, telefono, correo, estado
        FROM Inquilino
        WHERE estado = 1
        ORDER BY apellido, nombre
        LIMIT @limit OFFSET @offset;";

    using var cmd = new MySqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@limit", pageSize);
    cmd.Parameters.AddWithValue("@offset", offset);

    using var reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        lista.Add(new Inquilinos
        {
            IdInquilino = reader.GetInt32("idInquilino"),
            Apellido = reader.GetString("apellido"),
            Nombre = reader.GetString("nombre"),
            Dni = reader.GetString("dni"),
            Telefono = reader.IsDBNull(reader.GetOrdinal("telefono")) ? null : reader.GetString("telefono"),
            Correo = reader.GetString("correo"),
            Estado = reader.GetBoolean("estado")
        });
    }
    return lista;
}

    // SOLO ACTIVOS (estado = 1)
    //metodo q devuelve una lista de obj, me trae todos los inqu con estado 1
    public List<Inquilinos> ObtenerActivos()
    {
      // var vacia q va a guardar los datos
      var lista = new List<Inquilinos>();
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = "SELECT idInquilino, apellido, nombre, dni, telefono, correo, estado FROM Inquilino WHERE estado = 1";
        using (var cmd = new MySqlCommand(sql, conn)) //crea comando para ejecutar la consulta
        using (var reader = cmd.ExecuteReader())
        {
          while (reader.Read())
          {
            lista.Add(new Inquilinos //recorre las filas devueltas, crea un obj con sus propiedades
            {
              IdInquilino = reader.GetInt32("idInquilino"),
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

    // SOLO DADOS DE BAJA (estado = 0)
    public List<Inquilinos> ObtenerDadosDeBaja()
    {
      var lista = new List<Inquilinos>();
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = "SELECT idInquilino, apellido, nombre, dni, telefono, correo, estado FROM Inquilino WHERE estado = 0";
        using (var cmd = new MySqlCommand(sql, conn))
        using (var reader = cmd.ExecuteReader())
        {
          while (reader.Read())
          {
            lista.Add(new Inquilinos
            {
              IdInquilino = reader.GetInt32("idInquilino"),
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

    // Buscar por Id
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
                IdInquilino = reader.GetInt32("idInquilino"),
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
      return i;
    }

    // Validar que no se repita el DNI (excluyendo un id si estamos editando)
    public bool ObtenerPorDni(string dni, int? excluirId = null)
    {
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = "SELECT COUNT(*) FROM Inquilino WHERE dni=@dni";
        if (excluirId.HasValue)
        {
          sql += " AND idInquilino != @id";
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

    // Alta
    public int Alta(Inquilinos i)
    {
      int res = -1;
      if (ObtenerPorDni(i.Dni))
        throw new Exception("El DNI ya está registrado. Revise la tabla de inquilinos.");

      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sqlInsert = @"INSERT INTO Inquilino (apellido, nombre, dni, telefono, correo, estado)
                             VALUES (@apellido, @nombre, @dni, @telefono, @correo, 1)";
        using (var cmd = new MySqlCommand(sqlInsert, conn))
        {
          cmd.Parameters.AddWithValue("@apellido", i.Apellido);
          cmd.Parameters.AddWithValue("@nombre", i.Nombre);
          cmd.Parameters.AddWithValue("@dni", i.Dni);
          cmd.Parameters.AddWithValue("@telefono", i.Telefono);
          cmd.Parameters.AddWithValue("@correo", i.Correo);
          res = cmd.ExecuteNonQuery();
        }
      }

      return res;
    }

    // Modificar (NO toca estado)
    public int Modificar(Inquilinos i)
    {
      int res = -1;
      if (ObtenerPorDni(i.Dni, i.IdInquilino))
        throw new Exception("El DNI ya está registrado en otro inquilino.");

      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = @"UPDATE Inquilino SET
                               apellido=@apellido, nombre=@nombre, dni=@dni,
                               telefono=@telefono, correo=@correo
                               WHERE idInquilino=@id";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@apellido", i.Apellido);
          cmd.Parameters.AddWithValue("@nombre", i.Nombre);
          cmd.Parameters.AddWithValue("@dni", i.Dni);
          cmd.Parameters.AddWithValue("@telefono", i.Telefono);
          cmd.Parameters.AddWithValue("@correo", i.Correo);
          cmd.Parameters.AddWithValue("@id", i.IdInquilino);
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
        string sql = "UPDATE Inquilino SET estado = 0 WHERE idInquilino = @id";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          res = cmd.ExecuteNonQuery();
        }
      }
      return res;
    }
    // buscar x dni , en todos los buscador me trae datos poniendo una letra, numero, pero los de estado 1
    public List<Inquilinos> BuscarPorDni(string dni)
    {
      var lista = new List<Inquilinos>();
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = @"SELECT idInquilino, apellido, nombre, dni, telefono, correo, estado
                       FROM Inquilino
                       WHERE dni LIKE @dni AND estado = 1";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@dni", dni + "%");
          using (var reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              lista.Add(new Inquilinos
              {
                IdInquilino = reader.GetInt32("idInquilino"),
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
    public List<Inquilinos> BuscarPorApellido(string apellido)
    {
      var lista = new List<Inquilinos>();
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = @"SELECT idInquilino, apellido, nombre, dni, telefono, correo, estado
                       FROM Inquilino
                       WHERE apellido LIKE @apellido AND estado = 1";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@apellido", apellido + "%");
          using (var reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              lista.Add(new Inquilinos
              {
                IdInquilino = reader.GetInt32("idInquilino"),
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
    public List<Inquilinos> BuscarPorEmail(string email)
    {
      var lista = new List<Inquilinos>();
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = @"SELECT idInquilino, apellido, nombre, dni, telefono, correo, estado
                       FROM Inquilino
                       WHERE correo LIKE @correo AND estado = 1";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@correo", email + "%");
          using (var reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              lista.Add(new Inquilinos
              {
                IdInquilino = reader.GetInt32("idInquilino"),
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
    public int Reactivar(Inquilinos i)
    {
      int res = -1;
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = "UPDATE Inquilino SET estado = 1 WHERE idInquilino=@id";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@id", i.IdInquilino);
          res = cmd.ExecuteNonQuery();
        }
      }
      return res;
    }


  }
}
