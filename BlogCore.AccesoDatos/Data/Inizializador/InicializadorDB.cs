using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Inizializador
{
    public class InicializadorDB : IInicializadorDB
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManger;

        //CREAMOS EL CONSTRUCTOR
        public InicializadorDB(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManger = roleManager;

             
        }

        public void Inizializar()
        {
            try
            {
                //si tenemos migraciones PENDIENTES SE VAN A REALIZAR
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }

            }catch (Exception)
            {
            }
            //SI ENCUTRA ALGUN ROL QUE TENGA ADMIN se van crear los siguientes roles
            if (_db.Roles.Any(ro => ro.Name == CNT.Admin)) return;

            //creacion de roles
            _roleManger.CreateAsync(new IdentityRole(CNT.Admin)).GetAwaiter().GetResult();
            _roleManger.CreateAsync(new IdentityRole(CNT.Usuario)).GetAwaiter().GetResult();

            //creacion del usuario inicial
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin_esr@gmail.com",
                Email    = "admin_esr@gmail.com",
                EmailConfirmed = true,
                Nombre   = "admin_esr"
            }, "Admin123*").GetAwaiter().GetResult();

            //agreagmos el usuario creado al rol de admin
            ApplicationUser usuario = _db.ApplicationUser
                .Where(us => us.Email == "admin_esr@gmail.com")
                .FirstOrDefault();
            _userManager.AddToRoleAsync(usuario, CNT.Admin).GetAwaiter().GetResult();
        }
    }
}
