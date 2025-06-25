using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RaymiMusic.Api.Data;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configura HttpClient para consumir tu API
builder.Services.AddHttpClient("RaymiMusicApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7153/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
//Configurar base de datos para usuarios
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RaymiMusicDb")));
builder.Services.AddSession();


// Registra tus servicios de consumo
builder.Services.AddScoped<IUsuarioApiService, UsuarioApiService>();
builder.Services.AddScoped<IPlanesApiService, PlanesApiService>();
builder.Services.AddScoped<IArtistasApiService, ArtistasApiService>();
builder.Services.AddScoped<ICancionesApiService, CancionesApiService>();
builder.Services.AddScoped<IGenerosApiService, GenerosApiService>();

// Cuando crees servicios para Planes, Artistas, Canciones, etc.,
// registra aquí las líneas equivalentes:
// builder.Services.AddScoped<IPlanesApiService, PlanesApiService>();
// builder.Services.AddScoped<IArtistasApiService, ArtistasApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


//Usuario admin
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RaymiMusic.Api.Data.AppDbContext>();

    // Crear usuario admin si no existe
    if (!context.Usuarios.Any(u => u.Correo == "admin"))
    {
        var planFreeId = context.Planes.First(p => p.Nombre == "Free").Id;

        context.Usuarios.Add(new Usuario
        {
            Id = Guid.NewGuid(),
            Correo = "admin@admin.com",
            HashContrasena = BCrypt.Net.BCrypt.HashPassword("admin"),
            Rol = "Admin",
            PlanSuscripcionId = planFreeId
        });

        context.SaveChanges();
        Console.WriteLine("Usuario admin creado con éxito (admin/admin)");
    }
}


app.UseAuthorization();
app.UseSession();
// Si en el futuro agregas controladores MVC, descomenta esto:
// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
