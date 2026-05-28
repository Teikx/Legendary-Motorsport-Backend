namespace Legendary_Motorsport_Backend.Models
{
    public class CheckoutRequest
    {
        public int IdTarjeta { get; set; }
        public int IdDireccion { get; set; }
    }

    public class CheckoutResponse
    {
        public int IdCompra { get; set; }
        public decimal Total { get; set; }
        public List<CheckoutItemDto> Items { get; set; } = new();
    }

    public class CheckoutItemDto
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
    }
}
