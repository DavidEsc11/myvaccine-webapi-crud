using FluentValidation;
using MyVaccine.WebApi.Dtos.Allergies;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

/// <summary>CRUD de alergias (Allergies). La lógica común está en CrudControllerBase.</summary>
public class AllergiesController : CrudControllerBase<AllergyRequestDto, AllergyResponseDto>
{
    public AllergiesController(IAllergyService service, IValidator<AllergyRequestDto> validator) : base(service, validator)
    {
    }
}
