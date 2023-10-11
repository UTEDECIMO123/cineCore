using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Articulo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre del Articulo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage ="La descripcion es obligatoria")]
        public string Description { get; set; }

        [Display(Name = "Fecha de Creacion")]
        public string FechaDeCreacion {  get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string UrlImagen { get; set; }

        //SE REALIZA LA REALCION ENTRE TABLAS CATEGORIAS-ARTICULOS
        //SE CREA EL ID
        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int CategoriaId {  get; set; }

        //articulo depende de categoria
        //invocamos a categoria
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }
    }
}
