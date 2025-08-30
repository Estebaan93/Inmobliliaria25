using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //Uso de espacios de nombres
namespace inmobiliaria25.Models;

public class Contrato
{
  [Required]
  public int idContrato { get; set; }

  [Required(ErrorMessage = "El campo inquilo es obligatorio")]
  public int idInquilino { get; set; }

  [Required(ErrorMessage = "El campo inmueble es obligatorio")]
  public int idInmueble { get; set; }
  //public Inquilinos inquilino { get; set; }
  //public Inmuebles inmueble { get; set; }

  [Required(ErrorMessage = "El monto es obligatorio")]
  [Column(TypeName ="decimal(10,0)")] //Para EF le dice que tipo exacto que debe usar en la BD
  [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
  public decimal monto { get; set; }

  [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
  [DataType(DataType.Date)]
  public DateTime fechaInicio { get; set; }

  [Required(ErrorMessage = "La fecha de fin es obligatoria")]
  [DataType(DataType.Date)]
  public DateTime fechaFin { get; set; }

  [DataType(DataType.Date)]
  public DateTime? fechaAnulacion { get; set; } //nullable en la BD

  [Required]
  public bool estado { get; set; }

  public decimal? multaGenerada { get; set; }

  public bool? multaPagada { get; set; }

}