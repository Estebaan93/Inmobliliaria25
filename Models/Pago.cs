using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria25.Models
{
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }

        [Required]
        public int IdContrato { get; set; }
        public Contrato? Contrato { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El importe debe ser mayor a 0")]
        public decimal Importe { get; set; }

        [Required]
        public string NumeroPago { get; set; } = "";

        public string? Detalle { get; set; }

        public bool Estado { get; set; } = true;
    }
}
