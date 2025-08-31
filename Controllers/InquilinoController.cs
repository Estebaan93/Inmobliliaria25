using Inmobiliaria25.Models;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria25.Controllers
{
    // defino la clase que hereda de controller
    public class InquilinoController : Controller
    {
        //var privada y de lectura, permite q el controller use los metodos
        private readonly RepositorioInquilino repo;

        // constructior del controllador,  ASP .net inyecta dependencia, pasa autom objeto al repo y se guarda 
        // en la var this.repo para ser usado por todod los metodos del controller
        public InquilinoController(RepositorioInquilino repo)
        {
            this.repo = repo;
        }

        // Listar activos
        public IActionResult Index()
        {
            // llama al metodo, este consulta sql devuelve una lista que se guarda en la var lista
            var lista = repo.ObtenerActivos();
            return View(lista);
        }

        // Crear (GET)
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        // al hacer click recibe un obj Inq inq
        public IActionResult Guardar(Inquilinos inquilino)
        {
            try
            {
                // pregunta inq tiene id mayor a 0?, si es si quiere decir q ya existe, si dice no es nuevo
                if (inquilino.idInquilino > 0)
                {
                    // EDItAR // consulta al repo si hay otro inq con ese dni aparte del que se edita
                    if (repo.ObtenerPorDni(inquilino.dni, inquilino.idInquilino))
                    {
                        ModelState.AddModelError("dni", "El DNI ya está registrado en otro inquilino.");
                        return View("Editar", inquilino);
                    }

                    repo.Modificar(inquilino);
                    TempData["Mensaje"] = "Inquilino modificado con éxito";
                }
                else
                {
                    // CREAR // consulta al repo si hay un inqu con ese dni
                    if (repo.ObtenerPorDni(inquilino.dni))
                    {
                        ModelState.AddModelError("dni", "El DNI ya está registrado. Revise la tabla de propietarios.");
                        return View("Crear", inquilino);
                    }

                    repo.Alta(inquilino);
                    TempData["Mensaje"] = "Propietario creado con éxito";
                }

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // Editar (GET)
        // ESTE ES CUANDO HAGO CLICK
        public IActionResult Editar(int id)
        {
            //llama al repo , la respuesta la guarda en la var inquilino
            var inquilino = repo.ObtenerPorId(id);
            if (inquilino == null)
            {
                return NotFound();
            }
            return View(inquilino);
        }

        // Editar (POST)
        //ESTE CUANDO YA SE EDITA EN EL FORM
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Inquilinos inquilino)
        {
            if (!ModelState.IsValid)
            {
                return View(inquilino);
            }

            repo.Modificar(inquilino);
            TempData["Success"] = "Inquilino modificado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // Baja 
        [HttpPost]
        public IActionResult Borrar(int id)
        {
            repo.Baja(id);
            TempData["Success"] = "Inquilino eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }


        // Listar dados de baja
        [HttpGet]
        public IActionResult DadosDeBaja()
        {
            var lista = repo.ObtenerDadosDeBaja();
            return View("Index", lista); // reuso no se me genera una nueva vista
        }

        //buscar x dni
        public IActionResult BuscarPorDni(string dni)
        {
            if (string.IsNullOrEmpty(dni))
            {
                TempData["Error"] = "Debe ingresar un DNI para buscar.";
                return RedirectToAction("Index");
            }

            var lista = repo.BuscarPorDni(dni);
            return View("Index", lista); // reusa la vista Index
        }

        // buscar x apellido
        public IActionResult BuscarPorApellido(string apellido)
        {
            if (string.IsNullOrEmpty(apellido))
            {
                TempData["Error"] = "Debe ingresar un apellido para buscar.";
                return RedirectToAction("Index");
            }

            var lista = repo.BuscarPorApellido(apellido);
            return View("Index", lista);
        }

        // buscar x mail
        public IActionResult BuscarPorEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Debe ingresar un correo para buscar.";
                return RedirectToAction("Index");
            }

            var lista = repo.BuscarPorEmail(email);
            return View("Index", lista);
        }
        public IActionResult Detalles(int id)
        {
            Inquilinos inquilino = repo.ObtenerPorId(id);
            if (inquilino != null)
            {
                return View(inquilino);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult Alta(int id)
        {
            try
            {
                Inquilinos i = repo.ObtenerPorId(id);
                i.estado = true;
                int filasAfectadas = repo.Reactivar(i);
            }
            catch (System.Exception)
            {
                ModelState.AddModelError("dni", "Error al querer dar el alta");
            }
            return RedirectToAction("Index", "Inquilino");
        }


    }
}
