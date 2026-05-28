using Legendary_Motorsport_Backend.Models;
using Legendary_Motorsport_Backend.Repositories;

namespace Legendary_Motorsport_Backend.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICheckoutRepository _checkoutRepository;

        public CheckoutService(ICheckoutRepository checkoutRepository)
        {
            _checkoutRepository = checkoutRepository;
        }

        public async Task<CheckoutResponse> ProcesarCheckoutAsync(int idCliente, CheckoutRequest request)
        {
            if (request.IdTarjeta <= 0 || request.IdDireccion <= 0)
            {
                throw new ArgumentException("IdTarjeta e IdDireccion son obligatorios.");
            }

            return await _checkoutRepository.ProcesarCheckoutAsync(idCliente, request.IdTarjeta, request.IdDireccion);
        }
    }
}
