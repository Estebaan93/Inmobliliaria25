using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Inmobiliaria25.Models
{
	public enum EstadoInmueble
	{
		NoDisponible = 0,
		Disponible = 1,
		Alquilado = 2
	}

	public class Inmueble
	{
		[Key]
		public int IdInmueble { get; set; }
		public int IdPropietario { get; set; }
		public int IdDireccion { get; set; }
		public int IdTipo { get; set; }
		public string Metros2 { get; set; }
		public int CantidadAmbientes { get; set; }
		public Decimal Precio { get; set; }
		public string Descripcion { get; set; }
		public Boolean Cochera { get; set; }
		public Boolean Piscina { get; set; }
		public Boolean Mascotas { get; set; }
		public string UrlImagen { get; set; }
		public Propietarios? propietario { get; set; }
		public Tipo? tipo { get; set; }
		public Direccion? direccion { get; set; }
		public EstadoInmueble estado { get; set; }

		//para que me traiga bien todo

		[ValidateNever]  // lo pomgo para inmueble, aqui le digo a ASPNET no me toques esta propiedad la manejo yo, PERO SI LA USO PARA CONTRATO PORQUE AHI SE HACE JOIN 
		public string DescripcionCompleta => $"{tipo.Observacion} - {Descripcion}";
	}
}
