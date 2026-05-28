using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Repositories
{
    public interface IVehiculoRepository
    {
        Task<IEnumerable<CatalogoResumenDto>> ObtenerCatalogoAsync();
        Task<CatalogoDetalleDto?> ObtenerDetalleAsync(int idVehiculo);
    }
}
