using FluentValidation;
using MyVaccine.WebApi.Dtos.Vaccines;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

/// <summary>CRUD de vacunas (Vaccines). La lógica común está en CrudControllerBase.</summary>
public class VaccinesController : CrudControllerBase<VaccineRequestDto, VaccineResponseDto>
{
    public VaccinesController(IVaccineService service, IValidator<VaccineRequestDto> validator) : base(service, validator)
    {
    }
}
