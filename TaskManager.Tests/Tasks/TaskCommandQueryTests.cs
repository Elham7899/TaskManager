using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.DTOs.Task;
using TaskManager.Application.Mapping;
using TaskManager.Application.Tasks.Commands;
using TaskManager.Application.Tasks.Queries;
using TaskManager.Infrastructure.DBContext;

namespace TaskManager.Tests.Tasks;

public class TaskCommandQueryTests
{
    private readonly IMapper _mapper;

    public TaskCommandQueryTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    private ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task CreateTaskCommand_Should_Add_Task_To_Database()
    {
        using var _dbContext = CreateDbContext();
        var handler = new CreateTaskCommandHandler(_dbContext, _mapper);
        var dto = new CreateTaskDto { Title = "Test Task" };

        var result = await handler.Handle(new CreateTaskCommand(dto, "test-user"), CancellationToken.None);

        Assert.Equal("Test Task", result.Title);
        Assert.Single(_dbContext.Tasks);
    }

    [Fact]
    public async Task GetTasksQuery_Should_Return_Empty_When_No_Tasks()
    {
        using var _dbContext = CreateDbContext();
        var handler = new GetTasksQueryHandler(_dbContext, _mapper);

        var result = await handler.Handle(new GetTasksQuery(1, 10), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }

    [Fact]
    public async Task GetTasksQuery_Should_Return_Tasks_After_Creation()
    {
        using var _dbContext = CreateDbContext();
        var createHandler = new CreateTaskCommandHandler(_dbContext, _mapper);
        await createHandler.Handle(new CreateTaskCommand(new CreateTaskDto { Title = "Task 1" }, "test-user"), CancellationToken.None);

        var queryHandler = new GetTasksQueryHandler(_dbContext, _mapper);
        var result = await queryHandler.Handle(new GetTasksQuery(1, 10), CancellationToken.None);

        Assert.Single(result.Items);
        Assert.Equal("Task 1", result.Items.First().Title);
    }

    [Fact]
    public async Task GetTaskByIdQuery_Should_Return_Null_When_Task_Does_Not_Exist()
    {
        using var _dbContext = CreateDbContext();
        var handler = new GetTaskByIdQueryHandler(_dbContext, _mapper);

        var result = await handler.Handle(new GetTaskByIdQuery(999), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetTaskByIdQuery_Should_Return_Task_When_It_Exists()
    {
        using var _dbContext = CreateDbContext();
        var createHandler = new CreateTaskCommandHandler(_dbContext, _mapper);
        var dto = new CreateTaskDto { Title = "Existing Task" };
        var created = await createHandler.Handle(new CreateTaskCommand(dto, "test-user"), CancellationToken.None);

        var handler = new GetTaskByIdQueryHandler(_dbContext, _mapper);
        var result = await handler.Handle(new GetTaskByIdQuery(created.Id), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Existing Task", result.Title);
        Assert.Equal(created.Id, result.Id);
    }
}
