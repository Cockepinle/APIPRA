using System;
using System.Collections.Generic;
using APIPRA.Models;

namespace APIPRA.Models;

public partial class Forumpost
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
    public User User { get; set; }  // Навигационное свойство

}

