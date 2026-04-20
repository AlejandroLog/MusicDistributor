using Microsoft.AspNetCore.Mvc;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;
using MusicDistributor.Core.Http;

namespace MusicDistributor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuariosController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<Usuario>>>> GetAll()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return Ok(new Response<IEnumerable<Usuario>> { Data = usuarios, Success = true, Message = "Usuarios obtenidos correctamente." });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Usuario>>> GetById(int id)
        {
            if (id <= 0) return BadRequest(new Response<Usuario> { Success = false, Message = "ID inválido." });

            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null) return NotFound(new Response<Usuario> { Success = false, Message = "Usuario no encontrado." });

            return Ok(new Response<Usuario> { Data = usuario, Success = true, Message = "Usuario encontrado." });
        }

        [HttpPost]
        public async Task<ActionResult<Response<int>>> Create([FromBody] Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Username)) return BadRequest(new Response<int> { Success = false, Message = "El Username es obligatorio." });
            if (string.IsNullOrWhiteSpace(usuario.PasswordHash)) return BadRequest(new Response<int> { Success = false, Message = "El Password es obligatorio." });
            if (string.IsNullOrWhiteSpace(usuario.Rol)) return BadRequest(new Response<int> { Success = false, Message = "El Rol es obligatorio." });

            var newId = await _usuarioRepository.AddAsync(usuario);
            return Ok(new Response<int> { Data = newId, Success = true, Message = "Usuario creado exitosamente." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<bool>>> Update(int id, [FromBody] Usuario usuario)
        {
            if (id <= 0 || id != usuario.Id) return BadRequest(new Response<bool> { Success = false, Message = "Los IDs no coinciden o son inválidos." });
            
            if (string.IsNullOrWhiteSpace(usuario.Username)) return BadRequest(new Response<bool> { Success = false, Message = "El Username es obligatorio." });
            if (string.IsNullOrWhiteSpace(usuario.Rol)) return BadRequest(new Response<bool> { Success = false, Message = "El Rol es obligatorio." });

            var result = await _usuarioRepository.UpdateAsync(usuario);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "Usuario no encontrado o no se pudo actualizar." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Usuario actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id, [FromQuery] string deletedBy)
        {
            if (id <= 0) return BadRequest(new Response<bool> { Success = false, Message = "ID inválido." });
            if (string.IsNullOrWhiteSpace(deletedBy)) return BadRequest(new Response<bool> { Success = false, Message = "Debe especificar el usuario que elimina (deletedBy)." });

            var result = await _usuarioRepository.DeleteAsync(id, deletedBy);
            if (!result) return NotFound(new Response<bool> { Success = false, Message = "Usuario no encontrado." });

            return Ok(new Response<bool> { Data = result, Success = true, Message = "Usuario eliminado correctamente." });
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<Usuario>>> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new Response<Usuario> { Success = false, Message = "Usuario y contraseña son requeridos." });

            var usuario = await _usuarioRepository.ValidateUserAsync(request.Username, request.Password);
            
            if (usuario == null) 
                return Unauthorized(new Response<Usuario> { Success = false, Message = "Credenciales incorrectas." });

            return Ok(new Response<Usuario> { Data = usuario, Success = true, Message = "Autenticación exitosa." });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}