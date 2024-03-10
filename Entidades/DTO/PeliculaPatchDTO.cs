using PeliculasAPI.Validaciones;

namespace PeliculasAPI.Entidades.DTO
{
    public class PeliculaPatchDTO
    {
        public string Titulo { get; set; }
        public bool enCines { get; set; }
        public DateTime FechaEstreno { get; set; }
    }
}
