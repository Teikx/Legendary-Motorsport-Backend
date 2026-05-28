using MySqlConnector;

namespace Legendary_Motorsport_Backend.Data
{
    public class ConexionDb : IConexionDb
    {
        private readonly string _cadenaConexion;

        public ConexionDb(IConfiguration configuracion)
        {
            // Busca la conexión en tu appsettings.json
            _cadenaConexion = configuracion.GetConnectionString("ConexionMySQL");
        }

        public MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(_cadenaConexion);
        }
    }
}