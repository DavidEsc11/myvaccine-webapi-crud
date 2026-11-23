namespace MyVaccine.WebApi.Dtos.FamilyGroups;

/// <summary>Versión reducida de un usuario: evita exponer datos innecesarios.</summary>
public class FamilyGroupMemberDto
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class FamilyGroupResponseDto : IHasId
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<FamilyGroupMemberDto> Users { get; set; } = new();
}
