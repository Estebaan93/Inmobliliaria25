using System.ComponentModel.DataAnnotations;
namespace inmobiliaria25.Models;

public class Inquilinos
{
  [Key]
  public int idInquilino { get; set; }

  [Required]
  public string? dni { get; set; }

  [Required]
  public string? apellido { get; set; }

  [Required]
  public string? nombre { get; set; }

  public string? telefono { get; set; }

  [Required]
  public string? correo { get; set; }
  
  public bool estado { get; set; }
}