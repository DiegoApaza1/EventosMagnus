using Microsoft.AspNetCore.Mvc;
using Magnus.Application.DTOs;
using Magnus.Domain.Entities;
using Magnus.Domain.Interfaces.Repositories;
using MediatR;
using Magnus.Application.Features.Organizadores.Commands.CrearOrganizador;
using Magnus.Application.Features.Organizadores.Queries.ObtenerOrganizadorPorId;
using Magnus.Application.Features.Organizadores.Queries.ObtenerOrganizadorPorUsuarioId;
using Magnus.Application.Features.Organizadores.Queries.ListarOrganizadores;
using Magnus.Application.Features.Organizadores.Queries.ObtenerEstadisticasOrganizador;

namespace Magnus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizadoresController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrganizadoresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<OrganizadorResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CrearOrganizador([FromBody] OrganizadorCreacionDto dto)
        {
            var command = new CrearOrganizadorCommand(
                dto.NombreEmpresa,
                dto.Descripcion,
                dto.Telefono,
                dto.Direccion,
                dto.PrecioPorEvento,
                dto.AñosExperiencia,
                dto.Especialidad,
                dto.UsuarioId);
            var organizador = await _mediator.Send(command);

            var response = ApiResponse<OrganizadorResponseDto>.SuccessResponse(
                organizador,
                "Organizador creado exitosamente"
            );

            return CreatedAtAction(nameof(ObtenerOrganizadorPorId), new { id = organizador.Id }, response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<OrganizadorResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerOrganizadorPorId(Guid id)
        {
            var query = new ObtenerOrganizadorPorIdQuery(id);
            var organizador = await _mediator.Send(query);

            if (organizador == null)
            {
                var error = ApiResponse<object>.ErrorResponse($"Organizador con ID {id} no encontrado.");
                return NotFound(error);
            }

            var response = ApiResponse<OrganizadorResponseDto>.SuccessResponse(organizador);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<OrganizadorResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarOrganizadores()
        {
            var query = new ListarOrganizadoresQuery();
            var organizadores = await _mediator.Send(query);

            var response = ApiResponse<IEnumerable<OrganizadorResponseDto>>.SuccessResponse(organizadores);
            return Ok(response);
        }

        [HttpGet("usuario/{usuarioId}")]
        [ProducesResponseType(typeof(ApiResponse<OrganizadorResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerOrganizadorPorUsuarioId(Guid usuarioId)
        {
            var query = new ObtenerOrganizadorPorUsuarioIdQuery(usuarioId);
            var organizador = await _mediator.Send(query);

            if (organizador == null)
            {
                var error = ApiResponse<object>.ErrorResponse($"No se encontró un organizador para el usuario con ID {usuarioId}");
                return NotFound(error);
            }

            var response = ApiResponse<OrganizadorResponseDto>.SuccessResponse(organizador);
            return Ok(response);
        }

        [HttpGet("{id}/estadisticas")]
        [ProducesResponseType(typeof(ApiResponse<OrganizadorStatsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerEstadisticasOrganizador(Guid id)
        {
            try
            {
                var query = new ObtenerEstadisticasOrganizadorQuery(id);
                var stats = await _mediator.Send(query);

                var response = ApiResponse<OrganizadorStatsDto>.SuccessResponse(stats);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                var error = ApiResponse<object>.ErrorResponse(ex.Message);
                return NotFound(error);
            }
        }
    }

}
