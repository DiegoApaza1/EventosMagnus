using Microsoft.AspNetCore.Mvc;
using Magnus.Application.DTOs;
using Magnus.Domain.Entities;
using Magnus.Domain.Interfaces;
using MediatR;
using Magnus.Application.Features.Organizadores.Commands.CrearOrganizador;
using Magnus.Application.Features.Organizadores.Queries.ObtenerOrganizadorPorId;
using Magnus.Application.Features.Organizadores.Queries.ListarOrganizadores;

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
                dto.Telefono,
                dto.PrecioPorEvento,
                dto.AñosExperiencia,
                dto.UsuarioId,
                dto.Descripcion,
                dto.Direccion,
                dto.Especialidad);
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
    }

}
