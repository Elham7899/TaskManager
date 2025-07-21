namespace TaskManager.Domain.Entities;

public class Label
{
    //Identity Key
    public int Id { get; set; }

    //Props
    public string Name { get; set; }

    //Navigations
    public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
