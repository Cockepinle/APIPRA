using System.ComponentModel.DataAnnotations;

namespace APIPRA.Models
{
    public class TestResultDto
    {
        public int TestId { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
    }


}