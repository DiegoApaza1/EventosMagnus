using Microsoft.AspNetCore.Mvc;
using Magnus.Application.DTOs;
using Magnus.Application.Features.Eventos.Commands.CrearEvento;
using Magnus.Application.Features.Eventos.Commands.ActualizarEvento;
using Magnus.Application.Features.Eventos.Commands.EliminarEvento;
using Magnus.Application.Features.Eventos.Queries.ObtenerEventoPorId;
using Magnus.Application.Features.Eventos.Queries.ListarEventosPorOrganizador;
using Microsoft.AspNetCore.Authorization;
using MediatR;

namespace Magnus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<EventoResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<EventoResponseDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CrearEvento([FromBody] EventoCreacionDto dto)
        {
            var command = new CrearEventoCommand(dto);
            var result = await _mediator.Send(command);
            
            var response = ApiResponse<EventoResponseDto>.SuccessResponse(
                result, 
                "Evento creado exitosamente"
            );
            
            return CreatedAtAction(nameof(ObtenerEventoPorId), new { id = result.Id }, response);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EventoResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<EventoResponseDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerEventoPorId(Guid id)
        {
            var query = new ObtenerEventoPorIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                var errorResponse = ApiResponse<EventoResponseDto>.ErrorResponse(
                    $"Evento con ID {id} no encontrado."
                );
                return NotFound(errorResponse);
            }

            var response = ApiResponse<EventoResponseDto>.SuccessResponse(result);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("organizador/{organizadorId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EventoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarEventosPorOrganizador(Guid organizadorId)
        {
            var query = new ListarEventosPorOrganizadorQuery(organizadorId);
            var result = await _mediator.Send(query);
            
            var response = ApiResponse<IEnumerable<EventoResponseDto>>.SuccessResponse(
                result,
                $"Se encontraron {result.Count()} eventos"
            );
            
            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EventoResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<EventoResponseDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<EventoResponseDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ActualizarEvento(Guid id, [FromBody] EventoActualizacionDto dto)
        {
            var command = new ActualizarEventoCommand(
                id, 
                dto.Titulo, 
                dto.FechaInicio, 
                dto.FechaFin, 
                dto.Lugar, 
                dto.Capacidad, 
                dto.Descripcion
            );

            var result = await _mediator.Send(command);
            
            var response = ApiResponse<EventoResponseDto>.SuccessResponse(
                result,
                "Evento actualizado exitosamente"
            );
            
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EliminarEvento(Guid id)
        {
            var command = new EliminarEventoCommand(id);
            await _mediator.Send(command);
            
            var response = ApiResponse<object>.SuccessResponse(
                new { eventoId = id },
                "Evento eliminado exitosamente"
            );
            
            return Ok(response);
        }
    }
}