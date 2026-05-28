namespace Legendary_Motorsport_Backend.Models
{
    public class Tarjeta
    {
        public int IdTarjeta { get; set; } // 
        public string Numero { get; set; } // 
        public string Expiracion { get; set; } // Ahora es string por el VARCHAR(5) 
        public string Ccv { get; set; } // Ahora es string por el VARCHAR(4) 
        public string Nombre { get; set; } // 
        public int IdCliente { get; set; } // 
    }
}