using Microsoft.AspNetCore.Mvc;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;
using MusicDistributor.Core.Http;

namespace MusicDistributor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormatosFisicosController : ControllerBase
    {
        private readonly IFormatoFisicoRepository _formatoRepository;

        public FormatosFisicosController(IFormatoFisicoRepository formatoRepository)
        {
            _formatoRepository = formatoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<FormatoFisico>>>> GetAll()
        {
            var formatos = await _formatoRepository.GetAllAsync();
            return Ok(new Response<IEnumerable<FormatoFisico>> { Data = formatos, Success = true, Message = "Formatos obtenidos correctamente." });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<FormatoFisico>>> GetById(int id)
        {
            if (id <= 0) return BadRequest(new Response<FormatoFisico> { Success = false, Message = "ID inválido." });

            var formato = await _formatoRepository.GetByIdAsync(id);
            if (formato == null) return NotFound(new Response<FormatoFisico> { Success = false, Message = "Formato no encontrado." });

            return Ok(new Response<FormatoFisico> { Data = formato, Success = true, Message = "Formato encontrado." });
        }

        [HttpPost]
        public async Task<ActionResult<Response<int>>> Create([FromBody] FormatoFisico formato)
        {
            if (string.IsNullOrWhiteSpace(formato.Nombre)) 
                return BadRequest(new Response<int> { Success = false, Message = "El Nombre del formato es obligatorio." });

            var newId = await _formatoRepository.AddAsync(formato);
            return Ok(new Response<int> { Data = newId, Success = true, Message = "Formato creado exitosamente." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<bool>>> Update(int id, [FromBody] FormatoFisico formato)
        {
            if (id <= 0 || id != formato.Id) 
                return BadRequest(new Response<bool> { Success = false, Message = "Los IDs no coinciden o son inválidos." });
            
            if (string.IsNullOrWhiteSpace(formato.Nombre)) 
                return BadRequest(new Response<bool> { Success = false, Message = "El Nombre del formato es obligatorio." });

            var result = await _formatoRepository.UpdateAsync(formato);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "Formato no encontrado o no se pudo actualizar." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Formato actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id, [FromQuery] string deletedBy)
        {
            if (id <= 0) return BadRequest(new Response<bool> { Success = false, Message = "ID inválido." });
            if (string.IsNullOrWhiteSpace(deletedBy)) return BadRequest(new Response<bool> { Success = false, Message = "Debe especificar el usuario que elimina (deletedBy)." });

            var result = await _formatoRepository.DeleteAsync(id, deletedBy);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "Formato no encontrado." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Formato eliminado correctamente." });
        }
    }
}