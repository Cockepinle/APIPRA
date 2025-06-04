namespace APIPRA.Models
{
    public class TestQuestion
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string QuestionText { get; set; }      // добавить вместо Question
        public List<string> Options { get; set; }     // добавить
        public string CorrectAnswer { get; set; }     // добавить
        public string QuestionType { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}