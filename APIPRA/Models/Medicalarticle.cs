using System;
using System.Collections.Generic;

namespace APIPRA.Models;

public partial class Medicalarticle
{
    public int Id { get; set; }

    public string Topic { get; set; } = null!;

    public string Content { get; set; } = null!;
}
