namespace Legendary_Motorsport_Backend.Models
{
    public class CatalogoResumenDto
    {
        public int IdVehiculo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string ImagenUrl { get; set; }
        public decimal PrecioMinimo { get; set; }
        public int StockTotal { get; set; }
        public int ColoresDisponibles { get; set; }
    }

    public class CatalogoDetalleDto
    {
        public int IdVehiculo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string ImagenUrl { get; set; }
        public List<InventarioItemDto> Inventario { get; set; } = new();
    }

    public class InventarioItemDto
    {
        public int IdProducto { get; set; }
        public string Color { get; set; }
        public int Kilometraje { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
}
