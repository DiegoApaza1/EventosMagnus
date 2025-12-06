using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Magnus.Domain.Interfaces.Repositories;
using Magnus.Domain.Interfaces.Services;
using MediatR;

namespace Magnus.Application.Features.Reportes.GenerarReporteEventos
{
    public class GenerarReporteEventosQueryHandler : IRequestHandler<GenerarReporteEventosQuery, (byte[] FileBytes, string FileName)>
    {
        private readonly IUnitOfWork _uow;
        private readonly IReportService _reportService;
        private readonly IMapper _mapper;

        public GenerarReporteEventosQueryHandler(IUnitOfWork uow, IReportService reportService, IMapper mapper)
        {
            _uow = uow;
            _reportService = reportService;
            _mapper = mapper;
        }

        public async Task<(byte[] FileBytes, string FileName)> Handle(GenerarReporteEventosQuery query, CancellationToken ct)
        {
            var eventos = query.OrganizadorId.HasValue
                ? await _uow.Eventos.GetByOrganizadorIdAsync(query.OrganizadorId.Value)
                : await _uow.Eventos.GetAllAsync();

            var eventosDto = _mapper.Map<IEnumerable<Magnus.Application.DTOs.EventoResponseDto>>(eventos);
            return await _reportService.GenerarReporteEventosAsync(eventosDto, "ReporteEventos");
        }
    }
}
