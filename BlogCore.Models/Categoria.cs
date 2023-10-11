using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Categoria
    {
        //EL DISPLAY NOS SIRVE COMO UN PLACEHOLDER
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Ingrese Nombre de la categoria")]
        [Display(Name ="Nombre categoria")]
        public string Nombre { get; set; }

        [Display(Name = "Orden de visualizacion")]
        public int? Order { get; set;}
    }
}
