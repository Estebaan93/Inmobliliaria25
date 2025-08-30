using System.ComponentModel.DataAnnotations;
namespace inmobiliaria25.Models;

public class Usuario
{
  [Key]
  public int idUsuario { get; set; }

  [Required(ErrorMessage = "El campo email es obligatorio")]
  [EmailAddress(ErrorMessage = "Formato de correo no valido")]
  public string email { get; set; } = string.Empty;

  [Required(ErrorMessage = "El campo password es obligatorio")]
  public string password { get; set; } = string.Empty;

  [Required]
  public string rol { get; set; } = string.Empty;

  public string avatar { get; set; } = string.Empty;

  [Required(ErrorMessage = "El campo nombre es obligatorio")]
  public string nombre { get; set; } = string.Empty;

  [Required(ErrorMessage = "El campo apellido es obligatorio")]
  public string apellido { get; set; } = string.Empty;

  public bool estado { get; set; } = true;

}