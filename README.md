# 🛠️ Ferretería API – Sistema de Facturación

Este proyecto implementa una API RESTful con ASP.NET Core y Entity Framework Core para gestionar empleados, productos, facturas y los detalles de cada venta. Ideal para una ferretería o comercio similar.

---

## 🚀 Tecnologías utilizadas

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Swagger (OpenAPI)
- Visual Studio 2022

---

## 📦 Estructura de Entidades

### 🧑 Empleado
```csharp
public class Empleado
{
    int Id;
    string Nombre;
    string Cargo;
}

### 📦 Item
```csharp
public class Item
{
    int Id;
    string Nombre;
    decimal Precio;
    int StockDisponible;
}

### 🧾 Factura
```csharp

public class Factura
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public int EmpleadoId { get; set; }
    public Empleado? Empleado { get; set; }
    public bool EsAnulada { get; set; } = false;
    public List<FacturaDetalle> Detalles { get; set; } = new();
}

### 🧾 FacturaDetalle
```csharp
public class FacturaDetalle
{
    public int Id { get; set; }
    public int FacturaId { get; set; }
    public Factura? Factura { get; set; }
    public int ItemId { get; set; }
    public Item? Item { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}

---


## 🔧 Endpoints disponibles

### 🧑 Empleado – `/api/Empleado`
- `GET` - Listar empleados
- `GET /{id}` - Ver un empleado por ID
- `POST` - Crear nuevo empleado
- `PUT /{id}` - Editar empleado existente
- `DELETE /{id}` - Eliminar empleado

---

### 📦 Item – `/api/Item`
- `GET` - Listar todos los items
- `GET /{id}` - Ver un item específico
- `POST` - Crear nuevo item
- `PUT /{id}` - Editar item
- `DELETE /{id}` - Eliminar item

---

### 🧾 Factura – `/api/Factura`
- `GET` - Listar todas las facturas activas (excluye anuladas)
- `GET /{id}` - Ver una factura con detalles, empleado e ítems
- `POST` - Crear nueva factura (requiere solo `empleadoId`)
- `PUT /{id}` - Editar datos generales de la factura (ej: cambiar `empleadoId`)
- `PATCH /anular/{id}` - Anular factura (marca `EsAnulada = true`, no se borra)

---

### 📑 FacturaDetalle – `/api/FacturaDetalle`
- `GET /{id}` - Ver un detalle específico
- `POST` - Agregar ítem a factura (valida que haya stock suficiente)
- `PUT /{id}` - Modificar cantidad o precio (ajusta stock correctamente)
- `DELETE /{id}` - Eliminar ítem de factura y devolver el stock al inventario
