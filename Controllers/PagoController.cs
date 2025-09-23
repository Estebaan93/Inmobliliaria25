//Controllers/PagoController.cs
using System.Security.Claims;
using Inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Inmobiliaria25.Controllers
{
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

		// lista pagos
		public IActionResult Index(int idContrato)
		{
			var contrato = _repoContrato.Obtener(idContrato);
			if (contrato == null) return RedirectToAction("Index", "Contrato");

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

		// creo
		public IActionResult Crear(int idContrato)
		{
			var contrato = _repoContrato.Obtener(idContrato);
			if (contrato == null) return RedirectToAction("Index", "Contrato");

			var pagos = _repoPago.ListarPorContrato(idContrato);
			var multaPendiente = pagos.FirstOrDefault(p => p.NumeroPago == "Multa" && !p.Estado);

			// siempre mostrar un número secuencial en el formulario
			var numeroGenerado = _repoPago.GenerarNumeroPago(idContrato);

			var pago = new Pago
			{
				IdContrato = idContrato,
				Contrato = contrato,
				FechaPago = DateTime.Today,
				Importe = multaPendiente?.Importe ?? 0,
				NumeroPago = numeroGenerado,                // mostrado en readonly
				Detalle = multaPendiente?.Detalle ?? "",
				IdPago = multaPendiente?.IdPago ?? 0        // si existe multa, la actualizaremos en el POST
			};

			return View(pago);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Crear(Pago pago)
		{
			if (!ModelState.IsValid) return View(pago);

			try
			{
				// garantizar número secuencial (no confiar en entrada del cliente)
				pago.NumeroPago = _repoPago.GenerarNumeroPago(pago.IdContrato);

				if (pago.IdPago > 0)
				{
					// actualizar el registro de multa original con el número y marcar como pagado
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
				_repoAuditoria.RegistrarAuditoria(pago.IdPago, TipoEntidad.pago, AccionAuditoria.crear, idUsuario, $"Pago registrado (Contrato {pago.IdContrato}, N° {pago.NumeroPago})");

				TempData["Exito"] = "Pago registrado correctamente.";
				return RedirectToAction("Index", new { idContrato = pago.IdContrato });
			}
			catch (Exception ex)
			{
				TempData["Error"] = "Error al registrar pago: " + ex.Message;
				return View(pago);
			}
		}

		/*
		// post crear
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Crear(Pago pago)
		{
			if (!ModelState.IsValid)
				return View(pago);

			try
			{
				_repoPago.Crear(pago);
				TempData["Exito"] = "Pago creado correctamente.";
				return RedirectToAction("Index", new { idContrato = pago.IdContrato });
			}
			catch (Exception ex)
			{
				TempData["Error"] = "Error al crear el pago: " + ex.Message;
				return View(pago);
			}
		}
*/



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

                // Registrar auditoría: anulación/eliminación de pago
                var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("IdUsuario")?.Value;
                int idUsuario = int.TryParse(claim, out var tmp) ? tmp : 0;
                _repoAuditoria.RegistrarAuditoria(id, TipoEntidad.pago, AccionAuditoria.anular, idUsuario, $"Pago eliminado (ID {id})");

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
