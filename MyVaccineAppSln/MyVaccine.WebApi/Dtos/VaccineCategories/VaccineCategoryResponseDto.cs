namespace MyVaccine.WebApi.Dtos.VaccineCategories;

public class VaccineCategoryResponseDto : VaccineCategoryRequestDto, IHasId
{
    public int Id { get; set; }
}
