namespace Legendary_Motorsport_Backend.Models
{
    public class Vehiculo
    {
        public int IdVehiculo { get; set; } 
        public string Marca { get; set; } 
        public string Modelo { get; set; } 
        public DateTime FechaAnadido { get; set; } 
        public string Imagen { get; set; }
    }
}