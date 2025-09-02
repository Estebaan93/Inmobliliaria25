using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Inmobiliaria25.Models
{
    public class Contrato
    {
        [Key]
        public int IdContrato { get; set; }

        [Required(ErrorMessage = "El campo Inquilino es obligatorio.")]
        public int IdInquilino { get; set; }

        [Required(ErrorMessage = "El campo Inmueble es obligatorio.")]
        public int IdInmueble { get; set; }

        // las validate propiedades de navegacion No validan
        [ValidateNever]
        public Inquilinos inquilino { get; set; }

        [ValidateNever]
        public Inmueble inmueble { get; set; }

        [Required(ErrorMessage = "El Monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El Monto debe ser mayor a 0.")]
        public double Monto { get; set; }

        [Required(ErrorMessage = "La Fecha de Inicio es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La Fecha de Fin es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        public DateTime? FechaAnulacion { get; set; }

        public bool Estado { get; set; } = true;

        public decimal? MultaGenerada { get; set; }

        public bool? MultaPagada { get; set; }

        [ValidateNever]
        public string EstadoDescripcion { get; set; }
    }
}
