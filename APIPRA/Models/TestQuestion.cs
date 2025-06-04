using System.ComponentModel.DataAnnotations.Schema;

namespace APIPRA.Models
{
    [Table("testquestions")]
    public class TestQuestion
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public Languagetest Test { get; set; } // Навигационное свойство
        public string Question { get; set; }
        public string Answer { get; set; }
        public string QuestionType { get; set; }
    }
}