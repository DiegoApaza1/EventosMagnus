using ClosedXML.Excel;
using Magnus.Domain.Interfaces.Services;

namespace Magnus.Infrastructure.Adapters.Services
{
    public class ExcelReportService : IReportService
    {
        public Task<(byte[] FileBytes, string FileName)> GenerarReporteEventosAsync(
            IEnumerable<object> eventos, 
            string nombreArchivo = "ReporteEventos")
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Eventos");
            
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Título";
            worksheet.Cell(1, 3).Value = "Descripción";
            worksheet.Cell(1, 4).Value = "Fecha Inicio";
            worksheet.Cell(1, 5).Value = "Fecha Fin";
            worksheet.Cell(1, 6).Value = "Lugar";
            worksheet.Cell(1, 7).Value = "Capacidad";

            var headerRange = worksheet.Range("A1:G1");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;
            foreach (dynamic evento in eventos)
            {
                worksheet.Cell(row, 1).Value = evento.Id.ToString();
                worksheet.Cell(row, 2).Value = evento.Titulo ?? "";
                worksheet.Cell(row, 3).Value = evento.Descripcion ?? "";
                worksheet.Cell(row, 4).Value = evento.FechaInicio.ToString("yyyy-MM-dd HH:mm");
                worksheet.Cell(row, 5).Value = evento.FechaFin.ToString("yyyy-MM-dd HH:mm");
                worksheet.Cell(row, 6).Value = evento.Lugar ?? "";
                worksheet.Cell(row, 7).Value = evento.Capacidad;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var fileBytes = stream.ToArray();

            string fileName = $"{nombreArchivo}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return Task.FromResult((fileBytes, fileName));
        }
    }
}
