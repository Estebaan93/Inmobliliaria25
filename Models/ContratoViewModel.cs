//Models/ContratoViewModel.cs
using System.ComponentModel.DataAnnotations;
using Inmobiliaria25.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace Inmobiliaria25.Models
{
	public class ContratoViewModel
	{
		public Contrato Contrato { get; set; }


		[ValidateNever] // atributo de asp.net, con esto le digo al model che cuando recibas el post y armes 
										// no me valides esto, ya que son listas para llenar los select
		public List<Inquilinos> Inquilinos { get; set; }

		[ValidateNever]
		public List<Inmueble> Inmuebles { get; set; }
	}
}