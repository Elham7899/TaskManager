
namespace TaskManager.Domain.Entities;

public class TaskLabel
{
    //Fk
    public int LabelId { get; set; }
    public int TaskId { get; set; }

    //Navigation
    public Label Label { get; set; }
    public TaskItem Task { get; set; }
}