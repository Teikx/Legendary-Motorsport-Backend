using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Repositories
{
    public interface ITarjetaRepository
    {
        Task<IEnumerable<Tarjeta>> ObtenerPorClienteIdAsync(int idCliente);
        Task<bool> CrearAsync(Tarjeta tarjeta);
        Task<bool> ActualizarAsync(Tarjeta tarjeta);
        Task<bool> EliminarAsync(int idTarjeta);
    }
}