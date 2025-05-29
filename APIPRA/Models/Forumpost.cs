using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using APIPRA.Models;

namespace APIPRA.Models;

public class Forumpost
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;

    [Column(TypeName = "timestamp with time zone")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}