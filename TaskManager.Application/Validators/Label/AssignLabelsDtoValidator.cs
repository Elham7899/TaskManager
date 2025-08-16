using FluentValidation;
using TaskManager.Application.DTOs.Label;

namespace TaskManager.Application.Validators.Label;

public class AssignLabelsDtoValidator : AbstractValidator<AssignLabelsDto>
{
    public AssignLabelsDtoValidator()
    {
        RuleFor(x => x.LabelIds).NotNull().NotEmpty().WithMessage("Id is required");
    }
}
