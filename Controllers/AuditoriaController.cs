//Controllers/AuditoriaController.cs
using Inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria25.Controllers
{
  public class AuditoriaController : Controller
  {
    private readonly RepositorioAuditoria _repoAuditoria;
    private readonly RepositorioUsuario _repoUsuario;

    public AuditoriaController(RepositorioAuditoria repoAuditoria, RepositorioUsuario repoUsuario)
    {
      _repoAuditoria = repoAuditoria;
      _repoUsuario = repoUsuario;
    }

    //[Authorize(Roles = "Administrador")]
    public IActionResult Index(int? idUsuario = null)
    {
      List<Auditoria> auditorias;

      if (idUsuario.HasValue)
        auditorias = _repoAuditoria.ListarPorIdUsuario(idUsuario.Value);
      else
        auditorias = _repoAuditoria.Listar();

      List<Usuario> usuarios = _repoUsuario.Listar();

      AuditoriaUsuarioViewModel auvm = new AuditoriaUsuarioViewModel
      {
        Auditorias = auditorias,
        Usuarios = usuarios
      };

      return View(auvm);
    }

  }
}