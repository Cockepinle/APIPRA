namespace APIPRA.Models
{
    public class QuestionDto
    {
        public int Id { get; set; }

        public string Question { get; set; }
        public string Answer { get; set; }
        public string QuestionType { get; set; }
        public List<string> Options { get; set; } // важно, чтобы было в API и Web
    }

}
