using Microsoft.AspNetCore.Mvc;
using Magnus.Application.Interfaces;
using Magnus.Application.DTOs;
using Magnus.Application.Mappers;
using Magnus.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;

namespace Magnus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public AuthController(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Registro rápido de usuario (para pruebas)
        /// </summary>
        /// <response code="200">Usuario registrado exitosamente</response>
        /// <response code="400">Datos de entrada inválidos</response>
        [AllowAnonymous]
        [HttpPost("registrar")]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponseDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registrar([FromBody] RegistroDto dto)
        {
            // Crear usuario simple (sin hash de password por ahora)
            var passwordHash = ComputeSha256Hash(dto.Password);
            var usuario = new Usuario(dto.Nombre, dto.Email, passwordHash);
            
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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto credentials)
        {
            // Buscar usuario por email
            var user = await _unitOfWork.Usuarios.GetByEmailAsync(credentials.Email);
            if (user == null)
            {
                return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResponse("Credenciales inválidas"));
            }

            var incomingHash = ComputeSha256Hash(credentials.Password);
            if (!string.Equals(incomingHash, user.PasswordHash, StringComparison.Ordinal))
            {
                return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResponse("Credenciales inválidas"));
            }

            var token = _tokenService.GenerateToken(user.Id, user.Nombre, user.Email);

            var response = new LoginResponseDto
            {
                Token = token,
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(60),
                User = user.ToResponseDto()
            };

            return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(response, "Login exitoso"));
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var sb = new StringBuilder();
            foreach (var b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

    public class RegistroDto
    {
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
