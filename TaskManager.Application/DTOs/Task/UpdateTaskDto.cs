namespace TaskManager.Application.DTOs.Task;

public class UpdateTaskDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public List<int>? LabelIds { get; set; }
    public List<string>? LabelNames { get; set; }
}
