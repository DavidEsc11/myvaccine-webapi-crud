using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Dtos.Vaccines;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

/// <summary>
/// CRUD de la tabla Vaccines. Una vacuna tiene varias categorías (muchos a muchos),
/// por eso se usa Include para traerlas y se validan los ids de categorías recibidos.
/// </summary>
public class VaccineService : IVaccineService
{
    private readonly IBaseRepository<Vaccine> _repository;
    private readonly IBaseRepository<VaccineCategory> _categoryRepository;
    private readonly IMapper _mapper;

    public VaccineService(
        IBaseRepository<Vaccine> repository,
        IBaseRepository<VaccineCategory> categoryRepository,
        IMapper mapper)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    // Consulta base: la vacuna junto con sus categorías.
    private IQueryable<Vaccine> WithCategories() => _repository.GetAll().Include(v => v.Categories);

    public async Task<IEnumerable<VaccineResponseDto>> GetAll()
    {
        var items = await WithCategories().AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<VaccineResponseDto>>(items);
    }

    public async Task<VaccineResponseDto?> GetById(int id)
    {
        var item = await WithCategories().AsNoTracking().FirstOrDefaultAsync(x => x.VaccineId == id);
        return item is null ? null : _mapper.Map<VaccineResponseDto>(item);
    }

    public async Task<VaccineResponseDto> Add(VaccineRequestDto request)
    {
        var categories = await ResolveCategories(request.CategoryIds);

        var item = new Vaccine
        {
            Name = request.Name,
            RequiresBooster = request.RequiresBooster,
            Categories = categories
        };

        await _repository.Add(item);
        return _mapper.Map<VaccineResponseDto>(item);
    }

    public async Task<VaccineResponseDto?> Update(VaccineRequestDto request, int id)
    {
        // Con tracking y con las categorías cargadas para poder reemplazarlas.
        var item = await WithCategories().FirstOrDefaultAsync(x => x.VaccineId == id);
        if (item is null) return null;

        var categories = await ResolveCategories(request.CategoryIds);

        item.Name = request.Name;
        item.RequiresBooster = request.RequiresBooster;
        item.Categories.Clear();
        item.Categories.AddRange(categories);

        await _repository.Update(item);
        return _mapper.Map<VaccineResponseDto>(item);
    }

    public async Task<VaccineResponseDto?> Delete(int id)
    {
        var item = await WithCategories().FirstOrDefaultAsync(x => x.VaccineId == id);
        if (item is null) return null;

        var response = _mapper.Map<VaccineResponseDto>(item);
        await _repository.Delete(item);
        return response;
    }

    // Busca las categorías por id. Si alguna no existe se lanza ArgumentException,
    // que el controlador convierte en un 400 Bad Request con un mensaje claro.
    private async Task<List<VaccineCategory>> ResolveCategories(List<int> ids)
    {
        var distinctIds = ids.Distinct().ToList();
        var categories = await _categoryRepository
            .FindBy(c => distinctIds.Contains(c.VaccineCategoryId))
            .ToListAsync();

        if (categories.Count != distinctIds.Count)
        {
            throw new ArgumentException("Una o más categorías no existen.");
        }
        return categories;
    }
}
