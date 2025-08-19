using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.Task;

public class CreateTaskDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }

    public List<int>? LabelIds { get; set; }
    public List<string>? LabelNames { get; set; }
}