using System.ComponentModel.DataAnnotations;
namespace Inmobiliaria25.Models;

public class Auditoria
{
  [Key]
  public int idAuditoria { get; set; }

  [Required]
  public int idUsuario { get; set; }

  [Required]
  public string accion { get; set; } = string.Empty;

  public string observacion { get; set; } = string.Empty;

  public DateTime fechaYHora { get; set; }

}