using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Inmobiliaria25.Db
{
    public class DataContext
    {
        private readonly string connectionString;

        public DataContext(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
         // el get me crea la conex y devuelve el obj MySqlConnection
         // TENER EN CUENTA NO ABRE LA CONEX, LA PREPARA, EN EL REPO SE ABRE CON conn.Open()
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}