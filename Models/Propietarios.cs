using System.ComponentModel.DataAnnotations;
namespace Inmobiliaria25.Models;

public class Propietarios
{
  [Key]
  [Required]
  public int IdPropietario { get; set; }

   [Required]
  public string? Dni { get; set; }

   [Required]
  public string? Apellido { get; set; }

   [Required]
  public string? Nombre { get; set; }

  public string? Telefono { get; set; }

  [Required]
  public string? Correo { get; set; }
  
  public bool Estado { get; set; }
}