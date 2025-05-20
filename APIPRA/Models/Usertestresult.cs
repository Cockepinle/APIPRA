using System;
using System.Collections.Generic;

namespace APIPRA.Models;

public partial class Usertestresult
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? TestId { get; set; }

    public int Score { get; set; }

    public DateTime? CompletedAt { get; set; }

}
