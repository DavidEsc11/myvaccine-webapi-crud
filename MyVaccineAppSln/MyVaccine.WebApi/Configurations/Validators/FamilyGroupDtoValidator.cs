using FluentValidation;
using MyVaccine.WebApi.Dtos.FamilyGroups;

namespace MyVaccine.WebApi.Configurations.Validators;

public class FamilyGroupDtoValidator : AbstractValidator<FamilyGroupRequestDto>
{
    public FamilyGroupDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(255);
        RuleFor(dto => dto.UserIds).NotNull();
        RuleForEach(dto => dto.UserIds).GreaterThan(0);
    }
}
