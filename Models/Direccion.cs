//Models/Direccion.cs
using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria25.Models
{
  public class Direccion
  {
    public int IdDireccion { get; set; }

    [Required]
    public string Calle { get; set; }

    [Required]
    public int Altura { get; set; }

    public string? Cp { get; set; }
    public string? Ciudad { get; set; }
    public string? Coordenadas { get; set; }
  }
}
