using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PeliculasAPI.Entidades
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string Foto { get; set; }  
        public List<PeliculasActores> PeliculasActores { get; set; }


    }
}
