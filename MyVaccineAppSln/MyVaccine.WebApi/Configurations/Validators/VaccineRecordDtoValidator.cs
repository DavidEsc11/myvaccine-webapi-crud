using FluentValidation;
using MyVaccine.WebApi.Dtos.VaccineRecords;

namespace MyVaccine.WebApi.Configurations.Validators;

public class VaccineRecordDtoValidator : AbstractValidator<VaccineRecordRequestDto>
{
    public VaccineRecordDtoValidator()
    {
        RuleFor(dto => dto.UserId).GreaterThan(0);
        RuleFor(dto => dto.DependentId).GreaterThan(0);
        RuleFor(dto => dto.VaccineId).GreaterThan(0);
        RuleFor(dto => dto.DateAdministered).NotEmpty();
        RuleFor(dto => dto.AdministeredLocation).MaximumLength(255);
        RuleFor(dto => dto.AdministeredBy).MaximumLength(255);
    }
}
