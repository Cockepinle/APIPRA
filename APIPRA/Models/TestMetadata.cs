using System.Collections.Generic;

namespace APIPRA.Models
{
    public class TestMetadata
    {
        public string TestType { get; set; }
        public List<TestQuestionMetadata> Questions { get; set; }
    }

}