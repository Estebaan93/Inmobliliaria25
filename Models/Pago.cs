using System.ComponentModel.DataAnnotations;
namespace Inmobiliaria25.Models;

public class Pago
{
  [Key]
  public int idPago { get; set; }

  [Required]
  public int idContrato { get; set; }

  [Required]
  [DataType(DataType.Date)]
  public DateTime fechaPago { get; set; }

  [Required]
  [Range(0.01, double.MaxValue, ErrorMessage ="El importe debe ser mayor a 0")]
  public decimal importe { get; set; }

  [Required]
  public string numeroPago { get; set; } = string.Empty;

  [Required]
  public string detalle { get; set; } = string.Empty;
  
  public bool estado {get; set;}  

}