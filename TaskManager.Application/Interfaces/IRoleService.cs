namespace TaskManager.Application.Interfaces;

public interface IRoleService
{
    Task<bool> AssignRoleToUserAsync(string userId, string role);
}
