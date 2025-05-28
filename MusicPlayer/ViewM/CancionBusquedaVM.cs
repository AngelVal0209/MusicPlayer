using MusicPlayer.Models;

namespace MusicPlayer.ViewM
{
    public class CancionBusquedaVM
    {
        public string? Query { get; set; }
        public List<Cancion> Resultados { get; set; } = new();
    }

}

