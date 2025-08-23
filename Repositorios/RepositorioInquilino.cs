//Repositorios/RepositorioInquilino.cs
using Inmobiliaria25.Db; //Hace uso de la clase dataContext
using MySql.Data.MySqlClient; //Hace uso de mysql
using inmobiliaria25.Models;  //

namespace Inmobiliaria25.Repositorios
{
  public class RepositorioInquilino
  {
    //Declaro variable privada, es el puente para conectar la bd con los metodos
    private readonly DataContext _context;

    public RepositorioInquilino(DataContext context)
    {
      _context = context;
    }

    // SOLO ACTIVOS (estado = 1)
    //Metodo que devuelve una lista de objetos, me trae todos los inquilinos con estado 1
    public List<Inquilinos> ObtenerActivos()
    {
      //Var vacia que va guardar los datos
      var lista = new List<Inquilinos>();
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = "SELECT idInquilino, apellido, nombre, dni, telefono, correo, estado FROM Inquilino WHERE estado = 1";
        using (var cmd = new MySqlCommand(sql, conn)) //Crea comando para ejecutar la consulta
        using (var reader = cmd.ExecuteReader())
        {
          while (reader.Read())
          {
            lista.Add(new Inquilinos //Recorre las filas, crea un objeto con sus propiedades
            {
              idInquilino = reader.GetInt32("idInquilino"),
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
              idInquilino = reader.GetInt32("idInquilino"),
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

    // Buscar por Id
    public Inquilinos? ObtenerPorId(int id)
    {
      Inquilinos? i = null; //Tengo un inquiino con id null?
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

    // Validar que no se repita el DNI (excluyendo un id si estamos editando)
    public bool ObtenerPorDni(string dni, int? excluirId = null) //parámetro opcional y nullable. Si tiene valor, se usa en el WHERE para excluir un registro específico.
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
      if (ObtenerPorDni(i.dni))
        throw new Exception("El DNI ya está registrado. Revise la tabla de inquilinos.");

      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sqlInsert = @"INSERT INTO Inquilino (apellido, nombre, dni, telefono, correo, estado)
                             VALUES (@apellido, @nombre, @dni, @telefono, @correo, 1)";
        using (var cmd = new MySqlCommand(sqlInsert, conn))
        {
          cmd.Parameters.AddWithValue("@apellido", i.apellido);
          cmd.Parameters.AddWithValue("@nombre", i.nombre);
          cmd.Parameters.AddWithValue("@dni", i.dni);
          cmd.Parameters.AddWithValue("@telefono", i.telefono);
          cmd.Parameters.AddWithValue("@correo", i.correo);
          res = cmd.ExecuteNonQuery();
        }
      }

      return res;
    }

    // Modificar (NO toca estado)
    public int Modificar(Inquilinos i)
    {
      int res = -1;
      if (ObtenerPorDni(i.dni, i.idInquilino))
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
          cmd.Parameters.AddWithValue("@apellido", i.apellido);
          cmd.Parameters.AddWithValue("@nombre", i.nombre);
          cmd.Parameters.AddWithValue("@dni", i.dni);
          cmd.Parameters.AddWithValue("@telefono", i.telefono);
          cmd.Parameters.AddWithValue("@correo", i.correo);
          cmd.Parameters.AddWithValue("@id", i.idInquilino);
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
  }
}
