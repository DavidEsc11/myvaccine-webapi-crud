using FluentValidation;
using MyVaccine.WebApi.Dtos.Vaccines;

namespace MyVaccine.WebApi.Configurations.Validators;

public class VaccineDtoValidator : AbstractValidator<VaccineRequestDto>
{
    public VaccineDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(255);
        RuleFor(dto => dto.CategoryIds).NotNull();
        // Cada id de categoría debe ser un número positivo.
        RuleForEach(dto => dto.CategoryIds).GreaterThan(0);
    }
}
