//Controllers/PagoController.cs
using System.Security.Claims;
using System.Reflection;
using Inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Inmobiliaria25.Controllers
{
	[Authorize] //todas las acciones requieren autenticacion
	public class PagoController : Controller
	{
		private readonly RepositorioPago _repoPago;
		private readonly RepositorioContrato _repoContrato;
		private readonly RepositorioAuditoria _repoAuditoria;

		public PagoController(RepositorioPago repoPago, RepositorioContrato repoContrato, RepositorioAuditoria repoAuditoria)
		{
			_repoPago = repoPago;
			_repoContrato = repoContrato;
			_repoAuditoria = repoAuditoria;
		}

        public IActionResult Index(int idContrato)
        {
            var contrato = _repoContrato.Obtener(idContrato);
            if (contrato == null) return RedirectToAction("Index", "Contrato");

            // determinar si se permite crear nuevos pagos (contrato no anulado)
            ViewBag.PermiteNuevoPago = !ContratoEstaAnulado(contrato);

            var pagos = _repoPago.ListarPorContrato(idContrato);

            var vm = new PagoContrato
            {
                Contrato = contrato,
                Pagos = pagos
            };

            return View(vm);
        }


		// detalle pago
		public IActionResult Detalle(int id)
		{
			var pago = _repoPago.Obtener(id);
			if (pago == null) return RedirectToAction("Index", "Contrato");

			return View(pago);
		}

		    // GET: Crear nuevo pago
        public IActionResult Crear(int idContrato)
        {
            var contrato = _repoContrato.Obtener(idContrato);
            if (contrato == null) return RedirectToAction("Index", "Contrato");

            // bloquear acceso si contrato anulado
            if (ContratoEstaAnulado(contrato))
            {
                TempData["Error"] = "No puede registrar pagos: el contrato está anulado.";
                return RedirectToAction("Index", new { idContrato });
            }

			// preparar pago (vista espera un pago)
						var pago = new Pago { IdContrato = idContrato, FechaPago = DateTime.Now, NumeroPago = _repoPago.GenerarNumeroPago(idContrato), Importe = (decimal)contrato.Monto };
 
				ViewBag.Contrato = contrato; // pasar contrato a la vista
			return View(pago);
        }

			  // Helper: detecta si un Contrato esta anulado (intenta varias propiedades comunes)
        private bool ContratoEstaAnulado(Contrato contrato)
        {
            if (contrato == null) return true;

            var type = contrato.GetType();

            // 1) Propiedad bool Anulado
            var prop = type.GetProperty("Anulado", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop != null && prop.PropertyType == typeof(bool))
            {
                return (bool)(prop.GetValue(contrato) ?? false);
            }

            // 2) Propiedad string Estado == "Anulado"
            prop = type.GetProperty("Estado", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop != null)
            {
                var val = prop.GetValue(contrato);
                if (val is string s) return s.Equals("Anulado", StringComparison.OrdinalIgnoreCase);
                if (val is bool b) return !b; // si Estado es bool (activo/inactivo) -> inactivo = anulado
            }

            // 3) Propiedad bool Activo (false => anulado)
            prop = type.GetProperty("Activo", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop != null && prop.PropertyType == typeof(bool))
            {
                return !(bool)(prop.GetValue(contrato) ?? true);
            }

            // por defecto: no esta anulado
            return false;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Pago pago)
        {
            // Si el model es invalido, volver a mostrar la vista con el Pago y el Contrato en ViewBag
            if (!ModelState.IsValid)
            {
                ViewBag.Contrato = _repoContrato.Obtener(pago.IdContrato);
                return View(pago);
            }

            try
            {
                // garantizar numero secuencial (no confiar en entrada del cliente)
                pago.NumeroPago = _repoPago.GenerarNumeroPago(pago.IdContrato);

                if (pago.IdPago > 0)
                {
                    // actualizar el registro de multa original con el nmero y marcar como pagado
                    pago.Estado = true;
                    _repoPago.ActualizarPagoCompleto(pago.IdPago, pago);
                }
                else
                {
                    // crear nuevo pago normal
                    pago.Estado = true;
                    _repoPago.Crear(pago);
                }
                //Registrar auditoria en creacion de pago
                var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("IdUsuario")?.Value;
                int idUsuario = int.TryParse(claim, out var tmp) ? tmp : 0;
                _repoAuditoria.RegistrarAuditoria(pago.IdPago, TipoEntidad.pago, AccionAuditoria.crear, idUsuario, $"Pago registrado (Contrato N° {pago.IdContrato}, pago N° {pago.NumeroPago})");

                TempData["Exito"] = "Pago registrado correctamente.";
                return RedirectToAction("Index", new { idContrato = pago.IdContrato });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al registrar pago: " + ex.Message;
                ViewBag.Contrato = _repoContrato.Obtener(pago.IdContrato);
                return View(pago);
            }
        }



		// editar
		public IActionResult Editar(int id)
		{
			var pago = _repoPago.Obtener(id);
			if (pago == null) return RedirectToAction("Index", "Contrato");

			return View(pago);
		}


		[HttpPost]
		public IActionResult Editar([FromBody] Pago pago)
		{
			if (pago == null || pago.IdPago <= 0 || string.IsNullOrWhiteSpace(pago.Detalle))
				return BadRequest("Datos inválidos");

			try
			{
				_repoPago.Modificar(pago); // solo actualiza el campo Detalle
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}



		//  elimino logico
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Eliminar(int id, int idContrato)
		        {
            try
            {
                _repoPago.Eliminar(id);

                // Registrar auditoria: anulacion/eliminacion de pago
                var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("IdUsuario")?.Value;
                int idUsuario = int.TryParse(claim, out var tmp) ? tmp : 0;
                _repoAuditoria.RegistrarAuditoria(id, TipoEntidad.pago, AccionAuditoria.anular, idUsuario, $"Pago eliminado (N° {id})");

                TempData["Exito"] = "Pago eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar el pago: " + ex.Message;
            }

            return RedirectToAction("Index", new { idContrato });
        }


		
	}
}
