using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Data;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")] // ME PERMITE QUE NO NAVEGEN POR LA URL
    [Area("Admin")]
    public class CategoriasController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;


        //creamos constructor
        public CategoriasController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
           
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //--------------------------------------------------------------METODO PARA CREAR -> FORMULARIO 
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            if(ModelState.IsValid)
            {
                _contenedorTrabajo.Categoria.Add(categoria);
                _contenedorTrabajo.save();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        //--------------------------------------------------------------METODO PARA Editar -> FORMULARIO
        [HttpGet]
        public IActionResult Edit(int id) //recibe como parametro id
        {
            //instanciamos categoria dentro una variable
            //
            //un OBJETO a partir de un una clase
            Categoria categoria = new Categoria();
            categoria = _contenedorTrabajo.Categoria.Get(id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Categoria.Update(categoria);
                _contenedorTrabajo.save();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }


        //aqui se empexaran a realizar tosas las consultas
        //-------------------------------------------------------------en lista--------------------
        #region llamada a ala api
        [HttpGet]
        public IActionResult GetALL()
        {
            //OPCION 1
            return Json(new { data = _contenedorTrabajo.Categoria.GetAll() });
        }

        //Metodo para Borarr 
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _contenedorTrabajo.Categoria.Get(id);
            if (objFromDb == null) 
            {
                return Json(new { success = false, message = "Error borrando la categoria" });
            }

            _contenedorTrabajo.Categoria.Remove(objFromDb);
            _contenedorTrabajo.save();
            return Json(new { success = true, message = "categoria borrada correctamente" });
        }

        #endregion
    }
}
 