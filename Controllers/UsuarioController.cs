//Controllers/UsuarioController.cs
using Inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Data;


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
    [Authorize(Policy = "Administrador")] //Solo administradores pueden listar usuarios
    public IActionResult Index()
    {
      var usuarios = _repo.Listar();
      return View(usuarios);
    }

    //  Crear (GET)
    [Authorize(Policy = "Administrador")]
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

      //Obt rol del usuario actual
      var userRol = User.FindFirst(ClaimTypes.Role)?.Value;
      var puedeEditarRol = userRol == "Administrador";
      var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

      // Usuarios solo pueden editar su propio nombre si son administradores
      var puedeEditarNombre = puedeEditarRol || (currentUserId != id.ToString());


      ViewBag.PuedeEditarRol = puedeEditarRol;
      ViewBag.puedeEditarNombre = puedeEditarNombre;

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
    public async Task<IActionResult> GuardarEditar(UsuarioEditar vm)
    {
      if (!ModelState.IsValid) return View("Editar", vm);

      var usuario = _repo.Obtener(vm.IdUsuario);
      if (usuario == null) return NotFound();

      //Obt rol del usuario actual
      var userRol = User.FindFirst(ClaimTypes.Role)?.Value;
      var esAdministrador = userRol == "Administrador";
      var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

      //Si no es adm mantiene datos sensibles
      if (!esAdministrador && currentUserId == vm.IdUsuario.ToString())
      {
        vm.Rol = usuario.Rol; //
        if (currentUserId == vm.IdUsuario.ToString())
        {
          vm.Nombre = usuario.Nombre; //Mantiene su nombre
          vm.Apellido = usuario.Apellido; //appelid
        }


      }

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

      //Actualizar los claims del usuario para el avatar
      if (User.FindFirst(ClaimTypes.NameIdentifier)?.Value == vm.IdUsuario.ToString())
      {
        var identity = User.Identity as ClaimsIdentity;

        // Crear una nueva identidad con los claims actualizados
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol),
            new Claim("AvatarUrl", usuario.Avatar ?? "/img/avatars/default.jpg")
        };

        var newIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // Cerrar sesión actual y volver a autenticar con los nuevos claims
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(newIdentity),
            new AuthenticationProperties
            {
              IsPersistent = true,
              ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
            });
      }
      return RedirectToAction("Index", "Home", new { t = DateTime.Now.Ticks });
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
    public async Task<IActionResult> CambiarPassword(UsuarioEditar vm)
    {
      if (vm.NewPassword != vm.ConfirmPassword)
      {
        ModelState.AddModelError("ConfirmPassword", "Las contraseñas no coinciden.");
        return View(vm);
      }

      var usuario = _repo.Obtener(vm.IdUsuario);
      if (usuario == null) return NotFound();

      var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var userRol = User.FindFirst(ClaimTypes.Role)?.Value;
      var esAdministrador = userRol == "Administrador";

      // Si el usuario logueado esta cambiando su propia clave, verificar la antigua.
      // Si no es el mismo usuario y no es administrador, denegar.
      if (currentUserId == vm.IdUsuario.ToString())
      {
        if (string.IsNullOrEmpty(vm.OldPassword))
        {
          ModelState.AddModelError("OldPassword", "Debe ingresar la contraseña actual.");
          return View(vm);
        }
        if (!_repo.VerificarPassword(vm.IdUsuario, vm.OldPassword))
        {
          ModelState.AddModelError("OldPassword", "La contraseña actual es incorrecta.");
          return View(vm);
        }
      }
      else if (!esAdministrador)
      {
        // No es propietario ni administrador -> prohibido
        return Forbid();
      }

      // Actualiza la contraseña
      _repo.Actualizar(usuario, vm.NewPassword);

      // Si el usuario cambioo su propia contraseña, cerrar sesión y pedir re-login
      /*if (currentUserId == vm.IdUsuario.ToString())
      {*/
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["Success"] = "Contraseña cambiada. Por favor inicie sesión con la nueva contraseña.";
        return RedirectToAction("Index", "Login");
      //}
      //return RedirectToAction("Index", "Home", new { t = DateTime.Now.Ticks });
      /*
      // Si un admin cambi0 la contraseña de otro usuario, redirigir al listado o detalle
      TempData["Success"] = "Contraseña actualizada correctamente.";
      return RedirectToAction("Index", "Home", new { t = DateTime.Now.Ticks });*/
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
