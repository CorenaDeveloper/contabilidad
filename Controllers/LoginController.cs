using contabsv.Models;
using Microsoft.AspNetCore.Mvc;
using ApiSmartGarden.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace contabsv.Controllers
{
    public class LoginController : Controller
    {

        private readonly ApplicationDbContext _ApplicationDbContext;
        private readonly HashPasswordSHA256 _hashPasswordSHA256;

        public LoginController(ApplicationDbContext applicationDbContext, HashPasswordSHA256 hashPasswordSHA256)
        {
            _ApplicationDbContext = applicationDbContext;
            _hashPasswordSHA256 = hashPasswordSHA256;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home"); 
            }

            return View();
        }
          
        public IActionResult RegistrarUsuario()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Session(string UsuarioEmail, string Password)
        {
            try
            {
                string hashedPassword = _hashPasswordSHA256.HashPassword(Password);

                var result = await _ApplicationDbContext.Database
                    .SqlQueryRaw<LoginUsuariosModel>("EXEC Login @UsuarioEmail, @Password",
                    new SqlParameter("@UsuarioEmail", SqlDbType.NVarChar) { Value = UsuarioEmail },
                    new SqlParameter("@Password", SqlDbType.NVarChar) { Value = hashedPassword }
                    )
                    .ToListAsync();

                var user = result.FirstOrDefault();

                if (user.IdUsuario > 0) {

                    // Consultar los permisos del usuario
                    var permisos = await _ApplicationDbContext.Database
                        .SqlQueryRaw<ModuloesModel>("EXEC ListaPermisosxUsuario @idUduario",
                        new SqlParameter("@idUduario", SqlDbType.Int) { Value = user.IdUsuario })
                        .ToListAsync();

                    // Crear claims para el usuario
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Name, user.Nombre),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim("Usuario", user.Usuario),
                    new Claim("Permisos", Newtonsoft.Json.JsonConvert.SerializeObject(permisos)),
                    new Claim("IdCliente", user.idCliente.ToString())
                };

                    // Crear identidad del usuario
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Iniciar sesión con cookies
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return Ok(result);
                }
                else
                {
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la lista: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuariosModel add)
        {
            try
            {
                if (add == null)
                {
                    return BadRequest("El JSON del producto no puede estar vacío.");
                }

                // Hashear la contraseña antes de enviarla
                string hashedPassword = _hashPasswordSHA256.HashPassword(add.Password);

                var data = new
                {
                    flag = "create",
                    Nombre = add.Nombre,
                    Apellido = add.Apellido,
                    Email = add.Email,
                    Usuario = add.Usuario,
                    Password = hashedPassword
                };

                var json = JsonSerializer.Serialize(data);

                var message = await _ApplicationDbContext.Database
                    .ExecuteSqlRawAsync("EXEC ManageUsuario @UsuarioData",
                        new SqlParameter("@UsuarioData", SqlDbType.NVarChar) { Value = json });

                return Ok(new { Message = message });

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error al registrar datos : {ex.Message}");
            }
        }

       
    }
}
