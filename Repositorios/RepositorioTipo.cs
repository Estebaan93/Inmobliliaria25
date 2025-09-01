using Inmobiliaria25.Db;
using MySql.Data.MySqlClient;
using Inmobiliaria25.Models;


namespace Inmobiliaria25.Repositorios
{
  public class RepositorioTipo
  {
    private readonly DataContext _context;

    public RepositorioTipo(DataContext context)
    {
      _context = context;
    }

    // CREAR
    public int Crear(Tipo tipo)
    {
      int idCreado = -1;
      using (var conn = _context.GetConnection())// creo la conexion
      {
        conn.Open();// abro la conexion
        string sql = @"INSERT INTO Tipo (observacion) 
                               VALUES (@observacion); 
                               SELECT LAST_INSERT_ID();";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@observacion", tipo.Observacion);// asigno valores a los aprametro
          idCreado = Convert.ToInt32(cmd.ExecuteScalar());
        }
      }
      return idCreado;
    }

    // MODIFICAR
    public int Modificar(Tipo tipo)
    {
      int res = -1;
      using (var conn = _context.GetConnection())// creo la conexion
      {
        conn.Open();// abro la conexion
        string sql = @"UPDATE Tipo 
                               SET observacion=@observacion 
                               WHERE idTipo=@idTipo";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          // asigno valores a los aprametros
          cmd.Parameters.AddWithValue("@observacion", tipo.Observacion);
          cmd.Parameters.AddWithValue("@idTipo", tipo.IdTipo);
          res = cmd.ExecuteNonQuery();
        }
      }
      return res;
    }

    // LISTAR
    public List<Tipo> Listar()
    {
      var lista = new List<Tipo>();// lista vacia q voy a llenar
      using (var conn = _context.GetConnection())// creo la conexion
      {
        conn.Open();// abro la conexion
        string sql = "SELECT idTipo, observacion FROM Tipo";
        using (var cmd = new MySqlCommand(sql, conn))
        using (var reader = cmd.ExecuteReader())
        {
          while (reader.Read())// recorro cada fila
          {
            lista.Add(new Tipo  // creo el objet con los datos de las filas
            {
              IdTipo = reader.GetInt32("idTipo"),
              Observacion = reader.GetString("observacion")
            });
          }
        }// se cierra el reader, cierra la conexion
      }
      return lista;
    }

    // OBTENER POR ID
    public Tipo? Obtener(int idTipo)
    {
      Tipo? tipo = null;
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = "SELECT idTipo, observacion FROM Tipo WHERE idTipo=@idTipo";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@idTipo", idTipo);
          using (var reader = cmd.ExecuteReader()) //devuelve un DataReader para recorrer filas(lo uso xq quiero leer datos de la bd)
          {
            if (reader.Read())
            {
              tipo = new Tipo
              {
                IdTipo = reader.GetInt32("idTipo"),
                Observacion = reader.GetString("observacion")
              };
            }
          }// se cierra el reader, cierra la conexion
        }
      }
      return tipo;
    }

    // ELIMINAR
    public int Eliminar(int idTipo)
    {
      int res = -1; // inicializa
      using (var conn = _context.GetConnection()) // creo la conexion
      {
        conn.Open();// abro conexion
        string sql = "DELETE FROM Tipo WHERE idTipo=@idTipo";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@idTipo", idTipo);
          res = cmd.ExecuteNonQuery(); //  metodo que devuelve la cant de filas afectadas(lo uso para insert delete update)
        }
      }
      return res;
    }
  }
}
