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

    // Listar todas las auditorías
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
        auditorias.Add(CrearAuditoriaDesdeReader(reader));
      }

      return auditorias;
    }

    // Listar auditorías por usuario
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
        auditorias.Add(CrearAuditoriaDesdeReader(reader));
      }

      return auditorias;
    }

    // Listar auditorías por tipo de entidad y ID de entidad
    public List<Auditoria> ListarPorEntidad(TipoEntidad tipoEntidad, int idEntidad)
    {
      var auditorias = new List<Auditoria>();

      using var conn = _context.GetConnection();
      conn.Open();

      string sql = "SELECT * FROM auditoria WHERE tipoEntidad = @tipoEntidad AND idEntidad = @idEntidad ORDER BY fechaYHora DESC";
      using var cmd = new MySqlCommand(sql, conn);
      cmd.Parameters.AddWithValue("@tipoEntidad", tipoEntidad.ToString());
      cmd.Parameters.AddWithValue("@idEntidad", idEntidad);

      using var reader = cmd.ExecuteReader();
      while (reader.Read())
      {
        auditorias.Add(CrearAuditoriaDesdeReader(reader));
      }

      return auditorias;
    }

    // Obtener auditoría por ID
    public Auditoria? ObtenerPorId(int id)
    {
      Auditoria? auditoria = null;

      using var conn = _context.GetConnection();
      conn.Open();

      string sql = "SELECT * FROM auditoria WHERE idAuditoria = @id";
      using var cmd = new MySqlCommand(sql, conn);
      cmd.Parameters.AddWithValue("@id", id);

      using var reader = cmd.ExecuteReader();
      if (reader.Read())
      {
        auditoria = CrearAuditoriaDesdeReader(reader);
      }

      return auditoria;
    }

    // Guardar nueva auditoría
    public int GuardarAuditoria(Auditoria auditoria)
    {
      try
      {
        using var conn = _context.GetConnection();
        conn.Open();

        string sql = @"INSERT INTO auditoria 
                      (idEntidad, tipoEntidad, accion, idUsuario, observacion, fechaYHora, estado) 
                      VALUES (@IdEntidad, @TipoEntidad, @Accion, @IdUsuario, @Observacion, @FechaYHora, @Estado)";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@IdEntidad", auditoria.IdEntidad);
        cmd.Parameters.AddWithValue("@TipoEntidad", auditoria.TipoEntidad.ToString());
        cmd.Parameters.AddWithValue("@Accion", auditoria.Accion.ToString());
        cmd.Parameters.AddWithValue("@IdUsuario", auditoria.IdUsuario);
        cmd.Parameters.AddWithValue("@Observacion", (object?)auditoria.Observacion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@FechaYHora", auditoria.FechaYHora);
        cmd.Parameters.AddWithValue("@Estado", auditoria.Estado);

        var filas = cmd.ExecuteNonQuery();
        Console.WriteLine($"✔ Auditoría insertada: {filas} fila(s)");
        return filas;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"❌ ERROR al guardar auditoría: {ex.Message}");
        throw;
      }
    }


    // Método auxiliar para registrar auditorías fácilmente
    public int RegistrarAuditoria(int idEntidad, TipoEntidad tipoEntidad, AccionAuditoria accion, int idUsuario, string? observacion = null)
    {
      var auditoria = new Auditoria
      {
        IdEntidad = idEntidad,
        TipoEntidad = tipoEntidad,
        Accion = accion,
        IdUsuario = idUsuario,
        Observacion = observacion,
        FechaYHora = DateTime.Now,
        Estado = true
      };

      return GuardarAuditoria(auditoria);
    }

    // Método privado para crear objetos Auditoria desde MySqlDataReader
    private Auditoria CrearAuditoriaDesdeReader(MySqlDataReader reader)
    {
      var idUsuario = reader.GetInt32("idUsuario");
      var usuario = _repoUsuario.Obtener(idUsuario);

      return new Auditoria
      {
        IdAuditoria = reader.GetInt32("idAuditoria"),
        IdEntidad = reader.GetInt32("idEntidad"),
        TipoEntidad = Enum.Parse<TipoEntidad>(reader.GetString("tipoEntidad")),
        Accion = Enum.Parse<AccionAuditoria>(reader.GetString("accion")),
        IdUsuario = idUsuario,
        Usuario = usuario,
        Observacion = reader.IsDBNull(reader.GetOrdinal("observacion")) ? null : reader.GetString("observacion"),
        FechaYHora = reader.GetDateTime("fechaYHora"),
        Estado = reader.GetBoolean("estado")
      };
    }
  }
}