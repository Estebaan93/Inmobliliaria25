//Models/Auditoria.cs
using System.ComponentModel.DataAnnotations;
namespace Inmobiliaria25.Models;

public class Auditoria
{
  [Key]
  public int IdAuditoria { get; set; }

  [Required]
  public int IdEntidad { get; set; }

  public string TipoEntidad { get; set; }

  [Required]
  public string Accion { get; set; } = string.Empty;

  public int IdUsuario { get; set; }

  public string Observacion { get; set; }

  public DateTime FechaYHora { get; set; }

  public bool Estado { get; set; } = true;

}

//Enum 
public enum TipoEntidad
{
  contrato,
  pago
}

public enum AccionAuditoria
{
  crear,
  terminar,
  anular
}