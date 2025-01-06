using contabsv.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Data;
using System.Text.Json;
using contabsv.Services;


namespace contabsv.Controllers
{
    public class AuxiliaresController : Controller
    {
        private readonly ApplicationDbContext _ApplicationDbContext;
        public AuxiliaresController(ApplicationDbContext applicationDbContext, HashPasswordSHA256 hashPasswordSHA256)
        {
            _ApplicationDbContext = applicationDbContext;

        }
        public IActionResult Proveedores()
        {
            var idCliente = User.Claims.FirstOrDefault(c => c.Type == "IdCliente")?.Value;
            ViewData["IdCliente"] = idCliente;

            if (!User.HasPermission("Proveedores"))
            {
                return RedirectToAction("Privacy", "Home");
            }
            return View();
        }
        
        public IActionResult ListarProvedores()
        {
            if (!User.HasPermission("ListarProvedores"))
            {
                return RedirectToAction("Privacy", "Home");
            }
            return View();
        }
        
        public IActionResult Clientes()
        {
            var idCliente = User.Claims.FirstOrDefault(c => c.Type == "IdCliente")?.Value;
            ViewData["IdCliente"] = idCliente;

            if (!User.HasPermission("Clientes"))
            {
                return RedirectToAction("Privacy", "Home");
            }
            return View();
        } 
        
        public IActionResult ListarClientes()
        {
            if (!User.HasPermission("ListarClientes"))
            {
                return RedirectToAction("Privacy", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListarProveedores()
        {
            try
            {
                var idClienteR = User.Claims.FirstOrDefault(c => c.Type == "IdCliente")?.Value;
                var result = await _ApplicationDbContext.Database
                    .SqlQueryRaw<listaProveedores>("EXEC ListaProveedores @idCliente",
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
        public async Task<IActionResult> CrearProveedores([FromBody] List<Proveedor> proveedores)
        {
            try
            {
                if (proveedores == null || !proveedores.Any())
                {
                    return BadRequest("La lista de proveedores no puede estar vacía.");
                }

                var json = JsonSerializer.Serialize(proveedores);
                var message = await _ApplicationDbContext.Database
                    .ExecuteSqlRawAsync("EXEC Crud_Proveedores @ProveedoresData",
                        new SqlParameter("@ProveedoresData", SqlDbType.NVarChar) { Value = json });

                if (message > 0)
                {
                    return Json(new { success = true, message = "Proveedores ingresados correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudieron ingresar los proveedores." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar datos : {ex.Message}");
            }
        }


        [HttpPut]
        public async Task<IActionResult> EditarProveeedores([FromBody] List<EditarProveedor> proveedores)
        {
            try
            {
                if (proveedores == null || !proveedores.Any())
                {
                    return BadRequest("La lista de proveedores no puede estar vacía.");
                }

                var json = JsonSerializer.Serialize(proveedores);
                var message = await _ApplicationDbContext.Database
                    .ExecuteSqlRawAsync("EXEC Crud_Proveedores @ProveedoresData",
                        new SqlParameter("@ProveedoresData", SqlDbType.NVarChar) { Value = json });

                if (message > 0)
                {
                    return Json(new { success = true, message = "Proveedores eliminado correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudieron eliminar los proveedores." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar datos : {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarProveeedores([FromBody] List<EliminarProveedor> proveedores)
        {
            try
            {
                if (proveedores == null || !proveedores.Any())
                {
                    return BadRequest("La lista de proveedores no puede estar vacía.");
                }

                var json = JsonSerializer.Serialize(proveedores);
                var message = await _ApplicationDbContext.Database
                    .ExecuteSqlRawAsync("EXEC Crud_Proveedores @ProveedoresData",
                        new SqlParameter("@ProveedoresData", SqlDbType.NVarChar) { Value = json });

                if (message > 0)
                {
                    return Json(new { success = true, message = "Proveedores eliminado correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudieron eliminar los proveedores." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar datos : {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult PlantillaProveedores()
        {
            // Ruta completa al archivo dentro de la carpeta wwwroot
            var rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documentos/Proveedores.xlsx");

            // Verificar si el archivo existe
            if (!System.IO.File.Exists(rutaArchivo))
            {
                return NotFound("La plantilla no se encuentra disponible.");
            }

            // Leer el archivo y devolverlo como descarga
            var archivoBytes = System.IO.File.ReadAllBytes(rutaArchivo);
            return File(archivoBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Plantilla_Proveedores.xlsx");
        }

        [HttpPost]
        public async Task<IActionResult> CargarProveedores(IFormFile archivo)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (archivo == null || archivo.Length == 0)
            {
                return BadRequest("No se ha proporcionado un archivo válido.");
            }

            try
            {
                var idClienteR = User.Claims.FirstOrDefault(c => c.Type == "IdCliente");
                if (idClienteR == null || !int.TryParse(idClienteR.Value, out int idCliente))
                {
                    return Unauthorized("No se pudo determinar el IdCliente del usuario.");
                }

                var listaProveedores = new List<Proveedor>();

                using (var stream = new MemoryStream())
                {
                    await archivo.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0]; // Primer hoja del archivo
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++) // Comienza en 2 para omitir encabezados
                        {
                            // Verifica si todas las celdas de la fila están vacías
                            bool isRowEmpty = true;
                            for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                            {
                                if (!string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Text))
                                {
                                    isRowEmpty = false;
                                    break;
                                }
                            }

                            // Si la fila está vacía, omítela
                            if (isRowEmpty)
                            {
                                continue; // Salta la iteración actual y pasa a la siguiente fila
                            }

                            var proveedor = new Proveedor
                            {
                                flag = "create", 
                                nombres = worksheet.Cells[row, 1].Text.Trim(),
                                apellidos = worksheet.Cells[row, 2].Text.Trim(),
                                personaJuridica = string.IsNullOrWhiteSpace(worksheet.Cells[row, 3].Text.Trim())? false :(worksheet.Cells[row, 3].Text.Trim() == "0" ? false :true),
                                nombreRazonSocial = worksheet.Cells[row, 4].Text.Trim(),
                                nombreComercial = worksheet.Cells[row, 5].Text.Trim(),
                                duiCliente = worksheet.Cells[row, 6].Text.Trim(),
                                representanteLegal = worksheet.Cells[row, 7].Text.Trim(),
                                duiRepresentanteLegal = worksheet.Cells[row, 8].Text.Trim(),
                                telefonoCliente = worksheet.Cells[row, 9].Text.Trim(),
                                celular = worksheet.Cells[row, 10].Text.Trim(),
                                nrc = worksheet.Cells[row, 11].Text.Trim(),
                                nitProveedor = worksheet.Cells[row, 12].Text.Trim(),
                                idCliente = idCliente
                            };

                            listaProveedores.Add(proveedor);
                        }
                    }
                }

                // Llamamos al método `CrearProveedores` directamente con la lista
                var result = await CrearProveedores(listaProveedores);

                return result; // Retorna directamente el resultado del método CrearProveedores
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar el archivo: {ex.Message}");
            }
        }

        //CRUD DE CLIENTES 
        //========================================================================
        //========================================================================
        //========================================================================

        [HttpGet]
        public async Task<IActionResult> ListarClientesClt()
        {
            try
            {
                var idClienteR = User.Claims.FirstOrDefault(c => c.Type == "IdCliente")?.Value;
                var result = await _ApplicationDbContext.Database
                    .SqlQueryRaw<listarClientes>("EXEC ListaClintesInt @idCliente",
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
        public async Task<IActionResult> CrearClientes([FromBody] List<Cliente> add)
        {
            try
            {
                if (add == null || !add.Any())
                {
                    return BadRequest("La lista de Clientes no puede estar vacía.");
                }

                var json = JsonSerializer.Serialize(add);
                var message = await _ApplicationDbContext.Database
                    .ExecuteSqlRawAsync("EXEC Crud_ClientexClt @ClienteCltxData",
                        new SqlParameter("@ClienteCltxData", SqlDbType.NVarChar) { Value = json });

                if (message > 0)
                {
                    return Json(new { success = true, message = "Clientes ingresados correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudieron ingresar los clientes." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar datos : {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarClienteInternos([FromBody] List<EditarCliente> edt)
        {
            try
            {
                if (edt == null || !edt.Any())
                {
                    return BadRequest("la clase EditarCliente no puede ir vacia.");
                }

                var json = JsonSerializer.Serialize(edt);
                var message = await _ApplicationDbContext.Database
                    .ExecuteSqlRawAsync("EXEC Crud_ClientexClt @ClienteCltxData",
                        new SqlParameter("@ClienteCltxData", SqlDbType.NVarChar) { Value = json });

                if (message > 0)
                {
                    return Json(new { success = true, message = "Cliente/s editado correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "No se puede editar Cliente/s." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar datos : {ex.Message}");
            }
        }


        [HttpDelete]
        public async Task<IActionResult> Eliminarcliente ([FromBody] List<EliminarCliente> dt)
        {
            try
            {
                if (dt == null || !dt.Any())
                {
                    return BadRequest("La lista de cliente no puede estar vacía.");
                }

                var json = JsonSerializer.Serialize(dt);
                var message = await _ApplicationDbContext.Database
                    .ExecuteSqlRawAsync("EXEC Crud_ClientexClt @ClienteCltxData",
                        new SqlParameter("@ClienteCltxData", SqlDbType.NVarChar) { Value = json });

                if (message > 0)
                {
                    return Json(new { success = true, message = "Clientes eliminado correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudieron eliminar cliente o Clientes." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar datos : {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult PlantillaClientes()
        {
            var rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documentos/Plantilla_CLientes.xlsx");
            if (!System.IO.File.Exists(rutaArchivo)){return NotFound("La plantilla no se encuentra disponible.");}
            var archivoBytes = System.IO.File.ReadAllBytes(rutaArchivo);
            return File(archivoBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Plantilla_CLientes.xlsx");
        }

        [HttpPost]
        public async Task<IActionResult> CargarClientes(IFormFile archivo)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (archivo == null || archivo.Length == 0)
            {
                return BadRequest("No se ha proporcionado un archivo válido.");
            }

            try
            {
                var idClienteR = User.Claims.FirstOrDefault(c => c.Type == "IdCliente");
                if (idClienteR == null || !int.TryParse(idClienteR.Value, out int idCliente))
                {
                    return Unauthorized("No se pudo determinar el IdCliente del usuario.");
                }

                var listaClientes = new List<Cliente>();

                using (var stream = new MemoryStream())
                {
                    await archivo.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0]; // Primer hoja del archivo
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++) // Comienza en 2 para omitir encabezados
                        {
                            // Verifica si todas las celdas de la fila están vacías
                            bool isRowEmpty = true;
                            for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                            {
                                if (!string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Text))
                                {
                                    isRowEmpty = false;
                                    break;
                                }
                            }

                            // Si la fila está vacía, omítela
                            if (isRowEmpty)
                            {
                                continue; // Salta la iteración actual y pasa a la siguiente fila
                            }

                            var cliente = new Cliente
                            {
                                flag = "create",
                                nombres = worksheet.Cells[row, 1].Text.Trim(),
                                apellidos = worksheet.Cells[row, 2].Text.Trim(),
                                personaJuridica = string.IsNullOrWhiteSpace(worksheet.Cells[row, 3].Text.Trim()) ? false : (worksheet.Cells[row, 3].Text.Trim() == "0" ? false : true),
                                nombreRazonSocial = worksheet.Cells[row, 4].Text.Trim(),
                                nombreComercial = worksheet.Cells[row, 5].Text.Trim(),
                                duiCliente = worksheet.Cells[row, 6].Text.Trim(),
                                representanteLegal = worksheet.Cells[row, 7].Text.Trim(),
                                duiRepresentanteLegal = worksheet.Cells[row, 8].Text.Trim(),
                                telefonoCliente = worksheet.Cells[row, 9].Text.Trim(),
                                celular = worksheet.Cells[row, 10].Text.Trim(),
                                nrc = worksheet.Cells[row, 11].Text.Trim(),
                                nitCliente = worksheet.Cells[row, 12].Text.Trim(),
                                idCliente = idCliente
                            };

                            listaClientes.Add(cliente);
                        }
                    }
                }

                // Llamamos al método `CrearProveedores` directamente con la lista
                var result = await CrearClientes(listaClientes);

                return result; // Retorna directamente el resultado del método CrearProveedores
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar el archivo: {ex.Message}");
            }
        }
    }
}
