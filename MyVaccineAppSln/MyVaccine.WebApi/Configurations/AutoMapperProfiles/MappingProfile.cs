using AutoMapper;
using MyVaccine.WebApi.Dtos.Allergies;
using MyVaccine.WebApi.Dtos.Dependent;
using MyVaccine.WebApi.Dtos.FamilyGroups;
using MyVaccine.WebApi.Dtos.VaccineCategories;
using MyVaccine.WebApi.Dtos.VaccineRecords;
using MyVaccine.WebApi.Dtos.Vaccines;
using MyVaccine.WebApi.Models;

namespace MyVaccine.WebApi.Configurations.AutoMapperProfiles;

/// <summary>
/// Reglas de conversión entidad -> DTO. En cada tabla la llave primaria tiene un nombre
/// distinto (DependentId, AllergyId...), por eso se mapea a un "Id" común en la respuesta.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Dependents (ya existía en la clase del profesor)
        CreateMap<Dependent, DependentRequestDto>().ReverseMap();
        CreateMap<Dependent, DependentResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DependentId)).ReverseMap();

        // VaccineCategories
        CreateMap<VaccineCategory, VaccineCategoryResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.VaccineCategoryId));

        // Allergies
        CreateMap<Allergy, AllergyResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AllergyId));

        // Vaccines (la lista Categories se convierte usando el mapeo de VaccineCategory de arriba)
        CreateMap<Vaccine, VaccineResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.VaccineId));

        // FamilyGroups (Users se convierte con el mapeo User -> FamilyGroupMemberDto)
        CreateMap<User, FamilyGroupMemberDto>();
        CreateMap<FamilyGroup, FamilyGroupResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FamilyGroupId));

        // VaccineRecords
        CreateMap<VaccineRecord, VaccineRecordResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.VaccineRecordId));
    }
}
