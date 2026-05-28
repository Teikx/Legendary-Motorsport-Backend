namespace Legendary_Motorsport_Backend.Models
{
    public class DetalleCompra
    {
        public int IdDetalleCompra { get; set; } // 
        public int IdCompra { get; set; } // 
        public int IdProducto { get; set; } // 
        public int Cantidad { get; set; } // 
        public decimal Precio { get; set; } // 
    }
}