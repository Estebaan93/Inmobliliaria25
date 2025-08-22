//Controllers/InquilinosController.cs
using inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria25.Controllers
{
	public class InquilinoController : Controller
	{
		private readonly RepositorioInquilino repo;

		public InquilinoController(RepositorioInquilino repo)
		{
			this.repo = repo;
		}

		// Listar activos
		public IActionResult Index()
		{
			var lista = repo.ObtenerActivos();
			return View(lista);
		}

		// Crear (GET)
		public IActionResult Crear()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Guardar(Inquilinos inquilino)
		{
			try
			{
				if (inquilino.idInquilino > 0)
				{
					// EDITAR
					if (repo.ObtenerPorDni(inquilino.dni, inquilino.idInquilino))
					{
						ModelState.AddModelError("dni", "El DNI ya está registrado en otro inquilino.");
						return View("Editar", inquilino);
					}

					repo.Modificar(inquilino);
					TempData["Mensaje"] = "Inquilino modificado con éxito";
				}
				else
				{
					// CREAR
					if (repo.ObtenerPorDni(inquilino.dni))
					{
						ModelState.AddModelError("dni", "El DNI ya está registrado. Revise la tabla de propietarios.");
						return View("Crear", inquilino);
					}

					repo.Alta(inquilino);
					TempData["Mensaje"] = "Propietario creado con éxito";
				}

				return RedirectToAction("Index");

			}
			catch (Exception ex)
			{
				TempData["Error"] = ex.Message;
				return RedirectToAction("Index");
			}
		}

		// Editar (GET)
		public IActionResult Editar(int id)
		{
			var inquilino = repo.ObtenerPorId(id);
			if (inquilino == null)
			{
				return NotFound();
			}
			return View(inquilino);
		}

		// Editar (POST)
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Editar(Inquilinos inquilino)
		{
			if (!ModelState.IsValid)
			{
				return View(inquilino);
			}

			repo.Modificar(inquilino);
			TempData["Success"] = "Inquilino modificado correctamente.";
			return RedirectToAction(nameof(Index));
		}

		// Baja lógica
		[HttpPost]
		public IActionResult Borrar(int id)
		{
			repo.Baja(id);
			TempData["Success"] = "Inquilino eliminado correctamente.";
			return RedirectToAction(nameof(Index));
		}

		// Listar dados de baja (para el admin)
		public IActionResult DadosDeBaja()
		{
			var lista = repo.ObtenerDadosDeBaja();
			return View(lista);
		}
	}
}