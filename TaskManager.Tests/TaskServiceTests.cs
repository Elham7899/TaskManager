using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.DBContext;
using TaskManager.Infrastructure.Services;

namespace TaskManager.Tests;

public class TaskServiceTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task AddTaskAsync_Should_Add_Task_To_Database()
    {
        // Arrange
        var context = GetDbContext();
        var service = new TaskService(context);
        var task = new TaskItem { Title = "Test Task" };

        // Act
        var result = await service.AddTaskAsync(task);
        var allTasks = await service.GetAllTasksAsync();

        // Assert
        Assert.Single(allTasks);
        Assert.Equal("Test Task", result.Title);
    }

    [Fact]
    public async Task GetAllTasksAsync_Should_Return_Empty_List_When_No_Tasks()
    {
        // Arrange
        var context = GetDbContext();
        var service = new TaskService(context);

        // Act
        var tasks = await service.GetAllTasksAsync();

        // Assert
        Assert.Empty(tasks);
    }
}
