using PeliculasAPI.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Entidades.DTO
{
    public class ActorCreacionDTO
    {
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public DateTime fechaNacimiento { get; set; }
        [PesoArchivoValidacion(PesoMaximo:4)]
        [TipoArchivoValidacion(grupoArchivos:GrupoArchivos.Imagen)]
        public IFormFile Foto {  get; set; }
    }
}
