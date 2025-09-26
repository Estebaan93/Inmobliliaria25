//Controllers/AuditoriaController.cs
using Inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria25.Controllers
{
	[Authorize(Roles = "Administrador")] // Solo administradores pueden acceder
	public class AuditoriaController : Controller
	{
		private readonly RepositorioAuditoria _repoAuditoria;
		private readonly RepositorioUsuario _repoUsuario;

		public AuditoriaController(RepositorioAuditoria repoAuditoria, RepositorioUsuario repoUsuario)
		{
			_repoAuditoria = repoAuditoria;
			_repoUsuario = repoUsuario;
		}



		public IActionResult Index(int? idUsuario = null, string? tipoEntidad = null)
		{
			List<Auditoria> auditorias;
			string filtroAplicado = "";

			// Aplicar filtros
			if (idUsuario.HasValue)
			{
				// Filtrar por usuario
				auditorias = _repoAuditoria.ListarPorIdUsuario(idUsuario.Value);
				var usuario = _repoUsuario.Obtener(idUsuario.Value);
				filtroAplicado = $"Auditorías del Usuario: {usuario?.Apellido} {usuario?.Nombre}";
			}
			else if (!string.IsNullOrEmpty(tipoEntidad))
			{
				// Filtrar solo por tipo de entidad (mostrar todas las entidades de ese tipo)
				try
				{
					var tipoEntidadEnum = Enum.Parse<TipoEntidad>(tipoEntidad);
					auditorias = _repoAuditoria.Listar().Where(a => a.TipoEntidad.ToString() == tipoEntidad).ToList();
					filtroAplicado = $"Auditorías de {tipoEntidad.ToUpper()}S";
				}
				catch
				{
					auditorias = _repoAuditoria.Listar();
					filtroAplicado = "Tipo de entidad no válido. Mostrando todas las auditorías";
				}
			}
			else
			{
				// Mostrar todas las auditorías
				auditorias = _repoAuditoria.Listar();
				filtroAplicado = "Todas las Auditorías";
			}

			List<Usuario> usuarios = _repoUsuario.Listar();

			ViewBag.Filtro = filtroAplicado;
			ViewBag.TipoEntidadSeleccionado = tipoEntidad;
			ViewBag.IdUsuarioSeleccionado = idUsuario;

			AuditoriaUsuarioViewModel auvm = new AuditoriaUsuarioViewModel
			{
				Auditorias = auditorias,
				Usuarios = usuarios
			};

			return View(auvm);
		}

		/*public IActionResult Index(int? idUsuario = null, string? tipoEntidad = null, int? idEntidad = null)
		{
				List<Auditoria> auditorias;
				string filtroAplicado = "";

				// Aplicar filtros
				if (idUsuario.HasValue)
				{
						// Filtrar por usuario
						auditorias = _repoAuditoria.ListarPorIdUsuario(idUsuario.Value);
						var usuario = _repoUsuario.Obtener(idUsuario.Value);
						filtroAplicado = $"Auditorías del Usuario: {usuario?.Apellido} {usuario?.Nombre}";
				}
				else if (!string.IsNullOrEmpty(tipoEntidad) && idEntidad.HasValue)
				{
						// Filtrar por entidad específica
						try
						{
								var tipoEntidadEnum = Enum.Parse<TipoEntidad>(tipoEntidad);
								auditorias = _repoAuditoria.ListarPorEntidad(tipoEntidadEnum, idEntidad.Value);
								filtroAplicado = $"Auditorías del {tipoEntidad.ToUpper()} ID: {idEntidad.Value}";
						}
						catch
						{
								// Si el parseo falla, mostrar todas las auditorías
								auditorias = _repoAuditoria.Listar();
								filtroAplicado = "Tipo de entidad no válido. Mostrando todas las auditorías";
						}
				}
				else if (!string.IsNullOrEmpty(tipoEntidad))
				{
						// Filtrar solo por tipo de entidad (mostrar todas las entidades de ese tipo)
						try
						{
								var tipoEntidadEnum = Enum.Parse<TipoEntidad>(tipoEntidad);
								auditorias = _repoAuditoria.Listar().Where(a => a.TipoEntidad.ToString() == tipoEntidad).ToList();
								filtroAplicado = $"Auditorías de {tipoEntidad.ToUpper()}S";
						}
						catch
						{
								auditorias = _repoAuditoria.Listar();
								filtroAplicado = "Tipo de entidad no válido. Mostrando todas las auditorías";
						}
				}
				else
				{
						// Mostrar todas las auditorías
						auditorias = _repoAuditoria.Listar();
						filtroAplicado = "Todas las Auditorías";
				}

				List<Usuario> usuarios = _repoUsuario.Listar();

				ViewBag.Filtro = filtroAplicado;
				ViewBag.TipoEntidadSeleccionado = tipoEntidad;
				ViewBag.IdEntidadSeleccionado = idEntidad;
				ViewBag.IdUsuarioSeleccionado = idUsuario;

				AuditoriaUsuarioViewModel auvm = new AuditoriaUsuarioViewModel
				{
						Auditorias = auditorias,
						Usuarios = usuarios
				};

				return View(auvm);
		}*/

		// GET: Auditoria/PorContrato/5
		public IActionResult PorContrato(int id)
		{
			var auditorias = _repoAuditoria.ListarPorEntidad(TipoEntidad.contrato, id);
			ViewBag.Filtro = $"Auditorías del Contrato ID: {id}";

			List<Usuario> usuarios = _repoUsuario.Listar();

			AuditoriaUsuarioViewModel auvm = new AuditoriaUsuarioViewModel
			{
				Auditorias = auditorias,
				Usuarios = usuarios
			};

			return View("Index", auvm);
		}

		// GET: Auditoria/PorPago/5
		public IActionResult PorPago(int id)
		{
			var auditorias = _repoAuditoria.ListarPorEntidad(TipoEntidad.pago, id);
			ViewBag.Filtro = $"Auditorías del Pago ID: {id}";

			List<Usuario> usuarios = _repoUsuario.Listar();

			AuditoriaUsuarioViewModel auvm = new AuditoriaUsuarioViewModel
			{
				Auditorias = auditorias,
				Usuarios = usuarios
			};

			return View("Index", auvm);
		}

		// GET: Auditoria/PorUsuario/5
		public IActionResult PorUsuario(int id)
		{
			var auditorias = _repoAuditoria.ListarPorIdUsuario(id);
			var usuario = _repoUsuario.Obtener(id);

			ViewBag.Filtro = $"Auditorías del Usuario: {usuario?.Apellido} {usuario?.Nombre}";

			List<Usuario> usuarios = _repoUsuario.Listar();

			AuditoriaUsuarioViewModel auvm = new AuditoriaUsuarioViewModel
			{
				Auditorias = auditorias,
				Usuarios = usuarios
			};

			return View("Index", auvm);
		}

		// Método auxiliar para registrar auditoría desde otros controladores
		public static void RegistrarAuditoriaDesdeExterno(IServiceProvider serviceProvider, int idEntidad, TipoEntidad tipoEntidad, AccionAuditoria accion, int idUsuario, string observacion = null)
		{
			using (var scope = serviceProvider.CreateScope())
			{
				var repoAuditoria = scope.ServiceProvider.GetRequiredService<RepositorioAuditoria>();
				repoAuditoria.RegistrarAuditoria(idEntidad, tipoEntidad, accion, idUsuario, observacion);
			}
		}
	}
}