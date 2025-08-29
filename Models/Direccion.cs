using System.ComponentModel.DataAnnotations;
namespace inmobiliaria25.Models;

public class Direcion
{
  [Key]
  public int idDireccion { get; set; }

  [Required]
  public string? calle { get; set; }

  [Required]
  public int altura { get; set; }

  [Required]
  public string? cp { get; set; }

  [Required]
  public string? ciudad { get; set; }

  [Required]
  public string? coordenadas { get; set; }


}