using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Entidades.DTO
{
    public class GeneroDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Nombre { get; set; }
    }
}
