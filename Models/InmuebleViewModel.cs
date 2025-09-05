//Models/InmuebleViewModel.cs
using System.Collections.Generic;
using Inmobiliaria25.Models;

namespace Inmobiliaria25.Models
{
	public class InmuebleViewModel
	{
		public List<Inmueble> Inmuebles { get; set; } = new();
		public List<Propietarios> Propietarios { get; set; } = new();

		
		/*PAGNADO
		public int CurrentPage { get; set; }
    public int TotalPages  { get; set; }
    public int TotalItems  { get; set; }
    public int PageSize    { get; set; }*/
	}
	

}
