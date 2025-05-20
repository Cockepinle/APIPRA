using System;
using System.Collections.Generic;

namespace APIPRA.Models;

public partial class Userquestion
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Question { get; set; } = null!;

    public string? Answer { get; set; }

    public string Status { get; set; } = null!;

}
