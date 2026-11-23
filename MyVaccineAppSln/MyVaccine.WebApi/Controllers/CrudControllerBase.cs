using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Dtos;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

/// <summary>
/// Controlador base con los 5 endpoints del CRUD (GetAll, GetById, Create, Update, Delete).
/// Cada tabla hereda de esta clase y solo indica sus DTOs: así no se repite el mismo código
/// seis veces y todas las tablas responden con las mismas reglas.
///
///   GET    api/[controller]        -> lista todos
///   GET    api/[controller]/{id}   -> uno (404 si no existe)
///   POST   api/[controller]        -> crea (201 Created)
///   PUT    api/[controller]/{id}   -> actualiza (404 si no existe)
///   DELETE api/[controller]/{id}   -> elimina (404 si no existe)
///
/// [Authorize]: todos los endpoints exigen el token JWT (header Authorization: Bearer ...).
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public abstract class CrudControllerBase<TRequest, TResponse> : ControllerBase
    where TRequest : class
    where TResponse : class, IHasId
{
    private readonly ICrudService<TRequest, TResponse> _service;
    private readonly IValidator<TRequest> _validator;

    protected CrudControllerBase(ICrudService<TRequest, TResponse> service, IValidator<TRequest> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.GetAll();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetById(id);
        if (item is null) return NotFound(new { message = $"No existe un registro con id {id}." });
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TRequest request)
    {
        // 1) Validar con FluentValidation.
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage }));
        }

        try
        {
            // 2) Guardar y responder 201 con la URL del nuevo recurso (header Location).
            var created = await _service.Add(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (DbUpdateException)
        {
            // Normalmente: una llave foránea apunta a un registro que no existe.
            return BadRequest(new { message = "No se pudo guardar: revise que los ids relacionados existan." });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage }));
        }

        try
        {
            var updated = await _service.Update(request, id);
            if (updated is null) return NotFound(new { message = $"No existe un registro con id {id}." });
            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (DbUpdateException)
        {
            return BadRequest(new { message = "No se pudo actualizar: revise que los ids relacionados existan." });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _service.Delete(id);
            if (deleted is null) return NotFound(new { message = $"No existe un registro con id {id}." });
            return Ok(deleted);
        }
        catch (DbUpdateException)
        {
            return Conflict(new { message = "No se puede eliminar: el registro está siendo usado por otros datos." });
        }
    }
}
