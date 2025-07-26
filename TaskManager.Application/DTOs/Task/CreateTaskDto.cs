using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.Task;

public class CreateTaskDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public bool IsCompleted { get; set; } = false;

    public List<int>? LabelIds { get; set; }
}