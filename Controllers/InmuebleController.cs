//Controllers/InmuebleController.cs
using inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria25.Controllers
{
  //Definimos la clase que hereda de controller
  public class InmuebleController : Controller
  {
    //Variable priva que permite que el controller use los metodos del repo
    private readonly RepositorioInmueble repo;

    //Constructor del controlador asp.net inyecta dependencias
    public InmuebleController(RepositorioInmueble repo)
    {
      this.repo = repo;
    }
    
    //Listar activos
    public IActionResult Index(){
      //Llamamos al metodo
      //var lista= repo.ObtenerActivos();
      return View();
      
    }
 



  }



}