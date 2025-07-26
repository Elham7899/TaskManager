using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.Label;

public class CreateLabelDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}