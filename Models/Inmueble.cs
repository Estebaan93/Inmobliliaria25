using System.ComponentModel.DataAnnotations;
using Inmobiliaria25.Models;

public class Inmueble
{
    [Key]
    public int idInmueble { get; set; }
    public int idPropietario { get; set; }
    public int idDireccion { get; set; }
    public int idTipo { get; set; }
    public string metros2 { get; set; }
    public int cantidadAmbientes { get; set; }
    public Boolean disponible { get; set; }
    public Decimal precio { get; set; }
    public string descripcion { get; set; }
    public Boolean cochera { get; set; }
    public Boolean piscina { get; set; }
    public Boolean mascotas { get; set; }
    public Boolean estado { get; set; }
    public string UrlImagen { get; set; }
    public Propietarios? propietario { get; set; }
    public Tipo? tipo { get; set; }

}