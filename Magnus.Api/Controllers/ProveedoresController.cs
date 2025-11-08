using Microsoft.AspNetCore.Mvc;
using Magnus.Application.Features.Proveedores.Queries.BuscarProveedores;
using Magnus.Application.DTOs;
using Magnus.Application.Interfaces;

namespace Magnus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly BuscarProveedoresQueryHandler _handler;

        public ProveedoresController(IUnitOfWork unitOfWork)
        {
            _handler = new BuscarProveedoresQueryHandler(unitOfWork);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] ProveedorBusquedaDto nombre)
        {
            var query = new BuscarProveedoresQuery(nombre);
            var resultado = await _handler.Handle(query);
            return Ok(resultado);
        }
    }
}