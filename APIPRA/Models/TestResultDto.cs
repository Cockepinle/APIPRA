using System.ComponentModel.DataAnnotations;

namespace APIPRA.Models
{
    public class TestResultDto
    {
        [Required]
        public int TestId { get; set; }

        [Required]
        public List<TestAnswerDto> Answers { get; set; }
    }

   
}