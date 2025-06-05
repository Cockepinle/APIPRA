namespace APIPRA.Models
{
    public class AnswerRequest
    {
        public int QuestionId { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
