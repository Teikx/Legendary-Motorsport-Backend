namespace Legendary_Motorsport_Backend
{
    public class DetalleCarrito
    {
        public int IdDetalleCarrito { get; set; }
        public int IdCarrito { get; set; } 
        public int IdVehiculo { get; set; } 
        public int Cantidad { get; set; }
        // Se guarda el precio al momento de agregarlo por si el precio del vehículo cambia luego
        public decimal PrecioUnitario { get; set; } 
    }
}