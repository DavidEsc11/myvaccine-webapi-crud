namespace MyVaccine.WebApi.Dtos.VaccineRecords;

public class VaccineRecordResponseDto : VaccineRecordRequestDto, IHasId
{
    public int Id { get; set; }
}
