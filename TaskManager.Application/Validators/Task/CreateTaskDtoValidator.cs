using FluentValidation;
using TaskManager.Application.DTOs.Task;

namespace TaskManager.Application.Validators.Task;

public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title must be under 100 characters");
    }
}
