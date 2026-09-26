# Tourism Booking API - Caso Práctico de Desarrollo Backend .NET

API RESTful desarrollada para la gestión de experiencias turísticas y el registro de reservas, con control de capacidad disponible y manejo de concurrencia.

## 🛠️ Tecnologías Utilizadas

- **Lenguaje/Framework:** .NET 8 (ASP.NET Core Web API)
- **Persistencia/ORM:** Entity Framework Core 8 (SQL Server)
- **Arquitectura:** Clean Architecture (Domain, Application, Infrastructure, Api, Tests)
- **Documentación API:** Swagger / OpenAPI
- **Pruebas Automatizadas:** xUnit + Entity Framework Core InMemory Database

---

## 🏛️ Decisiones de Arquitectura y Criterios Técnicos

1. **Clean Architecture & SOLID:** Se separaron las responsabilidades en 4 proyectos desacoplados. Las entidades de dominio no dependen de librerías externas ni de Entity Framework Core.
2. **Estrategia de Acceso a Datos (EF Core):** Se utilizó Entity Framework Core mediante Fluent API en la capa de Infraestructura para garantizar mantenibilidad, mapeo fuertemente tipado e Inversión de Dependencias.
3. **Manejo de Concurrencia (Sobreventa):** Para prevenir que dos peticiones simultáneas superen la capacidad de una experiencia en la misma fecha, el registro de la reserva se ejecuta dentro de una **transacción explícita con Nivel de Aislamiento `Serializable`**, garantizando atomicidad y consistencia estricta en el cálculo de disponibilidad.
4. **Soft Delete / Inactivación Lógica:** Las experiencias usan el estado `ExperienceStatus.Inactive` y las reservas `BookingStatus.Cancelled`. No existen borrados físicos en la base de datos para preservar la trazabilidad histórica.
5. **Manejo Centralizado de Errores:** Implementado mediante un Middleware personalizado que mapea excepciones de negocio (`ArgumentException`, `InvalidOperationException`) a respuestas estandarizadas con códigos HTTP apropiados (400, 404, 500).

---

## 🚀 Instrucciones de Configuración y Ejecución

### Prerrequisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server/) (Instancia local o SQL Express)

### Pasos para Ejecutar

1. **Clonar o descomprimir el proyecto.**
2. **Configurar Cadena de Conexión:**
   Abre el archivo `src/TourismBooking.Api/appsettings.json` y ajusta el `ConnectionString` según tu servidor local de SQL Server:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=TourismBookingDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```
3. **Aplicar Migraciones y Base de Datos:**
    Abre una terminal en la raíz de la solución y ejecuta:
    ```Bash
    dotnet ef database update --project src/TourismBooking.Infrastructure --startup-project src/TourismBooking.Api
    ```
4. **Ejecutar la API:**
    En la terminal ejecuta:
    ```Bash
    dotnet run --project src/TourismBooking.Api
    ```
    Abre tu navegador en https://localhost:7025/swagger (o el puerto configurado) para interactuar con Swagger.
5. **Ejecutar Pruebas Unitarias:**
    En la terminal ejecuta:
    ```Bash
    dotnet test
    ```

## 📁 Scripts de Base de Datos
En la carpeta /Database se incluyen los scripts SQL consolidados:
* 01_CreateDatabaseAndTables.sql: Script completo de creación de BD, tablas, restricciones CHECK, claves foráneas e índices optimizados.
* 02_SeedData.sql: Datos iniciales de prueba para cargar experiencias.

## 👤 Autor
- **Nombre:** Juan Andres Garzon Garay
- **Correo Electrónico:** jag_4@hotmail.com
- **LinkedIn:** https://www.linkedin.com/in/juan-a-garzon-g/