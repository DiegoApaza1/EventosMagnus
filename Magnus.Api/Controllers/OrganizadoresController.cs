using Microsoft.AspNetCore.Mvc;
using Magnus.Application.DTOs;
using Magnus.Domain.Entities;
using Magnus.Application.Interfaces;

namespace Magnus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizadoresController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrganizadoresController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CrearOrganizador([FromBody] OrganizadorDto dto)
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(dto.UsuarioId);
            if (usuario == null)
            {
                var error = ApiResponse<object>.ErrorResponse("Usuario no encontrado");
                return BadRequest(error);
            }

            var organizador = new Organizador(dto.NombreEmpresa, dto.Telefono, dto.UsuarioId);

            await _unitOfWork.Organizadores.AddAsync(organizador);
            await _unitOfWork.CommitAsync();

            var response = ApiResponse<object>.SuccessResponse(
                new { organizador.Id, organizador.Nombre },
                "Organizador creado exitosamente"
            );

            return CreatedAtAction(nameof(ObtenerOrganizadorPorId), new { id = organizador.Id }, response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Organizador>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerOrganizadorPorId(Guid id)
        {
            var organizador = await _unitOfWork.Organizadores.GetByIdAsync(id);

            if (organizador == null)
            {
                var error = ApiResponse<object>.ErrorResponse($"Organizador con ID {id} no encontrado.");
                return NotFound(error);
            }

            var response = ApiResponse<Organizador>.SuccessResponse(organizador);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Organizador>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarOrganizadores()
        {
            var organizadores = await _unitOfWork.Organizadores.GetAllAsync();
            var response = ApiResponse<IEnumerable<Organizador>>.SuccessResponse(organizadores);
            return Ok(response);
        }
    }

    public class OrganizadorDto
    {
        public string NombreEmpresa { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public Guid UsuarioId { get; set; }
    }
}
