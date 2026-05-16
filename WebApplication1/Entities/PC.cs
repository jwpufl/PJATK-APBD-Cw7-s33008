using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entities;

[Table("PCs")]
public class PC
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "float(5)")]
    public double Weight { get; set; }

    public int Warranty { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    public int Stock { get; set; }

    public ICollection<PCComponent> PCComponents { get; set; } = new List<PCComponent>();
}
