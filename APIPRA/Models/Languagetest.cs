using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIPRA.Models;

public partial class Languagetest
{
    public int Id { get; set; }
    public string Name { get; set; }
    [Column("type")]

    public string Type { get; set; }
    public ICollection<Testimage> Testimages { get; set; } = new List<Testimage>();

}
