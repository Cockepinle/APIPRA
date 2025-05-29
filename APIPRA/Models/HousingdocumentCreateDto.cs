using System.ComponentModel.DataAnnotations;

namespace APIPRA.Models
{
    public class HousingdocumentCreateDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string FileUrl { get; set; }
    }
}
