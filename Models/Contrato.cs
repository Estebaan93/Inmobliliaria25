using System.ComponentModel.DataAnnotations; //Uso de espacios de nombres
namespace inmobiliaria25.Models;

public class Contrato
{
  [Required]
  public int idContrato { get; set; }

  [Required(ErrorMessage = "El campo inquilo es obligatorio")]
  public int idInquilino { get; set; }

  [Required(ErrorMessage = "El campo inmueble es obligatorio")]
  public int idInmueble { get; set; }
  //ublic Inquilinos inquilino { get; set; }
  //public Inmuebles inmueble { get; set; }

  [Required(ErrorMessage = "El monto es obligatorio")]
  [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
  public double monto { get; set; }

  [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
  [DataType(DataType.Date)]
  public DateTime fechaInicio { get; set; }

  [Required(ErrorMessage = "La fecha de fin es obligatoria")]
  [DataType(DataType.Date)]
  public DateTime fechaFin { get; set; }

  public DateTime fechaAnulacion { get; set; }

  public bool estado { get; set; }

  public decimal? multaGenerada { get; set; }

  public bool? multaPagada { get; set; }

}