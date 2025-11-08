using Microsoft.AspNetCore.Mvc;
using Magnus.Application.Interfaces;
using Magnus.Application.DTOs;
using Magnus.Application.Features.Eventos.Commands.CrearEvento;
using Magnus.Application.Features.Eventos.Commands.ActualizarEvento;
using Magnus.Application.Features.Eventos.Commands.EliminarEvento;
using Magnus.Application.Features.Eventos.Queries.ObtenerEventoPorId;
using Magnus.Application.Features.Eventos.Queries.ListarEventosPorOrganizador;

namespace Magnus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public EventosController(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        /// <summary>
        /// Crea un nuevo evento
        /// </summary>
        /// <response code="201">Evento creado exitosamente</response>
        /// <response code="400">Datos de entrada inválidos</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<EventoResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<EventoResponseDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CrearEvento([FromBody] EventoCreacionDto dto)
        {
            var handler = new CrearEventoCommandHandler(_unitOfWork, _emailService);
            var command = new CrearEventoCommand(dto);
            var result = await handler.Handle(command);
            
            var response = ApiResponse<EventoResponseDto>.SuccessResponse(
                result, 
                "Evento creado exitosamente"
            );
            
            return CreatedAtAction(nameof(ObtenerEventoPorId), new { id = result.Id }, response);
        }

        /// <summary>
        /// Obtiene un evento por su ID
        /// </summary>
        /// <response code="200">Evento encontrado</response>
        /// <response code="404">Evento no encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EventoResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<EventoResponseDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerEventoPorId(Guid id)
        {
            var handler = new ObtenerEventoPorIdQueryHandler(_unitOfWork);
            var query = new ObtenerEventoPorIdQuery(id);
            var result = await handler.Handle(query);

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

        /// <summary>
        /// Lista todos los eventos de un organizador
        /// </summary>
        /// <response code="200">Lista de eventos del organizador</response>
        [HttpGet("organizador/{organizadorId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EventoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarEventosPorOrganizador(Guid organizadorId)
        {
            var handler = new ListarEventosPorOrganizadorQueryHandler(_unitOfWork);
            var query = new ListarEventosPorOrganizadorQuery(organizadorId);
            var result = await handler.Handle(query);
            
            var response = ApiResponse<IEnumerable<EventoResponseDto>>.SuccessResponse(
                result,
                $"Se encontraron {result.Count()} eventos"
            );
            
            return Ok(response);
        }

        /// <summary>
        /// Actualiza un evento existente
        /// </summary>
        /// <response code="200">Evento actualizado exitosamente</response>
        /// <response code="404">Evento no encontrado</response>
        /// <response code="400">Datos de entrada inválidos</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EventoResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<EventoResponseDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<EventoResponseDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ActualizarEvento(Guid id, [FromBody] EventoActualizacionDto dto)
        {
            var handler = new ActualizarEventoCommandHandler(_unitOfWork);
            var command = new ActualizarEventoCommand(
                id, 
                dto.Titulo, 
                dto.FechaInicio, 
                dto.FechaFin, 
                dto.Lugar, 
                dto.Capacidad, 
                dto.Descripcion
            );

            var result = await handler.Handle(command);
            
            var response = ApiResponse<EventoResponseDto>.SuccessResponse(
                result,
                "Evento actualizado exitosamente"
            );
            
            return Ok(response);
        }

        /// <summary>
        /// Elimina un evento
        /// </summary>
        /// <response code="200">Evento eliminado exitosamente</response>
        /// <response code="404">Evento no encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EliminarEvento(Guid id)
        {
            var handler = new EliminarEventoCommandHandler(_unitOfWork);
            var command = new EliminarEventoCommand(id);
            await handler.Handle(command);
            
            var response = ApiResponse<object>.SuccessResponse(
                new { eventoId = id },
                "Evento eliminado exitosamente"
            );
            
            return Ok(response);
        }
    }
}