using BlogCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BlogCore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        //AQUI PONEMOS EL MODELO QUE VAMOS A USAR o que estemos creando
        public DbSet<Categoria> Categoria { get; set; }

        public DbSet<Articulo> Articulo { get; set; }

        public DbSet<Slider> Slider {  get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

    }
}