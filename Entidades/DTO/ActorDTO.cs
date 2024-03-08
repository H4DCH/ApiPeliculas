using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Entidades.DTO
{
    public class ActorDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string Foto { get; set; }

    }
}
