using Microsoft.AspNetCore.Mvc;
using Legendary_Motorsport_Backend.Services;

namespace Legendary_Motorsport_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        // DIP: Inyectamos el servicio
        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var clientes = await _clienteService.ObtenerTodosAsync();
            return Ok(clientes);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente cliente)
        {
            try
            {
                var resultado = await _clienteService.RegistrarClienteAsync(cliente);
                if (resultado) 
                {
                    return Ok(new { mensaje = "Cliente registrado con éxito" });
                }
                return BadRequest("No se pudo registrar el cliente en la base de datos");
            }
            catch (Exception ex)
            {
                // Si el servicio lanza el error del Email, lo atrapamos aquí
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}