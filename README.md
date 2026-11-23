# MyVaccine Web API: CRUD completo (.NET + SQL Server + JWT)

Proyecto de la actividad **Integración con .NET: serie DOTNET_06 a DOTNET_17**.
Parte del código de la serie del profesor ([juaneangulou/dotnet-webapi-basic](https://github.com/juaneangulou/dotnet-webapi-basic), rama `07_DOTNET_COMPLETE_CODE`) y le agrega
**operaciones CRUD completas para todas las tablas restantes** de la base de datos de la aplicación, siguiendo la misma arquitectura.

**Autor:** David Escudero Rios

## Qué hace la aplicación

MyVaccine es un carnet de vacunación familiar: un usuario registra a sus dependientes, las vacunas que se les aplicaron, sus alergias y sus grupos familiares.

| Tabla | Descripción | Estado en el código del profesor | En este repositorio |
|---|---|---|---|
| Users + Identity | Registro, login, JWT | Hecho (AuthController) | Sin cambios |
| **Dependents** | Hijos o personas a cargo | Solo GET y POST | **CRUD completo** (se agregaron PUT y DELETE) |
| **VaccineCategories** | Categorías de vacunas | Solo el modelo | **CRUD completo** |
| **Vaccines** | Vacunas (con categorías) | Solo el modelo | **CRUD completo** |
| **Allergies** | Alergias de un usuario | Solo el modelo | **CRUD completo** |
| **FamilyGroups** | Grupos familiares (con usuarios) | Solo el modelo | **CRUD completo** |
| **VaccineRecords** | Vacunas aplicadas | Solo el modelo | **CRUD completo** |

## Arquitectura (la misma de los videos)

```
Controller  ->  Service  ->  Repository genérico  ->  DbContext (EF Core)  ->  SQL Server
 (HTTP)        (reglas)     IBaseRepository<T>
   |              |
 FluentValidation + AutoMapper (DTOs de entrada y salida)
```

- **Controllers/**: reciben la petición HTTP. `CrudControllerBase<TRequest, TResponse>` contiene los 5 endpoints del CRUD y cada tabla solo hereda de él.
- **Services/**: lógica de cada tabla (`ICrudService<,>` y una implementación por tabla).
- **Repositories/**: `BaseRepository<T>` (genérico, del profesor) hace el acceso a datos y rellena `CreatedAt` / `UpdatedAt`.
- **Dtos/**: objetos de entrada (`...RequestDto`) y de salida (`...ResponseDto`), para no exponer las entidades.
- **Configurations/**: inyección de dependencias, validadores (FluentValidation), mapeos (AutoMapper), base de datos y JWT.
- **Models/** y **Migrations/**: entidades y migraciones de Entity Framework.

### Endpoints (todos exigen `Authorization: Bearer <token>`)

Para cada recurso: `dependents`, `vaccinecategories`, `vaccines`, `allergies`, `familygroups`, `vaccinerecords`

| Método | Ruta | Descripción | Respuestas |
|---|---|---|---|
| GET | `/api/{recurso}` | Lista todos | 200 |
| GET | `/api/{recurso}/{id}` | Obtiene uno | 200, 404 |
| POST | `/api/{recurso}` | Crea | 201 (con header `Location`), 400 |
| PUT | `/api/{recurso}/{id}` | Actualiza | 200, 400, 404 |
| DELETE | `/api/{recurso}/{id}` | Elimina | 200, 404, 409 |

Autenticación (sin token): `POST /api/auth/register`, `POST /api/auth/login`. Con token: `GET /api/auth/user-info`, `POST /api/auth/refresh-token`.

Ejemplos de cuerpo (JSON):

```json
// POST /api/vaccinecategories
{ "name": "Infantiles" }

// POST /api/vaccines  (relación muchos a muchos con categorías)
{ "name": "Triple viral (SRP)", "requiresBooster": true, "categoryIds": [1] }

// POST /api/allergies
{ "name": "Penicilina", "userId": 1 }

// POST /api/dependents
{ "name": "Sofía Escudero", "dateOfBirth": "2021-05-14T00:00:00", "userId": 1 }

// POST /api/familygroups  (relación muchos a muchos con usuarios)
{ "name": "Familia Escudero", "userIds": [1] }

// POST /api/vaccinerecords
{ "userId": 1, "dependentId": 1, "vaccineId": 1, "dateAdministered": "2024-03-10T00:00:00",
  "administeredLocation": "Clínica Las Américas", "administeredBy": "Dra. Gómez" }
```

## Cómo ejecutarlo

### Requisitos
- [.NET SDK 8](https://dotnet.microsoft.com/download) o superior
- Una instancia de **SQL Server** (elige una opción)
- [Postman](https://www.postman.com/downloads/) para probar

### 1. Base de datos (elige una)

**Opción A: Docker (la del video BONUS)**
```bash
docker compose up -d
```
Levanta SQL Server 2022 en `localhost,14330` (usuario `sa`, contraseña `Abc.123456`). No hace falta cambiar nada más.

**Opción B: SQL Server local sin Docker** (LocalDB o SQL Server Express). Define la cadena de conexión antes de ejecutar:
```powershell
# PowerShell, para LocalDB
$env:MY_VACCINE_CONNECTION_STRING = "Server=(localdb)\MSSQLLocalDB;Database=MyVaccineAppDb;Trusted_Connection=True;TrustServerCertificate=True;"
```
Para SQL Server Express usa `Server=.\SQLEXPRESS;Database=MyVaccineAppDb;Trusted_Connection=True;TrustServerCertificate=True;`.

**Opción C: GitHub Codespaces** (si el equipo no puede ejecutar Docker). Abre el repositorio en un Codespace y ejecuta ahí `docker compose up -d`.

### 2. Ejecutar la API
```bash
cd MyVaccineAppSln/MyVaccine.WebApi
dotnet restore
dotnet run --launch-profile http
```
- La API queda en `http://localhost:5133` y Swagger en `http://localhost:5133/swagger`.
- En modo Development **las migraciones se aplican solas** al arrancar y se crean todas las tablas (equivale a `dotnet ef database update`).
- Las variables `JWT_KEY` y `MY_VACCINE_CONNECTION_STRING` ya están definidas en `Properties/launchSettings.json`. Si ejecutas de otra forma, defínelas tú (la clave JWT debe tener 32 caracteres o más).

### 3. Probar con Postman
1. Importa `postman/MyVaccine.postman_collection.json` (Import → Upload Files).
2. Ejecuta las carpetas **en orden**: `0 - Autenticación`, `1` a `6`, y al final `7 - Limpieza`.
   - `Login` guarda el token en la variable `{{token}}` y toda la colección lo envía como Bearer.
   - `Info del usuario` guarda `{{userId}}`; cada `Crear` guarda su id (`{{categoryId}}`, `{{vaccineId}}`...).
   - Cada petición trae pruebas automáticas (código 201, 200, 404, 401...).
3. También puedes usar **Runner** de Postman para ejecutar toda la colección de una vez.

## Decisiones de diseño (qué se agregó respecto al código del profesor)

- **`CrudControllerBase`**: un solo controlador genérico con los 5 endpoints; así los 6 controladores son iguales y no repiten código.
- **Códigos HTTP correctos**: 201 al crear, 404 si el id no existe, 400 si los datos son inválidos o apuntan a ids que no existen, 409 si no se puede borrar por estar en uso.
- **Relaciones muchos a muchos** (Vaccine ↔ VaccineCategory, FamilyGroup ↔ User): el cliente envía solo los ids y el servicio los valida con `Include` y `ArgumentException`.
- **`[Authorize]`** en todos los endpoints de datos: sin token responden 401.
- **Validación** con FluentValidation, con los mismos límites que el `DbContext` (por ejemplo `Name` máximo 100 o 255).
- **Cadena de conexión por variable de entorno** (el código del profesor la tenía escrita dentro del código) y migraciones automáticas en Development.
- **.NET 8** (`RollForward` permite ejecutarlo también en versiones más nuevas) y `docker-compose.yml` con SQL Server.

## Estructura de carpetas

```
MyVaccineAppSln/MyVaccine.WebApi/
  Controllers/      CrudControllerBase + un controlador por tabla + AuthController
  Services/         Contracts/ (interfaces)  Implementations/ (lógica por tabla)
  Repositories/     IBaseRepository<T> y BaseRepository<T>
  Dtos/             Allergies, Dependent, FamilyGroups, VaccineCategories, VaccineRecords, Vaccines
  Models/           Entidades y MyVaccineAppDbContext
  Configurations/   DependencyInjections, DbConfigurations, AuthConfigurations, Validators, AutoMapperProfiles
  Migrations/       Migraciones de EF Core
postman/            Colección de Postman
docker-compose.yml  SQL Server para desarrollo
```

## Créditos
Base del proyecto y arquitectura: serie de videos **DevTygers (DOTNET_06 a DOTNET_17)** y el repositorio [juaneangulou/dotnet-webapi-basic](https://github.com/juaneangulou/dotnet-webapi-basic).
