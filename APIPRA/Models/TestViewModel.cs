namespace APIPRA.Models
{
    public class TestViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public List<TestQuestion> Questions { get; set; }
    }
}