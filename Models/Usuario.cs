//Models/Usuario.cs
using System.ComponentModel.DataAnnotations;
namespace Inmobiliaria25.Models;

public class Usuario
{
  [Key]
  public int IdUsuario { get; set; }

  [Required(ErrorMessage = "El campo email es obligatorio")]
  [EmailAddress(ErrorMessage = "Formato de correo no valido")]
  public string Email { get; set; } = string.Empty;

  [Required(ErrorMessage = "El campo password es obligatorio")]
  public string Password { get; set; } = string.Empty;

  [Required]
  public string Rol { get; set; } = string.Empty;

  public string Avatar { get; set; } = string.Empty;

  [Required(ErrorMessage = "El campo nombre es obligatorio")]
  public string Nombre { get; set; } = string.Empty;

  [Required(ErrorMessage = "El campo apellido es obligatorio")]
  public string Apellido { get; set; } = string.Empty;

  public bool Estado { get; set; } = true;

}