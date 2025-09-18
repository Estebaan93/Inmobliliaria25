//Models/Auditoria.cs
using System.ComponentModel.DataAnnotations;
namespace Inmobiliaria25.Models;

public class Auditoria
{
  [Key]
  public int IdAuditoria { get; set; }

  [Required]
  public int IdUsuario { get; set; }

  public Usuario Usuario { get; set; }

  [Required]
  public string Accion { get; set; } = string.Empty;

  public string Observacion { get; set; } = string.Empty;

  public DateTime FechaYHora { get; set; }

}