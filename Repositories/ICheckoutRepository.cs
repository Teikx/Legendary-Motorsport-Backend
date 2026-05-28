using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Repositories
{
    public interface ICheckoutRepository
    {
        Task<CheckoutResponse> ProcesarCheckoutAsync(int idCliente, int idTarjeta, int idDireccion);
    }
}
