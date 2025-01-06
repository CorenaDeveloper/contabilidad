using System.Security.Claims;
using System.Text.Json;

namespace contabsv.Services
{
    public static class PermissionChecker
    {
        public static bool HasPermission(this ClaimsPrincipal user, string descripcion)
        {
            var permisosJson = user.Claims.FirstOrDefault(c => c.Type == "Permisos")?.Value;

            if (string.IsNullOrEmpty(permisosJson))
                return false;

            var permisos = JsonSerializer.Deserialize<List<contabsv.Models.ModuloesModel>>(permisosJson);
            return permisos.Any(p => p.descripcion == descripcion && p.activo);
        }
    }
}
