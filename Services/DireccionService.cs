using Legendary_Motorsport_Backend.Models;
using Legendary_Motorsport_Backend.Repositories;

namespace Legendary_Motorsport_Backend.Services
{
    public class DireccionService : IDireccionService
    {
        private readonly IDireccionRepository _direccionRepository;

        public DireccionService(IDireccionRepository direccionRepository)
        {
            _direccionRepository = direccionRepository;
        }

        public async Task<IEnumerable<Direccion>> ObtenerDireccionesDeClienteAsync(int idCliente)
        {
            return await _direccionRepository.ObtenerPorClienteIdAsync(idCliente);
        }

        public async Task<bool> AgregarDireccionAsync(Direccion direccion)
        {
            if (direccion.IdCliente <= 0)
                throw new ArgumentException("El ID del cliente es obligatorio para registrar una dirección.");

            if (string.IsNullOrWhiteSpace(direccion.DetalleDireccion))
                throw new ArgumentException("La dirección exacta no puede estar vacía.");

            return await _direccionRepository.CrearAsync(direccion);
        }

        public async Task<bool> ModificarDireccionAsync(Direccion direccion)
        {
            if (direccion.IdDire <= 0)
                throw new ArgumentException("ID de dirección inválido para actualización.");

            return await _direccionRepository.ActualizarAsync(direccion);
        }

        public async Task<bool> BorrarDireccionAsync(int idDire)
        {
            return await _direccionRepository.EliminarAsync(idDire);
        }
    }
}