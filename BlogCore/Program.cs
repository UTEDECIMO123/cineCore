using BlogCore.AccesoDatos.Data.Inizializador;
using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ConexionSQL") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//para el apartado de autenticacion agregamos lo siguente
//AddIdentity(ApplicationUser, IdentityRole)
//AddDefaultUI
//
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();
builder.Services.AddControllersWithViews();


//Agregar contenedor de trabajo para que se pueda ver en la vista 
builder.Services.AddScoped<IContenedorTrabajo, ContenedorTrabajo>();


//aqui va la siembra de datos!!! --paso 1
builder.Services.AddScoped<IInicializadorDB, InicializadorDB>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

//Metodo que ejecuta la siembra de datos, paso 2
SiembraDeDatos();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Cliente}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();


//funcionalidad de metodo siembra de datos
void SiembraDeDatos()
{
    using (var scope = app.Services.CreateScope())
    {
    var inicializadorDb = scope.ServiceProvider.GetRequiredService<IInicializadorDB>();
        inicializadorDb.Inizializar();//IMVOCAMOS
    }
}
