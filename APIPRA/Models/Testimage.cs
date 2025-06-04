using System;
using System.Text.Json;

namespace APIPRA.Models
{
    public class Testimage
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        // поле, которое напрямую мапится на колонку metadata (jsonb)
        public string Metadata { get; set; }

        public string ImageUrl { get; set; }
        public int TestId { get; set; }

        public Languagetest Test { get; set; }

        // "виртуальное" свойство для получения description из JSON
        public string Description
        {
            get
            {
                if (string.IsNullOrEmpty(Metadata))
                    return null;
                try
                {
                    using (JsonDocument doc = JsonDocument.Parse(Metadata))
                    {
                        if (doc.RootElement.TryGetProperty("description", out JsonElement desc))
                            return desc.GetString();
                    }
                }
                catch
                {
                    // Если JSON некорректный — вернуть null или пустую строку
                }
                return null;
            }
        }
    }
}
