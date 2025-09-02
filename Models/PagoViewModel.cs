namespace Inmobiliaria25.Models
{
    public class PagoViewModel
    {
        public Pago Pago { get; set; }
        public Contrato Contrato { get; set; }
    }

    public class PagoContrato
    {
        public List<Pago> Pagos { get; set; }
        public Contrato Contrato { get; set; }
    }
}
