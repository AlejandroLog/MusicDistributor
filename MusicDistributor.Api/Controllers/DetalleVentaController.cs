using Microsoft.AspNetCore.Mvc;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;
using MusicDistributor.Core.Http;

namespace MusicDistributor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleVentaController : ControllerBase
    {
        private readonly IDetalleVentaRepository _detalleRepository;

        public DetalleVentaController(IDetalleVentaRepository detalleRepository)
        {
            _detalleRepository = detalleRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<DetalleVenta>>>> GetAll()
        {
            var detalles = await _detalleRepository.GetAllAsync();
            return Ok(new Response<IEnumerable<DetalleVenta>> 
            { 
                Data = detalles, 
                Success = true, 
                Message = "Detalles de venta obtenidos correctamente." 
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<DetalleVenta>>> GetById(int id)
        {
            if (id <= 0) return BadRequest(new Response<DetalleVenta> { Success = false, Message = "ID inválido." });

            var detalle = await _detalleRepository.GetByIdAsync(id);
            if (detalle == null) return NotFound(new Response<DetalleVenta> { Success = false, Message = "Detalle de venta no encontrado." });

            return Ok(new Response<DetalleVenta> { Data = detalle, Success = true, Message = "Detalle de venta encontrado." });
        }

        [HttpGet("venta/{ventaId}")]
        public async Task<ActionResult<Response<IEnumerable<DetalleVenta>>>> GetByVenta(int ventaId)
        {
            if (ventaId <= 0) return BadRequest(new Response<IEnumerable<DetalleVenta>> { Success = false, Message = "ID de venta inválido." });

            var detalles = await _detalleRepository.GetByVentaIdAsync(ventaId);
            return Ok(new Response<IEnumerable<DetalleVenta>> 
            { 
                Data = detalles, 
                Success = true, 
                Message = $"Artículos de la venta {ventaId} obtenidos correctamente." 
            });
        }

        [HttpPost]
        public async Task<ActionResult<Response<int>>> Create([FromBody] DetalleVenta detalle)
        {
            if (detalle.VentaId <= 0 || detalle.InventarioId <= 0)
                return BadRequest(new Response<int> { Success = false, Message = "VentaId e InventarioId son obligatorios." });

            if (detalle.Cantidad <= 0)
                return BadRequest(new Response<int> { Success = false, Message = "La cantidad debe ser mayor a cero." });

            var newId = await _detalleRepository.AddAsync(detalle);
            return Ok(new Response<int> { Data = newId, Success = true, Message = "Detalle de venta creado exitosamente." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<bool>>> Update(int id, [FromBody] DetalleVenta detalle)
        {
            if (id <= 0 || id != detalle.Id)
                return BadRequest(new Response<bool> { Success = false, Message = "Los IDs no coinciden o son inválidos." });

            if (detalle.Cantidad <= 0)
                return BadRequest(new Response<bool> { Success = false, Message = "La cantidad debe ser mayor a cero." });

            var result = await _detalleRepository.UpdateAsync(detalle);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "Detalle de venta no encontrado o no se pudo actualizar." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Detalle de venta actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id, [FromQuery] string deletedBy)
        {
            if (id <= 0) return BadRequest(new Response<bool> { Success = false, Message = "ID inválido." });
            if (string.IsNullOrWhiteSpace(deletedBy)) 
                return BadRequest(new Response<bool> { Success = false, Message = "Debe especificar el usuario que elimina (deletedBy)." });

            var result = await _detalleRepository.DeleteAsync(id, deletedBy);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "Detalle de venta no encontrado." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Detalle de venta eliminado correctamente." });
        }
    }
}