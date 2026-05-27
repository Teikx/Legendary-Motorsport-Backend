namespace Legendary_Motorsport_Backend.Repositories
{
    public interface IClienteRepository
    {
        Task<bool> CrearClienteAsync(Cliente cliente);
        Task<IEnumerable<Cliente>> ObtenerTodosAsync();
    }
}