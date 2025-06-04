namespace APIPRA.Models
{
    public class TestResultDetailDto
    {
        public int Id { get; set; }
        public int TestId { get; set; }  // Добавьте это поле
        public string TestName { get; set; }
        public int Score { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
