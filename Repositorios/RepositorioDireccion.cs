//Repositorio/RepositorioDireccion.cs
using Inmobiliaria25.Db;
using MySql.Data.MySqlClient;
using Inmobiliaria25.Models;

namespace Inmobiliaria25.Repositorios
{
  public class RepositorioDireccion
  {
    private readonly DataContext _context;// heredo

    public RepositorioDireccion(DataContext context)// constructor
    {
      _context = context;
    }

    // crear 
    public int Crear(Direccion d)
    {
      int id = -1;
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = @"INSERT INTO Direccion (calle, altura, cp, ciudad, coordenadas)
                           VALUES (@calle, @altura, @cp, @ciudad, @coordenadas);
                           SELECT LAST_INSERT_ID();";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@calle", d.Calle);
          cmd.Parameters.AddWithValue("@altura", d.Altura);
          cmd.Parameters.AddWithValue("@cp", d.Cp);
          cmd.Parameters.AddWithValue("@ciudad", d.Ciudad);
          cmd.Parameters.AddWithValue("@coordenadas", d.Coordenadas);
          id = Convert.ToInt32(cmd.ExecuteScalar());
        }
      }
      return id;
    }
    // modif (cuando se edita un inmueble)
    public int Modificar(Direccion direccion)
    {
      int res = -1;
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = @"UPDATE Direccion 
                               SET calle=@calle, altura=@altura, cp=@cp, ciudad=@ciudad, coordenadas=@coordenadas
                               WHERE idDireccion=@idDireccion";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@calle", direccion.Calle);
          cmd.Parameters.AddWithValue("@altura", direccion.Altura);
          cmd.Parameters.AddWithValue("@cp", direccion.Cp);
          cmd.Parameters.AddWithValue("@ciudad", direccion.Ciudad);
          cmd.Parameters.AddWithValue("@coordenadas", direccion.Coordenadas);
          cmd.Parameters.AddWithValue("@idDireccion", direccion.IdDireccion);
          res = cmd.ExecuteNonQuery();
        }
      }
      return res;
    }

    // obtener 
    public Direccion? Obtener(int idDireccion)
    {
      Direccion? direccion = null;
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        string sql = "SELECT idDireccion, calle, altura, cp, ciudad, coordenadas FROM Direccion WHERE idDireccion=@idDireccion";
        using (var cmd = new MySqlCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@idDireccion", idDireccion);
          using (var reader = cmd.ExecuteReader())
          {
            if (reader.Read())
            {
              direccion = new Direccion
              {
                IdDireccion = reader.GetInt32("idDireccion"),
                Calle = reader.GetString("calle"),
                Altura = reader.GetInt32("altura"),
                Cp = reader.GetString("cp"),
                Ciudad = reader.GetString("ciudad"),
                Coordenadas = reader.GetString("coordenadas")
              };
            }
          }
        }
      }
      return direccion;
    }
  }
}
