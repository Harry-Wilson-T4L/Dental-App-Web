using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain;
using DentalDrill.CRM.Domain.Reporting;
using DentalDrill.CRM.Models.ViewModels;
using Telerik.Documents.SpreadsheetStreaming;

namespace DentalDrill.CRM.Services.ExcelExporter
{
    public class CorporateSurgeriesReportExcelExporter : ExcelExporterBase
    {
        private readonly CorporateDomainModel corporate;
        private readonly CorporateBrandsReportsDomainModel report;
        private readonly DateTime from;
        private readonly DateTime to;
        private readonly ReportDateAggregate dateAggregate;
        private readonly IReadOnlyList<String> dateRanges;
        private readonly (Boolean RatingAverage, Boolean CostSum, Boolean CostAverage, Boolean UnrepairedPercent, Boolean HandpiecesCount) reportFields;

        public CorporateSurgeriesReportExcelExporter(CorporateDomainModel corporate, CorporateBrandsReportsDomainModel report, DateTime @from, DateTime to, ReportDateAggregate dateAggregate, HashSet<String> reportFields)
        {
            this.corporate = corporate;
            this.report = report;
            this.from = from;
            this.to = to;
            this.dateAggregate = dateAggregate;
            this.reportFields = (reportFields.Contains("RatingAverage"), reportFields.Contains("CostSum"), reportFields.Contains("CostAverage"), reportFields.Contains("UnrepairedPercent"), reportFields.Contains("HandpiecesCount"));
            this.dateRanges = ReportDateAggregateHelper.GenerateDateRanges(this.from, this.to, this.dateAggregate);
        }

        protected IWorksheetExporter SurgeriesSheet { get; private set; }

        protected IWorksheetExporter SurgeriesBrandsSheet { get; private set; }

        protected IWorksheetExporter SurgeriesModelsSheet { get; private set; }

        protected SpreadCellStyle HeaderStyle { get; private set; }

        protected SpreadCellStyle BrandStyle { get; private set; }

        protected SpreadCellStyle BrandStyleNumber { get; private set; }

        protected SpreadCellStyle BrandStylePercent { get; private set; }

        protected SpreadCellStyle BrandStyleCurrency { get; private set; }

        protected SpreadCellStyle ContentStyle { get; private set; }

        protected SpreadCellStyle ContentStyleNumber { get; private set; }

        protected SpreadCellStyle ContentStylePercent { get; private set; }

        protected SpreadCellStyle ContentStyleCurrency { get; private set; }

        protected override async Task ProcessWorkbook()
        {
            this.HeaderStyle = this.Workbook.CellStyles.Add("HeaderStyle");
            this.HeaderStyle.IsBold = true;
            this.HeaderStyle.Fill = new SpreadPatternFill(SpreadPatternType.Solid, SpreadThemableColor.FromRgb(200, 200, 200), SpreadThemableColor.FromRgb(200, 200, 200));

            this.BrandStyle = this.Workbook.CellStyles.Add("BrandStyle");
            this.BrandStyle.IsBold = true;

            this.BrandStyleNumber = this.Workbook.CellStyles.Add("BrandStyleNumber");
            this.BrandStyleNumber.IsBold = true;
            this.BrandStyleNumber.NumberFormat = "#,##0.00";

            this.BrandStylePercent = this.Workbook.CellStyles.Add("BrandStylePercent");
            this.BrandStylePercent.IsBold = true;
            this.BrandStylePercent.NumberFormat = "#,##0.00%";

            this.BrandStyleCurrency = this.Workbook.CellStyles.Add("BrandStyleCurrency");
            this.BrandStyleCurrency.IsBold = true;
            this.BrandStyleCurrency.NumberFormat = "[$AU$-en-AU] #,##0";

            this.ContentStyle = this.Workbook.CellStyles.Add("ContentStyle");

            this.ContentStyleNumber = this.Workbook.CellStyles.Add("ContentStyleNumber");
            this.ContentStyleNumber.NumberFormat = "#,##0.00";

            this.ContentStylePercent = this.Workbook.CellStyles.Add("ContentStylePercent");
            this.ContentStylePercent.NumberFormat = "#,##0.00%";

            this.ContentStyleCurrency = this.Workbook.CellStyles.Add("ContentStyleCurrency");
            this.ContentStyleCurrency.NumberFormat = "[$AU$-en-AU] #,##0";

            using (this.SurgeriesSheet = this.Workbook.CreateWorksheetExporter("Surgeries"))
            {
                await this.RenderSurgeriesSheet();
            }

            using (this.SurgeriesBrandsSheet = this.Workbook.CreateWorksheetExporter("Surgeries and brands"))
            {
                await this.RenderSurgeriesAndBrandsSheet();
            }

            using (this.SurgeriesModelsSheet = this.Workbook.CreateWorksheetExporter("Surgeries and models"))
            {
                await this.RenderSurgeriesAndModelsSheet();
            }
        }

        protected async Task RenderSurgeriesSheet()
        {
            this.SurgeriesSheet.ConfigureColumn(widthInPixels: 200);
            var fieldsCount = this.ComputeFieldsCount() * this.dateRanges.Count;
            for (var i = 0; i < fieldsCount; i++)
            {
                this.SurgeriesSheet.ConfigureColumn(widthInPixels: 89);
            }

            await this.RenderInsights(this.SurgeriesSheet);
            await this.RenderHeader(this.SurgeriesSheet, "Surgeries");

            switch (this.dateAggregate)
            {
                case ReportDateAggregate.EntirePeriod:
                {
                    var queryResult = await this.report.GetSurgeriesForEntirePeriodAsync();
                    foreach (var surgery in queryResult.OrderBy(x => x.ClientName))
                    {
                        using (var row = this.SurgeriesSheet.CreateRowExporter())
                        {
                            row.CreateCell(surgery.ClientName, this.ContentStyle);
                            row.CreateCellIf(this.reportFields.RatingAverage, SpreadValue.FromDouble((Double)surgery.RatingAverage), this.ContentStyleNumber);
                            row.CreateCellIf(this.reportFields.UnrepairedPercent, SpreadValue.FromDouble((Double)surgery.UnrepairedPercent), this.ContentStylePercent);
                            row.CreateCellIf(this.reportFields.CostSum, SpreadValue.FromDouble((Double)surgery.CostSum), this.ContentStyleCurrency);
                            row.CreateCellIf(this.reportFields.CostAverage, SpreadValue.FromDouble((Double)surgery.CostAverage), this.ContentStyleCurrency);
                            row.CreateCellIf(this.reportFields.HandpiecesCount, SpreadValue.FromDouble((Double)surgery.HandpiecesCount), this.ContentStyle);
                        }
                    }

                    break;
                }

                case ReportDateAggregate.Yearly:
                case ReportDateAggregate.Quarterly:
                case ReportDateAggregate.Monthly:
                case ReportDateAggregate.Weekly:
                case ReportDateAggregate.Daily:
                {
                    var queryResult = await this.report.GetSurgeriesAggregatedAsync(this.dateAggregate);
                    foreach (var surgery in queryResult.OrderBy(x => x.ClientName))
                    {
                        using (var row = this.SurgeriesSheet.CreateRowExporter())
                        {
                            row.CreateCell(surgery.ClientName, this.ContentStyle);
                            row.CreateMultipleCellsIf(
                                this.reportFields.RatingAverage,
                                this.dateRanges.Select(range => surgery.RatingAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.ContentStyleNumber);
                            row.CreateMultipleCellsIf(
                                this.reportFields.UnrepairedPercent,
                                this.dateRanges.Select(range => surgery.UnrepairedPercent.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.ContentStylePercent);
                            row.CreateMultipleCellsIf(
                                this.reportFields.CostSum,
                                this.dateRanges.Select(range => surgery.CostSum.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.ContentStyleCurrency);
                            row.CreateMultipleCellsIf(
                                this.reportFields.CostAverage,
                                this.dateRanges.Select(range => surgery.CostAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.ContentStyleCurrency);
                            row.CreateMultipleCellsIf(
                                this.reportFields.HandpiecesCount,
                                this.dateRanges.Select(range => surgery.HandpiecesCount.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.ContentStyle);
                        }
                    }

                    break;
                }
            }

            await this.MergeHeader(this.SurgeriesSheet, rowOffset: 3);
        }

        protected async Task RenderSurgeriesAndBrandsSheet()
        {
            this.SurgeriesBrandsSheet.ConfigureColumn(widthInPixels: 200);
            var fieldsCount = this.ComputeFieldsCount() * this.dateRanges.Count;
            for (var i = 0; i < fieldsCount; i++)
            {
                this.SurgeriesBrandsSheet.ConfigureColumn(widthInPixels: 89);
            }

            await this.RenderHeader(this.SurgeriesBrandsSheet, "Suregery / Brand");
            switch (this.dateAggregate)
            {
                case ReportDateAggregate.EntirePeriod:
                {
                    var queryResult = await this.report.GetSurgeriesForEntirePeriodAsync();
                    var modelsResult = await this.report.GetSurgeriesAndBrandsForEntirePeriodAsync();
                    foreach (var client in queryResult.OrderBy(x => x.ClientName))
                    {
                        using (var row = this.SurgeriesBrandsSheet.CreateRowExporter())
                        {
                            row.CreateCell(client.ClientName, this.BrandStyle);
                            row.CreateCellIf(this.reportFields.RatingAverage, SpreadValue.FromDouble((Double)client.RatingAverage), this.BrandStyleNumber);
                            row.CreateCellIf(this.reportFields.UnrepairedPercent, SpreadValue.FromDouble((Double)client.UnrepairedPercent), this.BrandStylePercent);
                            row.CreateCellIf(this.reportFields.CostSum, SpreadValue.FromDouble((Double)client.CostSum), this.BrandStyleCurrency);
                            row.CreateCellIf(this.reportFields.CostAverage, SpreadValue.FromDouble((Double)client.CostAverage), this.BrandStyleCurrency);
                            row.CreateCellIf(this.reportFields.HandpiecesCount, SpreadValue.FromDouble((Double)client.HandpiecesCount), this.BrandStyle);
                        }

                        var brands = modelsResult.Where(x => x.ClientId == client.ClientId).OrderBy(x => x.Brand).ToList();
                        foreach (var brand in brands)
                        {
                            using (var row = this.SurgeriesBrandsSheet.CreateRowExporter())
                            {
                                row.CreateCell(brand.Brand, this.ContentStyle);
                                row.CreateCellIf(this.reportFields.RatingAverage, SpreadValue.FromDouble((Double)brand.RatingAverage), this.ContentStyleNumber);
                                row.CreateCellIf(this.reportFields.UnrepairedPercent, SpreadValue.FromDouble((Double)brand.UnrepairedPercent), this.ContentStylePercent);
                                row.CreateCellIf(this.reportFields.CostSum, SpreadValue.FromDouble((Double)brand.CostSum), this.ContentStyleCurrency);
                                row.CreateCellIf(this.reportFields.CostAverage, SpreadValue.FromDouble((Double)brand.CostAverage), this.ContentStyleCurrency);
                                row.CreateCellIf(this.reportFields.HandpiecesCount, SpreadValue.FromDouble((Double)brand.HandpiecesCount), this.ContentStyle);
                            }
                        }

                        using (var row = this.SurgeriesBrandsSheet.CreateRowExporter())
                        {
                            row.CreateCell(SpreadValue.Skipped);
                        }
                    }

                    break;
                }

                case ReportDateAggregate.Yearly:
                case ReportDateAggregate.Quarterly:
                case ReportDateAggregate.Monthly:
                case ReportDateAggregate.Weekly:
                case ReportDateAggregate.Daily:
                {
                    var queryResult = await this.report.GetSurgeriesAggregatedAsync(this.dateAggregate);
                    var modelResult = await this.report.GetSurgeriesAndBrandsAggregatedAsync(this.dateAggregate);
                    foreach (var client in queryResult.OrderBy(x => x.ClientName))
                    {
                        using (var row = this.SurgeriesBrandsSheet.CreateRowExporter())
                        {
                            row.CreateCell(client.ClientName, this.BrandStyle);
                            row.CreateMultipleCellsIf(
                                this.reportFields.RatingAverage,
                                this.dateRanges.Select(range => client.RatingAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStyleNumber);
                            row.CreateMultipleCellsIf(
                                this.reportFields.UnrepairedPercent,
                                this.dateRanges.Select(range => client.UnrepairedPercent.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStylePercent);
                            row.CreateMultipleCellsIf(
                                this.reportFields.CostSum,
                                this.dateRanges.Select(range => client.CostSum.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStyleCurrency);
                            row.CreateMultipleCellsIf(
                                this.reportFields.CostAverage,
                                this.dateRanges.Select(range => client.CostAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStyleCurrency);
                            row.CreateMultipleCellsIf(
                                this.reportFields.HandpiecesCount,
                                this.dateRanges.Select(range => client.HandpiecesCount.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStyle);
                        }

                        var models = modelResult.Where(x => x.ClientId == client.ClientId).OrderBy(x => x.Brand).ToList();
                        foreach (var model in models)
                        {
                            using (var row = this.SurgeriesBrandsSheet.CreateRowExporter())
                            {
                                row.CreateCell(model.Brand, this.ContentStyle);
                                row.CreateMultipleCellsIf(
                                    this.reportFields.RatingAverage,
                                    this.dateRanges.Select(range => model.RatingAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                    this.ContentStyleNumber);
                                row.CreateMultipleCellsIf(
                                    this.reportFields.UnrepairedPercent,
                                    this.dateRanges.Select(range => model.UnrepairedPercent.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                    this.ContentStylePercent);
                                row.CreateMultipleCellsIf(
                                    this.reportFields.CostSum,
                                    this.dateRanges.Select(range => model.CostSum.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                    this.ContentStyleCurrency);
                                row.CreateMultipleCellsIf(
                                    this.reportFields.CostAverage,
                                    this.dateRanges.Select(range => model.CostAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                    this.ContentStyleCurrency);
                                row.CreateMultipleCellsIf(
                                    this.reportFields.HandpiecesCount,
                                    this.dateRanges.Select(range => model.HandpiecesCount.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                    this.ContentStyle);
                            }
                        }

                        using (var row = this.SurgeriesBrandsSheet.CreateRowExporter())
                        {
                            row.CreateCell(SpreadValue.Skipped);
                        }
                    }

                    break;
                }
            }

            await this.MergeHeader(this.SurgeriesBrandsSheet);
        }

        protected async Task RenderSurgeriesAndModelsSheet()
        {
            this.SurgeriesModelsSheet.ConfigureColumn(widthInPixels: 200);
            var fieldsCount = this.ComputeFieldsCount() * this.dateRanges.Count;
            for (var i = 0; i < fieldsCount; i++)
            {
                this.SurgeriesModelsSheet.ConfigureColumn(widthInPixels: 89);
            }

            await this.RenderHeader(this.SurgeriesModelsSheet, "Suregery / Model");
            switch (this.dateAggregate)
            {
                case ReportDateAggregate.EntirePeriod:
                {
                    var queryResult = await this.report.GetSurgeriesForEntirePeriodAsync();
                    var modelsResult = await this.report.GetSurgeriesAndModelsForEntirePeriodAsync();
                    foreach (var client in queryResult.OrderBy(x => x.ClientName))
                    {
                        using (var row = this.SurgeriesModelsSheet.CreateRowExporter())
                        {
                            row.CreateCell(client.ClientName, this.BrandStyle);
                            row.CreateCellIf(this.reportFields.RatingAverage, SpreadValue.FromDouble((Double)client.RatingAverage), this.BrandStyleNumber);
                            row.CreateCellIf(this.reportFields.UnrepairedPercent, SpreadValue.FromDouble((Double)client.UnrepairedPercent), this.BrandStylePercent);
                            row.CreateCellIf(this.reportFields.CostSum, SpreadValue.FromDouble((Double)client.CostSum), this.BrandStyleCurrency);
                            row.CreateCellIf(this.reportFields.CostAverage, SpreadValue.FromDouble((Double)client.CostAverage), this.BrandStyleCurrency);
                            row.CreateCellIf(this.reportFields.HandpiecesCount, SpreadValue.FromDouble((Double)client.HandpiecesCount), this.BrandStyle);
                        }

                        var models = modelsResult.Where(x => x.ClientId == client.ClientId).OrderBy(x => x.Brand).ThenBy(x => x.Model).ToList();
                        foreach (var model in models)
                        {
                            using (var row = this.SurgeriesModelsSheet.CreateRowExporter())
                            {
                                row.CreateCell($"{model.Brand} {model.Model}", this.ContentStyle);
                                row.CreateCellIf(this.reportFields.RatingAverage, SpreadValue.FromDouble((Double)model.RatingAverage), this.ContentStyleNumber);
                                row.CreateCellIf(this.reportFields.UnrepairedPercent, SpreadValue.FromDouble((Double)model.UnrepairedPercent), this.ContentStylePercent);
                                row.CreateCellIf(this.reportFields.CostSum, SpreadValue.FromDouble((Double)model.CostSum), this.ContentStyleCurrency);
                                row.CreateCellIf(this.reportFields.CostAverage, SpreadValue.FromDouble((Double)model.CostAverage), this.ContentStyleCurrency);
                                row.CreateCellIf(this.reportFields.HandpiecesCount, SpreadValue.FromDouble((Double)model.HandpiecesCount), this.ContentStyle);
                            }
                        }

                        using (var row = this.SurgeriesModelsSheet.CreateRowExporter())
                        {
                            row.CreateCell(SpreadValue.Skipped);
                        }
                    }

                    break;
                }

                case ReportDateAggregate.Yearly:
                case ReportDateAggregate.Quarterly:
                case ReportDateAggregate.Monthly:
                case ReportDateAggregate.Weekly:
                case ReportDateAggregate.Daily:
                {
                    var queryResult = await this.report.GetSurgeriesAggregatedAsync(this.dateAggregate);
                    var modelResult = await this.report.GetSurgeriesAndModelsAggregatedAsync(this.dateAggregate);
                    foreach (var client in queryResult.OrderBy(x => x.ClientName))
                    {
                        using (var row = this.SurgeriesModelsSheet.CreateRowExporter())
                        {
                            row.CreateCell(client.ClientName, this.BrandStyle);
                            row.CreateMultipleCellsIf(
                                this.reportFields.RatingAverage,
                                this.dateRanges.Select(range => client.RatingAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStyleNumber);
                            row.CreateMultipleCellsIf(
                                this.reportFields.UnrepairedPercent,
                                this.dateRanges.Select(range => client.UnrepairedPercent.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStylePercent);
                            row.CreateMultipleCellsIf(
                                this.reportFields.CostSum,
                                this.dateRanges.Select(range => client.CostSum.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStyleCurrency);
                            row.CreateMultipleCellsIf(
                                this.reportFields.CostAverage,
                                this.dateRanges.Select(range => client.CostAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStyleCurrency);
                            row.CreateMultipleCellsIf(
                                this.reportFields.HandpiecesCount,
                                this.dateRanges.Select(range => client.HandpiecesCount.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStyle);
                        }

                        var models = modelResult.Where(x => x.ClientId == client.ClientId).OrderBy(x => x.Brand).ThenBy(x => x.Model).ToList();
                        foreach (var model in models)
                        {
                            using (var row = this.SurgeriesModelsSheet.CreateRowExporter())
                            {
                                row.CreateCell($"{model.Brand} {model.Model}", this.ContentStyle);
                                row.CreateMultipleCellsIf(
                                    this.reportFields.RatingAverage,
                                    this.dateRanges.Select(range => model.RatingAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                    this.ContentStyleNumber);
                                row.CreateMultipleCellsIf(
                                    this.reportFields.UnrepairedPercent,
                                    this.dateRanges.Select(range => model.UnrepairedPercent.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                    this.ContentStylePercent);
                                row.CreateMultipleCellsIf(
                                    this.reportFields.CostSum,
                                    this.dateRanges.Select(range => model.CostSum.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                    this.ContentStyleCurrency);
                                row.CreateMultipleCellsIf(
                                    this.reportFields.CostAverage,
                                    this.dateRanges.Select(range => model.CostAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                    this.ContentStyleCurrency);
                                row.CreateMultipleCellsIf(
                                    this.reportFields.HandpiecesCount,
                                    this.dateRanges.Select(range => model.HandpiecesCount.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                    this.ContentStyle);
                            }
                        }

                        using (var row = this.SurgeriesModelsSheet.CreateRowExporter())
                        {
                            row.CreateCell(SpreadValue.Skipped);
                        }
                    }

                    break;
                }
            }

            await this.MergeHeader(this.SurgeriesModelsSheet);
        }

        private async Task RenderInsights(IWorksheetExporter sheet)
        {
            var data = await this.report.GetSurgeriesForEntirePeriodAsync();
            using (var header = sheet.CreateRowExporter())
            {
                header.CreateCell("Insights", this.HeaderStyle);
                header.CreateCellIf(this.reportFields.RatingAverage, SpreadValue.FromString("Avg. Rating"), this.HeaderStyle);
                header.CreateCellIf(this.reportFields.UnrepairedPercent, SpreadValue.FromString("% Unrep."), this.HeaderStyle);
                header.CreateCellIf(this.reportFields.CostSum, SpreadValue.FromString("Cost"), this.HeaderStyle);
                header.CreateCellIf(this.reportFields.CostAverage, SpreadValue.FromString("Avg. Cost"), this.HeaderStyle);
                header.CreateCellIf(this.reportFields.HandpiecesCount, SpreadValue.FromString("# HPs"), this.HeaderStyle);
            }

            var totalCount = 0;
            var totalCost = 0.00m;
            var totalRating = 0.00m;
            var totalUnrepaired = 0.00m;
            foreach (var brand in data)
            {
                totalCount += brand.HandpiecesCount;
                totalCost += brand.CostSum;
                totalRating += brand.RatingAverage * brand.HandpiecesCount;
                totalUnrepaired += brand.UnrepairedPercent * brand.HandpiecesCount;
            }

            using (var row = sheet.CreateRowExporter())
            {
                row.CreateCell("Total", this.ContentStyle);
                row.CreateCellIf(this.reportFields.RatingAverage, SpreadValue.FromDouble((Double)(totalRating / totalCount)), this.ContentStyleNumber);
                row.CreateCellIf(this.reportFields.UnrepairedPercent, SpreadValue.FromDouble((Double)(totalUnrepaired / totalCount)), this.ContentStylePercent);
                row.CreateCellIf(this.reportFields.CostSum, SpreadValue.FromDouble((Double)totalCost), this.ContentStyleCurrency);
                row.CreateCellIf(this.reportFields.CostAverage, SpreadValue.FromDouble((Double)(totalCost / totalCount)), this.ContentStyleCurrency);
                row.CreateCellIf(this.reportFields.HandpiecesCount, SpreadValue.FromDouble(totalCount), this.ContentStyle);
            }

            using (var row = sheet.CreateRowExporter())
            {
                row.CreateCell(SpreadValue.Skipped);
            }
        }

        private Int32 ComputeFieldsCount()
        {
            var fieldsCount = 0;
            fieldsCount += this.reportFields.RatingAverage ? 1 : 0;
            fieldsCount += this.reportFields.UnrepairedPercent ? 1 : 0;
            fieldsCount += this.reportFields.CostSum ? 1 : 0;
            fieldsCount += this.reportFields.CostAverage ? 1 : 0;
            fieldsCount += this.reportFields.HandpiecesCount ? 1 : 0;

            return fieldsCount;
        }

        private Task RenderHeader(IWorksheetExporter sheet, String nameHeader)
        {
            if (this.dateAggregate == ReportDateAggregate.EntirePeriod)
            {
                using var header = sheet.CreateRowExporter();
                header.CreateCell(nameHeader, this.HeaderStyle);

                if (this.reportFields.RatingAverage)
                {
                    header.CreateCell("Avg. Rating", this.HeaderStyle);
                }

                if (this.reportFields.UnrepairedPercent)
                {
                    header.CreateCell("% Unrep.", this.HeaderStyle);
                }

                if (this.reportFields.CostSum)
                {
                    header.CreateCell("Cost", this.HeaderStyle);
                }

                if (this.reportFields.CostAverage)
                {
                    header.CreateCell("Avg. Cost", this.HeaderStyle);
                }

                if (this.reportFields.HandpiecesCount)
                {
                    header.CreateCell("# HPs", this.HeaderStyle);
                }
            }
            else
            {
                using (var header = sheet.CreateRowExporter())
                {
                    header.CreateCell(nameHeader, this.HeaderStyle);
                    if (this.reportFields.RatingAverage)
                    {
                        header.CreateCell("Avg. Rating", this.HeaderStyle);
                        header.SkipCells(this.dateRanges.Count - 1);
                    }

                    if (this.reportFields.UnrepairedPercent)
                    {
                        header.CreateCell("% Unrep.", this.HeaderStyle);
                        header.SkipCells(this.dateRanges.Count - 1);
                    }

                    if (this.reportFields.CostSum)
                    {
                        header.CreateCell("Cost", this.HeaderStyle);
                        header.SkipCells(this.dateRanges.Count - 1);
                    }

                    if (this.reportFields.CostAverage)
                    {
                        header.CreateCell("Avg. Cost", this.HeaderStyle);
                        header.SkipCells(this.dateRanges.Count - 1);
                    }

                    if (this.reportFields.HandpiecesCount)
                    {
                        header.CreateCell("# HPs", this.HeaderStyle);
                        header.SkipCells(this.dateRanges.Count - 1);
                    }
                }

                using (var header = sheet.CreateRowExporter())
                {
                    header.SkipCells(1);
                    var fieldsCount = this.ComputeFieldsCount();
                    for (var i = 0; i < fieldsCount; i++)
                    {
                        foreach (var range in this.dateRanges)
                        {
                            header.CreateCell(range, this.HeaderStyle);
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }

        private Task MergeHeader(IWorksheetExporter sheet, Int32 rowOffset = 0)
        {
            if (this.dateAggregate != ReportDateAggregate.EntirePeriod)
            {
                sheet.MergeCells(rowOffset + 0, 0, rowOffset + 1, 0);
                var fieldsCount = this.ComputeFieldsCount();
                for (var i = 0; i < fieldsCount; i++)
                {
                    sheet.MergeCells(rowOffset + 0, this.dateRanges.Count * i + 1, rowOffset + 0, this.dateRanges.Count * i + this.dateRanges.Count);
                }
            }

            return Task.CompletedTask;
        }
    }
}
