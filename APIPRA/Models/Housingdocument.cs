using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIPRA.Models;

[Table("housingdocuments")]
public class Housingdocument
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Required]
    [Column("file_url")]
    [StringLength(255)]
    public string FileUrl { get; set; } = null!;
}