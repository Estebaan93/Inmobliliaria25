using Inmobiliaria25.Db;
using MySql.Data.MySqlClient;
using inmobiliaria25.Models;

namespace Inmobiliaria25.Repositorios
{
    public class RepositorioDireccion
    {
        private readonly DataContext _context;// HEREDO

        public RepositorioDireccion(DataContext context)// CONSTRUCTOR
        {
            _context = context;
        }

        // CREAR (cuando se crea un inmueble nuevo)
        public int Crear(Direccion direccion)
        {
            int idCreado = -1;
            using (var conn = _context.GetConnection())
            {
                conn.Open();
                string sql = @"INSERT INTO Direccion (calle, altura, cp, ciudad, coordenadas)
                               VALUES (@calle, @altura, @cp, @ciudad, @coordenadas);
                               SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@calle", direccion.calle);
                    cmd.Parameters.AddWithValue("@altura", direccion.altura);
                    cmd.Parameters.AddWithValue("@cp", direccion.cp);
                    cmd.Parameters.AddWithValue("@ciudad", direccion.ciudad);
                    cmd.Parameters.AddWithValue("@coordenadas", direccion.coordenadas);
                    idCreado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return idCreado;
        }

        // MODIFICAR (cuando se edita un inmueble)
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
                    cmd.Parameters.AddWithValue("@calle", direccion.calle);
                    cmd.Parameters.AddWithValue("@altura", direccion.altura);
                    cmd.Parameters.AddWithValue("@cp", direccion.cp);
                    cmd.Parameters.AddWithValue("@ciudad", direccion.ciudad);
                    cmd.Parameters.AddWithValue("@coordenadas", direccion.coordenadas);
                    cmd.Parameters.AddWithValue("@idDireccion", direccion.idDireccion);
                    res = cmd.ExecuteNonQuery();
                }
            }
            return res;
        }

        // OBTENER (cuando se quiere mostrar detalles de un inmueble)
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
                                idDireccion = reader.GetInt32("idDireccion"),
                                calle = reader.GetString("calle"),
                                altura = reader.GetInt32("altura"),
                                cp = reader.GetString("cp"),
                                ciudad = reader.GetString("ciudad"),
                                coordenadas = reader.GetString("coordenadas")
                            };
                        }
                    }
                }
            }
            return direccion;
        }
    }
}
