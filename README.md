# 🛠️ Ferretería API – Sistema de Facturación con Seguridad JWT

Este proyecto implementa una API RESTful en **ASP.NET Core** para gestionar empleados, productos, facturas y sesiones de usuario autenticadas con **JWT** y cacheadas con `IMemoryCache`.

---

## 🚀 Tecnologías utilizadas

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT (Json Web Token)
- BCrypt.Net para hashing de contraseñas
- IMemoryCache para gestión de sesiones activas
- Swagger / OpenAPI
- Visual Studio 2022

---

## 🔐 Seguridad Implementada

- Autenticación con **JWT Bearer**
- Contraseñas hasheadas con **BCrypt**
- Sesiones activas almacenadas en memoria con **IMemoryCache**
- Autorización por roles (`[Authorize(Roles = "Admin")]`)

---

## 📦 Funcionalidades principales

### 🧑 Empleados
- Crear, listar, actualizar y eliminar empleados
- Registro de contraseñas seguras

### 📦 Items
- CRUD de productos (con validación de stock)

### 🧾 Facturas
- Crear y anular facturas (soft delete)
- Asociar múltiples productos a cada factura
- Actualización automática del stock

### 🧠 Sesión y Autenticación
- Login en `/api/Auth/login` genera token JWT
- Sesión activa almacenada con clave `session_{id}`
- Verificación de sesión activa: `/api/Auth/session/{id}`

---

## 🧪 Probar en Swagger

1. Ir a `/swagger`
2. Hacer login en `/api/Auth/login`
3. Copiar el token JWT
4. Clic en "Authorize" y pegar: `Bearer {tu_token}`
5. Acceder a endpoints protegidos como `/api/Empleado`

---

## 🧠 Ejemplo JSON para login

```
POST /api/Auth/login
{
  "usuario": "admin",
  "contrasena": "123"
}
```

---

## 🧠 Ejemplo JSON para crear empleado

```
POST /api/Empleado
{
  "nombre": "admin",
  "cargo": "Administrador",
  "contrasena": "123"
}
```

---

## 📁 Estructura de Carpetas

- `Models/` → Clases de dominio
- `Controllers/` → Endpoints API
- `Data/` → DbContext y migraciones
- `Migrations/` → EF Core Migrations

---

## 📌 Autor

Desarrollado por **Jhoao Reyes** como proyecto backend educativo.
