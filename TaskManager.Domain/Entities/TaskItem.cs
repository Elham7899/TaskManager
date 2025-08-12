namespace TaskManager.Domain.Entities;

public class TaskItem : BaseEntity
{
    //Primery Key
    public int Id { get; set; }

    //Props
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }

    //Navigations
    public List<TaskLabel> TaskLabels { get; set; } = new List<TaskLabel>();
}
