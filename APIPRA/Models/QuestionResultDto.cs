namespace APIPRA.Models
{
    public class QuestionResultDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public string UserAnswer { get; set; }     // Добавить
        public bool IsCorrect { get; set; }
    }
}
