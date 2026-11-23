namespace MyVaccine.WebApi.Dtos.Vaccines;

/// <summary>
/// Una vacuna puede pertenecer a varias categorías (relación muchos a muchos),
/// por eso el cliente envía solo los ids de las categorías.
/// </summary>
public class VaccineRequestDto
{
    public string Name { get; set; } = string.Empty;
    public bool RequiresBooster { get; set; }
    public List<int> CategoryIds { get; set; } = new();
}
