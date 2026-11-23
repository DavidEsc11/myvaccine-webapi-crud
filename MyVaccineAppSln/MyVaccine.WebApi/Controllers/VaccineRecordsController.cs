using FluentValidation;
using MyVaccine.WebApi.Dtos.VaccineRecords;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

/// <summary>CRUD de registros de vacunación (VaccineRecords). La lógica común está en CrudControllerBase.</summary>
public class VaccineRecordsController : CrudControllerBase<VaccineRecordRequestDto, VaccineRecordResponseDto>
{
    public VaccineRecordsController(IVaccineRecordService service, IValidator<VaccineRecordRequestDto> validator) : base(service, validator)
    {
    }
}
