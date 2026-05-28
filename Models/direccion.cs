namespace Legendary_Motorsport_Backend.Models
{
    public class Direccion
    {
        public int IdDire { get; set; } // [cite: 6]
        public int IdCliente { get; set; } // [cite: 6]
        public string Nombre { get; set; } // [cite: 6]
        public string DetalleDireccion { get; set; } // Equivale a la columna 'Direccion' [cite: 6]
        public string Ciudad { get; set; } // [cite: 6]
        public string Region { get; set; } // [cite: 6]
        public string CodigoPostal { get; set; } // [cite: 6]
        public string Pais { get; set; } // [cite: 6]
    }
}