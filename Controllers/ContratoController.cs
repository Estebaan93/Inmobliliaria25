using Inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria25.Controllers
{
  public class ContratoController : Controller
  {
    private readonly RepositorioContrato _repoContrato;
    private readonly RepositorioInquilino _repoInquilino;
    private readonly RepositorioInmueble _repoInmueble;

    public ContratoController(
        RepositorioContrato repoContrato,
        RepositorioInquilino repoInquilino,
        RepositorioInmueble repoInmueble)
    {
      _repoContrato = repoContrato;
      _repoInquilino = repoInquilino;
      _repoInmueble = repoInmueble;
    }


    // listar

    public IActionResult Index()
    {
      var contratos = _repoContrato.Listar();
      return View(contratos);
    }

    // crear

    public IActionResult Crear()
    {
      var vm = new ContratoViewModel
      {
        Contrato = new Contrato(),
        Inquilinos = _repoInquilino.ObtenerActivos(),
        Inmuebles = _repoInmueble.ListarDisponible()
      };
      return View(vm);
    }

    [HttpPost]
    public IActionResult Crear(ContratoViewModel vm)
    {
      var contrato = vm.Contrato;

      Console.WriteLine("====== DEBUG CONTRATO ======");
      Console.WriteLine($"Contrato: {contrato.IdContrato}");
      Console.WriteLine($"Inquilino: {contrato.IdInquilino}");
      Console.WriteLine($"Inmueble: {contrato.IdInmueble}");
      Console.WriteLine($"Monto: {contrato.Monto}");
      Console.WriteLine($"FechaInicio: {contrato.FechaInicio}");
      Console.WriteLine($"FechaFin: {contrato.FechaFin}");
      Console.WriteLine($"FechaAnulacion: {contrato.FechaAnulacion}");
      Console.WriteLine("============================");

      if (contrato.FechaInicio < DateTime.Today)
      {
        ModelState.AddModelError("Contrato.FechaInicio", "La fecha de inicio no puede ser anterior a hoy.");
      }

      if (!ModelState.IsValid)
      {
        vm.Inquilinos = _repoInquilino.ObtenerActivos();
        vm.Inmuebles = _repoInmueble.ListarDisponible();
        return View(vm);
      }

      contrato.Estado = true;
      contrato.FechaAnulacion = null;

      int id = _repoContrato.Crear(contrato);
      Console.WriteLine($"Contrato insertado con ID {id}");

      return RedirectToAction("Index");
    }



    // editar

    public IActionResult Editar(int id)
    {
      var contrato = _repoContrato.Obtener(id);
      if (contrato == null) return NotFound();

      var vm = new ContratoViewModel
      {
        Contrato = contrato,
        Inquilinos = _repoInquilino.ObtenerActivos(),
        Inmuebles = _repoInmueble.Listar()
      };
      return View(vm);
    }

    [HttpPost]
    public IActionResult Editar(ContratoViewModel vm)
    {
      if (ModelState.IsValid)
      {
        _repoContrato.Modificar(vm.Contrato);
        return RedirectToAction("Index");
      }

      vm.Inquilinos = _repoInquilino.ObtenerActivos();
      vm.Inmuebles = _repoInmueble.Listar();
      return View(vm);
    }


    // detalles

    public IActionResult Detalles(int id)
    {
      var contrato = _repoContrato.Obtener(id);
      if (contrato == null) return NotFound();
      return View(contrato);
    }


    // elimino

    [HttpPost]
    public IActionResult Borrar(int id)
    {
      _repoContrato.Eliminar(id);
      return RedirectToAction("Index");
    }


    // JSON para ajax para que me arme mas bonito (propietario + precio)

    [HttpGet]
    public IActionResult ObtenerInmueble(int id)
    {
      var inmueble = _repoInmueble.Obtener(id);
      if (inmueble == null) return NotFound();

      return Json(new
      {
        propietario = inmueble.propietario?.Apellido + " " + inmueble.propietario?.Nombre,
        precio = inmueble.Precio
      });
    }
  }
}
