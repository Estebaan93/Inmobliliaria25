using Inmobiliaria25.Db;
using Inmobiliaria25.Models;
using MySql.Data.MySqlClient;

namespace Inmobiliaria25.Repositorios
{
	public class RepositorioPago
	{
		private readonly DataContext _context;
		private readonly RepositorioContrato _repoContrato;

		public RepositorioPago(DataContext context)
		{
			_context = context;
			_repoContrato = new RepositorioContrato(context);
		}

		private Pago MapearPago(MySqlDataReader reader)
		{
			return new Pago
			{
				IdPago = reader.GetInt32("idPago"),
				IdContrato = reader.GetInt32("idContrato"),
				FechaPago = reader.GetDateTime("fechaPago"),
				Importe = reader.GetDecimal("importe"),
				NumeroPago = reader.GetString("numeroPago"),
				Detalle = reader.IsDBNull(reader.GetOrdinal("detalle")) ? null : reader.GetString("detalle"),
				Estado = reader.GetBoolean("estado"),
				Contrato = _repoContrato.Obtener(reader.GetInt32("idContrato"))
			};
		}

		public Pago? Obtener(int idPago)
		{
			using var conn = _context.GetConnection();
			conn.Open();
			const string sql = "SELECT * FROM pago WHERE idPago=@idPago";
			using var cmd = new MySqlCommand(sql, conn);
			cmd.Parameters.AddWithValue("@idPago", idPago);
			using var reader = cmd.ExecuteReader();
			return reader.Read() ? MapearPago(reader) : null;
		}

		public List<Pago> ListarPorContrato(int idContrato)
		{
			var lista = new List<Pago>();
			using var conn = _context.GetConnection();
			conn.Open();
			const string sql = "SELECT * FROM pago WHERE idContrato=@idContrato";
			using var cmd = new MySqlCommand(sql, conn);
			cmd.Parameters.AddWithValue("@idContrato", idContrato);
			using var reader = cmd.ExecuteReader();
			while (reader.Read())
				lista.Add(MapearPago(reader));
			return lista;
		}

		public int Crear(Pago pago)
		{
			using var conn = _context.GetConnection();
			conn.Open();
			const string sql = @"
                INSERT INTO pago (idContrato, fechaPago, importe, numeroPago, detalle, estado)
                VALUES (@idContrato, @fechaPago, @importe, @numeroPago, @detalle, @estado);
                SELECT LAST_INSERT_ID();";
			using var cmd = new MySqlCommand(sql, conn);
			cmd.Parameters.AddWithValue("@idContrato", pago.IdContrato);
			cmd.Parameters.AddWithValue("@fechaPago", pago.FechaPago);
			cmd.Parameters.AddWithValue("@importe", pago.Importe);
			cmd.Parameters.AddWithValue("@numeroPago", pago.NumeroPago);
			cmd.Parameters.AddWithValue("@detalle", (object?)pago.Detalle ?? DBNull.Value);
			cmd.Parameters.AddWithValue("@estado", pago.Estado);
			return Convert.ToInt32(cmd.ExecuteScalar());
		}

		public int Modificar(Pago pago)
		{
			using (var conn = _context.GetConnection())
			{
				conn.Open();
				string sql = "UPDATE pago SET detalle = @detalle WHERE idPago = @idPago";
				using (var cmd = new MySqlCommand(sql, conn))
				{
					cmd.Parameters.AddWithValue("@detalle", pago.Detalle);
					cmd.Parameters.AddWithValue("@idPago", pago.IdPago);
					return cmd.ExecuteNonQuery();
				}
			}
		}


		public int Eliminar(int idPago)
		{
			using var conn = _context.GetConnection();
			conn.Open();
			const string sql = "UPDATE pago SET estado=0 WHERE idPago=@idPago";
			using var cmd = new MySqlCommand(sql, conn);
			cmd.Parameters.AddWithValue("@idPago", idPago);
			return cmd.ExecuteNonQuery();
		}
	}
}
