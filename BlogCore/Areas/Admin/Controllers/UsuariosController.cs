using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")] // ME PERMITE QUE NO NAVEGEN POR LA URL
    [Area("Admin")]
    public class UsuariosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public UsuariosController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        //EN ESTA PARTE ESTAMOS MODIFICANDO LA LISTA DE USUARIO PARA BLOQUEAR Y DESBLOQUEAR
        //EN CASO DE QUE SEAS ADMINISTRADOR EEL NO SE MOSTRARA EL USUARIO EN LA LISTA
        [HttpGet]
        public ActionResult Index()
        {
            //opcion 1: Obtener todos la lista de usuarios

           // return View(_contenedorTrabajo.Usuario.GetAll());

            
            //OPCION 2; se ontiene la lista de usuarios esepto el que esta autenticado
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(_contenedorTrabajo.Usuario.GetAll(u => u.Id != usuarioActual.Value));

        }
        public ActionResult Bloquear(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.BloquearUsuario(Id);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Desbloquear(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.DesbloquearUsuario(Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
