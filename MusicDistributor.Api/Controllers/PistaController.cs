using Microsoft.AspNetCore.Mvc;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;
using MusicDistributor.Core.Http;

namespace MusicDistributor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PistasController : ControllerBase
    {
        private readonly IPistaRepository _pistaRepository;

        public PistasController(IPistaRepository pistaRepository)
        {
            _pistaRepository = pistaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<Pista>>>> GetAll()
        {
            var pistas = await _pistaRepository.GetAllAsync();
            return Ok(new Response<IEnumerable<Pista>> 
            { 
                Data = pistas, 
                Success = true, 
                Message = "Pistas obtenidas correctamente." 
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Pista>>> GetById(int id)
        {
            if (id <= 0) return BadRequest(new Response<Pista> { Success = false, Message = "ID inválido." });

            var pista = await _pistaRepository.GetByIdAsync(id);
            if (pista == null) return NotFound(new Response<Pista> { Success = false, Message = "Pista no encontrada." });

            return Ok(new Response<Pista> { Data = pista, Success = true, Message = "Pista encontrada." });
        }

        [HttpGet("lanzamiento/{lanzamientoId}")]
        public async Task<ActionResult<Response<IEnumerable<Pista>>>> GetByLanzamiento(int lanzamientoId)
        {
            if (lanzamientoId <= 0) return BadRequest(new Response<IEnumerable<Pista>> { Success = false, Message = "ID de lanzamiento inválido." });

            var pistas = await _pistaRepository.GetByLanzamientoIdAsync(lanzamientoId);
            return Ok(new Response<IEnumerable<Pista>> 
            { 
                Data = pistas, 
                Success = true, 
                Message = $"Pistas del lanzamiento {lanzamientoId} obtenidas correctamente." 
            });
        }

        [HttpPost]
        public async Task<ActionResult<Response<int>>> Create([FromBody] Pista pista)
        {
            if (pista.LanzamientoId <= 0) return BadRequest(new Response<int> { Success = false, Message = "El LanzamientoId es obligatorio." });
            if (string.IsNullOrWhiteSpace(pista.TituloCancion)) return BadRequest(new Response<int> { Success = false, Message = "El título de la canción es obligatorio." });
            if (pista.NumeroTrack <= 0) return BadRequest(new Response<int> { Success = false, Message = "El número de track debe ser mayor a cero." });

            var newId = await _pistaRepository.AddAsync(pista);
            return Ok(new Response<int> { Data = newId, Success = true, Message = "Pista creada exitosamente." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<bool>>> Update(int id, [FromBody] Pista pista)
        {
            if (id <= 0 || id != pista.Id) 
                return BadRequest(new Response<bool> { Success = false, Message = "Los IDs no coinciden o son inválidos." });
            
            if (string.IsNullOrWhiteSpace(pista.TituloCancion)) 
                return BadRequest(new Response<bool> { Success = false, Message = "El título es obligatorio." });

            var result = await _pistaRepository.UpdateAsync(pista);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "Pista no encontrada o no se pudo actualizar." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Pista actualizada correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id, [FromQuery] string deletedBy)
        {
            if (id <= 0) return BadRequest(new Response<bool> { Success = false, Message = "ID inválido." });
            if (string.IsNullOrWhiteSpace(deletedBy)) 
                return BadRequest(new Response<bool> { Success = false, Message = "Debe especificar quién elimina la pista." });

            var result = await _pistaRepository.DeleteAsync(id, deletedBy);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "Pista no encontrada." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Pista eliminada correctamente (Soft Delete)." });
        }
    }
}