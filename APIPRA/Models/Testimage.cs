using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIPRA.Models
{
    public class Testimage
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("test_id")]
        public int TestId { get; set; }

        [Column("image_url")]
        [MaxLength(255)]
        public string ImageUrl { get; set; } = null!;

        [Column("metadata", TypeName = "jsonb")]
        public string Metadata { get; set; } = null!;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("TestId")]
        [JsonIgnore]
        public virtual Languagetest Test { get; set; } = null!;  // <--- ВАЖНО
    }

}
