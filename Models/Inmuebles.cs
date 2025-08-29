using System.ComponentModel.DataAnnotations;
namespace inmobiliaria25.Models;

public class Inmuebles
{
  [Key]
  public int idInmueble { get; set; }

  [Required]
  public int idPropietario { get; set; }

  [Required]
  public int idDireccion { get; set; }

  [Required]
  public int idTipo { get; set; }

  [Required]
  [StringLength(100)]
  public string? metros2 { get; set; }

  [Required]
  public int cantidadAmbientes { get; set; }

  public bool disponible { get; set; }

  [Required]
  public decimal precio { get; set; }

  //[Required]
  [StringLength(300)]
  public string? descripcion { get; set; }

  public bool cochera { get; set; }

  public bool piscina { get; set; }
  
  public bool mascotas { get; set; }

  [Required]
  [StringLength(150)]
  public string? urlImagen { get; set; }

  [Required]
  public bool estado { get; set; }

}

