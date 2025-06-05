namespace APIPRA.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }

        public int UserTestResultId { get; set; }
        public Usertestresult UserTestResult { get; set; }

        public int QuestionId { get; set; }
        public TestQuestion Question { get; set; }

        public string UserAnswerText { get; set; }
        public bool IsCorrect { get; set; }

        public DateTime CreatedAt { get; set; }
    }

}


