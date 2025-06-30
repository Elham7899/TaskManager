namespace TaskManager.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!; // Note: Plain text for demo — never do this in real apps!
}
