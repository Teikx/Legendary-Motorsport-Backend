using Legendary_Motorsport_Backend.Models;
namespace Legendary_Motorsport_Backend.Services
{
    public interface IClienteService
    {
        Task<bool> RegistrarClienteAsync(Cliente cliente);
        Task<IEnumerable<Cliente>> ObtenerTodosAsync();
    }
}