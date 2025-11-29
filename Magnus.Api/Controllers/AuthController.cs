using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Magnus.Application.Interfaces;
using Magnus.Application.DTOs;
using Magnus.Application.Mappers;
using Magnus.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;
using MediatR;
using Magnus.Application.Features.Usuarios.Commands.SolicitarRestablecimientoPassword;
using Magnus.Application.Features.Usuarios.Commands.RestablecerPassword;
using Microsoft.AspNetCore.Http;

namespace Magnus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IMediator _mediator;

        public AuthController(IUnitOfWork unitOfWork, ITokenService tokenService, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("registrar")]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponseDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registrar([FromBody] RegistroDto dto)
        {
            var passwordHash = ComputeSha256Hash(dto.Password);
            var usuario = new Usuario(dto.Nombre, dto.Email, passwordHash);
            
            await _unitOfWork.Usuarios.AddAsync(usuario);
            await _unitOfWork.CommitAsync();

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

            var token = _tokenService.CreateToken(user);

            var response = new LoginResponseDto
            {
                Token = token,
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(60),
                User = user.ToResponseDto()
            };

            return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(response, "Login exitoso"));
        }

        [HttpPost("solicitar-restablecimiento")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SolicitarRestablecimiento([FromBody] string email)
        {
            var command = new SolicitarRestablecimientoPasswordCommand { Email = email };
            var result = await _mediator.Send(command);
            
            // Siempre devolver 200 aunque el correo no exista por seguridad
            return Ok(ApiResponse<object>.SuccessResponse(null, "Si el correo existe, se ha enviado un enlace para restablecer la contraseña"));
        }

        [HttpPost("restablecer-password")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RestablecerPassword([FromBody] Application.DTOs.RestablecerPasswordRequestDto dto)
        {
            var command = new RestablecerPasswordCommand(dto);
            var result = await _mediator.Send<bool>(command);
            
            if (!result)
                return BadRequest(ApiResponse<object>.ErrorResponse("No se pudo restablecer la contraseña. El token puede ser inválido o haber expirado."));
            
            return Ok(ApiResponse<object>.SuccessResponse(null, "Contraseña restablecida exitosamente"));
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
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", 
            ErrorMessage = "La contraseña debe contener al menos una mayúscula, una minúscula y un número")]
        public string Password { get; set; } = null!;
    }

    public class LoginRequestDto
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; } = null!;
    }
}
