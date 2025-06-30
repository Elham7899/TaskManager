namespace TaskManager.Domain.Entities;

public class TaskItem
{
    //Primery Key
    public int Id { get; set; }

    //Props
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}
