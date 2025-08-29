using System.ComponentModel.DataAnnotations;
namespace inmobiliaria25.Models;

public class Tipo
{
  [Key]
  public int idTipo { get; set; }

  
  //public string? observacion { get; set; }
  //public string observacion { get; set; } = null;
  //La observacion puede ser obligatoria??
  public string observacion { get; set; } = string.Empty;

  public override string ToString()
  {
    return observacion?? string.Empty;
  }

}