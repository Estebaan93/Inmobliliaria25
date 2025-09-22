//Repositorios/RepositorioInmueble.cs
using Inmobiliaria25.Db;
using MySql.Data.MySqlClient;
using Inmobiliaria25.Models;

namespace Inmobiliaria25.Repositorios
{
  public class RepositorioInmueble
  {
    private readonly DataContext _context;
    private readonly RepositorioPropietario _repoPropietario;
    private readonly RepositorioTipo _repoTipo;

    public RepositorioInmueble(DataContext context)
    {
      _context = context;
      _repoPropietario = new RepositorioPropietario(context);
      _repoTipo = new RepositorioTipo(context);
    }

    //Para la paginacion
    /*public int ContarActivos()
		{
			using var conn = _context.GetConnection();
			conn.Open();
			const string sql = "SELECT COUNT(*) FROM Inmueble";
			using var cmd = new MySqlCommand(sql, conn);
			return Convert.ToInt32(cmd.ExecuteScalar());
		}
		public List<Inmueble> ObtenerActivosPaginado(int page, int pageSize)
		{
			var lista = new List<Inmueble>();
			int offset = (page - 1) * pageSize;

			using var conn = _context.GetConnection();
			conn.Open();
			const string sql = @"
				SELECT i.idInmueble,i.idPropietario 
				AS i_idPropietario,	i.idDireccion        
				AS i_idDireccion,	i.idTipo             
				AS i_idTipo, i.descripcion,	i.cantidadAmbientes, i.precio, i.cochera,	i.piscina,
						i.mascotas,	i.estado,	i.metros2, i.UrlImagen,	p.idPropietario      
				AS p_idPropietario,	p.apellido,	p.nombre,	t.idTipo             
				AS t_idTipo, t.observacion        
				AS t_observacion 
				FROM Inmueble i
					INNER JOIN Propietario p ON i.idPropietario = p.idPropietario
					INNER JOIN Tipo t        ON i.idTipo        = t.idTipo
				ORDER BY i.idInmueble DESC
				LIMIT @limit OFFSET @offset;";

			using var cmd = new MySqlCommand(sql, conn);
			cmd.Parameters.AddWithValue("@limit", pageSize);
			cmd.Parameters.AddWithValue("@offset", offset);
			using var reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				lista.Add(new Inmueble
				{
					IdInmueble = reader.GetInt32("idInmueble"),
					IdPropietario = reader.GetInt32("i_idPropietario"),
					IdDireccion = reader.GetInt32("i_idDireccion"),
					IdTipo = reader.GetInt32("i_idTipo"),
					Descripcion = reader.GetString("descripcion"),
					CantidadAmbientes = reader.GetInt32("cantidadAmbientes"),
					Precio = reader.GetDecimal("precio"),
					Cochera = reader.GetBoolean("cochera"),
					Piscina = reader.GetBoolean("piscina"),
					Mascotas = reader.GetBoolean("mascotas"),
					estado = (EstadoInmueble)reader.GetInt32("estado"),
					Metros2 = reader["metros2"]?.ToString() ?? "",
					UrlImagen = reader["UrlImagen"]?.ToString() ?? "",
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
					}
				});
			}
			return lista;

		}*/

    // =====================
    //  listo (con filtros opcionales)
    // =====================
    public List<Inmueble> Listar(int? estado = null, int? idPropietario = null)
    {
      var lista = new List<Inmueble>();
      using var conn = _context.GetConnection();
      conn.Open();

      // condición que detecta contrato vigente hoy
      string contratoCond = @"EXISTS (
        SELECT 1 FROM contrato c
        WHERE c.idInmueble = i.idInmueble
          AND c.estado = 1
          AND (c.fechaAnulacion IS NULL OR c.fechaAnulacion = '' OR c.fechaAnulacion = '0000-00-00' OR c.fechaAnulacion = '0000-00-00 00:00:00')
          AND c.fechaInicio <= CURDATE()
          AND c.fechaFin >= CURDATE()
    )";

      var sql = $@"
        SELECT DISTINCT
            i.idInmueble, i.idPropietario, i.idTipo, i.descripcion, i.cantidadAmbientes,
            i.precio, i.cochera, i.piscina, i.mascotas, i.estado AS estado_tabla, i.metros2, i.UrlImagen,
            t.idTipo, t.observacion,
            p.idPropietario AS p_id, p.nombre AS p_nombre, p.apellido AS p_apellido,
            CASE
                WHEN {contratoCond} THEN 2
                WHEN i.estado = 0 THEN 0
                ELSE 1
            END AS estado_calculado
        FROM Inmueble i
        INNER JOIN Tipo t ON i.idTipo = t.idTipo
        INNER JOIN Propietario p ON i.idPropietario = p.idPropietario
    ";

      var whereParts = new List<string>();

      if (estado.HasValue)
      {
        if (estado.Value == 2) // alquilados
          whereParts.Add($"{contratoCond}");
        else if (estado.Value == 1) // disponibles (activo y sin contrato vigente)
          whereParts.Add($"i.estado = 1 AND NOT ({contratoCond})");
        else if (estado.Value == 0) // no disponible por estado
          whereParts.Add("i.estado = 0");
      }

      if (idPropietario.HasValue)
        whereParts.Add("i.idPropietario = @idPropietario");

      if (whereParts.Count > 0)
        sql += " WHERE " + string.Join(" AND ", whereParts);

      sql += " ORDER BY i.idInmueble DESC;";

      using var cmd = new MySqlCommand(sql, conn);
      if (idPropietario.HasValue)
        cmd.Parameters.AddWithValue("@idPropietario", idPropietario.Value);

      using var reader = cmd.ExecuteReader();
      while (reader.Read())
      {
        var inm = new Inmueble
        {
          IdInmueble = reader.GetInt32("idInmueble"),
          IdPropietario = reader.GetInt32("idPropietario"),
          IdTipo = reader.GetInt32("idTipo"),
          Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? "" : reader.GetString("descripcion"),
          CantidadAmbientes = reader.IsDBNull(reader.GetOrdinal("cantidadAmbientes")) ? 0 : reader.GetInt32("cantidadAmbientes"),
          Precio = reader.IsDBNull(reader.GetOrdinal("precio")) ? 0 : reader.GetDecimal("precio"),
          Cochera = !reader.IsDBNull(reader.GetOrdinal("cochera")) && reader.GetBoolean("cochera"),
          Piscina = !reader.IsDBNull(reader.GetOrdinal("piscina")) && reader.GetBoolean("piscina"),
          Mascotas = !reader.IsDBNull(reader.GetOrdinal("mascotas")) && reader.GetBoolean("mascotas"),
          Metros2 = reader.IsDBNull(reader.GetOrdinal("metros2")) ? "" : reader.GetString("metros2"),
          UrlImagen = reader.IsDBNull(reader.GetOrdinal("UrlImagen")) ? "" : reader.GetString("UrlImagen"),
          // usar el estado calculado por la consulta
          estado = (EstadoInmueble)(reader.IsDBNull(reader.GetOrdinal("estado_calculado")) ? 1 : reader.GetInt32("estado_calculado")),
          propietario = new Propietarios
          {
            IdPropietario = reader.IsDBNull(reader.GetOrdinal("p_id")) ? 0 : reader.GetInt32("p_id"),
            Nombre = reader.IsDBNull(reader.GetOrdinal("p_nombre")) ? "" : reader.GetString("p_nombre"),
            Apellido = reader.IsDBNull(reader.GetOrdinal("p_apellido")) ? "" : reader.GetString("p_apellido")
          },
          tipo = new Tipo
          {
            IdTipo = reader.IsDBNull(reader.GetOrdinal("idTipo")) ? 0 : reader.GetInt32("idTipo"),
            Observacion = reader.IsDBNull(reader.GetOrdinal("observacion")) ? "" : reader.GetString("observacion")
          }
        };

        lista.Add(inm);
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
                   i.estado AS estado_tabla, i.metros2, i.UrlImagen,
                   d.idDireccion, d.calle, d.altura, d.cp, d.ciudad, d.coordenadas,
                   p.idPropietario AS p_idPropietario, p.apellido, p.nombre,
                   t.idTipo AS t_idTipo, t.observacion AS t_observacion,
                   -- calcular estado real teniendo en cuenta contratos vigentes
                   CASE
                     WHEN EXISTS (
                       SELECT 1 FROM contrato c
                       WHERE c.idInmueble = i.idInmueble
                         AND c.estado = 1
                         AND (c.fechaAnulacion IS NULL OR c.fechaAnulacion = '' OR c.fechaAnulacion = '0000-00-00' OR c.fechaAnulacion = '0000-00-00 00:00:00')
                         AND c.fechaInicio <= CURDATE()
                         AND c.fechaFin >= CURDATE()
                     ) THEN 2
                     WHEN i.estado = 0 THEN 0
                     ELSE 1
                   END AS estado_calculado
            FROM Inmueble i
            INNER JOIN Propietario p ON i.idPropietario = p.idPropietario
            INNER JOIN Tipo t ON i.idTipo = t.idTipo
            INNER JOIN Direccion d ON i.idDireccion = d.idDireccion
            WHERE i.idInmueble=@id
        ";

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
                // usar el estado calculado por la consulta
                estado = (EstadoInmueble)reader.GetInt32("estado_calculado"),

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


    //Devuelve los disponibles que no tienen un contrato vigente
    public List<Inmueble> ListarDisponible(string filtro = "todos")
    {
      var lista = new List<Inmueble>();
      filtro = (filtro ?? "todos").ToLowerInvariant().Trim();

      using var conn = _context.GetConnection();
      conn.Open();

      // condición de contrato vigente (reutilizable)
      string contratoCond = @"EXISTS (
        SELECT 1 FROM contrato c
        WHERE c.idInmueble = i.idInmueble
          AND c.estado = 1
          AND (c.fechaAnulacion IS NULL OR c.fechaAnulacion = '' OR c.fechaAnulacion = '0000-00-00' OR c.fechaAnulacion = '0000-00-00 00:00:00')
          AND c.fechaInicio <= CURDATE()
          AND c.fechaFin >= CURDATE()
    )";

      var sql = $@"
        SELECT DISTINCT
            i.idInmueble, i.descripcion, i.precio, i.cantidadAmbientes, i.cochera, i.UrlImagen,
            i.idTipo, i.idPropietario, i.estado,
            p.idPropietario AS p_id, p.nombre AS p_nombre, p.apellido AS p_apellido,
            t.idTipo AS t_id, t.observacion AS t_obs,
            CASE
                WHEN {contratoCond} THEN 2
                WHEN i.estado = 0 THEN 0
                ELSE 1
            END AS estado_calculado
        FROM inmueble i
        INNER JOIN Propietario p ON i.idPropietario = p.idPropietario
        INNER JOIN Tipo t ON i.idTipo = t.idTipo
    ";

      if (filtro == "alquilados")
      {
        sql += $" WHERE {contratoCond} ";
      }
      else if (filtro == "disponible")
      {
        sql += $" WHERE i.estado = 1 AND NOT ({contratoCond}) ";
      }
      else if (filtro == "nodisponible" || filtro == "nodisponibles" || filtro == "nodisponible")
      {
        sql += " WHERE i.estado = 0 ";
      }
      // else "todos" -> no WHERE adicional

      sql += " ORDER BY i.idInmueble DESC;";

      using var cmd = new MySqlCommand(sql, conn);
      using var reader = cmd.ExecuteReader();
      while (reader.Read())
      {
        var inm = new Inmueble
        {
          IdInmueble = reader.GetInt32("idInmueble"),
          Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString("descripcion"),
          Precio = reader.IsDBNull(reader.GetOrdinal("precio")) ? 0 : reader.GetDecimal("precio"),
          CantidadAmbientes = reader.IsDBNull(reader.GetOrdinal("cantidadAmbientes")) ? 0 : reader.GetInt32("cantidadAmbientes"),
          Cochera = !reader.IsDBNull(reader.GetOrdinal("cochera")) && reader.GetBoolean("cochera"),
          UrlImagen = reader.IsDBNull(reader.GetOrdinal("UrlImagen")) ? null : reader.GetString("UrlImagen"),
          IdTipo = reader.IsDBNull(reader.GetOrdinal("idTipo")) ? 0 : reader.GetInt32("idTipo"),
          IdPropietario = reader.IsDBNull(reader.GetOrdinal("idPropietario")) ? 0 : reader.GetInt32("idPropietario"),
          // usar el estado calculado por la consulta (0/1/2)
          estado = (EstadoInmueble)(reader.IsDBNull(reader.GetOrdinal("estado_calculado")) ? 1 : reader.GetInt32("estado_calculado")),
          propietario = new Propietarios
          {
            IdPropietario = reader.IsDBNull(reader.GetOrdinal("p_id")) ? 0 : reader.GetInt32("p_id"),
            Nombre = reader.IsDBNull(reader.GetOrdinal("p_nombre")) ? "" : reader.GetString("p_nombre"),
            Apellido = reader.IsDBNull(reader.GetOrdinal("p_apellido")) ? "" : reader.GetString("p_apellido")
          },
          tipo = new Tipo
          {
            IdTipo = reader.IsDBNull(reader.GetOrdinal("t_id")) ? 0 : reader.GetInt32("t_id"),
            Observacion = reader.IsDBNull(reader.GetOrdinal("t_obs")) ? "" : reader.GetString("t_obs")
          }
        };

        lista.Add(inm);
      }

      return lista;
    }


    public List<Inmueble> ListarConDireccion(int? estado = null, int? idPropietario = null)
{
    var lista = new List<Inmueble>();
    using var conn = _context.GetConnection();
    conn.Open();

    string sql = @"
        SELECT i.idInmueble, i.idPropietario, i.idDireccion, i.idTipo, i.descripcion, i.cantidadAmbientes,
               i.precio, i.cochera, i.piscina, i.mascotas, i.estado, i.metros2, i.UrlImagen,
               d.idDireccion AS d_id, d.calle AS d_calle, d.altura AS d_altura
        FROM inmueble i
        LEFT JOIN direccion d ON i.idDireccion = d.idDireccion
        WHERE (@estado IS NULL OR i.estado = @estado)
          AND (@idPropietario IS NULL OR i.idPropietario = @idPropietario)
        ORDER BY i.idInmueble DESC;
    ";

    using var cmd = new MySqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@estado", (object?)estado ?? DBNull.Value);
    cmd.Parameters.AddWithValue("@idPropietario", (object?)idPropietario ?? DBNull.Value);

    using var reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        var inm = new Inmueble
        {
            IdInmueble = reader.GetInt32("idInmueble"),
            IdPropietario = reader.IsDBNull(reader.GetOrdinal("idPropietario")) ? 0 : reader.GetInt32("idPropietario"),
            IdDireccion = reader.IsDBNull(reader.GetOrdinal("idDireccion")) ? 0 : reader.GetInt32("idDireccion"),
            IdTipo = reader.IsDBNull(reader.GetOrdinal("idTipo")) ? 0 : reader.GetInt32("idTipo"),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString("descripcion"),
            CantidadAmbientes = reader.IsDBNull(reader.GetOrdinal("cantidadAmbientes")) ? 0 : reader.GetInt32("cantidadAmbientes"),
            Precio = reader.IsDBNull(reader.GetOrdinal("precio")) ? 0 : reader.GetDecimal("precio"),
            Cochera = !reader.IsDBNull(reader.GetOrdinal("cochera")) && reader.GetBoolean("cochera"),
            Piscina = !reader.IsDBNull(reader.GetOrdinal("piscina")) && reader.GetBoolean("piscina"),
            Mascotas = !reader.IsDBNull(reader.GetOrdinal("mascotas")) && reader.GetBoolean("mascotas"),
            Metros2 = reader.IsDBNull(reader.GetOrdinal("metros2")) ? null : reader.GetString("metros2"),
            UrlImagen = reader.IsDBNull(reader.GetOrdinal("UrlImagen")) ? null : reader.GetString("UrlImagen"),
            estado = reader.IsDBNull(reader.GetOrdinal("estado")) ? EstadoInmueble.NoDisponible : (EstadoInmueble)reader.GetInt32("estado")
        };

        if (!reader.IsDBNull(reader.GetOrdinal("d_id")))
        {
            inm.direccion = new Direccion
            {
                IdDireccion = reader.GetInt32("d_id"),
                Calle = reader.IsDBNull(reader.GetOrdinal("d_calle")) ? null : reader.GetString("d_calle"),
                Altura = reader.IsDBNull(reader.GetOrdinal("d_altura")) ? (int?)null : reader.GetInt32("d_altura")
            };
        }

        lista.Add(inm);
    }

    return lista;
}

  }
}