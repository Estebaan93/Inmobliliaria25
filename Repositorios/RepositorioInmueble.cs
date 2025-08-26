//Repositorios/RepositorioInmueble.cs
using Inmobiliaria25.Db;
using MySql.Data.MySqlClient; //Hace uso de mysql
using inmobiliaria25.Models; //Hace uso de Models, pero OJO esta en minuscula el namespace

namespace Inmobiliaria25.Repositorios
{
  public class RepositorioInmueble
  {
    private readonly DataContext _context;

    public RepositorioInmueble(DataContext context)
    {
      this._context = context;
    }

    //Solo activos (estado = 1) 
    public List<Inmuebles> ObtenerInmuebles()
    {
      List<Inmuebles> inmueble = new List<Inmuebles>();
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        /*string sql = $@" SELECT {nameof(Inmuebles.idInmueble)}, {nameof(Inmuebles.idPropietario)}, {nameof(Inmuebles.idDireccion)}, {nameof(Inmuebles.idTipo)}, {nameof(Inmuebles.metros2)}, 
        {nameof(Inmuebles.cantidadAmbientes)}, {nameof(Inmuebles.disponible)}, {nameof(Inmuebles.precio)}, {nameof(Inmuebles.descripcion)}, {nameof(Inmuebles.cochera)}, {nameof(Inmuebles.piscina)}, 
        {nameof(Inmuebles.mascotas)}, {nameof(Inmuebles.urlImagen)} FROM inmueble WHERE estado=1 ";*/
        string sql= @"SELECT idInmueble, idPropietario, idDireccion, idTipo, metros2, cantidadAmbientes, disponible, 
                              precio, descripcion, cochera, piscina, mascotas, urlImagen, estado
                        FROM inmueble
                        WHERE estado = 1";
        using (var cmd = new MySqlCommand(sql, conn))
        using (var reader = cmd.ExecuteReader())
        {
          while (reader.Read())
          {
            inmueble.Add(new Inmuebles
            {
              idInmueble = reader.GetInt32("idInmueble"),
              idPropietario = reader.GetInt32("idPropietario"),
              idDireccion = reader.GetInt32("idDireccion"),
              idTipo = reader.GetInt32("idTipo"),
              metros2 = reader.GetString("metros2"),
              cantidadAmbientes = reader.GetInt32("cantidadAmbientes"),

            });

          }
        }


      }



      return inmueble;
    }


  }


}
