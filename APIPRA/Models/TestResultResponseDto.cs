namespace APIPRA.Models
{
    public class TestResultResponseDto
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public int Score { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
