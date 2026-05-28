using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Legendary_Motorsport_Backend.Models;
using Legendary_Motorsport_Backend.Services;

namespace Legendary_Motorsport_Backend.Controllers
{
    [ApiController]
    [Route("api/carrito")]
    [Authorize]
    public class CarritoController : ControllerBase
    {
        private readonly ICarritoService _carritoService;

        public CarritoController(ICarritoService carritoService)
        {
            _carritoService = carritoService;
        }

        [HttpGet("activo")]
        public async Task<IActionResult> GetActivo()
        {
            var idCliente = ObtenerIdCliente();
            if (idCliente == 0)
            {
                return Unauthorized(new { error = "Token invalido" });
            }

            var carrito = await _carritoService.ObtenerOCrearActivoAsync(idCliente);
            return Ok(carrito);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] CarritoItemRequest request)
        {
            try
            {
                var idCliente = ObtenerIdCliente();
                if (idCliente == 0)
                {
                    return Unauthorized(new { error = "Token invalido" });
                }

                var carrito = await _carritoService.AgregarOActualizarItemAsync(idCliente, request.IdProducto, request.Cantidad);
                return Ok(carrito);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("items")]
        public async Task<IActionResult> UpdateItem([FromBody] CarritoItemRequest request)
        {
            try
            {
                var idCliente = ObtenerIdCliente();
                if (idCliente == 0)
                {
                    return Unauthorized(new { error = "Token invalido" });
                }

                var carrito = await _carritoService.ActualizarCantidadAsync(idCliente, request.IdProducto, request.Cantidad);
                return Ok(carrito);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("items/{idProducto:int}")]
        public async Task<IActionResult> DeleteItem(int idProducto)
        {
            try
            {
                var idCliente = ObtenerIdCliente();
                if (idCliente == 0)
                {
                    return Unauthorized(new { error = "Token invalido" });
                }

                var carrito = await _carritoService.EliminarItemAsync(idCliente, idProducto);
                return Ok(carrito);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private int ObtenerIdCliente()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null && int.TryParse(claim.Value, out var id) ? id : 0;
        }
    }
}
