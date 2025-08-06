namespace TaskManager.Application.DTOs.Login;

public class AssignRoleRequestDto
{
    public string UserId { get; set; } = null!;
    public string Role { get; set; } = null!;
}
