using MyVaccine.WebApi.Dtos.VaccineCategories;

namespace MyVaccine.WebApi.Dtos.Vaccines;

public class VaccineResponseDto : IHasId
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool RequiresBooster { get; set; }
    // En la respuesta devolvemos las categorías completas (id y nombre).
    public List<VaccineCategoryResponseDto> Categories { get; set; } = new();
}
