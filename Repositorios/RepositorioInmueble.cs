using Inmobiliaria25.Db;
using MySql.Data.MySqlClient;
using Inmobiliaria25.Models;


namespace Inmobiliaria25.Repositorios
{
    public class RepositorioInmueble
    {
        private readonly DataContext _context;

        public RepositorioInmueble(DataContext context)
        {
            _context = context;
        }

        // LISTAR con join para traer propietario y tipo
        public List<Inmueble> Listar()
        {
            var lista = new List<Inmueble>();
            using (var conn = _context.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT i.idInmueble, i.idPropietario, i.idTipo, i.descripcion, 
                           i.cantidadAmbientes, i.precio, i.cochera, i.piscina, i.mascotas,
                           t.idTipo, t.observacion,
                           p.idPropietario, p.nombre, p.apellido
                    FROM Inmueble i
                    INNER JOIN Tipo t ON i.idTipo = t.idTipo
                    INNER JOIN Propietario p ON i.idPropietario = p.idPropietario";

                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Inmueble
                        {
                            idInmueble = reader.GetInt32("idInmueble"),
                            idPropietario = reader.GetInt32("idPropietario"),
                            idTipo = reader.GetInt32("idTipo"),
                            descripcion = reader.GetString("descripcion"),
                            cantidadAmbientes = reader.GetInt32("cantidadAmbientes"),
                            precio = reader.GetDecimal("precio"),
                            cochera = reader.GetBoolean("cochera"),
                            piscina = reader.GetBoolean("piscina"),
                            mascotas = reader.GetBoolean("mascotas"),

                            propietario = new Propietarios
                            {
                                idPropietario = reader.GetInt32("idPropietario"),
                                nombre = reader.GetString("nombre"),
                                apellido = reader.GetString("apellido")
                            },
                            tipo = new Tipo
                            {
                                IdTipo = reader.GetInt32("idTipo"),
                                Observacion = reader.GetString("observacion")
                            }
                        });
                    }
                }
            }
            return lista;
        }
    }
}
