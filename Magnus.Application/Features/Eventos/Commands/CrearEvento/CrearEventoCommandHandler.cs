using System;
using System.Threading;
using System.Threading.Tasks;
using Magnus.Application.Interfaces;
using Magnus.Application.Features.Eventos.Commands.CrearEvento;
using Magnus.Application.DTOs;
using Magnus.Application.Mappers;
using Magnus.Domain.Entities;

namespace Magnus.Application.Features.Eventos.Commands.CrearEvento
{
    public class CrearEventoCommandHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailService? _emailService;

        public CrearEventoCommandHandler(IUnitOfWork uow, IEmailService? emailService = null)
        {
            _uow = uow;
            _emailService = emailService;
        }

        public async Task<EventoResponseDto> Handle(CrearEventoCommand command, CancellationToken ct = default)
        {
            var dto = command.Dto;

            if (string.IsNullOrWhiteSpace(dto.Titulo)) throw new ArgumentException("Titulo requerido.");
            if (dto.FechaInicio >= dto.FechaFin) throw new ArgumentException("FechaInicio debe ser anterior a FechaFin.");
            if (dto.Capacidad < 0) throw new ArgumentException("Capacidad inválida.");

            var organizador = await _uow.Usuarios.GetByIdAsync(dto.OrganizadorId);
            if (organizador == null) throw new InvalidOperationException("Organizador no encontrado.");

            var evento = new Evento(dto.Titulo, dto.FechaInicio, dto.FechaFin, dto.Lugar, dto.Capacidad, dto.OrganizadorId, dto.Descripcion);

            await _uow.Eventos.AddAsync(evento);
            await _uow.CommitAsync();
            if (_emailService != null)
            {
                var subject = $"Evento creado: {evento.Titulo}";
                var body = $"Tu evento '{evento.Titulo}' fue creado para {evento.FechaInicio:yyyy-MM-dd}.";
                _ = _emailService.SendEmailAsync(organizador.Email, subject, body);
            }

            return evento.ToResponseDto();
        }
    }
}
