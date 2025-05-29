using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIPRA.Models;

[Table("forumreplies")]
public partial class Forumreply
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("post_id")]
    public int? PostId { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("content")]
    [Required]
    public string Content { get; set; } = null!;

    [Column("created_at", TypeName = "timestamp with time zone")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? CreatedAt { get; set; }
}