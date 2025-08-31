//Repositorios/RepositorioInmueble.cs
using Inmobiliaria25.Db; //Uso de DB
using MySql.Data.MySqlClient; //Hace uso de mysql
using Inmobiliaria25.Models; //Hace uso de Models, pero OJO esta en minuscula el namespace

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
    public List<Inmuebles> ObtenerInmueblesActivos()
    {
      List<Inmuebles> inmueble = new List<Inmuebles>();
      using (var conn = _context.GetConnection())
      {
        conn.Open();
        /*string sql = $@" SELECT {nameof(Inmuebles.idInmueble)}, {nameof(Inmuebles.idPropietario)}, {nameof(Inmuebles.idDireccion)}, {nameof(Inmuebles.idTipo)}, {nameof(Inmuebles.metros2)}, 
        {nameof(Inmuebles.cantidadAmbientes)}, {nameof(Inmuebles.disponible)}, {nameof(Inmuebles.precio)}, {nameof(Inmuebles.descripcion)}, {nameof(Inmuebles.cochera)}, {nameof(Inmuebles.piscina)}, 
        {nameof(Inmuebles.mascotas)}, {nameof(Inmuebles.urlImagen)} FROM inmueble WHERE estado=1 ";*/
        string sql= @"SELECT i.idInmueble, i.idPropietario, i.idDireccion, i.idTipo, i.metros2, i.cantidadAmbientes, i.disponible, i.precio, i.descripcion, 
i.cochera, i.piscina, i.mascotas, i.urlImagen, i.estado, p.nombre, p.apellido, p.dni, p.correo
FROM inmueble i
INNER JOIN propietario p ON i.idPropietario = p.idPropietario
WHERE i.estado = 1";
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
              disponible = reader.GetBoolean("disponible"),
              precio = reader.GetDecimal("precio"),
              descripcion = reader.GetString("descripcion"),
              cochera = reader.GetBoolean("cochera"), //tinyInt corresponde a bool en c#
              piscina = reader.GetBoolean("piscina"),
              mascotas = reader.GetBoolean("mascotas"),
              urlImagen= reader.GetString("urlImagen")
            });
          }
        }
      }
      return inmueble;
    }


  }


}
