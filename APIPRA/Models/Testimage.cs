using System;
using System.Collections.Generic;

namespace APIPRA.Models;

public partial class Testimage
{
    public int Id { get; set; }

    public int? TestId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

}
