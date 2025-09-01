using System.ComponentModel.DataAnnotations;
namespace Inmobiliaria25.Models;

public class Pago
{
  [Key]
  public int IdPago { get; set; }

  [Required]
  public int IdContrato { get; set; }

  [Required]
  [DataType(DataType.Date)]
  public DateTime FechaPago { get; set; }

  [Required]
  [Range(0.01, double.MaxValue, ErrorMessage ="El importe debe ser mayor a 0")]
  public decimal Importe { get; set; }

  [Required]
  public string NumeroPago { get; set; } = string.Empty;

  [Required]
  public string Detalle { get; set; } = string.Empty;
  
  public bool Estado {get; set;}  

}