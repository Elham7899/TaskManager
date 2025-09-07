namespace TaskManager.Domain.Entities;

public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public long IsDeleted { get; set; }
}