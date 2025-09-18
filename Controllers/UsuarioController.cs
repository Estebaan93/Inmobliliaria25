//Controllers/UsuarioController.cs
using Inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace Inmobiliaria25.Controllers
{
  [Authorize] //todas las acciones requieren autenticacion
  public class UsuarioController : Controller
  {
    private readonly RepositorioUsuario _repo;

    public UsuarioController(RepositorioUsuario repo)
    {
      _repo = repo;
    }

    //  Listar usuarios
    public IActionResult Index()
    {
      var usuarios = _repo.Listar();
      return View(usuarios);
    }

    //  Crear (GET)
    public IActionResult Crear()
    {
      return View(new Usuario());
    }

    //  Crear (POST)
    [HttpPost]
    public IActionResult Guardar(Usuario usuario, IFormFile? archivo)
    {
      if (!ModelState.IsValid)
        return View("Crear", usuario);

      if (archivo != null && archivo.Length > 0)
      {
        string filePath = Path.Combine("wwwroot/img/avatars", archivo.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          archivo.CopyTo(stream);
        }
        usuario.Avatar = "/img/avatars/" + archivo.FileName;
      }

      _repo.Guardar(usuario);
      return RedirectToAction("Index");
    }

    //  Editar (GET)
    public IActionResult Editar(int id)
    {
      var usuario = _repo.Obtener(id);
      if (usuario == null) return NotFound();

      var vm = new UsuarioEditar
      {
        IdUsuario = usuario.IdUsuario,
        Nombre = usuario.Nombre,
        Apellido = usuario.Apellido,
        Email = usuario.Email,
        Rol = usuario.Rol,
        Avatar = usuario.Avatar
      };
      return View(vm);
    }

    // Editar (POST)
    [HttpPost]
    public IActionResult GuardarEditar(UsuarioEditar vm)
    {
      if (!ModelState.IsValid) return View("Editar", vm);

      var usuario = _repo.Obtener(vm.IdUsuario);
      if (usuario == null) return NotFound();

      usuario.Nombre = vm.Nombre;
      usuario.Apellido = vm.Apellido;
      usuario.Email = vm.Email;
      usuario.Rol = vm.Rol;

      if (vm.AvatarFile != null && vm.AvatarFile.Length > 0)
      {
        string filePath = Path.Combine("wwwroot/img/avatars", vm.AvatarFile.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          vm.AvatarFile.CopyTo(stream);
        }
        usuario.Avatar = "/img/avatars/" + vm.AvatarFile.FileName;
      }
      else if (vm.BorrarAvatar)
      {
        usuario.Avatar = null;
      }

      _repo.Actualizar(usuario);
      return RedirectToAction("Index", "Home");
    }

    //  Detalle
    public IActionResult Detalle(int id)
    {
      var usuario = _repo.Obtener(id);
      if (usuario == null) return NotFound();
      return View(usuario);
    }

    //  Cambiar Password (GET)
    public IActionResult CambiarPassword(int id)
    {
      var usuario = _repo.Obtener(id);
      if (usuario == null) return NotFound();

      var vm = new UsuarioEditar
      {
        IdUsuario = usuario.IdUsuario,
        Nombre = usuario.Nombre,
        Apellido = usuario.Apellido,
        Email = usuario.Email,
        Rol = usuario.Rol
      };
      return View(vm);
    }

    //  Cambiar Password (POST)
    [HttpPost]
    public IActionResult CambiarPassword(UsuarioEditar vm)
    {
      if (vm.NewPassword != vm.ConfirmPassword)
      {
        ModelState.AddModelError("ConfirmPassword", "Las contraseñas no coinciden.");
        return View(vm);
      }

      var usuario = _repo.Obtener(vm.IdUsuario);
      if (usuario == null) return NotFound();

      _repo.Actualizar(usuario, vm.NewPassword);
      return RedirectToAction("Index");
    }

    //  Eliminar
    [HttpPost]
    public IActionResult Borrar(int idUsuario)
    {
      _repo.Eliminar(idUsuario);
      return RedirectToAction("Index");
    }
  }
}
