namespace MyVaccine.WebApi.Dtos.VaccineCategories;

/// <summary>Datos que envía el cliente para crear o editar una categoría de vacuna.</summary>
public class VaccineCategoryRequestDto
{
    public string Name { get; set; } = string.Empty;
}
