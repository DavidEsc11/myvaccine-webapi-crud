namespace MyVaccine.WebApi.Dtos.Dependent;

public class DependentResponseDto : DependentRequestDto, IHasId
{
    public int Id { get; set; }
}
