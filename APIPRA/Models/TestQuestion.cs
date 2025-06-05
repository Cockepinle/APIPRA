using System.ComponentModel.DataAnnotations.Schema;

namespace APIPRA.Models
{
    // В APIPRA/Models/TestQuestion.cs
    public class TestQuestion
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string QuestionType { get; set; }
        public List<string> Options { get; set; } // Добавляем это свойство

        // Навигационные свойства
        public Languagetest Test { get; set; }
        public List<UserAnswer> UserAnswers { get; set; }
    }
}