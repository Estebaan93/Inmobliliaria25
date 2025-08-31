using Inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria25.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly RepositorioInmueble _repoInmueble;
        private readonly RepositorioPropietario _repoPropietario;

        public InmuebleController(RepositorioInmueble repoInmueble, RepositorioPropietario repoPropietario)
        {
            _repoInmueble = repoInmueble;
            _repoPropietario = repoPropietario;
        }

        public IActionResult Index()
        {
            var vm = new InmuebleViewModel
            {
                Inmuebles = _repoInmueble.Listar(),
                Propietarios = _repoPropietario.ObtenerActivos()
            };
            return View(vm);
        }
    }
}
