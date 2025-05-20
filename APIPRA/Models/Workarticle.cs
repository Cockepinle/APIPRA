using System;
using System.Collections.Generic;

namespace APIPRA.Models;

public partial class Workarticle
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string Category { get; set; } = null!;
}
