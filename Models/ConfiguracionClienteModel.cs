namespace contabsv.Models
{
    public class ConfiguracionClienteModel
    {
        public int idCliente {  get; set; }
        public string? nombres {  get; set; }
        public string? apellidos {  get; set; }
        public Boolean personaJuridica {  get; set; }
        public string? nombreRazonSocial {  get; set; }
        public string? nombreComercial {  get; set; }
        public string? duiCliente {  get; set; }
        public string? representanteLegal {  get; set; }
        public string? duiRepresentanteLegal {  get; set; }
        public string? telefonoCliente {  get; set; }
        public string? celular {  get; set; }
        public string? nrc {  get; set; }
        public string? nitCliente {  get; set; }
        public string? codSector {  get; set; }
        public string? nomSector {  get; set; }
        public string? direccion {  get; set; }
        public string? correo {  get; set; }
    }
}
