using Inmobiliaria25.Db;
using Inmobiliaria25.Repositorios;
using Microsoft.AspNetCore.Authentication.Cookies;

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


// Add services to the container.
builder.Services.AddControllersWithViews(); //CONTROLADOR


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();