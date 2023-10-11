using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        //enlistara las categorias en el drow
        IEnumerable<SelectListItem> GetListaCategorias();
        //actualizara
        void Update(Categoria categoria);
    }
}
