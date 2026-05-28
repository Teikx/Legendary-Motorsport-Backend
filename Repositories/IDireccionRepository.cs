using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Repositories
{
    public interface IDireccionRepository
    {
        Task<IEnumerable<Direccion>> ObtenerPorClienteIdAsync(int idCliente);
        Task<bool> CrearAsync(Direccion direccion);
        Task<bool> ActualizarAsync(Direccion direccion);
        Task<bool> EliminarAsync(int idDire);
    }
}