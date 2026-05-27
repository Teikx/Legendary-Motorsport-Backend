using Legendary_Motorsport_Backend.Repositories;

namespace Legendary_Motorsport_Backend.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        // DIP: Pedimos la interfaz del repositorio, no la clase concreta
        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<bool> RegistrarClienteAsync(Cliente cliente)
        {
            // SRP: Aquí aplicamos las reglas de negocio antes de tocar la base de datos
            if (string.IsNullOrWhiteSpace(cliente.Email))
            {
                throw new ArgumentException("El email es obligatorio para registrar un cliente.");
            }

            return await _clienteRepository.CrearClienteAsync(cliente);
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodosAsync()
        {
            return await _clienteRepository.ObtenerTodosAsync();
        }
    }
}