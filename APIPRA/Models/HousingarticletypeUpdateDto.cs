using System.ComponentModel.DataAnnotations;

namespace APIPRA.Models
{
    public class HousingarticletypeUpdateDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}
