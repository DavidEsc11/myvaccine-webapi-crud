using MyVaccine.WebApi.Dtos.VaccineRecords;

namespace MyVaccine.WebApi.Services.Contracts;

public interface IVaccineRecordService : ICrudService<VaccineRecordRequestDto, VaccineRecordResponseDto>
{
}
