using MySqlConnector;

namespace Legendary_Motorsport_Backend.Data
{
    public interface IConexionDb
    {
        MySqlConnection ObtenerConexion();
    }
}