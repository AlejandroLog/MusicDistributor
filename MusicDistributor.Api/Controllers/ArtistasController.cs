using Microsoft.AspNetCore.Mvc;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;
using MusicDistributor.Core.Http;

namespace MusicDistributor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistasController : ControllerBase
    {
        private readonly IArtistaRepository _artistaRepository;

        public ArtistasController(IArtistaRepository artistaRepository)
        {
            _artistaRepository = artistaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<Artista>>>> GetAll()
        {
            var artistas = await _artistaRepository.GetAllAsync();
            return Ok(new Response<IEnumerable<Artista>> { Data = artistas, Success = true, Message = "Artistas obtenidos correctamente." });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Artista>>> GetById(int id)
        {
            if (id <= 0) return BadRequest(new Response<Artista> { Success = false, Message = "ID inválido." });

            var artista = await _artistaRepository.GetByIdAsync(id);
            if (artista == null) return NotFound(new Response<Artista> { Success = false, Message = "Artista no encontrado." });

            return Ok(new Response<Artista> { Data = artista, Success = true, Message = "Artista encontrado." });
        }

        [HttpPost]
        public async Task<ActionResult<Response<int>>> Create([FromBody] Artista artista)
        {
            // Validaciones rigurosas
            if (string.IsNullOrWhiteSpace(artista.NombreBanda)) 
                return BadRequest(new Response<int> { Success = false, Message = "El Nombre de la banda es obligatorio." });
            
            if (artista.GeneroMusicalId <= 0)
                return BadRequest(new Response<int> { Success = false, Message = "Debe proporcionar un ID de Género Musical válido." });

            var newId = await _artistaRepository.AddAsync(artista);
            return Ok(new Response<int> { Data = newId, Success = true, Message = "Artista creado exitosamente." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<bool>>> Update(int id, [FromBody] Artista artista)
        {
            if (id <= 0 || id != artista.Id) 
                return BadRequest(new Response<bool> { Success = false, Message = "Los IDs no coinciden o son inválidos." });
            
            if (string.IsNullOrWhiteSpace(artista.NombreBanda)) 
                return BadRequest(new Response<bool> { Success = false, Message = "El Nombre de la banda es obligatorio." });

            if (artista.GeneroMusicalId <= 0)
                return BadRequest(new Response<bool> { Success = false, Message = "Debe proporcionar un ID de Género Musical válido." });

            var result = await _artistaRepository.UpdateAsync(artista);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "Artista no encontrado o no se pudo actualizar." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Artista actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id, [FromQuery] string deletedBy)
        {
            if (id <= 0) return BadRequest(new Response<bool> { Success = false, Message = "ID inválido." });
            if (string.IsNullOrWhiteSpace(deletedBy)) return BadRequest(new Response<bool> { Success = false, Message = "Debe especificar el usuario que elimina (deletedBy)." });

            var result = await _artistaRepository.DeleteAsync(id, deletedBy);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "Artista no encontrado." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Artista eliminado correctamente." });
        }
    }
}