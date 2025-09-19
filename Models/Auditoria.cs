//Models/Auditoria.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria25.Models;

public class Auditoria
{
    [Key]
    [Display(Name = "ID Auditoria")]
    public int IdAuditoria { get; set; }

    [Required]
    [Display(Name = "ID Entidad")]
    public int IdEntidad { get; set; }

    [Required]
    [Display(Name = "Tipo de Entidad")]
    public TipoEntidad TipoEntidad { get; set; }  // Cambiado a enum

    [Required]
    [Display(Name = "Accion")]
    public AccionAuditoria Accion { get; set; }  // Cambiado a enum

    [Required]
    [Display(Name = "ID Usuario")]
    public int IdUsuario { get; set; }

    [ForeignKey("IdUsuario")]
    public Usuario? Usuario { get; set; }

    [Display(Name = "Observacion")]
    public string? Observacion { get; set; }

    [Required]
    [Display(Name = "Fecha y Hora")]
    public DateTime FechaYHora { get; set; }

    [Required]
    [Display(Name = "Estado")]
    public bool Estado { get; set; } = true;

    // Propiedades de solo lectura para mejor visualizacin
    [Display(Name = "Entidad Auditada")]
    public string EntidadDescripcion => $"{TipoEntidad} #{IdEntidad}";

    [Display(Name = "Accion Realizada")]
    public string AccionFormateada
    {
        get
        {
            return Accion switch
            {
                AccionAuditoria.crear => "Creacion",
                AccionAuditoria.terminar => "Terminacion",
                AccionAuditoria.anular => "Anulacion",
                _ => Accion.ToString()
            };
        }
    }
}

// Enums
public enum TipoEntidad
{
  contrato,
  pago,
  inquilino,
  propietario,
  inmueble
}

public enum AccionAuditoria
{
  crear,
  modificar,
  terminar,
  anular,
  baja,
  reactivar
}