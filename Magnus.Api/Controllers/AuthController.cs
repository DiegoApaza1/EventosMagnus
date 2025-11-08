using Microsoft.AspNetCore.Mvc;
using Magnus.Application.Interfaces;
using Magnus.Application.DTOs;
using Magnus.Application.Mappers;
using Magnus.Domain.Entities;

namespace Magnus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Registro rápido de usuario (para pruebas)
        /// </summary>
        /// <response code="200">Usuario registrado exitosamente</response>
        /// <response code="400">Datos de entrada inválidos</response>
        [HttpPost("registrar")]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponseDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registrar([FromBody] RegistroDto dto)
        {
            // Crear usuario simple (sin hash de password por ahora)
            var usuario = new Usuario(dto.Nombre, dto.Email, dto.Password);
            
            await _unitOfWork.Usuarios.AddAsync(usuario);
            await _unitOfWork.CommitAsync();

            // Retornar DTO en lugar de exponer la entidad
            var usuarioDto = usuario.ToResponseDto();
            var response = ApiResponse<UsuarioResponseDto>.SuccessResponse(
                usuarioDto,
                "Usuario creado exitosamente. Usa este ID como organizadorId para crear eventos."
            );
            
            return Ok(response);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] dynamic credentials)
        {
            // Ejemplo simple, sin JWT por ahora
            if (credentials.username == "admin" && credentials.password == "123")
                return Ok(new { token = "fake-jwt-token", user = credentials.username });

            return Unauthorized(new { message = "Credenciales inválidas" });
        }
    }

    public class RegistroDto
    {
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
