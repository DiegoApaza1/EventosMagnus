using Microsoft.AspNetCore.Mvc;
using Magnus.Application.Features.Proveedores.Queries.BuscarProveedores;
using Magnus.Application.DTOs;
using MediatR;

namespace Magnus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProveedoresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] ProveedorBusquedaDto nombre)
        {
            var query = new BuscarProveedoresQuery(nombre);
            var resultado = await _mediator.Send(query);
            return Ok(resultado);
        }
    }
}