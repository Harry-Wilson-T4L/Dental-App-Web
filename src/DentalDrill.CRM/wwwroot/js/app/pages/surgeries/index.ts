namespace DentalDrill.CRM.Pages.Surgeries.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;

    export enum ExternalHandpieceStatus {
        None,
        Received,
        BeingEstimated,
        WaitingForApproval,
        EstimateSent,
        BeingRepaired,
        ReadyForReturn,
        SentComplete,
        Cancel
    }

    export interface SurgeryHandpieceViewModel {
        Id: string;
        JobNumber: string;
        Brand: string;
        MakeAndModel: string;
        Serial: string;
        Status: string;
        Rating: number;
        ImageUrl: string;
        SpeedType: string;
        Received: Date;
    }

    export class JobNumberGridFilterField extends DevGuild.Filters.Grids.GridFilterField {
        private readonly _root: JQuery;
        private readonly _fieldName: string;

        constructor(root: JQuery, fieldName: string) {
            super();
            this._root = root;
            this._fieldName = fieldName;
        }

        apply(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void {
            const value = this._root.val();
            if (!value) {
                return;
            }

            const regex = new RegExp(`^(\\d+)($|[-A-Za-z][-A-Za-z0-9]*$)`);
            const match = regex.exec(value.toString());
            if (!match || !match[1]) {
                return;
            }

            const jobNumber = match[1];
            
            if ((!exceptions || exceptions.every(x => x !== this._fieldName))) {
                filters.push({
                    field: this._fieldName,
                    operator: "eq",
                    value: jobNumber
                });
            }
        }

        reset(): void {
            this._root.val("").trigger("change");
        }

        get control(): JQuery {
            return this._root;
        }
    }

    export class StatusRepairsGridFilterFieldsCollection extends DevGuild.Filters.Grids.GridFilterFieldsCollection {
        private readonly _jobNumber: JobNumberGridFilterField;
        private readonly _serial: DevGuild.Filters.Grids.StringInputGridFilterField;
        private readonly _makeAndModel: DevGuild.Filters.Grids.DropDownListGridFilterField;
        private readonly _speedType: DevGuild.Filters.Grids.DropDownListGridFilterField;
        private readonly _receivedFrom: DevGuild.Filters.Grids.DatePickerFilterField;
        private readonly _receivedTo: DevGuild.Filters.Grids.DatePickerFilterField;

        constructor(root: JQuery) {
            super(root);
            this._jobNumber = new JobNumberGridFilterField($("#JobNumberFilter"), "JobNumber");
            this._makeAndModel = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#MakeAndModelFilter"), "MakeAndModel", { value: a => a.Value, defaultValue: "" });
            this._makeAndModel.applyValueDelegate = (filters, value) => {
                const split = value.split("||");
                filters.push({
                    field: "Brand",
                    operator: "eq",
                    value: split[0]
                });

                filters.push({
                    field: "MakeAndModel",
                    operator: "eq",
                    value: split[1]
                });
            };
            this._serial = new DevGuild.Filters.Grids.StringInputGridFilterField($("#SerialFilter"), "Serial");
            this._speedType = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#SpeedTypeFilter"), "SpeedType", { value: a => a.Value, defaultValue: 0 });
            this._speedType.resetValueDelegate = () => "";
            this._receivedFrom = new DevGuild.Filters.Grids.DatePickerFilterField($("#ReceivedFromFilter"), "Received", "gte");
            this._receivedTo = new DevGuild.Filters.Grids.DatePickerFilterField($("#ReceivedToFilter"), "Received", "lte");
        }

        applyAll(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void {
            this._jobNumber.apply(filters, exceptions);
            this._makeAndModel.apply(filters, exceptions);
            this._serial.apply(filters, exceptions);
            this._speedType.apply(filters, exceptions);
            this._receivedFrom.apply(filters, exceptions);
            this._receivedTo.apply(filters, exceptions);
        }

        resetAll(): void {
            this._jobNumber.reset();
            this._makeAndModel.reset();
            this._serial.reset();
            this._speedType.reset();
            this._receivedFrom.reset();
            this._receivedTo.reset();
        }
    }

    export class RepairsGridFilter extends DevGuild.Filters.Grids.GridFilterCore<StatusRepairsGridFilterFieldsCollection> {
        private readonly _page: SurgeryPage;

        constructor(page: SurgeryPage) {
            super(page.root);
            this._page = page;
        }

        createFields(root: JQuery<HTMLElement>): StatusRepairsGridFilterFieldsCollection {
            return new StatusRepairsGridFilterFieldsCollection(this.root);
        }

        applyFilter(filters) {
            this._page.updateDataSource(filters);

            const makeAndModel = $("#MakeAndModelFilter").data("kendoDropDownList").value() || "";
            MaintenanceHandpiecesListView.setFiltersData({
                JobNumber: $("#JobNumberFilter").data("kendoAutoComplete").value(),
                Brand: makeAndModel.split("||")[0],
                MakeAndModel: makeAndModel.split("||")[1],
                Serial: $("#SerialFilter").data("kendoAutoComplete").value(),
                SpeedType: $("#SpeedTypeFilter").data("kendoDropDownList").value(),
                ReceivedFrom: $("#ReceivedFromFilter").data("kendoDatePicker").value(),
                ReceivedTo: $("#ReceivedToFilter").data("kendoDatePicker").value()
            });
        }
    }

    export class TotalsRowItem {
        private readonly _root: JQuery;
        private readonly _valueNode: JQuery;
        private readonly _status: ExternalHandpieceStatus;

        constructor(root: JQuery) {
            this._root = root;
            this._valueNode = root.find(".totals-row__value");
            this._status = ExternalHandpieceStatus[root.attr("data-status")];
        }

        get value(): number {
            return parseInt(this._valueNode.text());
        }

        set value(val: number) {
            this._valueNode.text(val.toString());
        }

        get status(): ExternalHandpieceStatus {
            return this._status;
        }
    }

    export class TotalsRow {
        private readonly _root: JQuery;
        private readonly _items: TotalsRowItem[] = [];
        private readonly _itemsMap: Map<ExternalHandpieceStatus, TotalsRowItem> = new Map<ExternalHandpieceStatus, TotalsRowItem>();

        constructor(root: JQuery) {
            this._root = root;
            this._root.find(".totals-row__item").each((i, e) => {
                const item = new TotalsRowItem($(e));
                this._itemsMap.set(item.status, item);
                this._items.push(item);
            });
        }

        setValue(status: ExternalHandpieceStatus, value: number): void {
            this._itemsMap.get(status).value = value;
        }

        list(): TotalsRowItem[] {
            const result: TotalsRowItem[] = [];
            for (let i = 0; i < this._items.length; i++) {
                result.push(this._items[i]);
            }

            return result;
        }
    }

    export class SurgeryStatusGrid {
        private readonly _page: SurgeryPage;
        private readonly _host: JQuery;
        private readonly _dataSource: kendo.data.DataSource;
        private readonly _grid: kendo.ui.Grid;

        constructor(page: SurgeryPage, host: JQuery) {
            this._page = page;
            this._host = host;
            this._dataSource = this.createDataSource();
            this._grid = this.createGrid();
        }

        setTotal(status: ExternalHandpieceStatus, value: number) {
            const nodes = this._host.find(`.surgery-status-grid__status-counter[data-status=${status}]`);
            nodes.text(value.toString());
        }

        expandPresent(): void {
            const nodes = this._host.find(`.surgery-status-grid__status-counter`);
            nodes.each((i, x) => {
                const node = $(x);
                const nodeText = node.text();
                if (nodeText && nodeText !== "0" && nodeText !== "-") {
                    this._grid.expandRow(node.closest("tr"));
                }
            });
        }

        private createDataSource(): kendo.data.DataSource {
            return new kendo.data.DataSource({
                data: [
                    { Status: ExternalHandpieceStatus.Received, Name: "Received", StatusVisualisationNumber: 1 },
                    { Status: ExternalHandpieceStatus.BeingEstimated, Name: "Being Estimated", StatusVisualisationNumber: 2 },
                    { Status: ExternalHandpieceStatus.WaitingForApproval, Name: "Estimate Complete", StatusVisualisationNumber: 3 },
                    { Status: ExternalHandpieceStatus.EstimateSent, Name: "Estimate sent", StatusVisualisationNumber: 4 },
                    { Status: ExternalHandpieceStatus.BeingRepaired, Name: "Being Repaired", StatusVisualisationNumber: 5 },
                    { Status: ExternalHandpieceStatus.ReadyForReturn, Name: "Ready for Return", StatusVisualisationNumber: 6 },
                    { Status: ExternalHandpieceStatus.Cancel, Name: "Unrepaired", StatusVisualisationNumber: -7 },
                ]
            });
        }

        private createGrid(): kendo.ui.Grid {
            this._host.kendoGrid({
                columns: [
                    {
                        title: "Name",
                        field: "Name",
                        template: `#:Name# <strong>(<span class="surgery-status-grid__status-counter" data-status="#: Status#">-</span>)</strong>`
                    },
                    {
                        title: " ",
                        field: "StatusVisualisationNumber",
                        template: x => `<div class="handpiece-status-indicator" style="width: 150px" data-max="7" data-value="${Math.abs(x.StatusVisualisationNumber)}" data-danger="${x.StatusVisualisationNumber < 0 ? "True" : "False"}#">
  <div class="progress handpiece-status-indicator__progress">
    <div class="progress-bar"></div>
  </div>
  <div class="handpiece-status-indicator__points">
    <div class="handpiece-status-indicator__points__point"></div>
    <div class="handpiece-status-indicator__points__point"></div>
    <div class="handpiece-status-indicator__points__point"></div>
    <div class="handpiece-status-indicator__points__point"></div>
    <div class="handpiece-status-indicator__points__point"></div>
    <div class="handpiece-status-indicator__points__point"></div>
    <div class="handpiece-status-indicator__points__point"></div>
  </div>
</div>`
                    }
                ],
                columnMenu: false,
                scrollable: false,
                dataSource: this._dataSource,
                detailTemplate: `<div class="surgery-status-grid__list-view"></div> <div class="surgery-status-grid__list-view__pager"></div>`,
                detailInit: e => {
                    const status = e.data["Status"] as ExternalHandpieceStatus;
                    const dataSource = this._page.getStatusDataSource(status)
                    e.detailRow.find(".surgery-status-grid__list-view").kendoListView({
                        dataSource: dataSource,
                        template: kendo.template($("#surgery-repair-template").html()),
                        dataBound: dataBound => {
                            e.detailRow.find(`.surgery__repair__title[data-toggle="tooltip"]`).tooltip();
                            e.detailRow.find(`.surgery__repair__info[data-toggle="tooltip"]`).tooltip();
                        }
                    });
                    e.detailRow.find(".surgery-status-grid__list-view__pager").kendoPager({
                        pageSizes: [10, 25, 50, 100],
                        buttonCount: 5,
                        dataSource: dataSource
                    });
                },
                detailExpand: e => {
                    e.detailRow.find(".surgery-status-grid__list-view").data("kendoListView").dataSource.read();
                }
            });

            return this._host.data("kendoGrid");
        }
    }

    export class SurgeryPage {
        private readonly _root: JQuery;
        private readonly _id: string;
        private readonly _path: string;

        private readonly _handpiecesSource: kendo.data.DataSource;
        private readonly _statusSourceMap: Map<ExternalHandpieceStatus, kendo.data.DataSource> = new Map<ExternalHandpieceStatus, kendo.data.DataSource>();

        private readonly _totalsRow: TotalsRow;
        private readonly _filters: RepairsGridFilter;
        private readonly _statusGrid: SurgeryStatusGrid;

        constructor(root: JQuery) {
            this._root = root;
            this._id = root.attr("data-id");
            this._path = root.attr("data-path");

            this._handpiecesSource = this.createDataSource();

            // Creating totals wrapper
            this._totalsRow = new TotalsRow(root.find(".totals-row"));

            // Creating filters wrapper
            this._filters = new RepairsGridFilter(this);
            this._root.find(".surgery-filters__search").on("click", e => {
                this._filters.apply();
            });
            this._root.find(".surgery-filters__clear").on("click", e => {
                this._filters.reset();
            });

            // Creating status grid
            this._statusGrid = new SurgeryStatusGrid(this, this._root.find(".surgery-page__status-grid"));

            this._root.data("SurgeryPage", this);
        }

        get root(): JQuery {
            return this._root;
        }

        async initialize(): Promise<void> {
            await this.updateDataSource(undefined);
            this._statusGrid.expandPresent();
        }

        async updateDataSource(filters: any): Promise<void> {
            await this._handpiecesSource.query({ filter: filters });

            const data = this._handpiecesSource.data<SurgeryHandpieceViewModel>();
            const dataByStatus = data.map((x, i) => x)
                .reduce((previous, current) => {
                    if (previous[ExternalHandpieceStatus[current.Status]]) {
                        previous[ExternalHandpieceStatus[current.Status]].push(current);
                    } else {
                        previous[ExternalHandpieceStatus[current.Status]] = [current];
                    }

                    return previous;
                }, { });

            this._totalsRow.list().forEach(item => {
                const group = dataByStatus[item.status];
                if (group) {
                    item.value = group.length;
                    this._statusGrid.setTotal(item.status, group.length);
                    this.setStatusData(item.status, group);
                } else {
                    item.value = 0;
                    this._statusGrid.setTotal(item.status, 0);
                    this.setStatusData(item.status, []);
                }
            });
        }

        getStatusDataSource(status: ExternalHandpieceStatus): kendo.data.DataSource {
            return this._statusSourceMap.get(status);
        }

        private setStatusData(status: ExternalHandpieceStatus, data: SurgeryHandpieceViewModel[]) {
            if (this._statusSourceMap.has(status)) {
                const dataSource = this._statusSourceMap.get(status);
                const existing = dataSource["localData"] as any[];
                existing.splice(0, existing.length);
                for (let item of data) {
                    existing.push(item);
                }

                dataSource.read();
            } else {
                const dataSource = new kendo.data.DataSource({
                    transport: {
                        read: e => e.success(data),
                    },
                    schema: {
                        model: {
                            id: "Id",
                            fields: {
                                Id: { type: "string" },
                                JobNumber: { type: "string" },
                                Brand: { type: "string" },
                                MakeAndModel: { type: "string" },
                                Serial: { type: "string" },
                                Status: { type: "string" },
                                Rating: { type: "number" },
                                SpeedType: { type: "string" },
                                Received: { type: "date" },
                                ImageUrl: { type: "string" }
                            }
                        }
                    },
                    pageSize: 10
                });

                dataSource["localData"] = data;
                this._statusSourceMap.set(status, dataSource);
                dataSource.read();
            }
        }

        private createDataSource(): kendo.data.DataSource {
            return new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: `/Surgeries/${this._path}/Handpieces`
                    }
                },
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                serverGrouping: true,
                serverAggregates: true,
                filter: [],
                schema: {
                    data: "Data",
                    total: "Total",
                    errors: "Errors",
                    model: {
                        id: "Id",
                        fields: {
                            Id: { type: "string" },
                            JobNumber: { type: "string" },
                            Brand: { type: "string" },
                            MakeAndModel: { type: "string" },
                            Serial: { type: "string" },
                            Status: { type: "string" },
                            Rating: { type: "number" },
                            SpeedType: { type: "string" },
                            Received: { type: "date" },
                            ImageUrl: { type: "string" }
                        }
                    }
                }
            });;
        }
    }

    $(() => {
        const page = new SurgeryPage($(".surgery-page"));
        page.initialize();
    });

    export class StatusRepairsGrid {
        static handleDetailsClick(client: string, id: string) {

            const url = routes.surgeries.handpiece(client, id);
            const options = {
                title: "Repair details"
            };
            const dialogRoot = $("<div></div>");
            const dialogOptions = {
                title: "",
                actions: ["close"],
                content: url.value,
                width: "80%",
                maxWidth: 1000,
                //height: "500px",
                height: "auto",
                modal: true,
                visible: false,
                close: () => dialogRoot.data("kendoWindow").destroy(),
                refresh() {
                    dialog.center();
                }
            };

            $.extend(dialogOptions, options);

            (dialogRoot as any).kendoWindow(dialogOptions);
            
            const dialog = dialogRoot.data("kendoWindow");
            dialog.center();
            dialog.open();
        }
    }

    $(() => {
        $(document as any).on("click", ".surgery__repair", (e: JQueryEventObject) => {
            const target = e.currentTarget;
            const clientId = target.getAttribute("data-client-id");
            const id = target.getAttribute("data-id");

            if (clientId && id) {
                StatusRepairsGrid.handleDetailsClick(clientId, id);
            }
        });
    });

    export interface SurgeryMaintenanceHandpieceViewModel {
        Id: string;
        MakeAndModel: string;
        Serial: string;
        ImageUrl: string;
    }

    export class MaintenanceHandpiecesListView {
        private static _lastSelected: string;
        private static _filtersData: object;

        static get instance(): kendo.ui.ListView {
            return $("#MaintenanceHandpiecesListView").data("kendoListView");
        }

        static async handleDataBound(e: kendo.ui.ListViewEvent): Promise<void> {
            $('.surgery-maintenance-handpiece__title[data-toggle="tooltip"]').tooltip();
            $('.surgery-maintenance-handpiece__info__serial[data-toggle="tooltip"]').tooltip();
        }

        static async handleChange(e: kendo.ui.ListViewEvent): Promise<void> {
            const selected = MaintenanceHandpiecesListView.selectedItem();
            if (selected === undefined) {
                return;
            }

            if (selected) {
                MaintenanceHandpiecesListView._lastSelected = selected.Id;
                await MaintenanceHistory.loadHistory(selected);
            } else if (!selected && MaintenanceHandpiecesListView._lastSelected) {
                MaintenanceHandpiecesListView._lastSelected = undefined;
                await MaintenanceHistory.clearHistory();
            }
        }

        static async setFiltersData(data: object): Promise<void> {
            MaintenanceHandpiecesListView._filtersData = data;
            await MaintenanceHandpiecesListView.instance.dataSource.read();


            if (MaintenanceHandpiecesListView._lastSelected) {
                const listView = MaintenanceHandpiecesListView.instance;
                const data = listView.dataSource.data<SurgeryMaintenanceHandpieceViewModel>();
                for (let i = 0; i < data.length; i++) {
                    if (data[i].Id === MaintenanceHandpiecesListView._lastSelected) {
                        listView.select(listView.element.find(`[data-uid='${data[i].uid}']`));
                        return;
                    }
                }

                MaintenanceHandpiecesListView._lastSelected = undefined;
                MaintenanceHistory.clearHistory();
            }
        }

        static filtersData() {
            return MaintenanceHandpiecesListView._filtersData ? MaintenanceHandpiecesListView._filtersData : { };
        }

        static selectedItem(): SurgeryMaintenanceHandpieceViewModel {
            const listView = MaintenanceHandpiecesListView.instance;
            const selectedNodes = listView.select();
            for (let i = 0; i < selectedNodes.length; i++) {
                const element = selectedNodes[i] as any;
                return listView.dataItem<SurgeryMaintenanceHandpieceViewModel>(element);
            }

            return undefined;
        }
    }

    export class MaintenanceHistory {
        private static _template: (data: SurgeryMaintenanceHandpieceViewModel) => string;
        private static _container: JQuery<HTMLElement>;

        static get container(): JQuery<HTMLElement> {
            return MaintenanceHistory._container;
        }

        static get template(): (data: SurgeryMaintenanceHandpieceViewModel) => string {
            if (!MaintenanceHistory._template) {
                MaintenanceHistory._template = kendo.template($("#surgery-maintenance__history__template").html());
            }

            return MaintenanceHistory._template;
        }

        static async loadHistory(handpiece: SurgeryMaintenanceHandpieceViewModel): Promise<void> {
            const container = MaintenanceHistory.createContainer();
            container.html(MaintenanceHistory.template(handpiece));

            const handpieceElement = $(`.surgery-maintenance-handpiece[data-uid=${handpiece["uid"]}]`);
            const targetRowLastElement = MaintenanceHistory.getLastElementInRow(handpieceElement);

            container.insertAfter(targetRowLastElement);
        }

        static async clearHistory(): Promise<void> {
            MaintenanceHistory.removeContainer();
        }

        private static createContainer(): JQuery<HTMLElement> {
            MaintenanceHistory.removeContainer();
            MaintenanceHistory._container = $("<summary></summary>");
            MaintenanceHistory._container.addClass("surgery-maintenance-list-view__history-wrapper");
            return MaintenanceHistory._container;
        }

        private static removeContainer(): void {
            if (MaintenanceHistory._container) {
                MaintenanceHistory._container.remove();
                MaintenanceHistory._container = undefined;
            }
        }

        static findElementByCoordinates(top) {
            return $(".surgery-maintenance-list-view").find(".surgery-maintenance-handpiece").filter((i, x) => $(x).offset().top === top);
        }

        static getLastElementInRow(element) {
            const top = $(element).offset().top;
            const row = MaintenanceHistory.findElementByCoordinates(top);
            return row[row.length - 1];
        }
    }

    export class HandpieceImagesCarousel {
        private readonly _surgery: string;
        private readonly _id: string;
        private readonly _windowWrapper: JQuery;
        private readonly _window: kendo.ui.Window;
        private _firstTime: boolean;
        private _viewPortSize: { width: number, height: number };

        constructor(surgery: string, id: string, firstImage?: string) {
            this._surgery = surgery;
            this._id = id;

            this.updateViewPortSize();
            this._windowWrapper = $("<div></div>");
            this._windowWrapper.kendoWindow({
                width: `${Math.round(this._viewPortSize.width * 0.9)}px`,
                height: `${Math.round(this._viewPortSize.height * 0.9)}px`,
                title: "Handpiece Images",
                actions: ["close"],
                modal: true,
                visible: false,
                content: `/Surgeries/${this._surgery}/Handpiece/${id}/Images${firstImage ? `?image=${firstImage}` : ""}`,
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.center();
                }
            });

            this._window = this._windowWrapper.data("kendoWindow");
            this._firstTime = true;
        }

        show(image?: string): void {
            if (!this._firstTime && this.updateViewPortSize()) {
                this._window.setOptions({
                    width: `${Math.round(this._viewPortSize.width * 0.9)}px`,
                    height: `${Math.round(this._viewPortSize.height * 0.9)}px`,
                });
            }

            this._window.open();
            this._window.center();
            if (this._firstTime) {
                this._firstTime = false;
            } else {
                if (image) {
                    this.selectPage(image);
                }
            }
        }

        private updateViewPortSize(): boolean {
            const viewPortSize = {
                width: $(window).width(),
                height: $(window).height(),
            };

            if (!this._viewPortSize) {
                this._viewPortSize = viewPortSize;
                return true;
            }

            if (this._viewPortSize.width === viewPortSize.width && this._viewPortSize.height == viewPortSize.height) {
                return false;
            }

            this._viewPortSize = viewPortSize;
            return true;
        }

        private selectPage(id: string) {
            const scrollViewWrapper = this._window.wrapper.find(".handpiece-image-carousel");
            if (scrollViewWrapper.length === 0) {
                return;
            }

            const page = scrollViewWrapper.find(`.handpiece-image-carousel__page[data-id='${id}']`);
            if (page.length === 0) {
                return;
            }

            try {
                const pageNo = parseInt(page.attr("data-index"));
                const scrollView = scrollViewWrapper.data("kendoScrollView");
                scrollView.scrollTo(pageNo, false);
            } catch (exception) { }
        }
    }

    export class HandpieceImagesCarouselManager {
        private readonly _surgeryId: string;
        private readonly _carousels: Map<string, HandpieceImagesCarousel> = new Map<string, HandpieceImagesCarousel>();
        
        constructor(surgeryId: string) {
            this._surgeryId = surgeryId;
        }

        showCarousel(id: string, image?: string): void {
            let carousel: HandpieceImagesCarousel;
            if (this._carousels.has(id)) {
                carousel = this._carousels.get(id);
            } else {
                carousel = new HandpieceImagesCarousel(this._surgeryId, id, image);
                this._carousels.set(id, carousel);
            }

            carousel.show(image);
        }
    }

    $(() => {
        const surgeryId = $(".surgery-page").attr("data-path");
        const carouselManager = new HandpieceImagesCarouselManager(surgeryId);
        $(document).on("click", ".handpiece-image-show-carousel", e => {
            const target = $(e.target);
            const id = target.attr("data-id");
            const image = target.attr("data-image");
            carouselManager.showCarousel(id, image);
        });
    });
}