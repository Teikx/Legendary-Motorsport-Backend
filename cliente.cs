namespace Legendary_Motorsport_Backend
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public int IdRol { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}