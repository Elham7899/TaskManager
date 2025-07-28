using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.DTOs.Label;
using TaskManager.Application.Mapping;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.DBContext;
using TaskManager.Infrastructure.Services;

namespace TaskManager.Tests.Services;

public class LabelServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly LabelService _service;

    public LabelServiceTests()
    {
        var option = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(option);

        // AutoMapper configuration
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        var mapper = config.CreateMapper();

        _service = new LabelService(_context, mapper);
    }

    [Fact]
    public async Task AddLabelAsync_Should_Add_Label()
    {
        // Arrange
        var label = new Label { Name = "Test" };

        //Act
        var result = await _service.CreateLabelAsync(new CreateLabelDto { Name = label.Name });
        var labelInDb = await _context.Labels.FirstOrDefaultAsync(x => x.Name == "Test");

        //Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test");
        labelInDb.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateLabelAsync_Should_Update_Label_Name()
    {
        // Arrange
        var label = new Label { Name = "Bug" };
        _context.Labels.Add(label);
        await _context.SaveChangesAsync();

        //Act
        await _service.UpdateLabelAsync(label.Id, "Feature");
        var updated = await _context.Labels.FindAsync(label.Id);

        //Assert
        updated.Name.Should().Be("Feature");
    }

    [Fact]
    public async Task RemoveLabelAsync_Should_Remove_Label()
    {
        // Arrange
        var label = new Label { Name = "Test" };
        _context.Labels.Add(label);
        await _context.SaveChangesAsync();

        //Act
        await _service.DeleteLabelAsync(label.Id);
        var deleted = await _context.Labels.FindAsync(label.Id);

        //Assert
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task GetAllLabelsAsync_Should_Return_Labels()
    {
        // Arrange
        _context.Labels.AddRange(
            new Label { Name = "Low" },
            new Label { Name = "High" }
        );
        await _context.SaveChangesAsync();

        //Act
        var result = await _service.GetAllLabelsAsync();

        //Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task RemoveLabelFromTask_Should_Remove_Label_From_Task()
    {
        // Arrange
        var label = new Label { Name = "Test" };
        var task = new TaskItem { Title = "Studing", Labels = new List<Label> { label } };

        _context.Labels.Add(label);
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        //Act
        await _service.RemoveLabelsFromTaskAsync(task.Id, label.Id);

        var updatedTask = await _context.Tasks.Include(x => x.Labels).FirstOrDefaultAsync(a => a.Id == task.Id);

        //Assert
        Assert.NotNull(updatedTask);
        Assert.Empty(updatedTask.Labels);
    }

    [Fact]
    public async Task AssignLabelsToTaskAsync_AssignsLabelsSuccessfully()
    {
        // Arrange
        var task = new TaskItem { Title = "Test Task" };
        var label1 = new Label { Name = "Label A" };
        var label2 = new Label { Name = "Label B" };

        _context.Tasks.Add(task);
        _context.Labels.AddRange(label1, label2);
        await _context.SaveChangesAsync();

        // Act
        await _service.AssignLabelsToTaskAsync(task.Id, new List<int> { label1.Id, label2.Id });

        var updatedTask = await _context.Tasks.Include(t => t.Labels).FirstOrDefaultAsync(t => t.Id == task.Id);

        // Assert
        Assert.NotNull(updatedTask);
        Assert.Equal(2, updatedTask.Labels.Count);
    }
}
