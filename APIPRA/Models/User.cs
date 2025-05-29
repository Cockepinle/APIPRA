using System;
using System.Collections.Generic;

namespace APIPRA.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<Forumpost> Forumposts { get; set; } = new List<Forumpost>();
}
