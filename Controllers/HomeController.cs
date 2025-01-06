using contabsv.Models;
using contabsv.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;

namespace contabsv.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _ApplicationDbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _ApplicationDbContext = applicationDbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        

        
        [Authorize]
        public IActionResult Ventas()
        {
            if (!User.HasPermission("Ventas"))
            {
                return RedirectToAction("Privacy", "Home");
            }
            return View();
        }

        [Authorize]
        public IActionResult Perfil()
        {
            var idCliente = User.Claims.FirstOrDefault(c => c.Type == "IdCliente")?.Value;
            ViewData["IdCliente"] = idCliente;

            if (!User.HasPermission("Perfil"))
            {
                return RedirectToAction("Privacy", "Home");
            }
            return View();

        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> ConfiguracionCliente()
        {
            try
            {
                var idClienteR = User.Claims.FirstOrDefault(c => c.Type == "IdCliente")?.Value;
                var result = await _ApplicationDbContext.Database
                    .SqlQueryRaw<ConfiguracionClienteModel>("EXEC DataCliente @idCliente",
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
    }
}
