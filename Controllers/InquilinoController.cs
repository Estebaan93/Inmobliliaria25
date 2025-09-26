//Controllers/InquilinoController.cs
using Inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Inmobiliaria25.Controllers
{
  [Authorize] //todas las acciones requieren autenticacion
  // defino la clase que hereda de controller
  public class InquilinoController : Controller
  {
    //var privada y de lectura, permite q el controller use los metodos
    private readonly RepositorioInquilino repo;

    // constructior del controllador,  ASP .net inyecta dependencia, pasa autom objeto al repo y se guarda 
    // en la var this.repo para ser usado por todod los metodos del controller
    public InquilinoController(RepositorioInquilino repo)
    {
      this.repo = repo;
    }

    //Listar activos (paginado)

    public IActionResult Index(int page = 1)
    {
      const int pageSize = 5;

      int total = repo.ContarActivos();
      int totalPages = (int)Math.Ceiling(total / (double)pageSize);
      if (page < 1) page = 1;
      if (totalPages > 0 && page > totalPages) page = totalPages;

      var lista = repo.ObtenerActivosPaginado(page, pageSize);

      ViewBag.CurrentPage = page;
      ViewBag.TotalPages = totalPages;
      ViewBag.TotalItems = total;
      ViewBag.PageSize = pageSize;

      return View(lista); // 
    }

    // listar activos
    /*public IActionResult Index()
    {
      // llama al metodo, este consulta sql devuelve una lista que se guarda en la var lista
      var lista = repo.ObtenerActivos();
      return View(lista);
    }*/

    // crear (GET)
    public IActionResult Crear()
    {
      return View();
    }

    [HttpPost]
    public IActionResult Guardar(Inquilinos inquilino)
{
  try
  {
    if (!ModelState.IsValid)
    {
      if (inquilino.IdInquilino > 0) return View("Editar", inquilino);
      return View("Crear", inquilino);
    }

    int usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    if (inquilino.IdInquilino > 0)
    {
          //recupero el registro original y fuerzo el dni para que no se pueda editar
          var original = repo.ObtenerPorId(inquilino.IdInquilino);
          if (original == null) return NotFound();
          inquilino.Dni = original.Dni;



         /* if (repo.ObtenerPorDni(inquilino.Dni, inquilino.IdInquilino))
          {
            ModelState.AddModelError("Dni", "El DNI ya está registrado");
            return View("Editar", inquilino);
          }*/

      var filas = repo.Modificar(inquilino);
      /*if (filas > 0)
      {
        AuditoriaController.RegistrarAuditoriaDesdeExterno(
          HttpContext.RequestServices,
          inquilino.IdInquilino,
          TipoEntidad.inquilino,
          AccionAuditoria.modificar, // verifica que exista en tu enum
          usuarioId,
          "Inquilino modificado"
        );
        TempData["Mensaje"] = "Inquilino modificado con éxito";
      }*/
    }
    else
    {
      if (repo.ObtenerPorDni(inquilino.Dni))
      {
        ModelState.AddModelError("Dni", "El DNI ya está registrado. Revise la tabla de propietarios.");
        return View("Crear", inquilino);
      }

      var id = repo.Alta(inquilino); // ahora devuelve id y asigna inquilino.IdInquilino
      /*if (id > 0)
      {
        AuditoriaController.RegistrarAuditoriaDesdeExterno(
          HttpContext.RequestServices,
          inquilino.IdInquilino,
          TipoEntidad.inquilino,
          AccionAuditoria.crear,
          usuarioId,
          "Inquilino creado"
        );
        TempData["Mensaje"] = "Inquilino creado con éxito";
      }
      else
      {
        ModelState.AddModelError("", "No se pudo crear el inquilino.");
        return View("Crear", inquilino);
      }*/
    }

    return RedirectToAction("Index");
  }
  catch (Exception ex)
  {
    TempData["Error"] = ex.Message;
    return RedirectToAction("Index");
  }
}



    /*public IActionResult Guardar(Inquilinos inquilino)
    {
      try
      {

        if (!ModelState.IsValid)
        {

          if (inquilino.IdInquilino > 0)
          {
            return View("Editar", inquilino);
          }
          return View("Crear", inquilino);
        }


        if (inquilino.IdInquilino > 0)
        {
          // EDItAR // consulta al repo si hay otro inq con ese dni aparte del que se edita
          if (repo.ObtenerPorDni(inquilino.Dni, inquilino.IdInquilino))
          {
            ModelState.AddModelError("Dni", "El DNI ya está registrado");
            return View("Editar", inquilino);
          }

          repo.Modificar(inquilino);
          TempData["Mensaje"] = "Inquilino modificado con éxito";
        }
        else
        {
          // CREAR // consulta al repo si hay un inqu con ese dni
          if (repo.ObtenerPorDni(inquilino.Dni))
          {
            ModelState.AddModelError("Dni", "El DNI ya está registrado. Revise la tabla de propietarios.");
            return View("Crear", inquilino);
          }

          repo.Alta(inquilino);
          TempData["Mensaje"] = "Inquilino creado con éxito";
        }

        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {
        TempData["Error"] = ex.Message;
        return RedirectToAction("Index");
      }
    }*/

    // editar (GET)
    // cuando hago click
    public IActionResult Editar(int id)
    {
      //llama al repo , la respuesta la guarda en la var inquilino
      var inquilino = repo.ObtenerPorId(id);
      if (inquilino == null)
      {
        return NotFound();
      }
      return View(inquilino);
    }


    // baja 
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public IActionResult Borrar(int id)
    {
      repo.Baja(id);
      TempData["Success"] = "Inquilino eliminado correctamente.";
      return RedirectToAction(nameof(Index));
    }


    // listar dados de baja
    [HttpGet]
    public IActionResult DadosDeBaja()
    {
      var lista = repo.ObtenerDadosDeBaja();
      return View("Index", lista); // reuso no se me genera una nueva vista
    }

    //buscar x dni
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

    // buscar x apellido
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
      Inquilinos inquilino = repo.ObtenerPorId(id);
      if (inquilino != null)
      {
        return View(inquilino);
      }
      return RedirectToAction("Index", "Home");
    }
    [HttpPost]
    public IActionResult Alta(int id)
    {
      try
      {
        Inquilinos i = repo.ObtenerPorId(id);
        i.Estado = true;
        int filasAfectadas = repo.Reactivar(i);
      }
      catch (System.Exception)
      {
        ModelState.AddModelError("dni", "Error al querer dar el alta");
      }
      return RedirectToAction("Index", "Inquilino");
    }


  }
}
