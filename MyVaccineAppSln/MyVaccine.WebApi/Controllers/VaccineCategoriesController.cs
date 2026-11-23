using FluentValidation;
using MyVaccine.WebApi.Dtos.VaccineCategories;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

/// <summary>CRUD de categorías de vacunas (VaccineCategories). La lógica común está en CrudControllerBase.</summary>
public class VaccineCategoriesController : CrudControllerBase<VaccineCategoryRequestDto, VaccineCategoryResponseDto>
{
    public VaccineCategoriesController(IVaccineCategoryService service, IValidator<VaccineCategoryRequestDto> validator) : base(service, validator)
    {
    }
}
