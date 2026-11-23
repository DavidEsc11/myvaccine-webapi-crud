namespace MyVaccine.WebApi.Dtos.Allergies;

/// <summary>Datos para registrar una alergia de un usuario.</summary>
public class AllergyRequestDto
{
    public string Name { get; set; } = string.Empty;
    public int UserId { get; set; }
}
