using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models.ViewModels
{
    public class ArticuloVM
    {
        //obtenemos los datos del articulo
        public Articulo Articulo { get; set; }
        //obtenemos la lista para la lista desplegable DROWDOWN
        public IEnumerable<SelectListItem> ListaCategorias { get; set; }
    }
}
