using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //Uso de espacios de nombres
namespace Inmobiliaria25.Models;

public class Contrato
{
  [Required]
  public int IdContrato { get; set; }

  [Required(ErrorMessage = "El campo inquilo es obligatorio")]
  public int IdInquilino { get; set; }

  [Required(ErrorMessage = "El campo inmueble es obligatorio")]
  public int IdInmueble { get; set; }
  //public Inquilinos inquilino { get; set; }
  //public Inmuebles inmueble { get; set; }

  [Required(ErrorMessage = "El monto es obligatorio")]
  [Column(TypeName ="decimal(10,0)")] //Para EF le dice que tipo exacto que debe usar en la BD
  [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
  public decimal Monto { get; set; }

  [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
  [DataType(DataType.Date)]
  public DateTime FechaInicio { get; set; }

  [Required(ErrorMessage = "La fecha de fin es obligatoria")]
  [DataType(DataType.Date)]
  public DateTime FechaFin { get; set; }

  [DataType(DataType.Date)]
  public DateTime? FechaAnulacion { get; set; } //nullable en la BD

  [Required]
  public bool Estado { get; set; }

  public decimal? MultaGenerada { get; set; }

  public bool? MultaPagada { get; set; }

}