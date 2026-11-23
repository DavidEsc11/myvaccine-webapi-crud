using FluentValidation;
using MyVaccine.WebApi.Dtos.Dependent;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

/// <summary>CRUD de dependientes (Dependents). La lógica común está en CrudControllerBase.</summary>
public class DependentsController : CrudControllerBase<DependentRequestDto, DependentResponseDto>
{
    public DependentsController(IDependentService service, IValidator<DependentRequestDto> validator) : base(service, validator)
    {
    }
}
