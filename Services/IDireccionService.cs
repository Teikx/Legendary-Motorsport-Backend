using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Services
{
    public interface IDireccionService
    {
        Task<IEnumerable<Direccion>> ObtenerDireccionesDeClienteAsync(int idCliente);
        Task<bool> AgregarDireccionAsync(Direccion direccion);
        Task<bool> ModificarDireccionAsync(Direccion direccion);
        Task<bool> BorrarDireccionAsync(int idDire);
    }
}