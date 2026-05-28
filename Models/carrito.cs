namespace Legendary_Motorsport_Backend.Models
{
    public class Carrito
    {
        public int IdCarrito { get; set; } // 
        public int IdCliente { get; set; } // 
        public DateTime FechaCreacion { get; set; } // 
        public decimal Total { get; set; } // 
        public string Estado { get; set; } // 
    }
}