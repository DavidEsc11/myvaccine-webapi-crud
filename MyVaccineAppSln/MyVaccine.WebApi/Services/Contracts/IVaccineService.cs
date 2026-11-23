using MyVaccine.WebApi.Dtos.Vaccines;

namespace MyVaccine.WebApi.Services.Contracts;

public interface IVaccineService : ICrudService<VaccineRequestDto, VaccineResponseDto>
{
}
