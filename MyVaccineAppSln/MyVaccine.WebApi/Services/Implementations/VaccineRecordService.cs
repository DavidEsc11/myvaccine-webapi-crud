using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Dtos.VaccineRecords;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

/// <summary>
/// CRUD de la tabla VaccineRecords (carnet de vacunación).
/// Cada registro une un usuario, un dependiente y una vacuna.
/// </summary>
public class VaccineRecordService : IVaccineRecordService
{
    private readonly IBaseRepository<VaccineRecord> _repository;
    private readonly IMapper _mapper;

    public VaccineRecordService(IBaseRepository<VaccineRecord> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VaccineRecordResponseDto>> GetAll()
    {
        var items = await _repository.GetAll().AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<VaccineRecordResponseDto>>(items);
    }

    public async Task<VaccineRecordResponseDto?> GetById(int id)
    {
        var item = await _repository.FindByAsNoTracking(x => x.VaccineRecordId == id).FirstOrDefaultAsync();
        return item is null ? null : _mapper.Map<VaccineRecordResponseDto>(item);
    }

    public async Task<VaccineRecordResponseDto> Add(VaccineRecordRequestDto request)
    {
        var item = new VaccineRecord();
        Fill(item, request);
        await _repository.Add(item);
        return _mapper.Map<VaccineRecordResponseDto>(item);
    }

    public async Task<VaccineRecordResponseDto?> Update(VaccineRecordRequestDto request, int id)
    {
        var item = await _repository.FindBy(x => x.VaccineRecordId == id).FirstOrDefaultAsync();
        if (item is null) return null;

        Fill(item, request);
        await _repository.Update(item);
        return _mapper.Map<VaccineRecordResponseDto>(item);
    }

    public async Task<VaccineRecordResponseDto?> Delete(int id)
    {
        var item = await _repository.FindBy(x => x.VaccineRecordId == id).FirstOrDefaultAsync();
        if (item is null) return null;

        await _repository.Delete(item);
        return _mapper.Map<VaccineRecordResponseDto>(item);
    }

    // Copia los datos del DTO a la entidad (se reutiliza en Add y Update).
    private static void Fill(VaccineRecord item, VaccineRecordRequestDto request)
    {
        item.UserId = request.UserId;
        item.DependentId = request.DependentId;
        item.VaccineId = request.VaccineId;
        item.DateAdministered = request.DateAdministered;
        item.AdministeredLocation = request.AdministeredLocation ?? string.Empty;
        item.AdministeredBy = request.AdministeredBy ?? string.Empty;
    }
}
