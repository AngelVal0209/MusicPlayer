using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPlayer.Data;
using MusicPlayer.Models;
using MusicPlayer.ViewM;
using Microsoft.AspNetCore.Authorization;

namespace MusicPlayer.Controllers
{
    [Authorize]
    public class CancionController : Controller
    {
        private readonly AppDBContext _context;

        public CancionController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Buscar(string? query)
        {
            var canciones = _context.Canciones.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                canciones = canciones.Where(c =>
                    c.Titulo.Contains(query) || c.Artista.Contains(query));
            }

            var modelo = new CancionBusquedaVM
            {
                Query = query,
                Resultados = await canciones.ToListAsync()
            };


            return View(modelo);
        }
        [HttpGet]
        public async Task<IActionResult> Sugerencias(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(new string[0]);

            var canciones = await _context.Canciones
                .Where(c => c.Titulo.Contains(query) || c.Artista.Contains(query))
                .Select(c => $"{c.Titulo} - {c.Artista}")
                .Distinct()
                .Take(10)
                .ToListAsync();

            return Json(canciones);
        }

    }
}
