namespace APIPRA.Models
{
    public class TestResultDetailsDto
    {
        public int TestId { get; set; }
        public string TestName { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime CompletedAt { get; set; }
        public List<QuestionResultDto> Questions { get; set; }
    }
}
