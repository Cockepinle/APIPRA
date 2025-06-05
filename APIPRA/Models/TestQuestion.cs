using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APIPRA.Models
{
    public class TestQuestion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Test")]
        public int TestId { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string Question { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string Answer { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string QuestionType { get; set; }

        // Для хранения JSON массива в PostgreSQL
        public List<string> Options { get; set; } = new List<string>();

        [JsonIgnore]
        public virtual Languagetest Test { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}