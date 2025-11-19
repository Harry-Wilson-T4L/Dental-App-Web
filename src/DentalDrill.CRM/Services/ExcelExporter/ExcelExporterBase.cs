using System;
using System.IO;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels.Reports.Surgery;
using Microsoft.EntityFrameworkCore;
using Telerik.Documents.SpreadsheetStreaming;

namespace DentalDrill.CRM.Services.ExcelExporter
{
    public abstract class ExcelExporterBase
    {
        private IWorkbookExporter workbook;

        protected IWorkbookExporter Workbook => this.workbook;

        public async Task<Byte[]> ExportAsBytes()
        {
            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    await this.InitializeWorkbook(memoryStream);
                    await this.ProcessWorkbook();
                }
                finally
                {
                    await this.FinalizeWorkbook();
                }

                return memoryStream.ToArray();
            }
        }

        protected virtual Task InitializeWorkbook(Stream stream)
        {
            this.workbook = SpreadExporter.CreateWorkbookExporter(SpreadDocumentFormat.Xlsx, stream, SpreadExportMode.Create);
            return Task.CompletedTask;
        }

        protected abstract Task ProcessWorkbook();

        protected virtual Task FinalizeWorkbook()
        {
            this.workbook?.Dispose();
            return Task.CompletedTask;
        }
    }
}
