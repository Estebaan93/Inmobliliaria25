using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria25.Models
{
  public enum EstadoInmueble
  {
    NoDisponible = 0,
    Disponible = 1,
    Alquilado = 2
  }

  public class Inmueble
  {
    [Key]
    public int IdInmueble { get; set; }
    public int IdPropietario { get; set; }
    public int IdDireccion { get; set; }
    public int IdTipo { get; set; }
    public string Metros2 { get; set; }
    public int CantidadAmbientes { get; set; }
    public Boolean Disponible { get; set; }
    public Decimal Precio { get; set; }
    public string Descripcion { get; set; }
    public Boolean Cochera { get; set; }
    public Boolean Piscina { get; set; }
    public Boolean Mascotas { get; set; }
    public string UrlImagen { get; set; }
    public Propietarios? Propietario { get; set; }
    public Tipo? Tipo { get; set; }
    public Direccion? Direccion { get; set; }
    public EstadoInmueble Estado { get; set; }
  }
}
