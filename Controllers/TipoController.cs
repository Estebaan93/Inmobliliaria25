//tipocontroller
using Inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria25.Controllers
{
  public class TipoController : Controller // heredo de controller
  {
    private readonly RepositorioTipo _repo;

    public TipoController(RepositorioTipo repo) // constructor
    {
      _repo = repo;
    }

    // =======================
    // SOLO PARA SWEETALERT
    // =======================

    // listar
    [HttpGet]
    public IActionResult ListarJson() //todos los tipos en formato JSON  lo usa tu fetch 
    {
      var tipos = _repo.Listar();
      return Json(tipos);
    }

    // crear
    [HttpPost]
    public IActionResult CrearJson([FromBody] Tipo tipo) // creo en fetch y mando el json en el body
    {
      if (tipo == null || string.IsNullOrWhiteSpace(tipo.Observacion))
        return BadRequest("La observación es obligatoria");

      var id = _repo.Crear(tipo);
      return Json(new { id, mensaje = "Tipo creado con éxito" });
    }

    // editar
    [HttpPost]
    public IActionResult EditarJson([FromBody] Tipo tipo)
    {
      if (tipo == null || tipo.IdTipo == 0)
        return BadRequest("Datos inválidos");

      _repo.Modificar(tipo);
      return Json(new { mensaje = "Tipo actualizado con éxito" });
    }

    // elim
    [HttpPost]
    public IActionResult EliminarJson([FromBody] int id)
    {
      if (id == 0)
        return BadRequest("Id inválido");

      _repo.Eliminar(id);
      return Json(new { mensaje = "Tipo eliminado con éxito" });
    }
  }
}
