using System.ComponentModel.DataAnnotations;

namespace APIPRA.Models
{
    public class HousingarticletypeCreateDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}
