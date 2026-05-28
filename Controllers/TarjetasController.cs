using Microsoft.AspNetCore.Mvc;
using Legendary_Motorsport_Backend.Models;
using Legendary_Motorsport_Backend.Services;

namespace Legendary_Motorsport_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarjetasController : ControllerBase
    {
        private readonly ITarjetaService _tarjetaService;

        public TarjetasController(ITarjetaService tarjetaService)
        {
            _tarjetaService = tarjetaService;
        }

        // GET: api/tarjetas/cliente/5
        [HttpGet("cliente/{idCliente}")]
        public async Task<IActionResult> GetByCliente(int idCliente)
        {
            var tarjetas = await _tarjetaService.ObtenerTarjetasDeClienteAsync(idCliente);
            return Ok(tarjetas);
        }

        // POST: api/tarjetas
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Tarjeta tarjeta)
        {
            try
            {
                var resultado = await _tarjetaService.AgregarTarjetaAsync(tarjeta);
                if (resultado) return Ok(new { mensaje = "Tarjeta guardada con éxito" });
                return BadRequest("No se pudo guardar la tarjeta");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT: api/tarjetas
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Tarjeta tarjeta)
        {
            try
            {
                var resultado = await _tarjetaService.ModificarTarjetaAsync(tarjeta);
                if (resultado) return Ok(new { mensaje = "Tarjeta actualizada con éxito" });
                return NotFound("No se encontró la tarjeta para actualizar");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/tarjetas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _tarjetaService.BorrarTarjetaAsync(id);
            if (resultado) return Ok(new { mensaje = "Tarjeta eliminada con éxito" });
            return NotFound("No se encontró la tarjeta para eliminar");
        }
    }
}