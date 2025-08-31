using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria25.Models  
{
    public class Tipo
    {
        [Key]
        public int IdTipo { get; set; }
        public string Observacion { get; set; }

        public override string ToString()
        {
            return Observacion;
        }
    }
}
