using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Magnus.Domain.Interfaces.Repositories;
using Magnus.Domain.Interfaces.Services;
using Magnus.Application.DTOs;
using Magnus.Domain.Entities;
using MediatR;

namespace Magnus.Application.Features.Eventos.Commands.CrearEvento
{
    public class CrearEventoCommandHandler : IRequestHandler<CrearEventoCommand, EventoResponseDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailService? _emailService;
        private readonly IMapper _mapper;

        public CrearEventoCommandHandler(IUnitOfWork uow, IMapper mapper, IEmailService? emailService = null)
        {
            _uow = uow;
            _mapper = mapper;
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

            return _mapper.Map<EventoResponseDto>(evento);
        }
    }
}
