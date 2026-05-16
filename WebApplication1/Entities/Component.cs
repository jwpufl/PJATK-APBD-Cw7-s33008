using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entities;

[Table("Components")]
public class Component
{
    [Key]
    [Column(TypeName = "char(10)")]
    [MaxLength(10)]
    public string Code { get; set; } = null!;

    [Required]
    [MaxLength(300)]
    public string Name { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Description { get; set; } = null!;

    [ForeignKey(nameof(Manufacturer))]
    public int ComponentManufacturersId { get; set; }
    public ComponentManufacturer Manufacturer { get; set; } = null!;

    [ForeignKey(nameof(Type))]
    public int ComponentTypesId { get; set; }
    public ComponentType Type { get; set; } = null!;

    public ICollection<PCComponent> PCComponents { get; set; } = new List<PCComponent>();
}
