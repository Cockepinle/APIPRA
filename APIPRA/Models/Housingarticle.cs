using APIPRA.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace APIPRA.Models;

[Table("housingarticles")]
public partial class Housingarticle
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = null!;

    [Column("content")]
    [Required]
    public string Content { get; set; } = null!;

    [Column("type_id")]
    public int? TypeId { get; set; }

    [ForeignKey("TypeId")]
    public virtual Housingarticletype? ArticleType { get; set; }
}