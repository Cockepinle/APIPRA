using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APIPRA.Models;

public partial class Usertestresult
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int TestId { get; set; }
    public int Score { get; set; }
    public DateTime? CompletedAt { get; set; }
    [JsonIgnore]
    public virtual Languagetest Test { get; set; }
    [JsonIgnore]
    public virtual User User { get; set; }
    public ICollection<UserAnswer> UserAnswers { get; set; }


}
