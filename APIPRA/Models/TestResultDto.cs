namespace APIPRA.Models
{
    public class TestResultDto
    {
        public int TestId { get; set; }
        public int Score { get; set; }
        public List<QuestionAnswerDto> Answers { get; set; }
    }
}
