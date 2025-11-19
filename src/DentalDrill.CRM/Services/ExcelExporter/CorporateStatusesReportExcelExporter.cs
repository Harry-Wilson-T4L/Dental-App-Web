using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain;
using DentalDrill.CRM.Domain.Reporting;
using DentalDrill.CRM.Models.ViewModels.Reports.Corporate;
using Telerik.Documents.SpreadsheetStreaming;

namespace DentalDrill.CRM.Services.ExcelExporter
{
    public class CorporateStatusesReportExcelExporter : ExcelExporterBase
    {
        private readonly CorporateDomainModel corporate;
        private readonly CorporateStatusesReportsDomainModel report;

        public CorporateStatusesReportExcelExporter(CorporateDomainModel corporate, CorporateStatusesReportsDomainModel report)
        {
            this.corporate = corporate;
            this.report = report;
        }

        protected IWorksheetExporter SurgeriesSheet { get; private set; }

        protected SpreadCellStyle HeaderStyle { get; private set; }

        protected SpreadCellStyle ContentStyle { get; private set; }

        protected override async Task ProcessWorkbook()
        {
            this.HeaderStyle = this.Workbook.CellStyles.Add("HeaderStyle");
            this.HeaderStyle.IsBold = true;
            this.HeaderStyle.Fill = new SpreadPatternFill(SpreadPatternType.Solid, SpreadThemableColor.FromRgb(200, 200, 200), SpreadThemableColor.FromRgb(200, 200, 200));

            this.ContentStyle = this.Workbook.CellStyles.Add("ContentStyle");

            using (this.SurgeriesSheet = this.Workbook.CreateWorksheetExporter("Surgeries"))
            {
                await this.ProcessSurgeriesSheet();
            }
        }

        protected async Task ProcessSurgeriesSheet()
        {
            this.SurgeriesSheet.ConfigureColumn(widthInPixels: 200); // Surgery
            this.SurgeriesSheet.ConfigureColumn(widthInPixels: 100); // Received
            this.SurgeriesSheet.ConfigureColumn(widthInPixels: 100); // Being Est.
            this.SurgeriesSheet.ConfigureColumn(widthInPixels: 100); // Est. Comp.
            this.SurgeriesSheet.ConfigureColumn(widthInPixels: 100); // Est. Sent
            this.SurgeriesSheet.ConfigureColumn(widthInPixels: 100); // Being Rep.
            this.SurgeriesSheet.ConfigureColumn(widthInPixels: 100); // Rdy. For Ret.
            this.SurgeriesSheet.ConfigureColumn(widthInPixels: 100); // Unrepaired
            this.SurgeriesSheet.ConfigureColumn(widthInPixels: 100); // Complete

            var queryResult = await this.report.GetSurgeriesForEntirePeriodAsync();
            await this.RenderInsights(queryResult);
            await this.RenderHeader("Surgery");
            foreach (var surgery in queryResult.OrderBy(x => x.ClientName))
            {
                using var row = this.SurgeriesSheet.CreateRowExporter();
                row.CreateCell(surgery.ClientName, this.ContentStyle);
                row.CreateCell(surgery.StatusReceived, this.ContentStyle);
                row.CreateCell(surgery.StatusBeingEstimated, this.ContentStyle);
                row.CreateCell(surgery.StatusWaitingForApproval, this.ContentStyle);
                row.CreateCell(surgery.StatusEstimateSent, this.ContentStyle);
                row.CreateCell(surgery.StatusBeingRepaired, this.ContentStyle);
                row.CreateCell(surgery.StatusReadyToReturn, this.ContentStyle);
                row.CreateCell(surgery.StatusCancelled, this.ContentStyle);
                row.CreateCell(surgery.StatusSentComplete, this.ContentStyle);
            }
        }

        private async Task RenderInsights(List<CorporateSurgeryReportStatusItem> data)
        {
            await this.RenderHeader("Insights");

            var sum = new
            {
                StatusReceived = data.Sum(x => x.StatusReceived),
                StatusBeingEstimated = data.Sum(x => x.StatusBeingEstimated),
                StatusWaitingForApproval = data.Sum(x => x.StatusWaitingForApproval),
                StatusEstimateSent = data.Sum(x => x.StatusEstimateSent),
                StatusBeingRepaired = data.Sum(x => x.StatusBeingRepaired),
                StatusReadyToReturn = data.Sum(x => x.StatusReadyToReturn),
                StatusCancelled = data.Sum(x => x.StatusCancelled),
                StatusSentComplete = data.Sum(x => x.StatusSentComplete),
            };

            using (var row = this.SurgeriesSheet.CreateRowExporter())
            {
                row.CreateCell("Total", this.ContentStyle);
                row.CreateCell(sum.StatusReceived, this.ContentStyle);
                row.CreateCell(sum.StatusBeingEstimated, this.ContentStyle);
                row.CreateCell(sum.StatusWaitingForApproval, this.ContentStyle);
                row.CreateCell(sum.StatusEstimateSent, this.ContentStyle);
                row.CreateCell(sum.StatusBeingRepaired, this.ContentStyle);
                row.CreateCell(sum.StatusReadyToReturn, this.ContentStyle);
                row.CreateCell(sum.StatusCancelled, this.ContentStyle);
                row.CreateCell(sum.StatusSentComplete, this.ContentStyle);
            }

            var avg = new
            {
                StatusReceived = sum.StatusReceived * 1.0 / data.Count,
                StatusBeingEstimated = sum.StatusBeingEstimated * 1.0 / data.Count,
                StatusWaitingForApproval = sum.StatusWaitingForApproval * 1.0 / data.Count,
                StatusEstimateSent = sum.StatusEstimateSent * 1.0 / data.Count,
                StatusBeingRepaired = sum.StatusBeingRepaired * 1.0 / data.Count,
                StatusReadyToReturn = sum.StatusReadyToReturn * 1.0 / data.Count,
                StatusCancelled = sum.StatusCancelled * 1.0 / data.Count,
                StatusSentComplete = sum.StatusSentComplete * 1.0 / data.Count,
            };

            using (var row = this.SurgeriesSheet.CreateRowExporter())
            {
                row.CreateCell("Average", this.ContentStyle);
                row.CreateCell(avg.StatusReceived, this.ContentStyle);
                row.CreateCell(avg.StatusBeingEstimated, this.ContentStyle);
                row.CreateCell(avg.StatusWaitingForApproval, this.ContentStyle);
                row.CreateCell(avg.StatusEstimateSent, this.ContentStyle);
                row.CreateCell(avg.StatusBeingRepaired, this.ContentStyle);
                row.CreateCell(avg.StatusReadyToReturn, this.ContentStyle);
                row.CreateCell(avg.StatusCancelled, this.ContentStyle);
                row.CreateCell(avg.StatusSentComplete, this.ContentStyle);
            }

            using (var row = this.SurgeriesSheet.CreateRowExporter())
            {
                row.CreateCell(SpreadValue.Skipped);
            }
        }

        private Task RenderHeader(String name)
        {
            using var header = this.SurgeriesSheet.CreateRowExporter();
            header.CreateCell(name, this.HeaderStyle);
            header.CreateCell("Received", this.HeaderStyle);
            header.CreateCell("Being Est.", this.HeaderStyle);
            header.CreateCell("Est. Comp.", this.HeaderStyle);
            header.CreateCell("Est. Sent", this.HeaderStyle);
            header.CreateCell("Being Rep.", this.HeaderStyle);
            header.CreateCell("Rdy. For Ret.", this.HeaderStyle);
            header.CreateCell("Unrepaired", this.HeaderStyle);
            header.CreateCell("Complete", this.HeaderStyle);

            return Task.CompletedTask;
        }
    }
}
