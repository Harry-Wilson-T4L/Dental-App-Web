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
        var Pages;
        (function (Pages) {
            var GlobalReports;
            (function (GlobalReports) {
                var Reports;
                (function (Reports) {
                    var ReportsPageIdentifierBase = CRM.Controls.Reporting.ReportsPageIdentifierBase;
                    var ReportsPageBase = CRM.Controls.Reporting.ReportsPageBase;
                    var ReportsPageDateRangeMainControls = CRM.Controls.Reporting.ReportsPageDateRangeMainControls;
                    var ReportsPageTabCollection = CRM.Controls.Reporting.ReportsPageTabCollection;
                    var GlobalReportsPageIdentifier = /** @class */ (function (_super) {
                        __extends(GlobalReportsPageIdentifier, _super);
                        function GlobalReportsPageIdentifier() {
                            return _super !== null && _super.apply(this, arguments) || this;
                        }
                        return GlobalReportsPageIdentifier;
                    }(ReportsPageIdentifierBase));
                    Reports.GlobalReportsPageIdentifier = GlobalReportsPageIdentifier;
                    var GlobalReportsPage = /** @class */ (function (_super) {
                        __extends(GlobalReportsPage, _super);
                        function GlobalReportsPage() {
                            return _super !== null && _super.apply(this, arguments) || this;
                        }
                        GlobalReportsPage.prototype.loadIdentifier = function (root) {
                            return new GlobalReportsPageIdentifier();
                        };
                        GlobalReportsPage.prototype.loadMainControls = function (root, identifier) {
                            return new ReportsPageDateRangeMainControls(root);
                        };
                        GlobalReportsPage.prototype.loadTabs = function (root, identifier, mainControls) {
                            return new GlobalReportsTabCollection(root, identifier);
                        };
                        return GlobalReportsPage;
                    }(ReportsPageBase));
                    Reports.GlobalReportsPage = GlobalReportsPage;
                    var GlobalReportsTabCollection = /** @class */ (function (_super) {
                        __extends(GlobalReportsTabCollection, _super);
                        function GlobalReportsTabCollection(tabRoot, identifier) {
                            var _this = _super.call(this) || this;
                            _this.addTab(new Reports.ReportsTabGeneralStats(identifier, tabRoot.find(".reports__general-stats")));
                            _this.addTab(new Reports.ReportsTabServiceLevels(identifier, tabRoot.find(".reports__service-levels")));
                            _this.addTab(new Reports.ReportsTabOther(identifier, tabRoot.find(".reports__other")));
                            return _this;
                            //// this.addTab(new ReportsTabHandpieces(identifier, tabRoot.find(".reports__handpieces")));
                        }
                        return GlobalReportsTabCollection;
                    }(ReportsPageTabCollection));
                    Reports.GlobalReportsTabCollection = GlobalReportsTabCollection;
                })(Reports = GlobalReports.Reports || (GlobalReports.Reports = {}));
            })(GlobalReports = Pages.GlobalReports || (Pages.GlobalReports = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=ReportsPage.js.map