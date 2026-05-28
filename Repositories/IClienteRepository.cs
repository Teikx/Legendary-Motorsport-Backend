using Legendary_Motorsport_Backend.Models;
namespace Legendary_Motorsport_Backend.Repositories
{
    public interface IClienteRepository
    {
        Task<bool> CrearClienteAsync(Cliente cliente);
        Task<IEnumerable<Cliente>> ObtenerTodosAsync();
        Task<Cliente?> ObtenerPorIdAsync(int idCliente);
        Task<Cliente?> ObtenerPorEmailAsync(string email);
        Task<bool> ActualizarClienteAsync(Cliente cliente, bool actualizarContrasena);
        Task<bool> EliminarClienteAsync(int idCliente);
    }
}