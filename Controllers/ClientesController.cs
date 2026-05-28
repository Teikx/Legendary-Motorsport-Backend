using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Legendary_Motorsport_Backend.Services;
using Legendary_Motorsport_Backend.Models;

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
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Get()
        {
            var clientes = await _clienteService.ObtenerTodosAsync();
            return Ok(clientes);
        }

        [HttpGet("{idCliente:int}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetById(int idCliente)
        {
            var cliente = await _clienteService.ObtenerPorIdAsync(idCliente);
            if (cliente == null)
            {
                return NotFound(new { error = "Cliente no encontrado" });
            }

            return Ok(cliente);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] ClienteCreateRequest cliente)
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

        [HttpPut("{idCliente:int}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Put(int idCliente, [FromBody] ClienteUpdateRequest cliente)
        {
            try
            {
                var resultado = await _clienteService.ActualizarClienteAsync(idCliente, cliente);
                if (!resultado)
                {
                    return NotFound(new { error = "Cliente no encontrado" });
                }

                return Ok(new { mensaje = "Cliente actualizado con éxito" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{idCliente:int}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Delete(int idCliente)
        {
            var resultado = await _clienteService.EliminarClienteAsync(idCliente);
            if (!resultado)
            {
                return NotFound(new { error = "Cliente no encontrado" });
            }

            return Ok(new { mensaje = "Cliente eliminado con éxito" });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] ClienteLoginRequest login)
        {
            try
            {
                var resultado = await _clienteService.LoginAsync(login);
                if (resultado == null)
                {
                    return Unauthorized(new { error = "Credenciales inválidas" });
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}