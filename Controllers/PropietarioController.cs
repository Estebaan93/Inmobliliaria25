//Controllers/PropietarioController.cs
using inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria25.Controllers
{
  public class PropietarioController : Controller
  {
    private readonly RepositorioPropietario repo;

    public PropietarioController(RepositorioPropietario repo)
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
    public IActionResult Guardar(Propietarios propietario)
    {
      try
      {
        if (propietario.idPropietario > 0)
        {
          // EDITAR
          if (repo.ExisteDni(propietario.dni, propietario.idPropietario))
          {
            ModelState.AddModelError("dni", "El DNI ya está registrado en otro propietario.");
            return View("Editar", propietario);
          }

          repo.Modificar(propietario);
          TempData["Mensaje"] = "Propietario modificado con éxito";
        }
        else
        {
          // CREAR
          if (repo.ExisteDni(propietario.dni))
          {
            ModelState.AddModelError("dni", "El DNI ya está registrado. Revise la tabla de propietarios.");
            return View("Crear", propietario);
          }

          repo.Alta(propietario);
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
      var propietario = repo.ObtenerPorId(id);
      if (propietario == null)
      {
        return NotFound();
      }
      return View(propietario);
    }

    // Editar (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Propietarios propietario)
    {
      if (!ModelState.IsValid)
      {
        return View(propietario);
      }

      repo.Modificar(propietario);
      TempData["Success"] = "Propietario modificado correctamente.";
      return RedirectToAction(nameof(Index));
    }

    // Baja lógica
    [HttpPost]
    public IActionResult Borrar(int id)
    {
      repo.Baja(id);
      TempData["Success"] = "Propietario eliminado correctamente.";
      return RedirectToAction(nameof(Index));
    }

    // Listar dados de baja
    public IActionResult DadosDeBaja()
    {
      var lista = repo.ObtenerDadosDeBaja();
      return View(lista);
    }
  }
}