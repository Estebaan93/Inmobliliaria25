using Inmobiliaria25.Db;
using MySql.Data.MySqlClient;
using Inmobiliaria25.Models;

namespace Inmobiliaria25.Repositorios
{
	public class RepositorioContrato
	{
		private readonly DataContext _context;
		private readonly RepositorioInquilino _repoInquilino;
		private readonly RepositorioInmueble _repoInmueble;

		public RepositorioContrato(DataContext context)
		{
			_context = context;
			_repoInquilino = new RepositorioInquilino(context);
			_repoInmueble = new RepositorioInmueble(context);
		}

		public int Crear(Contrato contrato)
		{
			int idCreado = -1;
			using var conn = _context.GetConnection();
			conn.Open();

			string insertSql = @"
        INSERT INTO contrato 
        (idInquilino, idInmueble, monto, fechaInicio, fechaFin, fechaAnulacion, estado) 
        VALUES (@idInquilino, @idInmueble, @monto, @fechaInicio, @fechaFin, @fechaAnulacion, @estado);";

			using var cmd = new MySqlCommand(insertSql, conn);
			cmd.Parameters.AddWithValue("@idInquilino", contrato.IdInquilino);
			cmd.Parameters.AddWithValue("@idInmueble", contrato.IdInmueble);
			cmd.Parameters.AddWithValue("@monto", contrato.Monto);
			cmd.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
			cmd.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
			cmd.Parameters.AddWithValue("@fechaAnulacion", DBNull.Value);
			cmd.Parameters.AddWithValue("@estado", contrato.Estado);

			try
			{
				cmd.ExecuteNonQuery();

				using var idCmd = new MySqlCommand("SELECT LAST_INSERT_ID();", conn);
				idCreado = Convert.ToInt32(idCmd.ExecuteScalar());
			}
			catch
			{

				idCreado = -1;
			}

			return idCreado;
		}


		// modificar
		public int Modificar(Contrato contrato)
		{
			using var conn = _context.GetConnection();
			conn.Open();

			string sql = @"
                UPDATE contrato 
                SET idInquilino=@idInquilino, 
                    idInmueble=@idInmueble, 
                    monto=@monto, 
                    fechaInicio=@fechaInicio, 
                    fechaFin=@fechaFin, 
                    fechaAnulacion=@fechaAnulacion, 
                    estado=@estado
                WHERE idContrato=@idContrato";

			using var cmd = new MySqlCommand(sql, conn);
			cmd.Parameters.AddWithValue("@idInquilino", contrato.IdInquilino);
			cmd.Parameters.AddWithValue("@idInmueble", contrato.IdInmueble);
			cmd.Parameters.AddWithValue("@monto", contrato.Monto);
			cmd.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
			cmd.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
			cmd.Parameters.AddWithValue("@fechaAnulacion", contrato.FechaAnulacion.HasValue ? contrato.FechaAnulacion.Value : DBNull.Value);
			cmd.Parameters.AddWithValue("@estado", contrato.Estado);
			cmd.Parameters.AddWithValue("@idContrato", contrato.IdContrato);

			return cmd.ExecuteNonQuery();
		}

		// listra
		public List<Contrato> Listar()
		{
			var lista = new List<Contrato>();
			using var conn = _context.GetConnection();
			conn.Open();

			string sql = "SELECT * FROM contrato";
			using var cmd = new MySqlCommand(sql, conn);
			using var reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				var contrato = new Contrato
				{
					IdContrato = reader.GetInt32("idContrato"),
					IdInquilino = reader.GetInt32("idInquilino"),
					IdInmueble = reader.GetInt32("idInmueble"),
					Monto = reader.GetDouble("monto"),
					FechaInicio = reader.GetDateTime("fechaInicio"),
					FechaFin = reader.GetDateTime("fechaFin"),
					FechaAnulacion = reader.IsDBNull(reader.GetOrdinal("fechaAnulacion"))
								? (DateTime?)null
								: reader.GetDateTime("fechaAnulacion"),
					Estado = reader.GetBoolean("estado")
				};


				contrato.inquilino = _repoInquilino.ObtenerPorId(contrato.IdInquilino);
				contrato.inmueble = _repoInmueble.Obtener(contrato.IdInmueble);

				// estado dinamico manejo los 3
				if (contrato.FechaAnulacion.HasValue)
					contrato.EstadoDescripcion = "Anulado";
				else if (contrato.FechaFin < DateTime.Today)
					contrato.EstadoDescripcion = "Finalizado";
				else
					contrato.EstadoDescripcion = "Vigente";

				lista.Add(contrato);
			}

			return lista;
		}

		// obtenr x ID
		public Contrato Obtener(int id)
		{
			Contrato? contrato = null;
			using var conn = _context.GetConnection();
			conn.Open();

			string sql = "SELECT * FROM contrato WHERE idContrato=@id";
			using var cmd = new MySqlCommand(sql, conn);
			cmd.Parameters.AddWithValue("@id", id);

			using var reader = cmd.ExecuteReader();
			if (reader.Read())
			{
				contrato = new Contrato
				{
					IdContrato = reader.GetInt32("idContrato"),
					IdInquilino = reader.GetInt32("idInquilino"),
					IdInmueble = reader.GetInt32("idInmueble"),
					Monto = reader.GetDouble("monto"),
					FechaInicio = reader.GetDateTime("fechaInicio"),
					FechaFin = reader.GetDateTime("fechaFin"),
					FechaAnulacion = reader.IsDBNull(reader.GetOrdinal("fechaAnulacion"))
								? (DateTime?)null
								: reader.GetDateTime("fechaAnulacion"),
					Estado = reader.GetBoolean("estado")
				};
			}
			reader.Close();

			if (contrato != null)
			{
				contrato.inquilino = _repoInquilino.ObtenerPorId(contrato.IdInquilino);
				contrato.inmueble = _repoInmueble.Obtener(contrato.IdInmueble);

				if (contrato.FechaAnulacion.HasValue)
					contrato.EstadoDescripcion = "Anulado";
				else if (contrato.FechaFin < DateTime.Today)
					contrato.EstadoDescripcion = "Finalizado";
				else
					contrato.EstadoDescripcion = "Vigente";
			}

			return contrato;
		}

		// elimino logico
		public int Eliminar(int id)
		{
			using var conn = _context.GetConnection();
			conn.Open();

			string sql = "UPDATE contrato SET estado = 0 WHERE idContrato=@id";
			using var cmd = new MySqlCommand(sql, conn);
			cmd.Parameters.AddWithValue("@id", id);

			return cmd.ExecuteNonQuery();
		}

		//Anular contrato + mulota
		public int AnularContrato(int idContrato)
		{
			using var conn = _context.GetConnection();
			conn.Open();

			string sql = @"UPDATE contrato 
                   SET fechaAnulacion = @fechaAnulacion, 
                       estado = false
                   WHERE idContrato = @idContrato";

			using var cmd = new MySqlCommand(sql, conn);
			cmd.Parameters.AddWithValue("@fechaAnulacion", DateTime.Today);
			cmd.Parameters.AddWithValue("@idContrato", idContrato);

			return cmd.ExecuteNonQuery();
		}


	}
}
