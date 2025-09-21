//Controllers/InmuebleCOntroller.cs
using Inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

		//Listar activos paginado
		/* *PARA IMPLEMENTAR PAGINADO, PERO HAY QUE TOCAR EL JS*
public IActionResult Index(int page = 1)
		{
			const int pageSize = 5;

			if (page < 1) page = 1;

			int total = _repoInmueble.ContarActivos();
			int totalPages = (int)Math.Ceiling(total / (double)pageSize);
			if (totalPages == 0) totalPages = 1;        // evita dividir por cero cuando no hay datos
			if (page > totalPages) page = totalPages;   // clamp a último disponible

			var lista = _repoInmueble.ObtenerActivosPaginado(page, pageSize);

			var viewModel = new InmuebleViewModel
			{
				Inmuebles = lista,
				Propietarios = _repoPropietario.ObtenerActivos() ?? new List<Propietarios>(), // si usás filtro por propietario
				CurrentPage = page,
				TotalPages = totalPages,
				TotalItems = total,
				PageSize = pageSize
			};
			ViewBag.CurrentPage = page;
			ViewBag.TotalPages = totalPages;
			ViewBag.TotalItems = total;
			ViewBag.PageSize = pageSize;

			return View(viewModel);
		}*/

		public IActionResult Index()
		{
			var viewModel = new InmuebleViewModel
			{
				Inmuebles = _repoInmueble.Listar(), // si querés mostrar de una
				Propietarios = _repoPropietario.ObtenerActivos()
			};

			return View(viewModel);
		}

		// filtros ajax
		[HttpGet]
		public IActionResult Filtrar(int? estado = null, int? idPropietario = null)
		{
			var inmuebles = _repoInmueble.Listar(estado, idPropietario);
			return Json(inmuebles);
		}

		// crear
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

				int idDireccion = _repoDireccion.Crear(vm.Direccion);


				vm.Inmueble.IdDireccion = idDireccion;

				_repoInmueble.Crear(vm.Inmueble);

				return RedirectToAction("Index");
			}


			vm.Propietarios = _repoPropietario.ObtenerActivos() ?? new List<Propietarios>();
			vm.Tipos = _repoTipo.Listar() ?? new List<Tipo>();

			return View(vm);
		}


		// editar get
		public IActionResult Editar(int id)
		{
			var inmueble = _repoInmueble.Obtener(id);
			if (inmueble == null)
				return NotFound();

			var vm = new InmuebleFormViewModel
			{
				Inmueble = inmueble,
				Direccion = inmueble.direccion,
				Propietarios = _repoPropietario.ObtenerActivos(),
				Tipos = _repoTipo.Listar()
			};

			return View(vm);
		}


		// editar

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Editar(InmuebleFormViewModel vm)
		{
			if (ModelState.IsValid)
			{
				try
				{

					_repoDireccion.Modificar(vm.Direccion);

					vm.Inmueble.IdDireccion = vm.Direccion.IdDireccion;

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

		//Detalles
		public IActionResult Detalle(int id)
		{
			var inmueble = _repoInmueble.Obtener(id);
			if (inmueble == null)
				return NotFound();

			return View(inmueble);
		}


		// elimino con (AJAX con SweetAlert)
		[HttpPost]
		[Authorize(Roles = "Administrador")] // solo admin puede borrar
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

		[HttpGet]
    public IActionResult ListarDisponible()
    {
        try
        {
            var inmuebles = _repoInmueble.ListarDisponible();
            return Json(inmuebles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }


	}
}
