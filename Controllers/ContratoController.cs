//Controllers/ContratoController.cs
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
    private readonly RepositorioPago _repoPago;

    public ContratoController(
        RepositorioContrato repoContrato,
        RepositorioInquilino repoInquilino,
        RepositorioInmueble repoInmueble,
        RepositorioPago repoPago)
    {
      _repoContrato = repoContrato;
      _repoInquilino = repoInquilino;
      _repoInmueble = repoInmueble;
      _repoPago = repoPago;
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

      // Validar superposición de fechas
      if (_repoContrato.ExisteContratoSuperpuesto(contrato.IdInmueble, contrato.FechaInicio, contrato.FechaFin))
      {
        ModelState.AddModelError("Contrato.IdInmueble", "El inmueble ya está alquilado en el período seleccionado.");
        vm.Inquilinos = _repoInquilino.ObtenerActivos();
        vm.Inmuebles = _repoInmueble.ListarDisponible();
        return View(vm);
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

    /*public IActionResult Detalles(int id)
    {
      var contrato = _repoContrato.Obtener(id);
      if (contrato == null) return NotFound();
      return View(contrato);
    } */


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

    public IActionResult Detalles(int id)
    {
      var contrato = _repoContrato.Obtener(id);
      if (contrato == null) return NotFound();

      // monto mensual
      decimal montoMensual = (decimal)contrato.Monto;

      // deuda hasta la fecha
      decimal deuda = _repoPago.CalcularDeuda(contrato.IdContrato, (double)montoMensual, contrato.FechaInicio);

      // multa
      int diasTotales = (contrato.FechaFin - contrato.FechaInicio).Days;
      int diasTranscurridos = (DateTime.Today - contrato.FechaInicio).Days;

      decimal multa = (diasTranscurridos < diasTotales / 2) ? montoMensual * 2 : montoMensual;

      decimal totalMulta = deuda + multa;

      // paso los datos
      ViewBag.Multa = multa;
      ViewBag.Deuda = deuda;
      ViewBag.TotalMulta = totalMulta;

      return View(contrato);
    }

    [HttpPost]
    public IActionResult Finalizar(int id)
    {
      var contrato = _repoContrato.Obtener(id);
      if (contrato == null) return NotFound();

      //El monto del contrato es mensual
      decimal montoMensual = (decimal)contrato.Monto;

      //Calcular deuda de alquiler hasta hoy
      decimal deuda = _repoPago.CalcularDeuda(id, (double)montoMensual, contrato.FechaInicio);

      //Calcular multa
      int diasTotales = (contrato.FechaFin - contrato.FechaInicio).Days;
      int diasTranscurridos = (DateTime.Today - contrato.FechaInicio).Days;

      decimal multa = diasTranscurridos < (diasTotales / 2)
          ? montoMensual * 2   // menos de la mitad
          : montoMensual;      // más de la mitad

      //Total
      decimal totalMulta = deuda + multa;

      //Anular contrato
      _repoContrato.AnularContrato(id);

      //Registrar pago como pendiente
      var pago = new Pago
      {
        IdContrato = id,
        FechaPago = DateTime.Today,
        Importe = totalMulta,
        NumeroPago = "Multa",
        Detalle = $"Multa por contrato anulado (incluye deuda: {deuda})",
        Estado = false
      };
      _repoPago.CrearPagoSinValidacion(pago);

      TempData["Mensaje"] = $"Contrato N° {id} anulado. Multa: ${multa}, deuda: ${deuda}, total: ${totalMulta}.";
      return RedirectToAction("Detalles", new { id });
    }
  }
}
