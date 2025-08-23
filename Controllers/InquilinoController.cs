//Controllers/InquilinosController.cs
using inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria25.Controllers
{
		//Define la clase que hereda de controller
	public class InquilinoController : Controller
	{
		//var privada y lectura, permite que el controller use los metodos del repo
		private readonly RepositorioInquilino repo;

		//Constructor del controlador asp.net inyecta dependencias, pasa automaticamente objeto al repo y se guarda.
		//En la var this.repo para ser usado por todos los metodos
		public InquilinoController(RepositorioInquilino repo)
		{
			this.repo = repo;
		}

		// Listar activos
		public IActionResult Index()
		{
			//Llama al metodo, este consulta con sql devuelve una lista y guarda en la var lista
			var lista = repo.ObtenerActivos();
			return View(lista);
		}

		// Crear (GET)
		public IActionResult Crear()
		{
			return View();
		}

		[HttpPost]
		//Al hacer click recibe un objeto Inquilino inquilino 
		public IActionResult Guardar(Inquilinos inquilino)
		{
			try
			{
				//Pregunta inquilino tiene id > 0?, si es si queire decir que ya existe, si es no es nueva.
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
		//Este es cuando se hace click y consulta
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
		//Este cuando ya edita en el formulario, click en guardar
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