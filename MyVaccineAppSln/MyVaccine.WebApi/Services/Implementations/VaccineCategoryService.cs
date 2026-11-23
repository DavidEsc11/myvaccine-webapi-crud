using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Dtos.VaccineCategories;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

/// <summary>CRUD de la tabla VaccineCategories (categorías de vacunas).</summary>
public class VaccineCategoryService : IVaccineCategoryService
{
    private readonly IBaseRepository<VaccineCategory> _repository;
    private readonly IMapper _mapper;

    public VaccineCategoryService(IBaseRepository<VaccineCategory> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VaccineCategoryResponseDto>> GetAll()
    {
        var items = await _repository.GetAll().AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<VaccineCategoryResponseDto>>(items);
    }

    public async Task<VaccineCategoryResponseDto?> GetById(int id)
    {
        var item = await _repository.FindByAsNoTracking(x => x.VaccineCategoryId == id).FirstOrDefaultAsync();
        return item is null ? null : _mapper.Map<VaccineCategoryResponseDto>(item);
    }

    public async Task<VaccineCategoryResponseDto> Add(VaccineCategoryRequestDto request)
    {
        var item = new VaccineCategory { Name = request.Name };
        await _repository.Add(item);
        return _mapper.Map<VaccineCategoryResponseDto>(item);
    }

    public async Task<VaccineCategoryResponseDto?> Update(VaccineCategoryRequestDto request, int id)
    {
        var item = await _repository.FindBy(x => x.VaccineCategoryId == id).FirstOrDefaultAsync();
        if (item is null) return null;

        item.Name = request.Name;
        await _repository.Update(item);
        return _mapper.Map<VaccineCategoryResponseDto>(item);
    }

    public async Task<VaccineCategoryResponseDto?> Delete(int id)
    {
        var item = await _repository.FindBy(x => x.VaccineCategoryId == id).FirstOrDefaultAsync();
        if (item is null) return null;

        await _repository.Delete(item);
        return _mapper.Map<VaccineCategoryResponseDto>(item);
    }
}
