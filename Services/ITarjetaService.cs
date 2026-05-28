using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Services
{
    public interface ITarjetaService
    {
        Task<IEnumerable<Tarjeta>> ObtenerTarjetasDeClienteAsync(int idCliente);
        Task<bool> AgregarTarjetaAsync(Tarjeta tarjeta);
        Task<bool> ModificarTarjetaAsync(Tarjeta tarjeta);
        Task<bool> BorrarTarjetaAsync(int idTarjeta);
    }
}