namespace Legendary_Motorsport_Backend.Models
{
    public class Inventario
    {
        public int IdProducto { get; set; } // [cite: 5]
        public int IdVehiculo { get; set; } // [cite: 5]
        public int Stock { get; set; } // [cite: 5]
        public string Color { get; set; } // [cite: 5]
        public int Kilometraje { get; set; } // [cite: 5]
        public decimal Precio { get; set; } // [cite: 5]
    }
}