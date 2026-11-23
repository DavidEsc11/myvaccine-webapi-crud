namespace MyVaccine.WebApi.Dtos.VaccineRecords;

/// <summary>Registro de una vacuna aplicada a un dependiente.</summary>
public class VaccineRecordRequestDto
{
    public int UserId { get; set; }
    public int DependentId { get; set; }
    public int VaccineId { get; set; }
    public DateTime DateAdministered { get; set; }
    public string? AdministeredLocation { get; set; }
    public string? AdministeredBy { get; set; }
}
