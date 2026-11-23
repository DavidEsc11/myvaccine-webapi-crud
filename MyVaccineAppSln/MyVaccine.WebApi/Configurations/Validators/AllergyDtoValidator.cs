using FluentValidation;
using MyVaccine.WebApi.Dtos.Allergies;

namespace MyVaccine.WebApi.Configurations.Validators;

public class AllergyDtoValidator : AbstractValidator<AllergyRequestDto>
{
    public AllergyDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.UserId).GreaterThan(0);
    }
}
