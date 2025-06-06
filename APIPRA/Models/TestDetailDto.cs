﻿namespace APIPRA.Models
{
    public class TestDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public List<QuestionDto> Questions { get; set; }
        public string TestType { get; set; }
    }
}
