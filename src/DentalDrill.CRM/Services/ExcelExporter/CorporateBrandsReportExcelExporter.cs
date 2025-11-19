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
    public class CorporateBrandsReportExcelExporter : ExcelExporterBase
    {
        private readonly CorporateDomainModel corporate;
        private readonly CorporateBrandsReportsDomainModel report;
        private readonly DateTime from;
        private readonly DateTime to;
        private readonly ReportDateAggregate dateAggregate;
        private readonly IReadOnlyList<String> dateRanges;
        private readonly (Boolean RatingAverage, Boolean CostSum, Boolean CostAverage, Boolean UnrepairedPercent, Boolean HandpiecesCount) reportFields;

        public CorporateBrandsReportExcelExporter(CorporateDomainModel corporate, CorporateBrandsReportsDomainModel report, DateTime @from, DateTime to, ReportDateAggregate dateAggregate, HashSet<String> reportFields)
        {
            this.corporate = corporate;
            this.report = report;
            this.from = from;
            this.to = to;
            this.dateAggregate = dateAggregate;
            this.reportFields = (reportFields.Contains("RatingAverage"), reportFields.Contains("CostSum"), reportFields.Contains("CostAverage"), reportFields.Contains("UnrepairedPercent"), reportFields.Contains("HandpiecesCount"));
            this.dateRanges = ReportDateAggregateHelper.GenerateDateRanges(this.from, this.to, this.dateAggregate);
        }

        protected IWorksheetExporter BrandsSheet { get; private set; }

        protected IWorksheetExporter BrandsAndSurgeriesSheet { get; private set; }

        protected IWorksheetExporter BrandsAndModelsSheet { get; private set; }

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

            using (this.BrandsSheet = this.Workbook.CreateWorksheetExporter("Brands"))
            {
                await this.RenderBrandsSheet();
            }

            using (this.BrandsAndSurgeriesSheet = this.Workbook.CreateWorksheetExporter("Brands and surgeries"))
            {
                await this.RenderBrandsAndSurgeriesSheet();
            }

            using (this.BrandsAndModelsSheet = this.Workbook.CreateWorksheetExporter("Brands and model"))
            {
                await this.RenderBrandsAndModelsSheet();
            }
        }

        protected async Task RenderBrandsSheet()
        {
            this.BrandsSheet.ConfigureColumn(widthInPixels: 200);
            var fieldsCount = this.ComputeFieldsCount() * this.dateRanges.Count;
            for (var i = 0; i < fieldsCount; i++)
            {
                this.BrandsSheet.ConfigureColumn(widthInPixels: 89);
            }

            await this.RenderInsights(this.BrandsSheet);
            await this.RenderHeader(this.BrandsSheet, "Brand");

            switch (this.dateAggregate)
            {
                case ReportDateAggregate.EntirePeriod:
                {
                    var queryResult = await this.report.GetBrandsForEntirePeriodAsync();
                    foreach (var brand in queryResult.OrderBy(x => x.Brand))
                    {
                        using (var row = this.BrandsSheet.CreateRowExporter())
                        {
                            row.CreateCell(brand.Brand, this.ContentStyle);
                            row.CreateCellIf(this.reportFields.RatingAverage, SpreadValue.FromDouble((Double)brand.RatingAverage), this.ContentStyleNumber);
                            row.CreateCellIf(this.reportFields.UnrepairedPercent, SpreadValue.FromDouble((Double)brand.UnrepairedPercent), this.ContentStylePercent);
                            row.CreateCellIf(this.reportFields.CostSum, SpreadValue.FromDouble((Double)brand.CostSum), this.ContentStyleCurrency);
                            row.CreateCellIf(this.reportFields.CostAverage, SpreadValue.FromDouble((Double)brand.CostAverage), this.ContentStyleCurrency);
                            row.CreateCellIf(this.reportFields.HandpiecesCount, SpreadValue.FromDouble((Double)brand.HandpiecesCount), this.ContentStyle);
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
                    var queryResult = await this.report.GetBrandsAggregatedAsync(this.dateAggregate);
                    foreach (var brand in queryResult.OrderBy(x => x.Brand))
                    {
                        using (var row = this.BrandsSheet.CreateRowExporter())
                        {
                            row.CreateCell(brand.Brand, this.ContentStyle);
                            row.CreateMultipleCellsIf(
                                this.reportFields.RatingAverage,
                                this.dateRanges.Select(range => brand.RatingAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.ContentStyleNumber);
                            row.CreateMultipleCellsIf(
                                this.reportFields.UnrepairedPercent,
                                this.dateRanges.Select(range => brand.UnrepairedPercent.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.ContentStylePercent);
                            row.CreateMultipleCellsIf(
                                this.reportFields.CostSum,
                                this.dateRanges.Select(range => brand.CostSum.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.ContentStyleCurrency);
                            row.CreateMultipleCellsIf(
                                this.reportFields.CostAverage,
                                this.dateRanges.Select(range => brand.CostAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.ContentStyleCurrency);
                            row.CreateMultipleCellsIf(
                                this.reportFields.HandpiecesCount,
                                this.dateRanges.Select(range => brand.HandpiecesCount.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.ContentStyle);
                        }
                    }

                    break;
                }
            }

            await this.MergeHeader(this.BrandsSheet, rowOffset: 3);
        }

        protected async Task RenderBrandsAndSurgeriesSheet()
        {
            this.BrandsAndSurgeriesSheet.ConfigureColumn(widthInPixels: 200);
            var fieldsCount = this.ComputeFieldsCount() * this.dateRanges.Count;
            for (var i = 0; i < fieldsCount; i++)
            {
                this.BrandsAndSurgeriesSheet.ConfigureColumn(widthInPixels: 89);
            }

            await this.RenderHeader(this.BrandsAndSurgeriesSheet, "Brand / Surgery");
            switch (this.dateAggregate)
            {
                case ReportDateAggregate.EntirePeriod:
                {
                    var queryResult = await this.report.GetBrandsForEntirePeriodAsync();
                    var modelsResult = await this.report.GetBrandsAndSurgeriesForEntirePeriodAsync();
                    foreach (var brand in queryResult.OrderBy(x => x.Brand))
                    {
                        using (var row = this.BrandsAndSurgeriesSheet.CreateRowExporter())
                        {
                            row.CreateCell(brand.Brand, this.BrandStyle);
                            row.CreateCellIf(this.reportFields.RatingAverage, SpreadValue.FromDouble((Double)brand.RatingAverage), this.BrandStyleNumber);
                            row.CreateCellIf(this.reportFields.UnrepairedPercent, SpreadValue.FromDouble((Double)brand.UnrepairedPercent), this.BrandStylePercent);
                            row.CreateCellIf(this.reportFields.CostSum, SpreadValue.FromDouble((Double)brand.CostSum), this.BrandStyleCurrency);
                            row.CreateCellIf(this.reportFields.CostAverage, SpreadValue.FromDouble((Double)brand.CostAverage), this.BrandStyleCurrency);
                            row.CreateCellIf(this.reportFields.HandpiecesCount, SpreadValue.FromDouble((Double)brand.HandpiecesCount), this.BrandStyle);
                        }

                        var clients = modelsResult.Where(x => x.Brand == brand.Brand).OrderBy(x => x.ClientName).ToList();
                        foreach (var client in clients)
                        {
                            using (var row = this.BrandsAndSurgeriesSheet.CreateRowExporter())
                            {
                                row.CreateCell(client.ClientName, this.ContentStyle);
                                row.CreateCellIf(this.reportFields.RatingAverage, SpreadValue.FromDouble((Double)client.RatingAverage), this.ContentStyleNumber);
                                row.CreateCellIf(this.reportFields.UnrepairedPercent, SpreadValue.FromDouble((Double)client.UnrepairedPercent), this.ContentStylePercent);
                                row.CreateCellIf(this.reportFields.CostSum, SpreadValue.FromDouble((Double)client.CostSum), this.ContentStyleCurrency);
                                row.CreateCellIf(this.reportFields.CostAverage, SpreadValue.FromDouble((Double)client.CostAverage), this.ContentStyleCurrency);
                                row.CreateCellIf(this.reportFields.HandpiecesCount, SpreadValue.FromDouble((Double)client.HandpiecesCount), this.ContentStyle);
                            }
                        }

                        using (var row = this.BrandsAndSurgeriesSheet.CreateRowExporter())
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
                    var queryResult = await this.report.GetBrandsAggregatedAsync(this.dateAggregate);
                    var modelResult = await this.report.GetBrandsAndSurgeriesAggregatedAsync(this.dateAggregate);
                    foreach (var brand in queryResult.OrderBy(x => x.Brand))
                    {
                        using (var row = this.BrandsAndSurgeriesSheet.CreateRowExporter())
                        {
                            row.CreateCell(brand.Brand, this.BrandStyle);
                            row.CreateMultipleCellsIf(
                                this.reportFields.RatingAverage,
                                this.dateRanges.Select(range => brand.RatingAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStyleNumber);
                            row.CreateMultipleCellsIf(
                                this.reportFields.UnrepairedPercent,
                                this.dateRanges.Select(range => brand.UnrepairedPercent.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStylePercent);
                            row.CreateMultipleCellsIf(
                                this.reportFields.CostSum,
                                this.dateRanges.Select(range => brand.CostSum.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStyleCurrency);
                            row.CreateMultipleCellsIf(
                                this.reportFields.CostAverage,
                                this.dateRanges.Select(range => brand.CostAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStyleCurrency);
                            row.CreateMultipleCellsIf(
                                this.reportFields.HandpiecesCount,
                                this.dateRanges.Select(range => brand.HandpiecesCount.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStyle);
                        }

                        var clients = modelResult.Where(x => x.Brand == brand.Brand).OrderBy(x => x.ClientName).ToList();
                        foreach (var client in clients)
                        {
                            using (var row = this.BrandsAndSurgeriesSheet.CreateRowExporter())
                            {
                                row.CreateCell(client.ClientName, this.ContentStyle);
                                row.CreateMultipleCellsIf(
                                    this.reportFields.RatingAverage,
                                    this.dateRanges.Select(range => client.RatingAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                    this.ContentStyleNumber);
                                row.CreateMultipleCellsIf(
                                    this.reportFields.UnrepairedPercent,
                                    this.dateRanges.Select(range => client.UnrepairedPercent.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                    this.ContentStylePercent);
                                row.CreateMultipleCellsIf(
                                    this.reportFields.CostSum,
                                    this.dateRanges.Select(range => client.CostSum.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                    this.ContentStyleCurrency);
                                row.CreateMultipleCellsIf(
                                    this.reportFields.CostAverage,
                                    this.dateRanges.Select(range => client.CostAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                    this.ContentStyleCurrency);
                                row.CreateMultipleCellsIf(
                                    this.reportFields.HandpiecesCount,
                                    this.dateRanges.Select(range => client.HandpiecesCount.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                    this.ContentStyle);
                            }
                        }

                        using (var row = this.BrandsAndSurgeriesSheet.CreateRowExporter())
                        {
                            row.CreateCell(SpreadValue.Skipped);
                        }
                    }

                    break;
                }
            }

            await this.MergeHeader(this.BrandsAndSurgeriesSheet);
        }

        protected async Task RenderBrandsAndModelsSheet()
        {
            this.BrandsAndModelsSheet.ConfigureColumn(widthInPixels: 200);
            var fieldsCount = this.ComputeFieldsCount() * this.dateRanges.Count;
            for (var i = 0; i < fieldsCount; i++)
            {
                this.BrandsAndModelsSheet.ConfigureColumn(widthInPixels: 89);
            }

            await this.RenderHeader(this.BrandsAndModelsSheet, "Brand / Model");
            switch (this.dateAggregate)
            {
                case ReportDateAggregate.EntirePeriod:
                {
                    var queryResult = await this.report.GetBrandsForEntirePeriodAsync();
                    var modelsResult = await this.report.GetBrandsAndModelsForEntirePeriodAsync();
                    foreach (var brand in queryResult.OrderBy(x => x.Brand))
                    {
                        using (var row = this.BrandsAndModelsSheet.CreateRowExporter())
                        {
                            row.CreateCell(brand.Brand, this.BrandStyle);
                            row.CreateCellIf(this.reportFields.RatingAverage, SpreadValue.FromDouble((Double)brand.RatingAverage), this.BrandStyleNumber);
                            row.CreateCellIf(this.reportFields.UnrepairedPercent, SpreadValue.FromDouble((Double)brand.UnrepairedPercent), this.BrandStylePercent);
                            row.CreateCellIf(this.reportFields.CostSum, SpreadValue.FromDouble((Double)brand.CostSum), this.BrandStyleCurrency);
                            row.CreateCellIf(this.reportFields.CostAverage, SpreadValue.FromDouble((Double)brand.CostAverage), this.BrandStyleCurrency);
                            row.CreateCellIf(this.reportFields.HandpiecesCount, SpreadValue.FromDouble((Double)brand.HandpiecesCount), this.BrandStyle);
                        }

                        var models = modelsResult.Where(x => x.Brand == brand.Brand).OrderBy(x => x.Brand).ThenBy(x => x.Model).ToList();
                        foreach (var model in models)
                        {
                            using (var row = this.BrandsAndModelsSheet.CreateRowExporter())
                            {
                                row.CreateCell($"{model.Brand} {model.Model}", this.ContentStyle);
                                row.CreateCellIf(this.reportFields.RatingAverage, SpreadValue.FromDouble((Double)model.RatingAverage), this.ContentStyleNumber);
                                row.CreateCellIf(this.reportFields.UnrepairedPercent, SpreadValue.FromDouble((Double)model.UnrepairedPercent), this.ContentStylePercent);
                                row.CreateCellIf(this.reportFields.CostSum, SpreadValue.FromDouble((Double)model.CostSum), this.ContentStyleCurrency);
                                row.CreateCellIf(this.reportFields.CostAverage, SpreadValue.FromDouble((Double)model.CostAverage), this.ContentStyleCurrency);
                                row.CreateCellIf(this.reportFields.HandpiecesCount, SpreadValue.FromDouble((Double)model.HandpiecesCount), this.ContentStyle);
                            }
                        }

                        using (var row = this.BrandsAndModelsSheet.CreateRowExporter())
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
                    var queryResult = await this.report.GetBrandsAggregatedAsync(this.dateAggregate);
                    var modelResult = await this.report.GetBrandsAndModelsAggregatedAsync(this.dateAggregate);
                    foreach (var brand in queryResult.OrderBy(x => x.Brand))
                    {
                        using (var row = this.BrandsAndModelsSheet.CreateRowExporter())
                        {
                            row.CreateCell(brand.Brand, this.BrandStyle);
                            row.CreateMultipleCellsIf(
                                this.reportFields.RatingAverage,
                                this.dateRanges.Select(range => brand.RatingAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStyleNumber);
                            row.CreateMultipleCellsIf(
                                this.reportFields.UnrepairedPercent,
                                this.dateRanges.Select(range => brand.UnrepairedPercent.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStylePercent);
                            row.CreateMultipleCellsIf(
                                this.reportFields.CostSum,
                                this.dateRanges.Select(range => brand.CostSum.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStyleCurrency);
                            row.CreateMultipleCellsIf(
                                this.reportFields.CostAverage,
                                this.dateRanges.Select(range => brand.CostAverage.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStyleCurrency);
                            row.CreateMultipleCellsIf(
                                this.reportFields.HandpiecesCount,
                                this.dateRanges.Select(range => brand.HandpiecesCount.TryGetValue(range, out var rangeValue) ? SpreadValue.FromDouble((Double)rangeValue) : SpreadValue.Skipped),
                                this.BrandStyle);
                        }

                        var models = modelResult.Where(x => x.Brand == brand.Brand).OrderBy(x => x.Model).ToList();
                        foreach (var model in models)
                        {
                            using (var row = this.BrandsAndModelsSheet.CreateRowExporter())
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

                        using (var row = this.BrandsAndModelsSheet.CreateRowExporter())
                        {
                            row.CreateCell(SpreadValue.Skipped);
                        }
                    }

                    break;
                }
            }

            await this.MergeHeader(this.BrandsAndModelsSheet);
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
