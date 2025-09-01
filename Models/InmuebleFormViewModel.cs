namespace Inmobiliaria25.Models
{
  public class InmuebleFormViewModel
  {
    public Inmueble Inmueble { get; set; } = new Inmueble();
    public List<Propietarios> Propietarios { get; set; } = new();
    public List<Tipo> Tipos { get; set; } = new();
    public Direccion Direccion { get; set; } = new Direccion();
  }
}
