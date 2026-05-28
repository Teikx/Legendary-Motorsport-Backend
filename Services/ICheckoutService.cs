using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Services
{
    public interface ICheckoutService
    {
        Task<CheckoutResponse> ProcesarCheckoutAsync(int idCliente, CheckoutRequest request);
    }
}
