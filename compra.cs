namespace Legendary_Motorsport_Backend
{
    public class Compra
    {
        public int IdCompra { get; set; }
        public int IdCliente { get; set; } 
        public int IdTarjeta { get; set; } 
        public DateTime FechaCompra { get; set; }
        public decimal TotalCobrado { get; set; }
    }
}