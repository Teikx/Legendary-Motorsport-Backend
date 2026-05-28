using Legendary_Motorsport_Backend.Models;
using Legendary_Motorsport_Backend.Repositories;

namespace Legendary_Motorsport_Backend.Services
{
    public class VehiculoService : IVehiculoService
    {
        private readonly IVehiculoRepository _vehiculoRepository;

        public VehiculoService(IVehiculoRepository vehiculoRepository)
        {
            _vehiculoRepository = vehiculoRepository;
        }

        public async Task<IEnumerable<CatalogoResumenDto>> ObtenerCatalogoAsync()
        {
            return await _vehiculoRepository.ObtenerCatalogoAsync();
        }

        public async Task<CatalogoDetalleDto?> ObtenerDetalleAsync(int idVehiculo)
        {
            return await _vehiculoRepository.ObtenerDetalleAsync(idVehiculo);
        }
    }
}
