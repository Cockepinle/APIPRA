using System.ComponentModel.DataAnnotations.Schema;

namespace APIPRA.Models
{
    [Table("testquestions")]
    public class TestQuestion
    {
        public int Id { get; set; }

        public string Question { get; set; }
        public string Answer { get; set; }

        // 👇 Обязательное поле — внешний ключ
        public int TestId { get; set; }

        // 👇 Навигационное свойство (опционально)
        public Languagetest Test { get; set; }
        public string QuestionType { get; set; }

    }
}