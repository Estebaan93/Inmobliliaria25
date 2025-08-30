using System.ComponentModel.DataAnnotations;
namespace inmobiliaria25.Models;

public class Direcion
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