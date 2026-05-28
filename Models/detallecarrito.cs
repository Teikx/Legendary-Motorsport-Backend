namespace Legendary_Motorsport_Backend.Models
{
    public class DetalleCarrito
    {
        public int IdDetalleCarrito { get; set; } // 
        public int IdCarrito { get; set; } // 
        public int IdProducto { get; set; } // 
        public int Cantidad { get; set; } // 
        public decimal Precio { get; set; } // 
    }
}