namespace Legendary_Motorsport_Backend
{
    public class Carrito
    {
        public int IdCarrito { get; set; }
        public int IdCliente { get; set; } 
        public DateTime FechaCreacion { get; set; }
        public bool EstadoActivo { get; set; } 
    }
}