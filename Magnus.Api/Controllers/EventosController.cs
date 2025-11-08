using Microsoft.AspNetCore.Mvc;
using Magnus.Application.Interfaces;
using Magnus.Application.DTOs;
using Magnus.Application.Features.Eventos.Commands.CrearEvento;

namespace Magnus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly CrearEventoCommandHandler _handler;

        public EventosController(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _handler = new CrearEventoCommandHandler(unitOfWork, emailService);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearEvento([FromBody] EventoCreacionDto dto)
        {
            var command = new CrearEventoCommand(dto);
            var result = await _handler.Handle(command);
            return Ok(result);
        }
    }
}