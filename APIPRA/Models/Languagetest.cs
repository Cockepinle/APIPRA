using System;
using System.Collections.Generic;

namespace APIPRA.Models;

public partial class Languagetest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Testimage> Testimages { get; set; } = new List<Testimage>();
}
