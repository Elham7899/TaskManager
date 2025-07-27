using FluentValidation;
using TaskManager.Application.DTOs.Label;

namespace TaskManager.Application.Validators.Label;

public class CreateLabelDtoValidator : AbstractValidator<CreateLabelDto>
{
    public CreateLabelDtoValidator()
    {
        RuleFor(x=>x.Name).NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must be under 100 characters");
    }
}
