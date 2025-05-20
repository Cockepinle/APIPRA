using System;
using System.Collections.Generic;

namespace APIPRA.Models;

public partial class Forumreply
{
    public int Id { get; set; }

    public int? PostId { get; set; }

    public int? UserId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
