var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            var Reporting;
            (function (Reporting) {
                var PdfReportFlowWriter = DentalDrill.CRM.Controls.PdfReports.PdfReportFlowWriter;
                var SurgeryReportFlowWriter = /** @class */ (function (_super) {
                    __extends(SurgeryReportFlowWriter, _super);
                    function SurgeryReportFlowWriter(builder) {
                        return _super.call(this, builder) || this;
                    }
                    SurgeryReportFlowWriter.prototype.addInsightsTotals = function (title, dataContainer) {
                        var row1 = $("<div class=\"row\"></div>");
                        var totals = dataContainer.find(".reports__insights__total");
                        totals.each(function (index, element) {
                            var key = $(element).find(".reports__insights__total__key").text();
                            var value = $(element).find(".reports__insights__total__value").text();
                            var item = $("<div class=\"reports__insights__total col text-center\"></div>");
                            var itemValue = $("<div class=\"reports__insights__total__value\"></div>")
                                .text(value)
                                .appendTo(item);
                            var itemKey = $("<div class=\"reports__insights__total__key\"></div>")
                                .text(key)
                                .appendTo(item);
                            row1.append(item);
                        });
                        if (title) {
                            var row2 = $("<div class=\"row\"></div>");
                            var wrapper = $("<div class=\"col-12 reports__insights__total__caption text-center font-italic mt-2\"></div>").appendTo(row2);
                            this.addRow([row1, row2]);
                        }
                        else {
                            this.addRow([row1]);
                        }
                    };
                    SurgeryReportFlowWriter.prototype.addCharts = function (charts) {
                        for (var i = 0; i < charts.length; i++) {
                            var chart = charts[i];
                            var chartOptions = JSON.parse(JSON.stringify(chart.options));
                            if (!chartOptions.chartArea) {
                                chartOptions.chartArea = {};
                            }
                            chartOptions.chartArea.width = 350;
                            chartOptions.chartArea.height = 280;
                            var chartWrapper = $("<div class=\"chart chart__container col-8 offset-2\"></div>");
                            var chartHost = $("<div class=\"chart__host\"></div>").appendTo(chartWrapper);
                            chartHost.kendoChart(chartOptions);
                            this.addRow(chartWrapper);
                        }
                    };
                    SurgeryReportFlowWriter.prototype.addTable = function (table) {
                        var _this = this;
                        var addRenderedTable = function (rendered, adjustments) {
                            rendered.classList.add("report-table");
                            if (adjustments) {
                                adjustments(rendered);
                            }
                            var wrapped = $(rendered);
                            _this.addRow(wrapped);
                        };
                        var getAvailableNumberOfRows = function (startFrom) {
                            var totalItems = table.items.length;
                            if (startFrom >= totalItems) {
                                console.log("Row from " + startFrom + " count " + 0);
                                return 0;
                            }
                            var remainingHeight = _this.activePage.remainingHeight();
                            var count = 1;
                            while (startFrom + count < totalItems && table.measureHeight(startFrom, count + 1) <= remainingHeight) {
                                count++;
                            }
                            console.log("Row from " + startFrom + " count " + count);
                            return count;
                        };
                        var pageWidth = 530;
                        var columnSets = [];
                        var column = 0;
                        while (column < table.columns.length) {
                            var count = 1;
                            while ((column + count) < table.columns.length && table.measureWidth(column, count) < pageWidth) {
                                if (table.measureWidth(column, count + 1) < pageWidth) {
                                    count++;
                                }
                                else {
                                    break;
                                }
                            }
                            columnSets.push({ start: column, count: count });
                            column = column + count;
                        }
                        var row = 0;
                        if (this.activePage.usedHeight() === 0) {
                            // Current page is empty - nothing special, rendering first segment, and then breaking page and rendering next segments
                            var rowCount = getAvailableNumberOfRows(row);
                            for (var i = 0; i < columnSets.length; i++) {
                                addRenderedTable(table.renderFragment(columnSets[i].start, columnSets[i].count, row, rowCount));
                                this.nextPage();
                            }
                            row += rowCount;
                            rowCount = getAvailableNumberOfRows(row);
                            while (rowCount > 0) {
                                for (var i = 0; i < columnSets.length; i++) {
                                    addRenderedTable(table.renderFragment(columnSets[i].start, columnSets[i].count, row, rowCount));
                                    this.nextPage();
                                }
                                row += rowCount;
                                rowCount = getAvailableNumberOfRows(row);
                            }
                            this.popPage();
                        }
                        else if (this.activePage.remainingHeight() < table.rowHeight * 3) {
                            // Not enough space on current page - starting new page, rendering first segment, and then breaking page and rendering next segments
                            this.nextPage();
                            var rowCount = getAvailableNumberOfRows(row);
                            for (var i = 0; i < columnSets.length; i++) {
                                addRenderedTable(table.renderFragment(columnSets[i].start, columnSets[i].count, row, rowCount));
                                this.nextPage();
                            }
                            row += rowCount;
                            rowCount = getAvailableNumberOfRows(row);
                            while (rowCount > 0) {
                                for (var i = 0; i < columnSets.length; i++) {
                                    addRenderedTable(table.renderFragment(columnSets[i].start, columnSets[i].count, row, rowCount));
                                    this.nextPage();
                                }
                                row += rowCount;
                                rowCount = getAvailableNumberOfRows(row);
                            }
                            this.popPage();
                        }
                        else {
                            // Current page is not empty - remembering remaining space, which would be added for every starting segment page
                            var usedHeight_1 = this.activePage.usedHeight();
                            var rowCount = getAvailableNumberOfRows(row);
                            var _loop_1 = function (i) {
                                var col = i;
                                addRenderedTable(table.renderFragment(columnSets[i].start, columnSets[i].count, row, rowCount), function (t) {
                                    if (col > 0) {
                                        t.style.marginTop = usedHeight_1 + "px";
                                    }
                                });
                                this_1.nextPage();
                            };
                            var this_1 = this;
                            for (var i = 0; i < columnSets.length; i++) {
                                _loop_1(i);
                            }
                            row += rowCount;
                            rowCount = getAvailableNumberOfRows(row);
                            while (rowCount > 0) {
                                for (var i = 0; i < columnSets.length; i++) {
                                    addRenderedTable(table.renderFragment(columnSets[i].start, columnSets[i].count, row, rowCount));
                                    this.nextPage();
                                }
                                row += rowCount;
                                rowCount = getAvailableNumberOfRows(row);
                            }
                            this.popPage();
                        }
                    };
                    return SurgeryReportFlowWriter;
                }(PdfReportFlowWriter));
                Reporting.SurgeryReportFlowWriter = SurgeryReportFlowWriter;
            })(Reporting = Controls.Reporting || (Controls.Reporting = {}));
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=SurgeryReportFlowWriter.js.map