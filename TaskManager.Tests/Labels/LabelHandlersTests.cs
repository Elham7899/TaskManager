using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Labels.Commands.Assign;
using TaskManager.Application.Labels.Commands.Create;
using TaskManager.Application.Labels.Commands.Delete;
using TaskManager.Application.Labels.Commands.Remove;
using TaskManager.Application.Labels.Commands.Update;
using TaskManager.Application.Labels.Queries.GetAll;
using TaskManager.Application.Labels.Queries.GetBy;
using TaskManager.Application.Mapping;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.DBContext;

namespace TaskManager.Tests.Labels;

public class LabelHandlersTests
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public LabelHandlersTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        var config = new MapperConfiguration(x =>
        {
            x.AddProfile<MappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    //Command
    [Fact]
    public async Task CreateLabelCommand_Should_Add_Label()
    {
        //Arrange
        var handler = new CreateLabelCommandHandler(_context, _mapper);
        var command = new CreateLabelCommand(new Application.DTOs.Label.CreateLabelDto { Name = "Test" }, "user1");

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test");
        (await _context.Labels.FirstOrDefaultAsync(x => x.Name == "Test")).Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateLabelCommand_Should_Update_Label_Name()
    {
        //Arrange
        var label = new Label { Name = "Bug", UpdatedBy = "user", CreatedBy = "user" };
        _context.Labels.Add(label);
        await _context.SaveChangesAsync();

        var handler = new UpdateLabelCommandHandler(_context, _mapper);
        var command = new UpdateLabelCommand(label.Id, "Feature", "user1");

        //Act 
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.Name.Should().Be("Feature");
        (await _context.Labels.FindAsync(label.Id))!.Name.Should().Be("Feature");
    }


    [Fact]
    public async Task DeleteLabelCommand_Should_Remove_Label()
    {
        //Arrange
        var label = new Label { Name = "Test", UpdatedBy = "user", CreatedBy = "user" };
        _context.Labels.Add(label);
        await _context.SaveChangesAsync();

        var handler = new DeleteLabelCommandHandler(_context);
        var command = new DeleteLabelCommand(label.Id, "user");

        //Act
        await handler.Handle(command, CancellationToken.None);

        //Assert
        (await _context.Labels.FindAsync(label.Id)).Should().BeNull();
    }

    [Fact]
    public async Task AssignLabelsToTaskCommand_Should_Assign_Labels()
    {
        // Arrange
        var task = new TaskItem { Title = "Test Task", UpdatedBy = "user", CreatedBy = "user" };
        var label1 = new Label { Name = "Label A", UpdatedBy = "user", CreatedBy = "user" };
        var label2 = new Label { Name = "Label B", UpdatedBy = "user", CreatedBy = "user" };

        _context.Tasks.Add(task);
        _context.Labels.AddRange(label1, label2);
        await _context.SaveChangesAsync();

        var handler = new AssignLabelsToTaskCommandHandler(_context);
        var command = new AssignLabelsToTaskCommand(task.Id, new List<int> { label1.Id, label2.Id });

        //Act 
        await handler.Handle(command, CancellationToken.None);

        var updatedTask = await _context.Tasks
           .Include(t => t.TaskLabels)
           .ThenInclude(tl => tl.Label)
           .FirstOrDefaultAsync(t => t.Id == task.Id);

        //Assert
        updatedTask.Should().NotBeNull();
        updatedTask.TaskLabels.Should().HaveCount(2);
    }

    [Fact]
    public async Task RemoveLabelFromTaskCommand_Should_Remove_Label_From_Task()
    {
        // Arrange
        var label = new Label { Name = "Test", UpdatedBy = "user", CreatedBy = "user" };
        var task = new TaskItem { Title = "Studying", UpdatedBy = "user", CreatedBy = "user" };

        _context.Labels.Add(label);
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        _context.TaskLabels.Add(new TaskLabel { TaskId = task.Id, LabelId = label.Id });
        await _context.SaveChangesAsync();

        var handler = new RemoveLabelFromTaskCommandHandler(_context);
        var command = new RemoveLabelFromTaskCommand(task.Id, label.Id);

        // Act
        await handler.Handle(command, CancellationToken.None);

        var updatedTask = await _context.Tasks
            .Include(x => x.TaskLabels)
            .FirstOrDefaultAsync(x => x.Id == task.Id);

        // Assert
        updatedTask.Should().NotBeNull();
        updatedTask!.TaskLabels.Should().BeEmpty();
    }

    //Query
    [Fact]
    public async Task GetAllLabelsQuery_Should_Return_Labels()
    {
        //Arrange
        await _context.Labels.AddRangeAsync(new Label { Name = "Bad", UpdatedBy = "user", CreatedBy = "user" }, new Label { Name = "Good", UpdatedBy = "user", CreatedBy = "user" });
        await _context.SaveChangesAsync();

        var handler = new GetAllLabelsQueryHandler(_context, _mapper);
        var query = new GetAllLabelsQuery(1, 10);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.TotalCount.Should().Be(2);
        result.Items.Select(l => l.Name).Should().Contain(new[] { "Bad", "Good" });
    }

    [Fact]
    public async Task GetLabelByIdQuery_Should_Return_Label()
    {
        //Arrange
        var label = new Label { Name = "Test", UpdatedBy = "user", CreatedBy = "user" };
        _context.Labels.Add(label);

        await _context.SaveChangesAsync();

        var handler = new GetLabelByIdQueryHandler(_context, _mapper);
        var query = new GetLabelByIdQuery(label.Id);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test");
        result.Id.Should().Be(label.Id);
    }
}