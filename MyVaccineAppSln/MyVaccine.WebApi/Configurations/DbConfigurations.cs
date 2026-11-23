using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Literals;
using MyVaccine.WebApi.Models;

namespace MyVaccine.WebApi.Configurations;

public static class DbConfigurations
{
    public static IServiceCollection SetDatabaseConfiguration(this IServiceCollection services)
    {
        // Se lee de la variable de entorno MY_VACCINE_CONNECTION_STRING. Si no existe se usa el SQL Server
        // del docker-compose.yml de este repositorio (puerto 14330).
        var connectionString =
            Environment.GetEnvironmentVariable(MyVaccineLiterals.CONNECTION_STRING)
            ?? "Server=localhost,14330;Database=MyVaccineAppDb;User Id=sa;Password=Abc.123456;TrustServerCertificate=True;";

        services.AddDbContext<MyVaccineAppDbContext>(options =>
                    options.UseSqlServer(connectionString));
        return services;
    }
}
