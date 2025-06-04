using System.Collections.Generic;

namespace APIPRA.Models
{
    public class TestMetadata
    {
        public string TestType { get; set; } // "multiple-choice", "crossword", "fill-in"
        public string Description { get; set; } // Добавьте это

        public List<TestQuestion> Questions { get; set; }
    }

    
}