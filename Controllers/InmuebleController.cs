using Inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria25.Controllers
{
  public class InmuebleController : Controller
  {
    private readonly RepositorioInmueble _repoInmueble;
    private readonly RepositorioPropietario _repoPropietario;
    private readonly RepositorioTipo _repoTipo;
    private readonly RepositorioDireccion _repoDireccion;

    public InmuebleController(
        RepositorioInmueble repoInmueble,
        RepositorioPropietario repoPropietario,
        RepositorioTipo repoTipo,
        RepositorioDireccion repoDireccion)
    {
      _repoInmueble = repoInmueble;
      _repoPropietario = repoPropietario;
      _repoTipo = repoTipo;
      _repoDireccion = repoDireccion;
    }

    // vista principal
    public IActionResult Index()
    {
      var viewModel = new InmuebleViewModel
      {
        Inmuebles = _repoInmueble.Listar(),
        Propietarios = _repoPropietario.ObtenerActivos()
      };

      return View(viewModel);
    }

    // endpoint AJAX para filtros
    [HttpGet]
    public IActionResult Filtrar(int? Estado = null, int? IdPropietario = null)
    {
      var inmuebles = _repoInmueble.Listar(Estado, IdPropietario);
      return Json(inmuebles);
    }

    // crear (GET)
    public IActionResult Crear()
    {
      var vm = new InmuebleFormViewModel
      {
        Inmueble = new Inmueble(),
        Propietarios = _repoPropietario.ObtenerActivos() ?? new List<Propietarios>(),
        Tipos = _repoTipo.Listar() ?? new List<Tipo>()
      };

      return View(vm);
    }

    [HttpPost]
    public IActionResult Crear(InmuebleFormViewModel vm)
    {
      if (ModelState.IsValid)
      {
        // crear la direccion y obtener ID
        int idDireccion = _repoDireccion.Crear(vm.Direccion);

        // asignar al inmueble
        vm.Inmueble.IdDireccion = idDireccion;

        // guardar inmueble
        _repoInmueble.Crear(vm.Inmueble);

        return RedirectToAction("Index");
      }

      // recargar listas si hay error
      vm.Propietarios = _repoPropietario.ObtenerActivos() ?? new List<Propietarios>();
      vm.Tipos = _repoTipo.Listar() ?? new List<Tipo>();

      return View(vm);
    }


    //editar (GET)
    public IActionResult Editar(int id)
    {
      var inmueble = _repoInmueble.Obtener(id);
      if (inmueble == null)
        return NotFound();

      var vm = new InmuebleFormViewModel
      {
        Inmueble = inmueble,
        Direccion = inmueble.Direccion,
        Propietarios = _repoPropietario.ObtenerActivos(),
        Tipos = _repoTipo.Listar()
      };

      return View(vm);
    }


    //editar (POST)

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(InmuebleFormViewModel vm)
    {
      if (ModelState.IsValid)
      {
        try
        {
          //actualizar direccion
          _repoDireccion.Modificar(vm.Direccion);

          //aseguramos que el idDireccion quede en el inmueble
          vm.Inmueble.IdDireccion = vm.Direccion.IdDireccion;

          //actualizar inmueble
          _repoInmueble.Modificar(vm.Inmueble);

          TempData["Mensaje"] = "Inmueble modificado con éxito.";
          return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
          ModelState.AddModelError("", "Error al modificar el inmueble: " + ex.Message);
        }
      }

      // recargar listas si hay error
      vm.Propietarios = _repoPropietario.ObtenerActivos();
      vm.Tipos = _repoTipo.Listar();

      return View(vm);
    }


    //eliminar (AJAX con SweetAlert)
    [HttpPost]
    public IActionResult Borrar(int id)
    {
      try
      {
        _repoInmueble.Eliminar(id);
        return Json(new { success = true, mensaje = "Inmueble eliminado correctamente" });
      }
      catch (Exception ex)
      {
        return Json(new { success = false, mensaje = "Error al eliminar el inmueble: " + ex.Message });
      }
    }
  }
}
