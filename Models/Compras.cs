namespace contabsv.Models
{
    public class listaCompras
    {
        public int idDocCompra { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaEmision { get; set; }
        public DateTime fechaPresentacion { get; set; }
        public string claseDocumento { get; set; }
        public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string nitProveedor { get; set; }
        public string nrcProveedor { get; set; }
        public string nombreProveedor { get; set; }
        public decimal internasExentas { get; set; } = 0.00m;
        public decimal internacionalesExentasY_O_NSujetas { get; set; } = 0.00m;
        public decimal importacionesY_O_NSujetas { get; set; } = 0.00m;
        public decimal internasGravadas { get; set; } = 0.00m;
        public decimal internacionesGravadasBienes { get; set; } = 0.00m;
        public decimal importacionesGravadasBienes { get; set; } = 0.00m;
        public decimal importacionesGravadasServicios { get; set; } = 0.00m;
        public decimal creditoFiscal { get; set; } = 0.00m;
        public decimal totalCompras { get; set; }
        public string duiProveedor { get; set; }
        public string tipoOperacion { get; set; }
        public string clasificacion { get; set; }
        public string tipoCostoGasto { get; set; }
        public string numeroAnexo { get; set; }
        public Boolean posteado { get; set; }
        public Boolean anulado { get; set; }
        public Boolean eliminado { get; set; }
        public int idCliente { get; set; }
        public Boolean combustible { get; set; }
        public string numSerie { get; set; } = string.Empty;
        public string codSector { get; set; }
        public string nomSector { get; set; }
    }
    public class RegistroCompras
    {
        public string flag { get; set; }
        public int? idDocCompra { get; set; }
        public DateTime? fechaEmision { get; set; }
        public DateTime? fechaPresentacion { get; set; }
        public string claseDocumento { get; set; }
        public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string nitProveedor { get; set; }
        public string nrcProveedor { get; set; }
        public string nombreProveedor { get; set; }
        public decimal? internasExentas { get; set; }
        public decimal? internacionalesExentasY_O_NSujetos { get; set; }
        public decimal? importacionesY_O_NSujetos { get; set; }
        public decimal? internasGravadas { get; set; }
        public decimal? internacionesGravadasBienes { get; set; }
        public decimal? importacionesGravadasBienes { get; set; }
        public decimal? importacionesGravadasServicios { get; set; }
        public decimal? creditoFiscal { get; set; }
        public decimal? totalCompras { get; set; }
        public string duiProveedor { get; set; }
        public string tipoOperacion { get; set; }
        public string clasificacion { get; set; }
        public string tipoCostoGasto { get; set; }
        public string numeroAnexo { get; set; }
        public Boolean posteado { get; set; }
        public Boolean anulado { get; set; }
        public Boolean eliminado { get; set; }
        public int idCliente { get; set; }
        public Boolean combustible { get; set; }
        public string numSerie { get; set; }
    }
}
