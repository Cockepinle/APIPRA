using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIPRA.Models;

[Table("jobvacancies")]
public class Jobvacancy
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [Column("employer_name")]
    [StringLength(255)]
    public string EmployerName { get; set; } = null!;

    [Required]
    [Column("location")]
    [StringLength(255)]
    public string Location { get; set; } = null!;

    [Column("contact_info")]
    [StringLength(255)]
    public string? ContactInfo { get; set; }

    [Column("created_at", TypeName = "timestamp with time zone")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? CreatedAt { get; set; }
}