using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")] // ME PERMITE QUE NO NAVEGEN POR LA URL
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        //creamos constructor
        public SlidersController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostingEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnvironment = hostingEnvironment;
        } 

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        //-------------------------------------------------------------METODO CREAR--------------------

        [HttpGet]
        public IActionResult Create()
        {
           
            return View();
        }

        //-------------------------------------------------------------METODO PARA AGREGAR IMAGEN--------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            if (ModelState.IsValid)
            { 
                string rutaPrincipal = _hostingEnvironment.WebRootPath; //para acceder al aparatdo de ruta wwwroot
                var archivos = HttpContext.Request.Form.Files; //acceder a la acarpetas


                    //nuevo slider
                    //guid- para colocar una cadena de carecteres al principio del archivo
                    string nombreArchivo = Guid.NewGuid().ToString();//para el nombre del archivo
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders"); //para guardar la imagen
                    var extension = Path.GetExtension(archivos[0].FileName); //para extraer la extencion .png, jpg

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + extension;
                     
                    _contenedorTrabajo.Slider.Add(slider); //si se sube la imagen nos mnadara a la bista
                    _contenedorTrabajo.save();

                    return RedirectToAction(nameof(Index));
                
            }

            //si sale un error retornamos a la vista
            return View();
        }

        //-------------------------------------------------------------METODO PARA EDITAR--------------------

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            
            //si no es nulo va a buscar el id
            if (id != null) 
            {
                var slider =  _contenedorTrabajo.Slider.Get(id.GetValueOrDefault());
                return View(slider);
            }
            return View();
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath; //para acceder al aparatdo de ruta wwwroot
                var archivos = HttpContext.Request.Form.Files; //acceder a la acarpetas

                var sliderDesdeDb = _contenedorTrabajo.Slider.Get(slider.Id);

                if (archivos.Count() > 0) 
                {
                    //nuevo imagen para el slaider
                    //guid- para colocar una cadena de carecteres al principio del archivo
                    string nombreArchivo = Guid.NewGuid().ToString();//para el nombre del archivo
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders"); //para guardar la imagen
                    var extension = Path.GetExtension(archivos[0].FileName); //para extraer la extencion .png, jpg
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                    //obtenemos la ruta de la imagen
                    var rutaImagen = Path.Combine(rutaPrincipal, sliderDesdeDb.UrlImagen.TrimStart('\\'));

                    //VALIDACION PARA QUE SE ELIME LA ANTERIOR RUTA Y SE PONGA LA NUEVA
                    if(System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    //SUBIMOS EL ARCHIVO

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + extension;
                    
                    _contenedorTrabajo.Slider.Update(slider); //si se sube la imagen nos mnadara a la bista
                    _contenedorTrabajo.save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //CUANDO LA IMAGEN NO SE EDITA O SE CAMBIA
                    slider.UrlImagen = sliderDesdeDb.UrlImagen;
                }
                _contenedorTrabajo.Slider.Update(slider); //si se sube la imagen nos mnadara a la bista
                _contenedorTrabajo.save();

                return RedirectToAction(nameof(Index));
            }
            //para que no se pierda la lista de catalogo al momento de dejar un campo vacio
            return View();
        }
        //aqui se empexaran a realizar tosas las consultas
        //-------------------------------------------------------------en lista--------------------
        #region llamada a ala api
        [HttpGet]
        public IActionResult GetAll()
        {
            //se realiza la reacion con la tabla categorias
            return Json(new { data = _contenedorTrabajo.Slider.GetAll() });
        }

        //Metodo para Borarr 
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //al eliminar el articulo ta,bien se eliminara la imagen
            var sliderDesdeDb = _contenedorTrabajo.Slider.Get(id);
            
            if(sliderDesdeDb == null)
            {
                return Json (new { success = false, message = "Error borrando slider" });
            }


            _contenedorTrabajo.Slider.Remove(sliderDesdeDb);
            _contenedorTrabajo.save();
            return Json(new { success = true, message = "slider borrada correctamente" });
        }

        #endregion
    }
}
