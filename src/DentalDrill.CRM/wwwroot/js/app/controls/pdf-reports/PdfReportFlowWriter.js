var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            var PdfReports;
            (function (PdfReports) {
                var PdfReportFlowWriter = /** @class */ (function () {
                    function PdfReportFlowWriter(builder) {
                        this._builder = builder;
                        this._activePage = this._builder.addPage();
                    }
                    Object.defineProperty(PdfReportFlowWriter.prototype, "activePage", {
                        get: function () {
                            return this._activePage;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    PdfReportFlowWriter.prototype.addNode = function (node) {
                        this._activePage.appendJQuery(node);
                        this.fixOverflow();
                    };
                    PdfReportFlowWriter.prototype.nextPage = function () {
                        this._activePage = this._builder.addPage();
                    };
                    PdfReportFlowWriter.prototype.popPage = function () {
                        this._builder.popPage();
                        this._activePage = this._builder.lastPage();
                    };
                    PdfReportFlowWriter.prototype.addRow = function (content) {
                        if (typeof content === "object") {
                            var row = $("<div class=\"row\"></div>");
                            var col12 = $("<div class=\"col-12\"></div>").appendTo(row);
                            if (Array.isArray(content)) {
                                var contentItems = content;
                                for (var i = 0; i < contentItems.length; i++) {
                                    col12.append(contentItems[i]);
                                }
                            }
                            else {
                                col12.append(content);
                            }
                            this.addNode(row);
                        }
                    };
                    PdfReportFlowWriter.prototype.addHeading = function (level, text) {
                        if (level < 1 || level > 6) {
                            throw new Error("invalid Heading Level");
                        }
                        this.addRow($("<h" + level + "></h" + level + ">").text(text));
                    };
                    PdfReportFlowWriter.prototype.remainingHeight = function () {
                        return this._activePage.remainingHeight();
                    };
                    PdfReportFlowWriter.prototype.fixOverflow = function () {
                        var overflow = this._activePage.removeHeightOverflow();
                        while (overflow.length > 0) {
                            this.nextPage();
                            for (var i = 0; i < overflow.length; i++) {
                                this._activePage.appendHTML(overflow[i]);
                            }
                            overflow = this._activePage.removeHeightOverflow();
                        }
                    };
                    return PdfReportFlowWriter;
                }());
                PdfReports.PdfReportFlowWriter = PdfReportFlowWriter;
            })(PdfReports = Controls.PdfReports || (Controls.PdfReports = {}));
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=PdfReportFlowWriter.js.map