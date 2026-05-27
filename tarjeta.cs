namespace Legendary_Motorsport_Backend
{
    public class Tarjeta
    {
       public int IdTarjeta { get; set; }
       public int IdCliente { get; set; }
       public string Numero { get; set; }
       public string Nombre { get; set; } 
       public int Expiracion { get; set; }
       public int Ccv { get; set; }
    }
}