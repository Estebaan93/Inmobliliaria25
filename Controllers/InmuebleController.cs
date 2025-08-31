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

        //Vista principal
        public IActionResult Index()
        {
            var viewModel = new InmuebleViewModel
            {
                Inmuebles = new List<Inmueble>(), // tabla empieza vacía, se llena con AJAX
                Propietarios = _repoPropietario.ObtenerActivos()
            };

            return View(viewModel);
        }

        // Endpoint AJAX para los filtros
        [HttpGet]
public IActionResult Filtrar(int? estado = null, int? idPropietario = null)
{
    var inmuebles = _repoInmueble.Listar(estado, idPropietario);
    return Json(inmuebles);
}

    }
}
