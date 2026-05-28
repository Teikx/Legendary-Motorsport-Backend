namespace Legendary_Motorsport_Backend.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; } // [cite: 4]
        public string Nombre { get; set; } // [cite: 4]
        public string Apellido { get; set; } // [cite: 4]
        public string Telefono { get; set; } // [cite: 4]
        public string Email { get; set; } // [cite: 4]
        public int IdRol { get; set; } // [cite: 4]
        public DateTime FechaCreacion { get; set; } // [cite: 4]

        [System.Text.Json.Serialization.JsonIgnore]
        public string ContrasenaHash { get; set; }
    }
}