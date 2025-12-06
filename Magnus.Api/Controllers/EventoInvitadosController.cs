using Microsoft.AspNetCore.Mvc;
using Magnus.Application.DTOs;
using Magnus.Domain.Entities;
using MediatR;
using Magnus.Application.Features.EventoInvitados.Commands;
using Magnus.Application.Features.EventoInvitados.Queries;

namespace Magnus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoInvitadosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventoInvitadosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("invitar")]
        [ProducesResponseType(typeof(ApiResponse<EventoInvitadoResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InvitarUsuario([FromBody] InvitarUsuarioDto dto)
        {
            try
            {
                var command = new InvitarUsuarioCommand(dto.EventoId, dto.UsuarioId, dto.Mensaje);
                var invitacion = await _mediator.Send(command);

                var response = ApiResponse<EventoInvitadoResponseDto>.SuccessResponse(
                    invitacion,
                    "Usuario invitado exitosamente"
                );

                return CreatedAtAction(nameof(InvitarUsuario), response);
            }
            catch (Exception ex)
            {
                var error = ApiResponse<object>.ErrorResponse(ex.Message);
                return BadRequest(error);
            }
        }

        [HttpPost("autopostularse")]
        [ProducesResponseType(typeof(ApiResponse<EventoInvitadoResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Autopostularse([FromBody] AutopostularseDto dto)
        {
            try
            {
                // TODO: Get current user ID from JWT token
                var usuarioId = Guid.Parse("00000000-0000-0000-0000-000000000000"); // Placeholder
                
                var command = new AutopostularseCommand(dto.EventoId, usuarioId, dto.Mensaje);
                var invitacion = await _mediator.Send(command);

                var response = ApiResponse<EventoInvitadoResponseDto>.SuccessResponse(
                    invitacion,
                    "Solicitud enviada exitosamente"
                );

                return CreatedAtAction(nameof(Autopostularse), response);
            }
            catch (Exception ex)
            {
                var error = ApiResponse<object>.ErrorResponse(ex.Message);
                return BadRequest(error);
            }
        }

        [HttpPut("{id}/aceptar")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AceptarInvitacion(Guid id)
        {
            try
            {
                var command = new AceptarInvitacionCommand(id);
                await _mediator.Send(command);

                var response = ApiResponse<object>.SuccessResponse(null, "Invitación aceptada");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = ApiResponse<object>.ErrorResponse(ex.Message);
                return BadRequest(error);
            }
        }

        [HttpPut("{id}/rechazar")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RechazarInvitacion(Guid id)
        {
            try
            {
                var command = new RechazarInvitacionCommand(id);
                await _mediator.Send(command);

                var response = ApiResponse<object>.SuccessResponse(null, "Invitación rechazada");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = ApiResponse<object>.ErrorResponse(ex.Message);
                return BadRequest(error);
            }
        }

        [HttpPut("{id}/aprobar")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AprobarAutopostulacion(Guid id)
        {
            try
            {
                var command = new AprobarAutopostulacionCommand(id);
                await _mediator.Send(command);

                var response = ApiResponse<object>.SuccessResponse(null, "Solicitud aprobada");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = ApiResponse<object>.ErrorResponse(ex.Message);
                return BadRequest(error);
            }
        }

        [HttpPut("{id}/rechazar-organizador")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RechazarAutopostulacion(Guid id)
        {
            try
            {
                var command = new RechazarAutopostulacionCommand(id);
                await _mediator.Send(command);

                var response = ApiResponse<object>.SuccessResponse(null, "Solicitud rechazada");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = ApiResponse<object>.ErrorResponse(ex.Message);
                return BadRequest(error);
            }
        }

        [HttpGet("evento/{eventoId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EventoInvitadoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenerInvitacionesPorEvento(Guid eventoId)
        {
            var query = new ObtenerInvitacionesPorEventoQuery(eventoId);
            var invitaciones = await _mediator.Send(query);

            var response = ApiResponse<IEnumerable<EventoInvitadoResponseDto>>.SuccessResponse(invitaciones);
            return Ok(response);
        }

        [HttpGet("usuario/{usuarioId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EventoInvitadoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenerInvitacionesPorUsuario(Guid usuarioId)
        {
            var query = new ObtenerInvitacionesPorUsuarioQuery(usuarioId);
            var invitaciones = await _mediator.Send(query);

            var response = ApiResponse<IEnumerable<EventoInvitadoResponseDto>>.SuccessResponse(invitaciones);
            return Ok(response);
        }
    }
}
