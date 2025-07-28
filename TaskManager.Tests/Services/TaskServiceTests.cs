using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Mapping;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.DBContext;
using TaskManager.Infrastructure.Services;

namespace TaskManager.Tests.Services;

public class TaskServiceTests
{
    private readonly TaskService _taskService;
    private readonly ApplicationDbContext _dbContext;

    private TaskServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _dbContext = new ApplicationDbContext(options);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        var mapper = config.CreateMapper();

        _taskService = new TaskService(_dbContext, mapper);
    }

    [Fact]
    public async Task AddTaskAsync_Should_Add_Task_To_Database()
    {
        // Arrange
        var task = new TaskItem { Title = "Test Task" };

        // Act
        var result = await _taskService.AddTaskAsync(task);
        var allTasks = await _taskService.GetAllTasksAsync();

        // Assert
        Assert.Single(allTasks);
        Assert.Equal("Test Task", result.Title);
    }

    [Fact]
    public async Task GetAllTasksAsync_Should_Return_Empty_List_When_No_Tasks()
    {
        // Act
        var tasks = await _taskService.GetAllTasksAsync();

        // Assert
        Assert.Empty(tasks);
    }
}
