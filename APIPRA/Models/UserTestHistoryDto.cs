namespace APIPRA.Models
{
    public class UserTestHistoryDto
    {
        public int TestId { get; set; }
        public string TestName { get; set; }
        public DateTime PassedAt { get; set; }
        public int CorrectCount { get; set; }
        public int TotalCount { get; set; }
    }

}
