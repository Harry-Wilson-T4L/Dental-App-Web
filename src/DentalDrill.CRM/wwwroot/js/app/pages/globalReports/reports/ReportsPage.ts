namespace DentalDrill.CRM.Pages.GlobalReports.Reports {
    import ReportsPageIdentifierBase = CRM.Controls.Reporting.ReportsPageIdentifierBase;
    import ReportsPageBase = CRM.Controls.Reporting.ReportsPageBase;
    import ReportsPageDateRangeGlobalFilters = CRM.Controls.Reporting.ReportsPageDateRangeGlobalFilters;
    import ReportsPageDateRangeMainControls = CRM.Controls.Reporting.ReportsPageDateRangeMainControls;
    import ReportsPageTabCollection = CRM.Controls.Reporting.ReportsPageTabCollection;

    export class GlobalReportsPageIdentifier extends ReportsPageIdentifierBase {
    }

    export class GlobalReportsPage extends ReportsPageBase<GlobalReportsPageIdentifier, ReportsPageDateRangeGlobalFilters, ReportsPageDateRangeMainControls<ReportsPageDateRangeGlobalFilters>, GlobalReportsTabCollection> {
        loadIdentifier(root: JQuery<HTMLElement>): GlobalReportsPageIdentifier {
            return new GlobalReportsPageIdentifier();
        }

        loadMainControls(root: JQuery<HTMLElement>, identifier: GlobalReportsPageIdentifier): ReportsPageDateRangeMainControls<ReportsPageDateRangeGlobalFilters> {
            return new ReportsPageDateRangeMainControls<ReportsPageDateRangeGlobalFilters>(root);
        }

        loadTabs(root: JQuery<HTMLElement>, identifier: GlobalReportsPageIdentifier, mainControls: ReportsPageDateRangeMainControls<ReportsPageDateRangeGlobalFilters>): GlobalReportsTabCollection {
            return new GlobalReportsTabCollection(root, identifier);
        }
    }

    export class GlobalReportsTabCollection extends ReportsPageTabCollection<ReportsPageDateRangeGlobalFilters> {
        constructor(tabRoot: JQuery<HTMLElement>, identifier: GlobalReportsPageIdentifier) {
            super();
            this.addTab(new ReportsTabGeneralStats(identifier, tabRoot.find(".reports__general-stats")));
            this.addTab(new ReportsTabServiceLevels(identifier, tabRoot.find(".reports__service-levels")));
            this.addTab(new ReportsTabOther(identifier, tabRoot.find(".reports__other")));
            //// this.addTab(new ReportsTabHandpieces(identifier, tabRoot.find(".reports__handpieces")));
        }
    }
}