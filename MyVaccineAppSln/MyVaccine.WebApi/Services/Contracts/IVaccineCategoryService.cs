using MyVaccine.WebApi.Dtos.VaccineCategories;

namespace MyVaccine.WebApi.Services.Contracts;

public interface IVaccineCategoryService : ICrudService<VaccineCategoryRequestDto, VaccineCategoryResponseDto>
{
}
