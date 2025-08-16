namespace TaskManager.Domain.Entities;

public class Label : BaseEntity
{
    //Identity Key
    public int Id { get; set; }

    //Props
    public string Name { get; set; }

    //Navigations
    public List<TaskLabel> TaskLabels { get; set; } = new List<TaskLabel>();
}
