using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIPRA.Models;

public class Languagetest
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [MaxLength(255)]
    public string Name { get; set; } = null!;

    [Column("type")]
    [MaxLength(50)]
    public string Type { get; set; } = null!;

    // Связь с изображениями теста
    public virtual ICollection<Testimage> Testimages { get; set; } = new List<Testimage>();

    // Добавляем связь с вопросами теста
    public virtual ICollection<TestQuestion> TestQuestions { get; set; } = new List<TestQuestion>();
}

