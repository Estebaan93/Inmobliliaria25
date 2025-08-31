using System.ComponentModel.DataAnnotations;
namespace Inmobiliaria25.Models;

public class Direccion
{
  [Key]
  public int idDireccion { get; set; }

  [Required]
  public string calle { get; set; } = string.Empty;

  [Required]
  public int altura { get; set; }

  [Required]
  public string cp { get; set; } = string.Empty;

  [Required]
  public string ciudad { get; set; } = string.Empty;

  [Required]
  public string coordenadas { get; set; } = string.Empty;

}