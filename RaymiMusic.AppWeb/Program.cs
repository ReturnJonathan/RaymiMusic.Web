using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RaymiMusic.Api.Data;
using RaymiMusic.AppWeb.Services;
using RaymiMusic.Modelos;          // ← tu proyecto de entidades
using RaymiMusic.AppWeb.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Numerics;
using RaymiMusic.AppWeb;
using System.Text;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

/* ---------- Services ---------- */

// MVC
builder.Services.AddControllersWithViews();

// EF Core – SQL Server
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(
        builder.Configuration.GetConnectionString("RaymiMusicDb")));

// Autenticación por cookies
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Account/Login";
        opt.LogoutPath = "/Account/Logout";
        // opt.ExpireTimeSpan = TimeSpan.FromHours(2); // ← opcional
    });

builder.Services.AddAuthorization();


builder.Services.Configure<SendGridOptions>(
    builder.Configuration.GetSection("SendGrid"));
builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();

var app = builder.Build();

/* ---------- Middleware ---------- */

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();                       // 30 días por defecto
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();                 // ⚠️ primero autenticación
app.UseAuthorization();                  // luego autorización

/* ---------- Endpoints ---------- */

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// —– Seed automático del plan “Free” —–
//using (var scope = app.Services.CreateScope())
//{
//    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//    // Ojo: tu DbSet se llama 'Planes' y la clase es PlanSuscripcion
//    if (!ctx.Planes.Any(p => p.Nombre == "Free"))
//    {
//        ctx.Planes.Add(new PlanSuscripcion
//        {
//            Id = Guid.NewGuid(),
//            Nombre = "Free",
//            Precio = 0m,
//            DescargasMaximas = 0
//        });
//        ctx.SaveChanges();
//    }
//}


// —— Seed automático del plan “Free” ——
//using (var scope = app.Services.CreateScope())
//{
//    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//    // — Seed plan Free —
//    if (!ctx.Planes.Any(p => p.Nombre == "Free"))
//    {
//        ctx.Planes.Add(new PlanSuscripcion
//        {
//            Id = Guid.NewGuid(),
//            Nombre = "Free",
//            Precio = 0m,
//            DescargasMaximas = 0
//        });
//        ctx.SaveChanges();
//    }

//    // — Seed admin y confírmalo automáticamente —
//    if (!ctx.Usuarios.Any(u => u.Correo == "admin@admin.com"))
//    {
//        var freeId = ctx.Planes.First(p => p.Nombre == "Free").Id;
//        var adminId = Guid.NewGuid();

//        // 1) Crear admin
//        ctx.Usuarios.Add(new Usuario
//        {
//            Id = adminId,
//            Correo = "admin@admin.com",
//            HashContrasena = ComputeHash("123456"),
//            Rol = Roles.Admin,
//            PlanSuscripcionId = freeId
//        });

//        // 2) Auto-confirmar su email sin enviar nada
//        ctx.EmailConfirmations.Add(new EmailConfirmation
//        {
//            UsuarioId = adminId,
//            Token = "",
//            Expiration = DateTime.UtcNow.AddYears(1),
//            IsConfirmed = true,
//            Purpose = ConfirmationPurpose.EmailVerification
//        });

//        ctx.SaveChanges();
//    }
//}
//static string ComputeHash(string plain)
//{
//    using var sha = SHA256.Create();
//    var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(plain));
//    return Convert.ToBase64String(bytes);
//}


app.Run();
