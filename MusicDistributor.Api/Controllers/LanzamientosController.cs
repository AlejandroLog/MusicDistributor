using Microsoft.AspNetCore.Mvc;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;
using MusicDistributor.Core.Http;

namespace MusicDistributor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanzamientosController : ControllerBase
    {
        private readonly ILanzamientoRepository _lanzamientoRepository;

        public LanzamientosController(ILanzamientoRepository lanzamientoRepository)
        {
            _lanzamientoRepository = lanzamientoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<Lanzamiento>>>> GetAll()
        {
            var lanzamientos = await _lanzamientoRepository.GetAllAsync();
            return Ok(new Response<IEnumerable<Lanzamiento>> { Data = lanzamientos, Success = true, Message = "Lanzamientos obtenidos correctamente." });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Lanzamiento>>> GetById(int id)
        {
            if (id <= 0) return BadRequest(new Response<Lanzamiento> { Success = false, Message = "ID inválido." });

            var lanzamiento = await _lanzamientoRepository.GetByIdAsync(id);
            if (lanzamiento == null) return NotFound(new Response<Lanzamiento> { Success = false, Message = "Lanzamiento no encontrado." });

            return Ok(new Response<Lanzamiento> { Data = lanzamiento, Success = true, Message = "Lanzamiento encontrado." });
        }

        [HttpPost]
        public async Task<ActionResult<Response<int>>> Create([FromBody] Lanzamiento lanzamiento)
        {
            // Validaciones rigurosas
            if (lanzamiento.ArtistaId <= 0)
                return BadRequest(new Response<int> { Success = false, Message = "Debe proporcionar un ID de Artista válido." });

            if (string.IsNullOrWhiteSpace(lanzamiento.TituloObra)) 
                return BadRequest(new Response<int> { Success = false, Message = "El Título de la obra es obligatorio." });
            
            if (string.IsNullOrWhiteSpace(lanzamiento.TipoLanzamiento)) 
                return BadRequest(new Response<int> { Success = false, Message = "El Tipo de lanzamiento es obligatorio (Ej: EP, LP, Single)." });

            var newId = await _lanzamientoRepository.AddAsync(lanzamiento);
            return Ok(new Response<int> { Data = newId, Success = true, Message = "Lanzamiento creado exitosamente." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<bool>>> Update(int id, [FromBody] Lanzamiento lanzamiento)
        {
            if (id <= 0 || id != lanzamiento.Id) 
                return BadRequest(new Response<bool> { Success = false, Message = "Los IDs no coinciden o son inválidos." });
            
            if (lanzamiento.ArtistaId <= 0)
                return BadRequest(new Response<bool> { Success = false, Message = "Debe proporcionar un ID de Artista válido." });

            if (string.IsNullOrWhiteSpace(lanzamiento.TituloObra)) 
                return BadRequest(new Response<bool> { Success = false, Message = "El Título de la obra es obligatorio." });
            
            if (string.IsNullOrWhiteSpace(lanzamiento.TipoLanzamiento)) 
                return BadRequest(new Response<bool> { Success = false, Message = "El Tipo de lanzamiento es obligatorio." });

            var result = await _lanzamientoRepository.UpdateAsync(lanzamiento);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "Lanzamiento no encontrado o no se pudo actualizar." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Lanzamiento actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id, [FromQuery] string deletedBy)
        {
            if (id <= 0) return BadRequest(new Response<bool> { Success = false, Message = "ID inválido." });
            if (string.IsNullOrWhiteSpace(deletedBy)) return BadRequest(new Response<bool> { Success = false, Message = "Debe especificar el usuario que elimina (deletedBy)." });

            var result = await _lanzamientoRepository.DeleteAsync(id, deletedBy);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "Lanzamiento no encontrado." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Lanzamiento eliminado correctamente." });
        }
    }
}