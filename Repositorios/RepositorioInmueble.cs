using Inmobiliaria25.Db;
using MySql.Data.MySqlClient;
using Inmobiliaria25.Models;

namespace Inmobiliaria25.Repositorios
{
	public class RepositorioInmueble
	{
		private readonly DataContext _context;

		public RepositorioInmueble(DataContext context)
		{
			_context = context;
		}

		// =====================
		//  listo (con filtros opcionales)
		// =====================
		public List<Inmueble> Listar(int? estado = null, int? idPropietario = null)
		{
			var lista = new List<Inmueble>();
			using (var conn = _context.GetConnection())
			{
				conn.Open();
				string sql = @"
                    SELECT i.idInmueble, i.idPropietario, i.idTipo, i.descripcion, i.cantidadAmbientes, 
                           i.precio, i.cochera, i.piscina, i.mascotas, i.estado, i.metros2, i.UrlImagen,
                           t.idTipo, t.observacion,
                           p.idPropietario, p.nombre, p.apellido
                    FROM Inmueble i
                    INNER JOIN Tipo t ON i.idTipo = t.idTipo
                    INNER JOIN Propietario p ON i.idPropietario = p.idPropietario
                    WHERE 1=1";

				if (estado.HasValue)
					sql += " AND i.estado = @estado";

				if (idPropietario.HasValue)
					sql += " AND i.idPropietario = @idPropietario";

				using (var cmd = new MySqlCommand(sql, conn))
				{
					if (estado.HasValue)
						cmd.Parameters.AddWithValue("@estado", estado.Value);

					if (idPropietario.HasValue)
						cmd.Parameters.AddWithValue("@idPropietario", idPropietario.Value);

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							lista.Add(new Inmueble
							{
								IdInmueble = reader.GetInt32("idInmueble"),
								IdPropietario = reader.GetInt32("idPropietario"),
								IdTipo = reader.GetInt32("idTipo"),
								Descripcion = reader.GetString("descripcion"),
								CantidadAmbientes = reader.GetInt32("cantidadAmbientes"),
								Precio = reader.GetDecimal("precio"),
								Cochera = reader.GetBoolean("cochera"),
								Piscina = reader.GetBoolean("piscina"),
								Mascotas = reader.GetBoolean("mascotas"),
								estado = (EstadoInmueble)reader.GetInt32("estado"),
								Metros2 = reader["metros2"].ToString() ?? "",
								UrlImagen = reader["UrlImagen"].ToString() ?? "",

								propietario = new Propietarios
								{
									IdPropietario = reader.GetInt32("idPropietario"),
									Nombre = reader.GetString("nombre"),
									Apellido = reader.GetString("apellido")
								},
								tipo = new Tipo
								{
									IdTipo = reader.GetInt32("idTipo"),
									Observacion = reader.GetString("observacion")
								}
							});
						}
					}
				}
			}
			return lista;
		}


		//crear

		public int Crear(Inmueble i)
		{
			int idCreado = -1;
			using (var conn = _context.GetConnection())
			{
				conn.Open();
				string sql = @"INSERT INTO Inmueble 
                              (idPropietario, idDireccion,  idTipo, descripcion, cantidadAmbientes, 
                               precio, cochera, piscina, mascotas, estado, metros2, UrlImagen)
                              VALUES (@idPropietario,@idDireccion, @idTipo, @descripcion, @cantidadAmbientes, 
                                      @precio, @cochera, @piscina, @mascotas, @estado, @metros2, @urlImagen);
                              SELECT LAST_INSERT_ID();";
				using (var cmd = new MySqlCommand(sql, conn))
				{
					cmd.Parameters.AddWithValue("@idPropietario", i.IdPropietario);
					cmd.Parameters.AddWithValue("@idDireccion", i.IdDireccion);
					cmd.Parameters.AddWithValue("@idTipo", i.IdTipo);
					cmd.Parameters.AddWithValue("@descripcion", i.Descripcion);
					cmd.Parameters.AddWithValue("@cantidadAmbientes", i.CantidadAmbientes);
					cmd.Parameters.AddWithValue("@precio", i.Precio);
					cmd.Parameters.AddWithValue("@cochera", i.Cochera);
					cmd.Parameters.AddWithValue("@piscina", i.Piscina);
					cmd.Parameters.AddWithValue("@mascotas", i.Mascotas);
					cmd.Parameters.AddWithValue("@estado", (int)i.estado);
					cmd.Parameters.AddWithValue("@metros2", i.Metros2);
					cmd.Parameters.AddWithValue("@urlImagen", i.UrlImagen ?? "");

					idCreado = Convert.ToInt32(cmd.ExecuteScalar());
				}
			}
			return idCreado;
		}


		// editar

		public int Modificar(Inmueble i)
		{
			int res = -1;
			using (var conn = _context.GetConnection())
			{
				conn.Open();
				string sql = @"UPDATE Inmueble SET
                        idPropietario=@idPropietario,
                        idTipo=@idTipo,
                        idDireccion=@idDireccion,
                        descripcion=@descripcion,
                        metros2=@metros2,
                        cantidadAmbientes=@cantidadAmbientes,
                        precio=@precio,
                        cochera=@cochera,
                        piscina=@piscina,
                        mascotas=@mascotas,
                        urlImagen=@urlImagen,
                        estado=@estado
                       WHERE idInmueble=@idInmueble";
				using (var cmd = new MySqlCommand(sql, conn))
				{
					cmd.Parameters.AddWithValue("@idPropietario", i.IdPropietario);
					cmd.Parameters.AddWithValue("@idTipo", i.IdTipo);
					cmd.Parameters.AddWithValue("@idDireccion", i.IdDireccion);
					cmd.Parameters.AddWithValue("@descripcion", i.Descripcion);
					cmd.Parameters.AddWithValue("@metros2", i.Metros2);
					cmd.Parameters.AddWithValue("@cantidadAmbientes", i.CantidadAmbientes);
					cmd.Parameters.AddWithValue("@precio", i.Precio);
					cmd.Parameters.AddWithValue("@cochera", i.Cochera);
					cmd.Parameters.AddWithValue("@piscina", i.Piscina);
					cmd.Parameters.AddWithValue("@mascotas", i.Mascotas);
					cmd.Parameters.AddWithValue("@urlImagen", i.UrlImagen);
					cmd.Parameters.AddWithValue("@estado", (int)i.estado);
					cmd.Parameters.AddWithValue("@idInmueble", i.IdInmueble);

					res = cmd.ExecuteNonQuery();
				}
			}
			return res;
		}



		// obtener x id

		public Inmueble? Obtener(int idInmueble)
		{
			Inmueble? i = null;
			using (var conn = _context.GetConnection())
			{
				conn.Open();
				string sql = @"
            SELECT i.idInmueble, i.idPropietario, i.idTipo, i.descripcion, 
                   i.cantidadAmbientes, i.precio, i.cochera, i.piscina, i.mascotas, 
                   i.estado, i.metros2, i.UrlImagen,
                   d.idDireccion, d.calle, d.altura, d.cp, d.ciudad, d.coordenadas,
                   p.idPropietario AS p_idPropietario, p.apellido, p.nombre,
                   t.idTipo AS t_idTipo, t.observacion AS t_observacion
            FROM Inmueble i
            INNER JOIN Propietario p ON i.idPropietario = p.idPropietario
            INNER JOIN Tipo t ON i.idTipo = t.idTipo
            INNER JOIN Direccion d ON i.idDireccion = d.idDireccion
            WHERE i.idInmueble=@id";

				using (var cmd = new MySqlCommand(sql, conn))
				{
					cmd.Parameters.AddWithValue("@id", idInmueble);
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							i = new Inmueble
							{
								IdInmueble = reader.GetInt32("idInmueble"),
								IdPropietario = reader.GetInt32("idPropietario"),
								IdTipo = reader.GetInt32("idTipo"),
								Descripcion = reader.GetString("descripcion"),
								CantidadAmbientes = reader.GetInt32("cantidadAmbientes"),
								Precio = reader.GetDecimal("precio"),
								Cochera = reader.GetBoolean("cochera"),
								Piscina = reader.GetBoolean("piscina"),
								Mascotas = reader.GetBoolean("mascotas"),
								Metros2 = reader.GetString("metros2"),
								UrlImagen = reader.GetString("UrlImagen"),
								estado = (EstadoInmueble)reader.GetInt32("estado"),

								propietario = new Propietarios
								{
									IdPropietario = reader.GetInt32("p_idPropietario"),
									Apellido = reader.GetString("apellido"),
									Nombre = reader.GetString("nombre")
								},
								tipo = new Tipo
								{
									IdTipo = reader.GetInt32("t_idTipo"),
									Observacion = reader.GetString("t_observacion")
								},
								direccion = new Direccion
								{
									IdDireccion = reader.GetInt32("idDireccion"),
									Calle = reader.GetString("calle"),
									Altura = reader.GetInt32("altura"),
									Cp = reader.GetString("cp"),
									Ciudad = reader.GetString("ciudad"),
									Coordenadas = reader.GetString("coordenadas")
								}
							};
						}
					}
				}
			}
			return i;
		}



		// elimini logico

		public int Eliminar(int idInmueble)
		{
			int res = -1;
			using (var conn = _context.GetConnection())
			{
				conn.Open();
				string sql = "UPDATE Inmueble SET estado = 0 WHERE idInmueble = @id";
				using (var cmd = new MySqlCommand(sql, conn))
				{
					cmd.Parameters.AddWithValue("@id", idInmueble);
					res = cmd.ExecuteNonQuery();
				}
			}
			return res;
		}

		public List<Inmueble> ListarDisponible()
		{
			var lista = new List<Inmueble>();
			using var conn = _context.GetConnection();
			conn.Open();
			string sql = @"
        SELECT i.IdInmueble, i.Descripcion, i.Precio,
               p.IdPropietario, p.Nombre, p.Apellido,
               t.IdTipo, t.Observacion
        FROM Inmueble i
        INNER JOIN Propietario p ON i.IdPropietario = p.IdPropietario
        INNER JOIN Tipo t ON i.IdTipo = t.IdTipo
        WHERE i.Estado = 1";  // Solo inmuebles activos

			using var cmd = new MySqlCommand(sql, conn);
			using var reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				var inmueble = new Inmueble
				{
					IdInmueble = reader.GetInt32("IdInmueble"),
					Descripcion = reader.GetString("Descripcion"),
					Precio = reader.GetDecimal("Precio"),
					propietario = new Propietarios
					{
						IdPropietario = reader.GetInt32("IdPropietario"),
						Nombre = reader.GetString("Nombre"),
						Apellido = reader.GetString("Apellido")
					},
					tipo = new Tipo
					{
						IdTipo = reader.GetInt32("IdTipo"),
						Observacion = reader.GetString("Observacion")
					}
				};
				lista.Add(inmueble);
			}
			return lista;
		}

	}
}