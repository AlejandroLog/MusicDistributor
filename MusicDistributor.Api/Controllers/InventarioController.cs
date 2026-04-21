using Microsoft.AspNetCore.Mvc;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;
using MusicDistributor.Core.Http;

namespace MusicDistributor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly IInventarioRepository _inventarioRepository;

        public InventarioController(IInventarioRepository inventarioRepository)
        {
            _inventarioRepository = inventarioRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<Inventario>>>> GetAll()
        {
            var items = await _inventarioRepository.GetAllAsync();
            return Ok(new Response<IEnumerable<Inventario>> { Data = items, Success = true, Message = "Inventario obtenido correctamente." });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Inventario>>> GetById(int id)
        {
            if (id <= 0) return BadRequest(new Response<Inventario> { Success = false, Message = "ID inválido." });

            var item = await _inventarioRepository.GetByIdAsync(id);
            if (item == null) return NotFound(new Response<Inventario> { Success = false, Message = "Registro de inventario no encontrado." });

            return Ok(new Response<Inventario> { Data = item, Success = true, Message = "Registro de inventario encontrado." });
        }

        [HttpGet("lanzamiento/{lanzamientoId}")]
        public async Task<ActionResult<Response<IEnumerable<Inventario>>>> GetByLanzamiento(int lanzamientoId)
        {
            if (lanzamientoId <= 0) return BadRequest(new Response<IEnumerable<Inventario>> { Success = false, Message = "ID de lanzamiento inválido." });

            var items = await _inventarioRepository.GetByLanzamientoIdAsync(lanzamientoId);
            return Ok(new Response<IEnumerable<Inventario>> { Data = items, Success = true, Message = "Formatos del lanzamiento obtenidos." });
        }

        [HttpPost]
        public async Task<ActionResult<Response<int>>> Create([FromBody] Inventario inventario)
        {
            if (inventario.LanzamientoId <= 0 || inventario.FormatoFisicoId <= 0)
                return BadRequest(new Response<int> { Success = false, Message = "Lanzamiento y Formato son obligatorios." });
            
            if (inventario.PrecioVenta < 0)
                return BadRequest(new Response<int> { Success = false, Message = "El precio no puede ser negativo." });

            if (string.IsNullOrWhiteSpace(inventario.Sku))
                return BadRequest(new Response<int> { Success = false, Message = "El SKU es obligatorio para control de almacén." });

            var newId = await _inventarioRepository.AddAsync(inventario);
            return Ok(new Response<int> { Data = newId, Success = true, Message = "Registro de inventario creado exitosamente." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<bool>>> Update(int id, [FromBody] Inventario inventario)
        {
            if (id <= 0 || id != inventario.Id)
                return BadRequest(new Response<bool> { Success = false, Message = "Los IDs no coinciden o son inválidos." });

            if (inventario.PrecioVenta < 0)
                return BadRequest(new Response<bool> { Success = false, Message = "El precio no puede ser negativo." });

            var result = await _inventarioRepository.UpdateAsync(inventario);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "No se encontró el registro para actualizar." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Inventario actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id, [FromQuery] string deletedBy)
        {
            if (id <= 0) return BadRequest(new Response<bool> { Success = false, Message = "ID inválido." });
            if (string.IsNullOrWhiteSpace(deletedBy)) 
                return BadRequest(new Response<bool> { Success = false, Message = "Debe especificar quién realiza la baja." });

            var result = await _inventarioRepository.DeleteAsync(id, deletedBy);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "Registro de inventario no encontrado." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Registro eliminado del inventario (Soft Delete)." });
        }
    }
}