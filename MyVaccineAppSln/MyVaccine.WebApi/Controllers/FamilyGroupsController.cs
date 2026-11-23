using FluentValidation;
using MyVaccine.WebApi.Dtos.FamilyGroups;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

/// <summary>CRUD de grupos familiares (FamilyGroups). La lógica común está en CrudControllerBase.</summary>
public class FamilyGroupsController : CrudControllerBase<FamilyGroupRequestDto, FamilyGroupResponseDto>
{
    public FamilyGroupsController(IFamilyGroupService service, IValidator<FamilyGroupRequestDto> validator) : base(service, validator)
    {
    }
}
