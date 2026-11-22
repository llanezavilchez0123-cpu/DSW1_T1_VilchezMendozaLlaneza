
using System.ComponentModel.DataAnnotations;

namespace DSW1_T1_VilchezMendozaLlaneza.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingrese el nombre del libro")]
        [StringLength(120)]
        public string NombreLibro { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese el autor")]
        [StringLength(100)]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese la editorial")]
        [StringLength(100)]
        public string NombreEditorial { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese el ISDN")]
        [StringLength(20)]
        public string ISDN { get; set; } = string.Empty;
    }
}
