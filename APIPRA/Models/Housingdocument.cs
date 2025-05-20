using System;
using System.Collections.Generic;

namespace APIPRA.Models;

public partial class Housingdocument
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string FileUrl { get; set; } = null!;
}
