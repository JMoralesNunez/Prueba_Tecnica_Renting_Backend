# Eventia Backend - Gestión de Eventos

Este es el backend de la plataforma **Eventia**, desarrollado con **.NET 8** siguiendo los principios de **Clean Architecture** y **CQRS (MediatR)**.

## Tecnologías
- **Framework**: .NET 8 ASP.NET Core
- **Base de Datos**: MySQL (Pomelo Entity Framework Core)
- **Seguridad**: JWT Bearer Authentication & BCrypt for Password Hashing
- **Patrones**: Clean Architecture, Repository Pattern, CQRS with MediatR
- **Librerías Clave**: 
  - `MediatR` para mediación de comandos/consultas.
  - `Serilog` para logging estructurado.
  - `DotNetEnv` para gestión de variables de entorno.

## Estructura del Proyecto
- **Eventia.Domain**: Entidades Core, Enums e Interfaces de Repositorios.
- **Eventia.Application**: DTOs, Casos de Uso (Handlers), Interfaces de Servicios y Lógica de Negocio.
- **Eventia.Infrastructure**: Implementación de Repositorios, Contexto de BD (EF Core), Migraciones y Servicios Externos.
- **Eventia.API**: Controladores, Middlewares y Configuración de la Aplicación.

## Configuración y Ejecución

### 1. Requisitos
- .NET 8 SDK
- MySQL Server

### 2. Variables de Entorno
Crea un archivo `.env` en `src/Eventia.API/` basado en `.env.example`:
```env
DB_CONNECTION="Server=tu_servidor;Port=tu_puerto;Database=tu_db;User=root;Password=tu_pass;"
JWT_SECRET="una_clave_muy_secreta_de_al_menos_32_caracteres"
JWT_ISSUER="Eventia.API"
JWT_AUDIENCE="Eventia.Client"
```

### 3. Migraciones
Para aplicar el esquema a la base de datos:
```powershell
dotnet ef database update -p src/Eventia.Infrastructure -s src/Eventia.API
```

### 4. Ejecutar
```powershell
dotnet run --project src/Eventia.API
```

## Endpoints Principales
- **Auth**: `POST /api/auth/register`, `POST /api/auth/login`
- **Tickets**: `GET /api/tickets`, `POST /api/tickets`, `GET /api/tickets/{id}/history`
- **Users**: `GET /api/users/{id}`, `PATCH /api/users/{id}/disable`

## Trazabilidad
El sistema registra automáticamente cada creación y cambio de estado en la tabla `TicketHistory`, permitiendo un seguimiento auditable de cada evento.

---
Desarrollado para la Prueba Técnica de Eventia S.A.S.
