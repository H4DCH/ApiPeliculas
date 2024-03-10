using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.Helpers;
using PeliculasAPI.Validaciones;

namespace PeliculasAPI.Entidades.DTO
{
    public class PeliculaCreacionDTO
    {
        public int Id { get; set; }
       public string Titulo { get; set; }
        public bool enCines { get; set; }
        public DateTime FechaEstreno { get; set; }
        [PesoArchivoValidacion(PesoMaximo:4)]
        [TipoArchivoValidacion(GrupoArchivos.Imagen)]
        public IFormFile Poster { get; set; }
        [ModelBinder(binderType:typeof(TypeBinder<List<int>>))]
        public List<int> GenerosId { get; set; }
        [ModelBinder(binderType: typeof(TypeBinder<List<ActorPeliculasCreacionDTO>>))]
        public List<ActorPeliculasCreacionDTO> actores { get; set; }
    }
}
