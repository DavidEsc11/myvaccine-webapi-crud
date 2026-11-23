using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Dtos.FamilyGroups;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

/// <summary>
/// CRUD de la tabla FamilyGroups. Un grupo familiar tiene varios usuarios (muchos a muchos).
/// </summary>
public class FamilyGroupService : IFamilyGroupService
{
    private readonly IBaseRepository<FamilyGroup> _repository;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public FamilyGroupService(
        IBaseRepository<FamilyGroup> repository,
        IBaseRepository<User> userRepository,
        IMapper mapper)
    {
        _repository = repository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    // Consulta base: el grupo junto con sus usuarios.
    private IQueryable<FamilyGroup> WithUsers() => _repository.GetAll().Include(g => g.Users);

    public async Task<IEnumerable<FamilyGroupResponseDto>> GetAll()
    {
        var items = await WithUsers().AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<FamilyGroupResponseDto>>(items);
    }

    public async Task<FamilyGroupResponseDto?> GetById(int id)
    {
        var item = await WithUsers().AsNoTracking().FirstOrDefaultAsync(x => x.FamilyGroupId == id);
        return item is null ? null : _mapper.Map<FamilyGroupResponseDto>(item);
    }

    public async Task<FamilyGroupResponseDto> Add(FamilyGroupRequestDto request)
    {
        var users = await ResolveUsers(request.UserIds);

        var item = new FamilyGroup { Name = request.Name, Users = users };
        await _repository.Add(item);
        return _mapper.Map<FamilyGroupResponseDto>(item);
    }

    public async Task<FamilyGroupResponseDto?> Update(FamilyGroupRequestDto request, int id)
    {
        var item = await WithUsers().FirstOrDefaultAsync(x => x.FamilyGroupId == id);
        if (item is null) return null;

        var users = await ResolveUsers(request.UserIds);

        item.Name = request.Name;
        item.Users.Clear();
        item.Users.AddRange(users);

        await _repository.Update(item);
        return _mapper.Map<FamilyGroupResponseDto>(item);
    }

    public async Task<FamilyGroupResponseDto?> Delete(int id)
    {
        var item = await WithUsers().FirstOrDefaultAsync(x => x.FamilyGroupId == id);
        if (item is null) return null;

        var response = _mapper.Map<FamilyGroupResponseDto>(item);
        await _repository.Delete(item);
        return response;
    }

    private async Task<List<User>> ResolveUsers(List<int> ids)
    {
        var distinctIds = ids.Distinct().ToList();
        var users = await _userRepository.FindBy(u => distinctIds.Contains(u.UserId)).ToListAsync();

        if (users.Count != distinctIds.Count)
        {
            throw new ArgumentException("Uno o más usuarios no existen.");
        }
        return users;
    }
}
