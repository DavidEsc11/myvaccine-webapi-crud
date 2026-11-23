namespace MyVaccine.WebApi.Dtos;

/// <summary>
/// Todas las respuestas (Response DTO) exponen un "Id" genérico.
/// Así el controlador base puede construir la URL del recurso creado (201 Created)
/// sin conocer el nombre real de la llave primaria de cada tabla.
/// </summary>
public interface IHasId
{
    int Id { get; }
}
