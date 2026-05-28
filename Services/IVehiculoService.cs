using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Services
{
    public interface IVehiculoService
    {
        Task<IEnumerable<CatalogoResumenDto>> ObtenerCatalogoAsync();
        Task<CatalogoDetalleDto?> ObtenerDetalleAsync(int idVehiculo);
    }
}
