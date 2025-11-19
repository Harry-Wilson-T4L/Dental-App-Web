var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            var PdfReports;
            (function (PdfReports) {
                var PdfReportBuilder = /** @class */ (function () {
                    function PdfReportBuilder() {
                        var _this = this;
                        this._pages = [];
                        this._fileName = "Report.pdf";
                        this._windowWrapper = $("<div class=\"pdf-report pdf-report__window\"></div>");
                        this._windowWrapper.kendoWindow({
                            width: "90%",
                            height: "90%",
                            title: "Report",
                            actions: ["close"],
                            modal: true,
                            visible: false,
                            refresh: function (e) {
                                e.sender.center();
                            },
                            open: function (e) {
                                e.sender.center();
                            }
                        });
                        this._window = this._windowWrapper.data("kendoWindow");
                        this._wrapper = $("<div class=\"pdf-report__wrapper\"></div>").appendTo(this._windowWrapper);
                        this._toolbar = $("<div class=\"pdf-report__toolbar\"></div>").appendTo(this._wrapper);
                        this._toolbarExportButton = $("<button type=\"button\" class=\"pdf-report__toolbar__button btn btn-secondary btn-sm\"><span class=\"fas fa-fw fa-file-pdf\"></span> Save as PDF</button>")
                            .appendTo(this._toolbar);
                        this._toolbarExportButton.on("click", function (e) { return __awaiter(_this, void 0, void 0, function () {
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0: return [4 /*yield*/, this.saveAsPdf(this._fileName)];
                                    case 1:
                                        _a.sent();
                                        return [2 /*return*/];
                                }
                            });
                        }); });
                        this._containerWrapper = $("<div class=\"pdf-report__container-wrapper\"></div>").appendTo(this._wrapper);
                        this._container = $("<div class=\"pdf-report__container\"></div>").appendTo(this._containerWrapper);
                    }
                    PdfReportBuilder.prototype.open = function () {
                        this._window.open();
                    };
                    PdfReportBuilder.prototype.addPage = function (initializer) {
                        var pageBuilder = new PdfReports.PdfPageBuilder(this._container);
                        if (initializer) {
                            initializer(pageBuilder);
                        }
                        this._pages.push(pageBuilder);
                        return pageBuilder;
                    };
                    PdfReportBuilder.prototype.popPage = function () {
                        var page = this._pages.pop();
                        page.remove();
                    };
                    PdfReportBuilder.prototype.lastPage = function () {
                        return this._pages.length > 0 ? this._pages[this._pages.length - 1] : undefined;
                    };
                    Object.defineProperty(PdfReportBuilder.prototype, "fileName", {
                        get: function () {
                            return this._fileName;
                        },
                        set: function (val) {
                            this._fileName = val;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    PdfReportBuilder.prototype.saveAsPdf = function (fileName) {
                        return __awaiter(this, void 0, void 0, function () {
                            var root, _i, _a, page, pageGroups, _b, pageGroups_1, pageGroup, pdf;
                            return __generator(this, function (_c) {
                                switch (_c.label) {
                                    case 0:
                                        root = new kendo.drawing.Group({
                                            pdf: {
                                                paperSize: "A4",
                                                margin: "0cm",
                                            }
                                        });
                                        _i = 0, _a = this._pages;
                                        _c.label = 1;
                                    case 1:
                                        if (!(_i < _a.length)) return [3 /*break*/, 4];
                                        page = _a[_i];
                                        return [4 /*yield*/, page.exportGroups()];
                                    case 2:
                                        pageGroups = _c.sent();
                                        for (_b = 0, pageGroups_1 = pageGroups; _b < pageGroups_1.length; _b++) {
                                            pageGroup = pageGroups_1[_b];
                                            root.children.push(pageGroup);
                                        }
                                        _c.label = 3;
                                    case 3:
                                        _i++;
                                        return [3 /*break*/, 1];
                                    case 4: return [4 /*yield*/, kendo.drawing.exportPDF(root, { multiPage: true })];
                                    case 5:
                                        pdf = _c.sent();
                                        kendo.saveAs({
                                            dataURI: pdf,
                                            fileName: fileName
                                        });
                                        return [2 /*return*/];
                                }
                            });
                        });
                    };
                    return PdfReportBuilder;
                }());
                PdfReports.PdfReportBuilder = PdfReportBuilder;
            })(PdfReports = Controls.PdfReports || (Controls.PdfReports = {}));
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=PdfReportBuilder.js.map