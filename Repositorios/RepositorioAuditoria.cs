//Repositorio/RepositorioAuditoria.cs
using Inmobiliaria25.Db;
using Inmobiliaria25.Models;
using MySql.Data.MySqlClient;

namespace Inmobiliaria25.Repositorios
{
  public class RepositorioAuditoria
  {
    private readonly DataContext _context;
    private readonly RepositorioUsuario _repoUsuario;
    public RepositorioAuditoria(DataContext context)
    {
      _context = context;
      _repoUsuario = new RepositorioUsuario(context);
    }

    //Listar todas las auditorias
    public List<Auditoria> Listar()
    {
      var auditorias = new List<Auditoria>();

      using var conn = _context.GetConnection();
      conn.Open();

      string sql = "SELECT * FROM auditoria ORDER BY fechaYHora DESC";
      using var cmd = new MySqlCommand(sql, conn);
      using var reader = cmd.ExecuteReader();

      while (reader.Read())
      {
        var user = _repoUsuario.Obtener(reader.GetInt32("idUsuario"));
        if (user != null)
        {
          auditorias.Add(new Auditoria
          {
            IdAuditoria = reader.GetInt32("idAuditoria"),
            IdUsuario = reader.GetInt32("idUsuario"),
            Usuario = user,
            Accion = reader.GetString("accion"),
            Observacion = reader.GetString("observacion"),
            FechaYHora = reader.GetDateTime("fechaYHora")
          });
        }
      }

      return auditorias;
    }
    //Listar auditorías por usuario
    public List<Auditoria> ListarPorIdUsuario(int id)
    {
      var auditorias = new List<Auditoria>();

      using var conn = _context.GetConnection();
      conn.Open();

      string sql = "SELECT * FROM auditoria WHERE idUsuario = @id ORDER BY fechaYHora DESC";
      using var cmd = new MySqlCommand(sql, conn);
      cmd.Parameters.AddWithValue("@id", id);

      using var reader = cmd.ExecuteReader();
      while (reader.Read())
      {
        var user = _repoUsuario.Obtener(id);
        if (user != null)
        {
          auditorias.Add(new Auditoria
          {
            IdAuditoria = reader.GetInt32("idAuditoria"),
            IdUsuario = reader.GetInt32("idUsuario"),
            Usuario = user,
            Accion = reader.GetString("accion"),
            Observacion = reader.GetString("observacion"),
            FechaYHora = reader.GetDateTime("fechaYHora")
          });
        }
      }

      return auditorias;
    }

    //Guardar nueva auditoría
    public int GuardarAuditoria(Auditoria auditoria)
    {
      int filasAfectadas = 0;

      using var conn = _context.GetConnection();
      conn.Open();

      string sql = @"INSERT INTO auditoria (idUsuario, accion, observacion, fechaYHora) 
                           VALUES (@IdUsuario, @Accion, @Observacion, @FechaYHora)";

      using var cmd = new MySqlCommand(sql, conn);
      cmd.Parameters.AddWithValue("@IdUsuario", auditoria.IdUsuario);
      cmd.Parameters.AddWithValue("@Accion", auditoria.Accion);
      cmd.Parameters.AddWithValue("@Observacion", auditoria.Observacion);
      cmd.Parameters.AddWithValue("@FechaYHora", auditoria.FechaYHora);

      filasAfectadas = cmd.ExecuteNonQuery();

      return filasAfectadas;
    }
  }
}