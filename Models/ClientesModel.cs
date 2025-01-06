using System.ComponentModel.DataAnnotations;

namespace contabsv.Models
{
    public class Cliente
    {
        public string? flag { get; set; } // Indica la acción: "create", "update", o "delete"
        public string? nombres { get; set; }
        public string? apellidos { get; set; }
        public bool? personaJuridica { get; set; } // Puede ser null si no aplica
        public string? nombreRazonSocial { get; set; }
        public string? nombreComercial { get; set; }
        public string? duiCliente { get; set; }
        public string? representanteLegal { get; set; }
        public string? duiRepresentanteLegal { get; set; }
        public string? telefonoCliente { get; set; }
        public string? celular { get; set; }
        public string? nrc { get; set; } // Número de registro de contribuyente
        public string? nitCliente { get; set; }
        public int? idCliente { get; set; }

        /// <summary>
        /// Limpia el NRC eliminando espacios y guiones.
        /// </summary>
        public void LimpiarNRC()
        {
            if (!string.IsNullOrEmpty(nrc))
            {
                nrc = nrc.Replace(" ", "").Replace("-", "");
            }
        }

        /// <summary>
        /// Validación de datos básicos.
        /// </summary>
        /// <returns>Retorna true si los datos son válidos.</returns>
        public bool Validar()
        {
            if (string.IsNullOrEmpty(nrc))
                throw new ArgumentException("El NRC no puede estar vacío.");
            return true;
        }
    }


    public class listarClientes
    {
        [Key]
        public int? idClienteClt { get; set; }
        public string? nombres { get; set; }
        public string? apellidos { get; set; }
        public bool? personaJuridica { get; set; } 
        public string? nombreRazonSocial { get; set; }
        public string? nombreComercial { get; set; }
        public string? duiCliente { get; set; }
        public string? representanteLegal { get; set; }
        public string? duiRepresentanteLegal { get; set; }
        public string? telefonoCliente { get; set; }
        public string? celular { get; set; }
        public string? nrc { get; set; } 
        public string? nitCliente { get; set; }
    }

    public class EliminarCliente
    {
        [Key]
        public int? idClienteClt { get; set; }
        public string? flag { get; set; }

    }

    public class EditarCliente
    {
        [Key]
        public int? idClienteClt { get; set; }
        public string? flag { get; set; }
        public string? nombres { get; set; }
        public string? apellidos { get; set; }
        public bool? personaJuridica { get; set; }
        public string? nombreRazonSocial { get; set; }
        public string? nombreComercial { get; set; }
        public string? duiCliente { get; set; }
        public string? representanteLegal { get; set; }
        public string? duiRepresentanteLegal { get; set; }
        public string? telefonoCliente { get; set; }
        public string? celular { get; set; }
        public string? nrc { get; set; }
        public string? nitCliente { get; set; }
    }
}
