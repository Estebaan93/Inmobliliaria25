//Models/AnularContratoRequest.cs
public class AnularContratoRequest
{
  public int IdContrato { get; set; }
  public decimal multa { get; set; }
  public DateTime fechaAnulacion { get; set; }
}