using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Magnus.Application.DTOs;
using Magnus.Application.Mappers;
using Magnus.Application.Features.Usuarios.Commands.RegistrarUsuario;
using Magnus.Application.Features.Usuarios.Commands.LoginUsuario;
using Magnus.Application.Features.Usuarios.Commands.SolicitarRestablecimientoPassword;
using Magnus.Application.Features.Usuarios.Commands.RestablecerPassword;
using Microsoft.AspNetCore.Authorization;
using MediatR;

namespace Magnus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("registrar")]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponseDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registrar([FromBody] RegistroDto dto)
        {
            var registroDto = new UsuarioRegistroDto(dto.Nombre, dto.Email, dto.Password);
            var command = new RegistrarUsuarioCommand(registroDto);
            var usuario = await _mediator.Send(command);

            var usuarioDto = new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                CreatedAt = usuario.CreatedAt
            };
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
            try
            {
                var command = new LoginUsuarioCommand(credentials.Email, credentials.Password);
                var response = await _mediator.Send(command);

                return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(response, "Login exitoso"));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResponse("Credenciales inválidas"));
            }
        }

        [HttpPost("solicitar-restablecimiento")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SolicitarRestablecimiento([FromBody] string email)
        {
            var command = new SolicitarRestablecimientoPasswordCommand { Email = email };
            var result = await _mediator.Send(command);
            
            return Ok(ApiResponse<object>.SuccessResponse(new { }, "Si el correo existe, se ha enviado un enlace para restablecer la contraseña"));
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
            
            return Ok(ApiResponse<object>.SuccessResponse(new { }, "Contraseña restablecida exitosamente"));
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
