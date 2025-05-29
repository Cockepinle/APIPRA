using System.ComponentModel.DataAnnotations;

namespace APIPRA.Models
{
    public class JobvacancyUpdateDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        [StringLength(255)]
        public string EmployerName { get; set; }

        [Required]
        [StringLength(255)]
        public string Location { get; set; }

        [StringLength(255)]
        public string? ContactInfo { get; set; }
    }
}
