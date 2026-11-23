using FluentValidation;
using MyVaccine.WebApi.Dtos.VaccineCategories;

namespace MyVaccine.WebApi.Configurations.Validators;

public class VaccineCategoryDtoValidator : AbstractValidator<VaccineCategoryRequestDto>
{
    public VaccineCategoryDtoValidator()
    {
        // Mismos límites que en el DbContext: Name requerido y máximo 100 caracteres.
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(100);
    }
}
