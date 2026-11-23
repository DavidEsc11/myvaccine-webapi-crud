using MyVaccine.WebApi.Dtos.Allergies;

namespace MyVaccine.WebApi.Services.Contracts;

public interface IAllergyService : ICrudService<AllergyRequestDto, AllergyResponseDto>
{
}
