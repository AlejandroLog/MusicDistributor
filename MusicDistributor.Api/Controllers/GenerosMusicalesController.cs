using Microsoft.AspNetCore.Mvc;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;
using MusicDistributor.Core.Http;

namespace MusicDistributor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerosMusicalesController : ControllerBase
    {
        private readonly IGeneroMusicalRepository _generoRepository;

        public GenerosMusicalesController(IGeneroMusicalRepository generoRepository)
        {
            _generoRepository = generoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<GeneroMusical>>>> GetAll()
        {
            var generos = await _generoRepository.GetAllAsync();
            return Ok(new Response<IEnumerable<GeneroMusical>> { Data = generos, Success = true, Message = "Géneros obtenidos correctamente." });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<GeneroMusical>>> GetById(int id)
        {
            if (id <= 0) return BadRequest(new Response<GeneroMusical> { Success = false, Message = "ID inválido." });

            var genero = await _generoRepository.GetByIdAsync(id);
            if (genero == null) return NotFound(new Response<GeneroMusical> { Success = false, Message = "Género musical no encontrado." });

            return Ok(new Response<GeneroMusical> { Data = genero, Success = true, Message = "Género musical encontrado." });
        }

        [HttpPost]
        public async Task<ActionResult<Response<int>>> Create([FromBody] GeneroMusical genero)
        {
            if (string.IsNullOrWhiteSpace(genero.Nombre)) 
                return BadRequest(new Response<int> { Success = false, Message = "El Nombre del género es obligatorio." });

            var newId = await _generoRepository.AddAsync(genero);
            return Ok(new Response<int> { Data = newId, Success = true, Message = "Género musical creado exitosamente." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<bool>>> Update(int id, [FromBody] GeneroMusical genero)
        {
            if (id <= 0 || id != genero.Id) 
                return BadRequest(new Response<bool> { Success = false, Message = "Los IDs no coinciden o son inválidos." });
            
            if (string.IsNullOrWhiteSpace(genero.Nombre)) 
                return BadRequest(new Response<bool> { Success = false, Message = "El Nombre del género es obligatorio." });

            var result = await _generoRepository.UpdateAsync(genero);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "Género musical no encontrado o no se pudo actualizar." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Género musical actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id, [FromQuery] string deletedBy)
        {
            if (id <= 0) return BadRequest(new Response<bool> { Success = false, Message = "ID inválido." });
            if (string.IsNullOrWhiteSpace(deletedBy)) return BadRequest(new Response<bool> { Success = false, Message = "Debe especificar el usuario que elimina (deletedBy)." });

            var result = await _generoRepository.DeleteAsync(id, deletedBy);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "Género musical no encontrado." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Género musical eliminado correctamente." });
        }
    }
}