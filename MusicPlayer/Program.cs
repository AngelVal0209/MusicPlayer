using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MusicPlayer.Data;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Duración si el usuario elige "recordarme"
        options.SlidingExpiration = true; // Renueva la cookie si está activa

        // Solo activa la cookie persistente si tú la habilitas manualmente
        options.Events = new CookieAuthenticationEvents
        {
            OnSigningIn = context =>
            {
                // No se guarda la cookie como persistente, así expira al cerrar navegador
                context.Properties.IsPersistent = false;
                return Task.CompletedTask;
            }
        };
    });



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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=PlaylistAleatoria}/{action=Inicio}/{id?}");

app.Run();
