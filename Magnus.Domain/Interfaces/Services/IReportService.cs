namespace Magnus.Domain.Interfaces.Services
{
    public interface IReportService
    {
        Task<(byte[] FileBytes, string FileName)> GenerarReporteEventosAsync(IEnumerable<object> eventos, string nombreArchivo = "ReporteEventos");
    }
}
