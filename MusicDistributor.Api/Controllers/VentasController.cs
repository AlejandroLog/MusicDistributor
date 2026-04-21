using Microsoft.AspNetCore.Mvc;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;
using MusicDistributor.Core.Http;

namespace MusicDistributor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly IVentaRepository _ventaRepository;

        public VentasController(IVentaRepository ventaRepository)
        {
            _ventaRepository = ventaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<Venta>>>> GetAll()
        {
            var ventas = await _ventaRepository.GetAllAsync();
            return Ok(new Response<IEnumerable<Venta>> 
            { 
                Data = ventas, 
                Success = true, 
                Message = "Ventas obtenidas correctamente." 
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Venta>>> GetById(int id)
        {
            if (id <= 0) return BadRequest(new Response<Venta> { Success = false, Message = "ID inválido." });

            var venta = await _ventaRepository.GetByIdAsync(id);
            if (venta == null) return NotFound(new Response<Venta> { Success = false, Message = "Venta no encontrada." });

            return Ok(new Response<Venta> { Data = venta, Success = true, Message = "Venta encontrada." });
        }

        [HttpGet("cliente/{email}")]
        public async Task<ActionResult<Response<IEnumerable<Venta>>>> GetByCliente(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) 
                return BadRequest(new Response<IEnumerable<Venta>> { Success = false, Message = "El email es requerido." });

            var ventas = await _ventaRepository.GetByClienteEmailAsync(email);
            return Ok(new Response<IEnumerable<Venta>> 
            { 
                Data = ventas, 
                Success = true, 
                Message = $"Historial de ventas para {email} obtenido." 
            });
        }

        [HttpPost]
        public async Task<ActionResult<Response<int>>> Create([FromBody] Venta venta)
        {
            if (string.IsNullOrWhiteSpace(venta.ClienteEmail))
                return BadRequest(new Response<int> { Success = false, Message = "El email del cliente es obligatorio." });

            if (venta.TotalVenta < 0)
                return BadRequest(new Response<int> { Success = false, Message = "El total de la venta no puede ser negativo." });

            var newId = await _ventaRepository.AddAsync(venta);
            return Ok(new Response<int> { Data = newId, Success = true, Message = "Venta registrada exitosamente." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<bool>>> Update(int id, [FromBody] Venta venta)
        {
            if (id <= 0 || id != venta.Id)
                return BadRequest(new Response<bool> { Success = false, Message = "Los IDs no coinciden o son inválidos." });

            var result = await _ventaRepository.UpdateAsync(venta);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "No se pudo actualizar la venta." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Venta actualizada correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id, [FromQuery] string deletedBy)
        {
            if (id <= 0) return BadRequest(new Response<bool> { Success = false, Message = "ID inválido." });
            if (string.IsNullOrWhiteSpace(deletedBy)) 
                return BadRequest(new Response<bool> { Success = false, Message = "Debe especificar quién elimina la venta." });

            var result = await _ventaRepository.DeleteAsync(id, deletedBy);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "Venta no encontrada." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Venta eliminada correctamente." });
        }
    }
}