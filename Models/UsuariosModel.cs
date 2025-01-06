namespace ApiSmartGarden.Models
{
    public class LoginUsuariosModel
    {
        public int IdUsuario { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? Usuario { get; set; }
        public Boolean Estado { get; set; }
        public string? Message { get; set; }
        public int? idCliente { get; set; }
        public string? nrcCliente { get; set; }
    }


    public class UsuariosModel
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? Usuario { get; set; }
        public string? Password { get; set; }
    }

}
