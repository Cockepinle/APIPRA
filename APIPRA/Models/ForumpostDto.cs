namespace APIPRA.Models
{
    public class ForumpostDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public UserSimpleDto? User { get; set; }
    }
}
