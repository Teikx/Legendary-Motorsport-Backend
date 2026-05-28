namespace Legendary_Motorsport_Backend.Models
{
    public class Compra
    {
        public int IdCompra { get; set; } // 
        public int IdCliente { get; set; } // 
        public int IdTarjeta { get; set; } // 
        public int IdDireccion { get; set; } // 
        public decimal Total { get; set; } // 
        public DateTime FechaCompra { get; set; } // 
    }
}