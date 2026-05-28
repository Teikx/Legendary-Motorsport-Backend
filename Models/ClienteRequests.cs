namespace Legendary_Motorsport_Backend.Models
{
    public class ClienteCreateRequest
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public int IdRol { get; set; }
        public string Contrasena { get; set; }
    }

    public class ClienteUpdateRequest
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public int IdRol { get; set; }
        public string Contrasena { get; set; }
    }

    public class ClienteLoginRequest
    {
        public string Email { get; set; }
        public string Contrasena { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public int IdCliente { get; set; }
        public string Email { get; set; }
        public int IdRol { get; set; }
    }
}
