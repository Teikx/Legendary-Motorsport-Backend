using Legendary_Motorsport_Backend.Models;
using Legendary_Motorsport_Backend.Repositories;

namespace Legendary_Motorsport_Backend.Services
{
    public class TarjetaService : ITarjetaService
    {
        private readonly ITarjetaRepository _tarjetaRepository;

        // Inversión de Dependencias (DIP)
        public TarjetaService(ITarjetaRepository tarjetaRepository)
        {
            _tarjetaRepository = tarjetaRepository;
        }

        public async Task<IEnumerable<Tarjeta>> ObtenerTarjetasDeClienteAsync(int idCliente)
        {
            return await _tarjetaRepository.ObtenerPorClienteIdAsync(idCliente);
        }

        public async Task<bool> AgregarTarjetaAsync(Tarjeta tarjeta)
        {
            // Regla de negocio básica: la tarjeta debe estar atada a un cliente
            if (tarjeta.IdCliente <= 0)
                throw new ArgumentException("El ID del cliente es obligatorio para registrar una tarjeta.");
                
            return await _tarjetaRepository.CrearAsync(tarjeta);
        }

        public async Task<bool> ModificarTarjetaAsync(Tarjeta tarjeta)
        {
            if (tarjeta.IdTarjeta <= 0)
                throw new ArgumentException("ID de tarjeta inválido para actualización.");

            return await _tarjetaRepository.ActualizarAsync(tarjeta);
        }

        public async Task<bool> BorrarTarjetaAsync(int idTarjeta)
        {
            return await _tarjetaRepository.EliminarAsync(idTarjeta);
        }
    }
}