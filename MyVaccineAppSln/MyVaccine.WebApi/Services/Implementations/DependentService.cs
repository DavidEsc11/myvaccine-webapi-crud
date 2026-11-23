using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Dtos.Dependent;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

/// <summary>
/// CRUD completo de Dependent. En la clase del profesor solo estaban GetAll, GetById y Add;
/// aquí se completan Update y Delete y se manejan los casos "no existe" devolviendo null.
/// </summary>
public class DependentService : IDependentService
{
    private readonly IBaseRepository<Dependent> _dependentRepository;
    private readonly IMapper _mapper;

    public DependentService(IBaseRepository<Dependent> dependentRepository, IMapper mapper)
    {
        _dependentRepository = dependentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DependentResponseDto>> GetAll()
    {
        // AsNoTracking: solo lectura, EF no necesita vigilar los cambios (más rápido).
        var dependents = await _dependentRepository.GetAll().AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<DependentResponseDto>>(dependents);
    }

    public async Task<DependentResponseDto?> GetById(int id)
    {
        var dependent = await _dependentRepository.FindByAsNoTracking(x => x.DependentId == id).FirstOrDefaultAsync();
        return dependent is null ? null : _mapper.Map<DependentResponseDto>(dependent);
    }

    public async Task<DependentResponseDto> Add(DependentRequestDto request)
    {
        var dependent = new Dependent
        {
            Name = request.Name,
            DateOfBirth = request.DateOfBirth,
            UserId = request.UserId
        };

        await _dependentRepository.Add(dependent);
        return _mapper.Map<DependentResponseDto>(dependent);
    }

    public async Task<DependentResponseDto?> Update(DependentRequestDto request, int id)
    {
        // FindBy (con tracking): necesitamos que EF detecte los cambios que hacemos.
        var dependent = await _dependentRepository.FindBy(x => x.DependentId == id).FirstOrDefaultAsync();
        if (dependent is null) return null;

        dependent.Name = request.Name;
        dependent.DateOfBirth = request.DateOfBirth;
        dependent.UserId = request.UserId;

        await _dependentRepository.Update(dependent);
        return _mapper.Map<DependentResponseDto>(dependent);
    }

    public async Task<DependentResponseDto?> Delete(int id)
    {
        var dependent = await _dependentRepository.FindBy(x => x.DependentId == id).FirstOrDefaultAsync();
        if (dependent is null) return null;

        await _dependentRepository.Delete(dependent);
        return _mapper.Map<DependentResponseDto>(dependent);
    }
}
