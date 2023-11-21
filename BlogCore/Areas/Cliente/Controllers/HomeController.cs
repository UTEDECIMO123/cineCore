using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace BlogCore.Areas.Cliente.Controllers
{
    [Area("Cliente")] // para que permitar ejecutar el proyecto
    public class HomeController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public HomeController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;

        }

        public IActionResult Details( int Id)
        {
            var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(Id);
            return View(articuloDesdeDb);
          
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index(string searchString)
        {
            // Instanciamos el VM que se acaba de crear
            HomeVM homeVM = new HomeVM()
            {
                Slider = _contenedorTrabajo.Slider.GetAll(),
                ListaArticulos = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Categoria").Where(a => string.IsNullOrEmpty(searchString) || a.Nombre.ToLower().Contains(searchString.ToLower())),
            };

            ViewBag.IsHome = true;

            return View(homeVM);
        }
    }
}