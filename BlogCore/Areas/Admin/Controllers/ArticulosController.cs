using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")] // ME PERMITE QUE NO NAVEGEN POR LA URL
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        //creamos constructor
        public ArticulosController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostingEnvironment)
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
            //importamos el vm, asignamos variable y creamos una instancia
            ArticuloVM artivm = new ArticuloVM()
            {
                //para acceder a las propiedades de articulo y acceder a lista
                Articulo = new BlogCore.Models.Articulo(), 
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };
            return View(artivm);
        }

        //-------------------------------------------------------------METODO PARA AGREGAR IMAGEN--------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticuloVM artiVM)
        {
            if (ModelState.IsValid)
            { 
                string rutaPrincipal = _hostingEnvironment.WebRootPath; //para acceder al aparatdo de ruta wwwroot
                var archivos = HttpContext.Request.Form.Files; //acceder a la acarpetas

                if(artiVM.Articulo.Id == 0)
                {
                    //nuevo articulo
                    //guid- para colocar una cadena de carecteres al principio del archivo
                    string nombreArchivo = Guid.NewGuid().ToString();//para el nombre del archivo
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos"); //para guardar la imagen
                    var extension = Path.GetExtension(archivos[0].FileName); //para extraer la extencion .png, jpg

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    artiVM.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                    artiVM.Articulo.FechaDeCreacion = DateTime.Now.ToString(); //PARA NO TENER PROBLEMAS CON LA FECHA
                     
                    _contenedorTrabajo.Articulo.Add(artiVM.Articulo); //si se sube la imagen nos mnadara a la bista
                    _contenedorTrabajo.save();

                    return RedirectToAction(nameof(Index));
                }
            }
            //para que no se pierda la lista de catalogo al momento de dejar un campo vacio
            artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(artiVM);
        }

        //-------------------------------------------------------------METODO PARA EDITAR--------------------

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticuloVM artiVM =  new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };
            //si no es nulo va a buscar el id
            if (id != null) 
            {
                artiVM.Articulo =  _contenedorTrabajo.Articulo.Get(id.GetValueOrDefault());
            }
            return View(artiVM);
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticuloVM artiVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath; //para acceder al aparatdo de ruta wwwroot
                var archivos = HttpContext.Request.Form.Files; //acceder a la acarpetas

                var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(artiVM.Articulo.Id);

                if (archivos.Count() > 0) 
                {
                    //nuevo imagen para el articiulo
                    //guid- para colocar una cadena de carecteres al principio del archivo
                    string nombreArchivo = Guid.NewGuid().ToString();//para el nombre del archivo
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos"); //para guardar la imagen
                    var extension = Path.GetExtension(archivos[0].FileName); //para extraer la extencion .png, jpg
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                    //obtenemos la ruta de la imagen
                    var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));

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

                    artiVM.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                    artiVM.Articulo.FechaDeCreacion = DateTime.Now.ToString(); //PARA NO TENER PROBLEMAS CON LA FECHA

                    _contenedorTrabajo.Articulo.Update(artiVM.Articulo); //si se sube la imagen nos mnadara a la bista
                    _contenedorTrabajo.save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //CUANDO LA IMAGEN NO SE EDITA O SE CAMBIA
                    artiVM.Articulo.UrlImagen = articuloDesdeDb.UrlImagen;
                }
                _contenedorTrabajo.Articulo.Update(artiVM.Articulo); //si se sube la imagen nos mnadara a la bista
                _contenedorTrabajo.save();

                return RedirectToAction(nameof(Index));
            }

            //para que no se pierda la lista de catalogo al momento de dejar un campo vacio
            artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(artiVM);
        }



        //aqui se empexaran a realizar tosas las consultas
        //-------------------------------------------------------------en lista--------------------
        #region llamada a ala api
        [HttpGet]
        public IActionResult GetAll()
        {
            //se realiza la reacion con la tabla categorias
            return Json(new { data = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Categoria") });
        }

        //Metodo para Borarr 
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //al eliminar el articulo ta,bien se eliminara la imagen
            var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(id);
            string rutaDirectorioPrincipal = _hostingEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));

            //VALIDACION PARA QUE SE ELIME LA ANTERIOR RUTA Y SE PONGA LA NUEVA
            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }

            if(articuloDesdeDb == null)
            {
                return Json (new { success = false, message = "Error borrando articulo" });
            }


            _contenedorTrabajo.Articulo.Remove(articuloDesdeDb);
            _contenedorTrabajo.save();
            return Json(new { success = true, message = "Articulo borrada correctamente" });
        }

        #endregion
    }
}
