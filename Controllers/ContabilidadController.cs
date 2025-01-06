using contabsv.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace contabsv.Controllers
{
    public class ContabilidadController : Controller
    {
        private readonly ApplicationDbContext _ApplicationDbContext;

        public ContabilidadController(ApplicationDbContext applicationDbContext)
        {
            _ApplicationDbContext = applicationDbContext;
        }

        [Authorize]
        public IActionResult Compras()
        {
            var idCliente = User.Claims.FirstOrDefault(c => c.Type == "IdCliente")?.Value;
            ViewData["IdCliente"] = idCliente;

            if (!User.HasPermission("Compras"))
            {
                return RedirectToAction("Privacy", "Home");
            }
            return View();

        }

        [Authorize]
        public IActionResult ListaCompras()
        {
            var idCliente = User.Claims.FirstOrDefault(c => c.Type == "IdCliente")?.Value;
            ViewData["IdCliente"] = idCliente;

            if (!User.HasPermission("ListaCompras"))
            {
                return RedirectToAction("Privacy", "Home");
            }
            return View();

        }
    }
}
