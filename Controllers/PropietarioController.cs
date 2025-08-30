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
    [HttpGet]
    public IActionResult DadosDeBaja()
    {
      var lista = repo.ObtenerDadosDeBaja();
      return View("Index", lista); // reuso no se me genera una nueva vista
    }


    // Buscar x dni
    public IActionResult BuscarPorDni(string dni)
    {
      if (string.IsNullOrEmpty(dni))
      {
        TempData["Error"] = "Debe ingresar un DNI para buscar.";
        return RedirectToAction("Index");
      }

      var lista = repo.BuscarPorDni(dni);
      return View("Index", lista); // reusa la vista Index
    }

    //budcar x apellido
    public IActionResult BuscarPorApellido(string apellido)
    {
      if (string.IsNullOrEmpty(apellido))
      {
        TempData["Error"] = "Debe ingresar un apellido para buscar.";
        return RedirectToAction("Index");
      }

      var lista = repo.BuscarPorApellido(apellido);
      return View("Index", lista);
    }

    // buscar x mail
    public IActionResult BuscarPorEmail(string email)
    {
      if (string.IsNullOrEmpty(email))
      {
        TempData["Error"] = "Debe ingresar un correo para buscar.";
        return RedirectToAction("Index");
      }

      var lista = repo.BuscarPorEmail(email);
      return View("Index", lista);
    }
    public IActionResult Detalles(int id)
    {
      Propietarios propietario = repo.ObtenerPorId(id);
      if (propietario != null)
      {
        return View(propietario);
      }
      return RedirectToAction("Index", "Home");
    }
    [HttpPost]
    public IActionResult Alta(int id)
    {
      try
      {
        Propietarios p = repo.ObtenerPorId(id);
        p.estado = true;
        int filasAfectadas = repo.Reactivar(p);
      }
      catch (System.Exception)
      {
        ModelState.AddModelError("dni", "Error al querer dar el alta");
      }
      return RedirectToAction("Index", "Propietario");
    }


  }
}
