using Microsoft.AspNetCore.Mvc;
//using Inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using inmobiliaria25.Models;

namespace Inmobiliaria25.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly RepositorioInquilino repo;

        public InquilinoController(RepositorioInquilino repo)
        {
            this.repo = repo;
        }

        // Listar todos
        public IActionResult Index()
        {
            var lista = repo.ObtenerTodos();
            return View(lista);
        }

        // Detalle
        public IActionResult Detalle(int id)
        {
            var inquilino = repo.ObtenerPorId(id);
            if (inquilino == null) return RedirectToAction("Index");
            return View(inquilino);
        }

        // Crear
        public IActionResult Crear() => View();

        [HttpPost]
        public IActionResult Crear(Inquilinos inquilino)
        {
            repo.Alta(inquilino);
            return RedirectToAction(nameof(Index));
        }

        // Editar
        public IActionResult Editar(int id)
        {
            var inquilino = repo.ObtenerPorId(id);
            if (inquilino == null) return RedirectToAction("Index");
            return View(inquilino);
        }

        [HttpPost]
        public IActionResult Actualizar(Inquilinos inquilino)
        {
            repo.Modificar(inquilino);
            return RedirectToAction(nameof(Index));
        }

        // Eliminar
        public IActionResult Eliminar(int id)
        {
            var inquilino = repo.ObtenerPorId(id);
            if (inquilino == null) return RedirectToAction("Index");
            return View(inquilino);
        }

        [HttpPost]
        public IActionResult EliminarConfirmado(int id)
        {
            repo.Baja(id);
            return RedirectToAction(nameof(Index));
        }
    }
}