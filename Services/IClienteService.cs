using Legendary_Motorsport_Backend.Models;
namespace Legendary_Motorsport_Backend.Services
{
    public interface IClienteService
    {
        Task<bool> RegistrarClienteAsync(ClienteCreateRequest cliente);
        Task<IEnumerable<Cliente>> ObtenerTodosAsync();
        Task<Cliente?> ObtenerPorIdAsync(int idCliente);
        Task<bool> ActualizarClienteAsync(int idCliente, ClienteUpdateRequest cliente);
        Task<bool> EliminarClienteAsync(int idCliente);
        Task<LoginResponse?> LoginAsync(ClienteLoginRequest login);
    }
}