//Models/Propietarios.cs
using System.ComponentModel.DataAnnotations;
namespace Inmobiliaria25.Models;

public class Propietarios
{
  [Key]
  [Required]
  public int IdPropietario { get; set; }

  [Required]
  [RegularExpression(@"^[A-Za-z]{0,3}[0-9]+$",
    ErrorMessage = " Verificar documento.")]
  public string? Dni { get; set; }

  [Required(ErrorMessage = "El apellido es obligatorio.")]
  public string? Apellido { get; set; }
  [Required(ErrorMessage = "El nombre es obligatorio.")]
  public string? Nombre { get; set; }
  [RegularExpression(@"^[0-9]+$", 
  ErrorMessage = "El teléfono debe contener solo números.")]
  public string? Telefono { get; set; }
  [Required]
  [EmailAddress(ErrorMessage = "Formato de correo no válido.")]
  public string? Correo { get; set; }
  public bool Estado { get; set; }
}