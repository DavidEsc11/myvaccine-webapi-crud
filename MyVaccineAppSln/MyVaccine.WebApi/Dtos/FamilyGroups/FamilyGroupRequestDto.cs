namespace MyVaccine.WebApi.Dtos.FamilyGroups;

/// <summary>Un grupo familiar tiene un nombre y una lista de usuarios (muchos a muchos).</summary>
public class FamilyGroupRequestDto
{
    public string Name { get; set; } = string.Empty;
    public List<int> UserIds { get; set; } = new();
}
