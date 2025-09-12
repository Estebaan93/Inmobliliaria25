using System.ComponentModel.DataAnnotations;
namespace Inmobiliaria25.Models
{
  public class UsuarioEditar 
  {
    public int IdUsuario {get; set;}
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string Rol { get; set; }
    public string? Avatar { get; set; }
    public IFormFile? AvatarFile { get; set; }
    public bool BorrarAvatar { get; set; }

    // Passwords
    public string? NewPassword { get; set; }
    public string? ConfirmPassword { get; set; }
  }
}