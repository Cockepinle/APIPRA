namespace APIPRA.Models
{
    public class TestQuestion
    {
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
    }
}