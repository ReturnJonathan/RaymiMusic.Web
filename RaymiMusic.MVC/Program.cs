using System;
using RaymiMusic.MVC.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configura HttpClient para consumir tu API
builder.Services.AddHttpClient("RaymiMusicApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7153/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

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

app.UseAuthorization();

// Si en el futuro agregas controladores MVC, descomenta esto:
// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
