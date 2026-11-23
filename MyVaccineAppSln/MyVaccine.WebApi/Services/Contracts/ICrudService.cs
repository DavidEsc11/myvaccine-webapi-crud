namespace MyVaccine.WebApi.Services.Contracts;

/// <summary>
/// Contrato genérico de un CRUD. Cada tabla tiene su propio servicio que lo implementa.
/// GetById, Update y Delete devuelven null cuando el registro no existe
/// (el controlador lo traduce a un 404 Not Found).
/// </summary>
public interface ICrudService<TRequest, TResponse> where TResponse : class
{
    Task<IEnumerable<TResponse>> GetAll();
    Task<TResponse?> GetById(int id);
    Task<TResponse> Add(TRequest request);
    Task<TResponse?> Update(TRequest request, int id);
    Task<TResponse?> Delete(int id);
}
