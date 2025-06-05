using System.ComponentModel.DataAnnotations;

namespace APIPRA.Models
{
    public class TestAnswerDto
    {
        [Required]
        public int QuestionId { get; set; }

        [Required]
        public string Answer { get; set; }
    }
}
