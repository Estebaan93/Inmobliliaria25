//program.cs
using Inmobiliaria25.Db;
using Inmobiliaria25.Repositorios;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

//REPOSITORIOS Y SERVICIOS
builder.Services.AddSingleton<DataContext>();
builder.Services.AddTransient<RepositorioInquilino>();
builder.Services.AddTransient<RepositorioPropietario>();
builder.Services.AddTransient<RepositorioInmueble>();
builder.Services.AddScoped<RepositorioTipo>();
builder.Services.AddScoped<RepositorioDireccion>();
builder.Services.AddScoped<RepositorioContrato>();
builder.Services.AddScoped<RepositorioPago>();
builder.Services.AddScoped<RepositorioAuditoria>();
builder.Services.AddScoped<RepositorioUsuario>();
builder.Services.AddScoped<RepositorioLogin>();


//Autenticacion cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(options =>
  {
    options.LoginPath = "/Login/Index"; // si no está logueado redirige acá
    options.LogoutPath = "/Login/Logout";
    options.AccessDeniedPath = "/Home/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // tiempo de sesión 
  });

//Politicsa de autorizaxion administradores
  builder.Services.AddAuthorization(options=>
  {
    //solo administradores
    options.AddPolicy("Administrador", policy =>
      policy.RequireClaim(ClaimTypes.Role, "Administrador"));
      options.AddPolicy("Empleado", policy =>
      policy.RequireClaim(ClaimTypes.Role, "Empleado","Administrador"));
  });

//Exigir autenticacion en todos los controladores
/*builder.Services.AddControllersWithViews(options=>{
  var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
  options.Filters.Add(new AuthorizeFilter(policy));
});*/


// controlador con vistas
builder.Services.AddControllersWithViews(); //CONTROLADOR

//Inyectar HttpContext en los controladores
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//El orden en este sentido
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();