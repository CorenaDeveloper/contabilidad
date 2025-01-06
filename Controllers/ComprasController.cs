using contabsv.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace contabsv.Controllers
{
    public class ComprasController : Controller
    {
        private readonly ContabilidadDbContext _ContabilidadDbContext;
        public ComprasController(ContabilidadDbContext ContabilidadDbContext)
        {
            _ContabilidadDbContext = ContabilidadDbContext;

        }

        [HttpGet]
        public async Task<IActionResult> ListarCompras(int tipoFecha, string fechaInicio, string fechaFin)
        {
            try
            {
                var idClienteR = User.Claims.FirstOrDefault(c => c.Type == "IdCliente")?.Value;
                var result = await _ContabilidadDbContext.Database
                    .SqlQueryRaw<listaCompras>("EXEC ListarCompras @tipoFecha, @fechaInicio, @fechaFin,  @idCliente",
                    new SqlParameter("@tipoFecha", SqlDbType.Int) { Value = tipoFecha },
                    new SqlParameter("@fechaInicio", SqlDbType.DateTime) { Value = fechaInicio },
                    new SqlParameter("@fechaFin", SqlDbType.DateTime) { Value = fechaFin },
                    new SqlParameter("@idCliente", SqlDbType.Int) { Value = idClienteR }
                    )
                    .ToListAsync();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la lista: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearCompras([FromBody] List<RegistroCompras> add)
        {
            try
            {

                if (add == null || !add.Any())
                {
                    return BadRequest("La lista de compras no puede estar vacía.");
                }

                var json = JsonSerializer.Serialize(add);
                var message = await _ContabilidadDbContext.Database
                    .ExecuteSqlRawAsync("EXEC Crud_Compras @ComprasData",
                        new SqlParameter("@ComprasData", SqlDbType.NVarChar) { Value = json });

                if (message > 0)
                {
                    return Json(new { success = true, message = "compras ingresados correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudieron ingresar las compras." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar datos : {ex.Message}");
            }
        }
    }
}
