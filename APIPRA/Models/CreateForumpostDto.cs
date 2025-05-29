namespace APIPRA.Models
{
    public class CreateForumpostDto
    {
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}
