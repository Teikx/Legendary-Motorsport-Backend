using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Legendary_Motorsport_Backend.Models;
using Legendary_Motorsport_Backend.Services;

namespace Legendary_Motorsport_Backend.Controllers
{
    [ApiController]
    [Route("api/checkout")]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CheckoutRequest request)
        {
            try
            {
                var idCliente = ObtenerIdCliente();
                if (idCliente == 0)
                {
                    return Unauthorized(new { error = "Token invalido" });
                }

                var respuesta = await _checkoutService.ProcesarCheckoutAsync(idCliente, request);
                return Ok(respuesta);
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
