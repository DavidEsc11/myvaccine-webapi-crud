using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Dtos.Allergies;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

/// <summary>CRUD de la tabla Allergies (alergias de un usuario).</summary>
public class AllergyService : IAllergyService
{
    private readonly IBaseRepository<Allergy> _repository;
    private readonly IMapper _mapper;

    public AllergyService(IBaseRepository<Allergy> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AllergyResponseDto>> GetAll()
    {
        var items = await _repository.GetAll().AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<AllergyResponseDto>>(items);
    }

    public async Task<AllergyResponseDto?> GetById(int id)
    {
        var item = await _repository.FindByAsNoTracking(x => x.AllergyId == id).FirstOrDefaultAsync();
        return item is null ? null : _mapper.Map<AllergyResponseDto>(item);
    }

    public async Task<AllergyResponseDto> Add(AllergyRequestDto request)
    {
        var item = new Allergy { Name = request.Name, UserId = request.UserId };
        await _repository.Add(item);
        return _mapper.Map<AllergyResponseDto>(item);
    }

    public async Task<AllergyResponseDto?> Update(AllergyRequestDto request, int id)
    {
        var item = await _repository.FindBy(x => x.AllergyId == id).FirstOrDefaultAsync();
        if (item is null) return null;

        item.Name = request.Name;
        item.UserId = request.UserId;
        await _repository.Update(item);
        return _mapper.Map<AllergyResponseDto>(item);
    }

    public async Task<AllergyResponseDto?> Delete(int id)
    {
        var item = await _repository.FindBy(x => x.AllergyId == id).FirstOrDefaultAsync();
        if (item is null) return null;

        await _repository.Delete(item);
        return _mapper.Map<AllergyResponseDto>(item);
    }
}
