using Microsoft.AspNetCore.Mvc;
using Legendary_Motorsport_Backend.Models;
using Legendary_Motorsport_Backend.Services;

namespace Legendary_Motorsport_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DireccionesController : ControllerBase
    {
        private readonly IDireccionService _direccionService;

        public DireccionesController(IDireccionService direccionService)
        {
            _direccionService = direccionService;
        }

        // GET: api/direcciones/cliente/5
        [HttpGet("cliente/{idCliente}")]
        public async Task<IActionResult> GetByCliente(int idCliente)
        {
            var direcciones = await _direccionService.ObtenerDireccionesDeClienteAsync(idCliente);
            return Ok(direcciones);
        }

        // POST: api/direcciones
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Direccion direccion)
        {
            try
            {
                var resultado = await _direccionService.AgregarDireccionAsync(direccion);
                if (resultado) return Ok(new { mensaje = "Dirección guardada con éxito" });
                return BadRequest("No se pudo guardar la dirección");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT: api/direcciones
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Direccion direccion)
        {
            try
            {
                var resultado = await _direccionService.ModificarDireccionAsync(direccion);
                if (resultado) return Ok(new { mensaje = "Dirección actualizada con éxito" });
                return NotFound("No se encontró la dirección para actualizar");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/direcciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _direccionService.BorrarDireccionAsync(id);
            if (resultado) return Ok(new { mensaje = "Dirección eliminada con éxito" });
            return NotFound("No se encontró la dirección para eliminar");
        }
    }
}