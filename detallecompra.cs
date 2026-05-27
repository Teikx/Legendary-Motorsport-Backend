namespace Legendary_Motorsport_Backend
{
    public class DetalleCompra
    {
        public int IdDetalleCompra { get; set; }
        public int IdCompra { get; set; } 
        public int IdVehiculo { get; set; } 
        public int Cantidad { get; set; }
        // Precio definitivo al que se vendió, sin importar si el precio del catálogo cambia en el futuro
        public decimal PrecioFinal { get; set; } 
    }
}