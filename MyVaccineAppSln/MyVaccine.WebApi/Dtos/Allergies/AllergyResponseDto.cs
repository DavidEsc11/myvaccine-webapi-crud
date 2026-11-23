namespace MyVaccine.WebApi.Dtos.Allergies;

public class AllergyResponseDto : AllergyRequestDto, IHasId
{
    public int Id { get; set; }
}
