//Controllers/HomeController.cs
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NETmvc2108.Models;
using Inmobiliaria25.Repositorios;
using Inmobiliaria25.Models;
using Microsoft.AspNetCore.Authorization;

namespace NETmvc2108.Controllers;

[Authorize] //todas las acciones requieren autenticacion
public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;
  private readonly RepositorioInmueble _repoInmueble;
  private readonly RepositorioTipo _repoTipo;

  public HomeController(ILogger<HomeController> logger, RepositorioInmueble repoInmueble, RepositorioTipo repoTipo)
  {
    _logger = logger;
    _repoInmueble = repoInmueble;
    _repoTipo = repoTipo;
  }

  public IActionResult Index()
  {
    var vm = new InmuebleViewModel
    {
      Inmuebles = _repoInmueble.ListarDisponible(),
      Propietarios = new List<Propietarios>()
    };

    ViewBag.Tipos = _repoTipo.Listar();
    return View(vm);
  }

  [HttpGet]
  public IActionResult GetInmueblesFiltrados(
      int? idTipo = null,
      int? ambientes = null,
      bool? cochera = null,
      string? ordenPrecio = null)
  {
    var inmuebles = _repoInmueble.ListarDisponible();

    if (idTipo.HasValue)
      inmuebles = inmuebles.Where(i => i.IdTipo == idTipo.Value).ToList();

    if (ambientes.HasValue)
      inmuebles = inmuebles.Where(i => i.CantidadAmbientes == ambientes.Value).ToList();

    if (cochera.HasValue)
      inmuebles = inmuebles.Where(i => i.Cochera == cochera.Value).ToList();

    if (!string.IsNullOrEmpty(ordenPrecio))
    {
      if (ordenPrecio == "asc")
        inmuebles = inmuebles.OrderBy(i => i.Precio).ToList();
      else if (ordenPrecio == "desc")
        inmuebles = inmuebles.OrderByDescending(i => i.Precio).ToList();
    }

    return Json(inmuebles);
  }

  public IActionResult Privacy()
  {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
