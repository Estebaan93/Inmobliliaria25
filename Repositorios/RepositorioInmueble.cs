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

        // LISTAR con filtros
        public List<Inmueble> Listar(int? estado = null, int? idPropietario = null)
{
    var lista = new List<Inmueble>();
    using (var conn = _context.GetConnection())
    {
        conn.Open();
        string sql = @"
            SELECT i.idInmueble, i.idPropietario, i.idTipo, i.descripcion, 
                   i.cantidadAmbientes, i.precio, i.cochera, i.piscina, i.mascotas, i.estado,
                   t.idTipo, t.observacion,
                   p.idPropietario, p.nombre, p.apellido
            FROM Inmueble i
            INNER JOIN Tipo t ON i.idTipo = t.idTipo
            INNER JOIN Propietario p ON i.idPropietario = p.idPropietario
            WHERE 1=1";

        if (estado.HasValue)
            sql += " AND i.estado = @estado";

        if (idPropietario.HasValue)
            sql += " AND i.idPropietario = @idPropietario";

        using (var cmd = new MySqlCommand(sql, conn))
        {
            if (estado.HasValue)
                cmd.Parameters.AddWithValue("@estado", estado.Value);

            if (idPropietario.HasValue)
                cmd.Parameters.AddWithValue("@idPropietario", idPropietario.Value);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(new Inmueble
                    {
                        idInmueble = reader.GetInt32("idInmueble"),
                        descripcion = reader.GetString("descripcion"),
                        cantidadAmbientes = reader.GetInt32("cantidadAmbientes"),
                        precio = reader.GetDecimal("precio"),
                        cochera = reader.GetBoolean("cochera"),
                        piscina = reader.GetBoolean("piscina"),
                        mascotas = reader.GetBoolean("mascotas"),
                        estado = (EstadoInmueble)reader.GetInt32("estado"),

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
    }
    return lista;
}

    }
}
