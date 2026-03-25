# Eventia Backend - Gestión de Eventos 🚀

Este es el backend de la plataforma **Eventia**, desarrollado con **.NET 8** siguiendo los principios de **Clean Architecture** y **CQRS (MediatR)**.

## 🛠️ Tecnologías
- **Framework**: .NET 8 ASP.NET Core
- **Base de Datos**: MySQL (Pomelo Entity Framework Core)
- **Seguridad**: JWT Bearer Authentication & BCrypt for Password Hashing
- **Patrones**: Clean Architecture, Repository Pattern, CQRS with MediatR
- **Librerías Clave**: 
  - `MediatR` para mediación de comandos/consultas.
  - `Serilog` para logging estructurado.
  - `DotNetEnv` para gestión de variables de entorno.

## 🏗️ Estructura del Proyecto
- **Eventia.Domain**: Entidades Core, Enums e Interfaces de Repositorios.
- **Eventia.Application**: DTOs, Casos de Uso (Handlers), Interfaces de Servicios y Lógica de Negocio.
- **Eventia.Infrastructure**: Implementación de Repositorios, Contexto de BD (EF Core), Migraciones y Servicios Externos.
- **Eventia.API**: Controladores, Middlewares y Configuración de la Aplicación.

## 🚀 Configuración y Ejecución

### 1. Variables de Entorno
Crea un archivo `.env` en `src/Eventia.API/`:
```env
DB_CONNECTION="Server=tu_servidor;Port=tu_puerto;Database=tu_db;User=root;Password=tu_pass;"
JWT_SECRET="una_clave_muy_secreta_de_al_menos_32_caracteres"
JWT_ISSUER="Eventia.API"
JWT_AUDIENCE="Eventia.Client"
```

### 2. Migraciones y Ejecución
```powershell
dotnet ef database update -p src/Eventia.Infrastructure -s src/Eventia.API
dotnet run --project src/Eventia.API
```

---

## 📖 Documentación de Endpoints (API Reference)

Todos los endpoints (excepto Auth) requieren el encabezado: 
`Authorization: Bearer {tu_token}`

### 🔐 Autenticación (Auth)
Controlador encargado del acceso y registro de usuarios.

| Método | Endpoint | Descripción | Cuerpo (JSON) |
| :--- | :--- | :--- | :--- |
| **POST** | `/api/auth/register` | Registra un nuevo usuario en el sistema. | `{ "name", "email", "password", "role" (1-3) }` |
| **POST** | `/api/auth/login` | Autentica un usuario y devuelve un Token JWT. | `{ "email", "password" }` |

### 🎫 Tickets (Tickets)
Gestión completa del ciclo de vida de los tickets de soporte/logística.

| Método | Endpoint | Descripción | Notas |
| :--- | :--- | :--- | :--- |
| **GET** | `/api/tickets` | Obtiene la lista de todos los tickets. | Retorna lista de `TicketResponse`. |
| **GET** | `/api/tickets/{id}` | Obtiene los detalles de un ticket específico. | Requiere el `Guid` del ticket. |
| **POST** | `/api/tickets` | Crea un nuevo ticket. | El `CreatedById` se toma del Token JWT. |
| **PUT** | `/api/tickets/{id}` | Actualiza la información de un ticket. | Permite editar título, descripción y encargado. |
| **PATCH** | `/api/tickets/{id}/status` | Cambia el estado de un ticket (Abierto, En Proceso, Resuelto, etc). | `/status?status=2` (query param). |
| **DELETE** | `/api/tickets/{id}` | Elimina un ticket del sistema. | Acción irreversible. |
| **GET** | `/api/tickets/{id}/history` | **Trazabilidad**: Retorna el historial de cambios del ticket. | Muestra quién cambió qué y cuándo. |

### 👤 Usuarios (Users)
Gestión administrativa de usuarios.

| Método | Endpoint | Descripción | Seguridad |
| :--- | :--- | :--- | :--- |
| **GET** | `/api/users/{id}` | Obtiene la información de un usuario registrado. | Cualquiera autenticado. |
| **PATCH** | `/api/users/{id}/disable` | Desactiva la cuenta de un usuario. | Solo disponible para Administradores. |

---

## 📊 Trazabilidad y Observabilidad
- **TicketHistory**: Cada creación o cambio de estado genera un log automático en la base de datos para auditoría.
- **Logging**: Los logs se guardan en la carpeta `/logs` de la API y también se muestran en la consola mediante Serilog.

---
Desarrollado para la Prueba Técnica de Eventia S.A.S.
