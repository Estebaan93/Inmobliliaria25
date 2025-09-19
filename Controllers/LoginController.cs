//Controllers/LoginController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;

namespace Inmobiliaria25.Controllers
{
  public class LoginController : Controller
  {
    private readonly RepositorioLogin _repoLogin;

    public LoginController(RepositorioLogin repoLogin)
    {
      _repoLogin = repoLogin;
    }

    [HttpGet]
    public IActionResult Index()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Autenticacion(LoginViewModel user)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.Error = "Debe completar todos los campos";
        return View("Index", user);
      }

      var u = _repoLogin.Verificar(user);

      if (u != null)
      {
        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, $"{u.Nombre} {u.Apellido}"),
                    new Claim(ClaimTypes.Role, u.Rol),
                    new Claim(ClaimTypes.NameIdentifier, u.IdUsuario.ToString()),
                    new Claim("AvatarUrl", u.Avatar ?? "/img/avatars/default.jpg")
                };

        var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identidad));

        return RedirectToAction("Index", "Home");
      }

      ViewBag.Error = "Usuario o contraseña incorrectos";
      return View("Index", user);
    }

    public async Task<IActionResult> Logout()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      return RedirectToAction("Index", "Login");
    }
  }
}
