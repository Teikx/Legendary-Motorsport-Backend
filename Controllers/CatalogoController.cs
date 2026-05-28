using Microsoft.AspNetCore.Mvc;
using Legendary_Motorsport_Backend.Services;

namespace Legendary_Motorsport_Backend.Controllers
{
    [ApiController]
    [Route("api/catalogo")]
    public class CatalogoController : ControllerBase
    {
        private readonly IVehiculoService _vehiculoService;

        public CatalogoController(IVehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCatalogo()
        {
            var catalogo = await _vehiculoService.ObtenerCatalogoAsync();
            return Ok(catalogo);
        }

        [HttpGet("{idVehiculo:int}")]
        public async Task<IActionResult> GetDetalle(int idVehiculo)
        {
            var detalle = await _vehiculoService.ObtenerDetalleAsync(idVehiculo);
            if (detalle == null)
            {
                return NotFound(new { error = "Vehiculo no encontrado" });
            }

            return Ok(detalle);
        }
    }
}
