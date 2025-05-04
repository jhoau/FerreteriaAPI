using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FerreteriaAPI.Data;
using FerreteriaAPI.Models;

namespace FerreteriaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaDetallesController : ControllerBase
    {
        private readonly FerreteriaContext _context;

        public FacturaDetallesController(FerreteriaContext context)
        {
            _context = context;
        }

        // GET: api/FacturaDetalles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacturaDetalle>>> GetFacturaDetalles()
        {
            return await _context.FacturaDetalles.ToListAsync();
        }

        // GET: api/FacturaDetalles/5
        [HttpPost]
        public async Task<ActionResult<FacturaDetalle>> PostFacturaDetalle(FacturaDetalle detalle)
        {
            // Validar existencia del item
            var item = await _context.Items.FindAsync(detalle.ItemId);
            if (item == null)
                return BadRequest("El item especificado no existe.");

            // Validar existencia del stock
            if (item.StockDisponible < detalle.Cantidad)
                return BadRequest("No hay suficiente stock disponible para este item.");

            // Validar existencia de factura
            var factura = await _context.Facturas.FindAsync(detalle.FacturaId);
            if (factura == null)
                return BadRequest("La factura no existe.");

            if (factura.EsAnulada)
                return BadRequest("No se pueden agregar detalles a una factura anulada.");

            // Descontar stock
            item.StockDisponible -= detalle.Cantidad;

            // Registrar el precio actual
            detalle.PrecioUnitario = item.Precio;

            _context.FacturaDetalles.Add(detalle);
            await _context.SaveChangesAsync();

            return Ok(detalle);
        }

        // PUT: api/FacturaDetalle/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFacturaDetalle(int id, FacturaDetalle nuevoDetalle)
        {
            if (id != nuevoDetalle.Id)
                return BadRequest("El ID no coincide.");

            var detalleExistente = await _context.FacturaDetalles
                .Include(fd => fd.Item)
                .FirstOrDefaultAsync(fd => fd.Id == id);

            if (detalleExistente == null)
                return NotFound("Detalle no encontrado.");

            var item = detalleExistente.Item!;
            int diferencia = nuevoDetalle.Cantidad - detalleExistente.Cantidad;

            if (diferencia > 0 && item.StockDisponible < diferencia)
                return BadRequest("No hay suficiente stock disponible para aumentar la cantidad.");

            // Ajustar stock
            item.StockDisponible -= diferencia;

            // Actualizar valores
            detalleExistente.Cantidad = nuevoDetalle.Cantidad;
            detalleExistente.PrecioUnitario = nuevoDetalle.PrecioUnitario;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(500, "Error al guardar cambios.");
            }

            return Ok(detalleExistente);
        }

        // DELETE: api/FacturaDetalle/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacturaDetalle(int id)
        {
            var detalle = await _context.FacturaDetalles
                .Include(d => d.Item)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (detalle == null)
                return NotFound("Detalle no encontrado.");

            // Devolver stock
            detalle.Item!.StockDisponible += detalle.Cantidad;

            _context.FacturaDetalles.Remove(detalle);
            await _context.SaveChangesAsync();

            return Ok($"Detalle {id} eliminado y stock actualizado.");
        }

    }
}
