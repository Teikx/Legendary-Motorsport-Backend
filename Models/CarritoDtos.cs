namespace Legendary_Motorsport_Backend.Models
{
    public class CarritoDto
    {
        public int IdCarrito { get; set; }
        public int IdCliente { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public List<CarritoItemDto> Items { get; set; } = new();
    }

    public class CarritoItemDto
    {
        public int IdDetalleCarrito { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
        public int Kilometraje { get; set; }
        public string ImagenUrl { get; set; }
    }

    public class CarritoItemRequest
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
    }
}
