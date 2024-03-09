using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Entidades.DTO
{
    public class ActorPatchDTO
    {
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public DateTime fechaNacimiento { get; set; }
    }
}
