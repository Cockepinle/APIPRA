using System.ComponentModel.DataAnnotations.Schema;

namespace APIPRA.Models
{
    [Table("testquestions")]
    public class TestQuestion
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Question { get; set; } = null!;
        public string Answer { get; set; } = null!;
        public string QuestionType { get; set; } = null!;
    }



}