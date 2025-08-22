using Microsoft.AspNetCore.Mvc;
//using Inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using inmobiliaria25.Models;

namespace Inmobiliaria25.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly RepositorioPropietario repo;

        public PropietarioController(RepositorioPropietario repo)
        {
            this.repo = repo;
        }

        // Listar todos
        public IActionResult Index()
        {
            var lista = repo.ObtenerTodos();
            return View(lista);
        }



        // Crear
        public IActionResult Crear() => View();

        [HttpPost]
        public IActionResult Crear(Propietarios propietario)
        {
            repo.Alta(propietario);
            return RedirectToAction(nameof(Index));
        }

        // Editar
        public IActionResult Editar(int id)
        {
            var propietario = repo.ObtenerPorId(id);
            if (propietario == null) return RedirectToAction("Index");
            return View(propietario);
        }

        [HttpPost]
        public IActionResult Actualizar(Propietarios propietario)
        {
            repo.Modificar(propietario);
            return RedirectToAction(nameof(Index));
        }

        // Eliminar
        public IActionResult Eliminar(int id)
        {
            var propietario = repo.ObtenerPorId(id);
            if (propietario == null) return RedirectToAction("Index");
            return View(propietario);
        }

        [HttpPost]
        public IActionResult EliminarConfirmado(int id)
        {
            repo.Baja(id);
            return RedirectToAction(nameof(Index));
        }
    }
}