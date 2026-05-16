using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

public class PcUpdateDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    [Range(0, double.MaxValue)]
    public double Weight { get; set; }

    [Range(0, int.MaxValue)]
    public int Warranty { get; set; }

    public DateTime CreatedAt { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}
