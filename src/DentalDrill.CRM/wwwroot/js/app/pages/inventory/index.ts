namespace DentalDrill.CRM.Pages.Inventory.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    enum HandpieceSpeedCompatibility {
        None = 0,
        Other = 1,
        LowSpeed = 2,
        HighSpeed = 4,
        All = 7,
    }

    enum InventorySKUNodeType {
        Leaf = 0,
        Group = 1,
    }

    interface InventorySKUType {
        Id: string;
        Name: string;
        HandpieceSpeedCompatibility: HandpieceSpeedCompatibility;
        SKUCount: number;
    }

    enum InventorySKUStatusFilter {
        All,
        WarningOnly,
    }

    interface InventorySKU {
        Id: string;
        TypeId: string;
        ParentId: string;
        Name: string;
        TotalQuantity: number;
        ShelfQuantity: number;
        TrayQuantity: number;
        OrderedQuantity: number;
        RequestedQuantity: number;
        AveragePrice: number;
        TotalPrice: number;
        Description: string;
        NodeType: InventorySKUNodeType;
        IsDefaultChild: boolean;
        HasWarning: boolean;
        HasDescendantsWithWarning: boolean;
    }

    export class GridInnerHeader {
        private readonly _grid: kendo.ui.Grid;
        private _header: HTMLDivElement;

        constructor(grid: kendo.ui.Grid) {
            this._grid = grid;
        }

        init() {
            this.resetHeaderElement();
            this.setupVisibilityCalculatiuon();
            this.calculateVisibility();
        }
        
        refresh() {
            this.resetHeaderElement();
            this.setupVisibilityCalculatiuon();
            this.calculateVisibility();
        }

        private setupVisibilityCalculatiuon() {
            const gridWrapper = this._grid.wrapper[0];
            const gridContent = gridWrapper.querySelector(":scope > .k-grid-content");
            if (!gridWrapper || !gridContent) {
                throw new Error("Grid is not ready");
            }

            const $gridContent = $(gridContent);
            if (!$gridContent.data("GridInnerHeader")) {
                $gridContent.on("scroll", e => {
                    this.calculateVisibility();
                });

                $gridContent.data("GridInnerHeader", this);
            }
        }

        private calculateVisibility() {
            const gridWrapper = this._grid.wrapper[0];
            const gridContent = gridWrapper.querySelector(":scope > .k-grid-content");
            if (!gridWrapper || !gridContent) {
                throw new Error("Grid is not ready");
            }

            const $gridContent = $(gridContent);

            let headerVisible = false;
            let $trees = $gridContent.find(".inventory-skus-treelist");
            for (let i = 0; i < $trees.length; i++) {
                let $tree = $trees.eq(i);
                let treeOffsetTop = $tree.offset().top - $gridContent.offset().top;
                let treeHeight = $tree.height();
                if (treeOffsetTop < 0 && Math.abs(treeOffsetTop) < treeHeight) {
                    headerVisible = true;
                    console.log(`Tree ${i} Shall make header visible`);
                }
            }

            if (headerVisible) {
                this._header.classList.add("k-grid-inner-header--visible");
            } else {
                this._header.classList.remove("k-grid-inner-header--visible");
            }
        }

        private resetHeaderElement() {
            const gridWrapper = this._grid.wrapper[0];
            const gridContent = gridWrapper.querySelector(":scope > .k-grid-content");
            if (!gridWrapper || !gridContent) {
                throw new Error("Grid is not ready");
            }

            let header = gridWrapper.querySelector(".k-grid-inner-header") as HTMLDivElement;
            if (!header) {
                header = document.createElement("div");
                header.classList.add("k-grid-inner-header");
                header.classList.add("k-grid-header");
                header.classList.add("inventory-skus-treelist")
                gridContent.insertAdjacentElement("beforebegin", header);
            } else {
                header.innerHTML = ``;
            }

            this._header = header;

            const gridHeader = gridWrapper.children[0] as HTMLElement;
            header.style.paddingLeft = `${$(gridHeader.querySelector("th.k-hierarchy-cell")).outerWidth()}px`;
            header.style.paddingRight = gridHeader.style.paddingRight;

            const headerWrap = header.appendChild(document.createElement("div"));
            headerWrap.classList.add("k-grid-header-wrap");
            headerWrap.classList.add("k-auto-scrollable");
            headerWrap.style.paddingLeft = "0.25rem";
            headerWrap.style.paddingRight = "0.25rem";

            const headerTable = headerWrap.appendChild(document.createElement("table"));
            const headerColGroup = headerTable.appendChild(document.createElement("colgroup"));
            const headerTHead = headerTable.createTHead();
            const headerRow = headerTHead.insertRow();

            const addColumn = (text?: string, width?: string, colInit?: (col: HTMLTableColElement) => void, cellInit?: (th: HTMLTableHeaderCellElement) => void) => {
                const col = headerColGroup.appendChild(document.createElement("col"));
                if (width) {
                    col.style.width = width;
                }

                if (colInit) {
                    colInit(col);
                }

                const th = headerRow.appendChild(document.createElement("th"));
                th.classList.add("k-header")
                th.scope = "col";
                if (text) {
                    th.innerText = text;
                }

                if (cellInit) {
                    cellInit(th);
                }
            };

            addColumn("Name", "300px");
            addColumn("QTY", "45px");
            addColumn("Shelf", "45px");
            addColumn("Tray", "45px");
            addColumn("Ord", "45px");
            addColumn("Req", "45px");
            addColumn("Price (AUD)", "70px");
            addColumn("Total (AUD)", "70px");
            addColumn("Description", "200px");
            addColumn("Actions", "300px");
        }
    }

    export class InventoryIndexPage {
        private readonly _root: HTMLElement;
        private readonly _nestedTreeLists: Map<string, InventorySKUsTreeList>;
        private _gridTypes: kendo.ui.Grid;
        private _gridTypesSKUHeader: GridInnerHeader;
        private _filterSKUName: HTMLInputElement;
        private _filterSKUStatus: kendo.ui.DropDownList;
        private _filterWorkshop: kendo.ui.DropDownList;
        private _buttonCreateType: HTMLAnchorElement;
        private _canManage: boolean = false;
        private _canManageMovements: boolean = false;
        private _nextedTreeFilter: (item: InventorySKU) => boolean;

        private _statusFilter: InventorySKUStatusFilter = InventorySKUStatusFilter.All;
        private _workshopFilter: string = undefined;

        constructor(root: HTMLElement) {
            this._root = root;
            this._nestedTreeLists = new Map<string, InventorySKUsTreeList>();
        }

        get statusFilter(): InventorySKUStatusFilter {
            return this._statusFilter;
        }

        get workshopFilter(): string {
            return this._workshopFilter;
        }

        setCanManage(canManage: boolean) {
            this._canManage = canManage;
        }

        setCanManageMovements(canManageMovements: boolean) {
            this._canManageMovements = canManageMovements;
        }

        init() {
            // Filter
            setTimeout(() => {
                this._filterSKUStatus = $(this._root).find("input.filter-sku-status").data("kendoDropDownList");
                this._filterSKUStatus.bind("change", (e: kendo.ui.DropDownListChangeEvent) => {
                    const statusFilter = InventorySKUStatusFilter[e.sender.value()];
                    switch (statusFilter) {
                    case InventorySKUStatusFilter.All:
                    case InventorySKUStatusFilter.WarningOnly:
                        if (this._statusFilter !== statusFilter) {
                            this._statusFilter = statusFilter;
                            this._gridTypes.persistExpandedState("Id");
                            this._gridTypes.dataSource.read();
                        }
                        break;
                    default:
                        throw new Error("Invalid status filter");
                    }
                });

                this._filterSKUName = this._root.querySelector(".filter-sku-name");
                this._filterSKUName.addEventListener("input", e => {
                    const value = this._filterSKUName.value;
                    if (value === undefined || value === null || value === ``) {
                        this.filterTrees();
                    } else {
                        this.filterTrees(x => x.Name.toLowerCase().indexOf(value.toLowerCase()) >= 0);
                    }

                    this.updateQuantities();
                });

                this._filterWorkshop = $(this._root).find("input.filter-workshop").data("kendoDropDownList");
                this._filterWorkshop.bind("change", (e: kendo.ui.DropDownListChangeEvent) => {
                    const workshopFilter = e.sender.value() ? e.sender.value() : undefined;
                    if (this._workshopFilter !== workshopFilter) {
                        this._workshopFilter = workshopFilter;
                        this._gridTypes.persistExpandedState("Id");
                        this._gridTypes.dataSource.read();
                    }
                });

                // Grid
                this._gridTypes = this.createGrid();
                window.addEventListener("resize", e => {
                    this._gridTypes.setOptions({ height: "100px" });
                    this._gridTypes.resize(true);

                    this._gridTypes.setOptions({ height: "100%" });
                    this._gridTypes.resize(true);
                });
            });

            // Create Button
            this._buttonCreateType = this._root.querySelector(".inventory-type-create-action");
            GridHandlers.createGridButtonClickPopupHandler(
                ".inventory-type-create-action",
                target => new DevGuild.AspNet.Routing.Uri(`/InventoryTypes/Create`),
                target => ({
                    title: `Add SKU Type`,
                    width: `1000px`,
                    height: `auto`,
                    refresh: (e: kendo.ui.WindowEvent) => {
                        e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                            clickEvent.preventDefault();
                            e.sender.close();
                            e.sender.destroy();
                        });
                        e.sender.center();
                    },
                    open: async (e: kendo.ui.WindowEvent) => {
                        await AjaxFormsManager.waitFor("InventoryTypesCreate");
                        this._gridTypes.persistExpandedState("Id");
                        await this._gridTypes.dataSource.read();
                        e.sender.close();
                        e.sender.destroy();
                    }
                }));
        }

        private filterTrees(predicate?: (item: InventorySKU) => boolean): void {
            this._nextedTreeFilter = predicate;
            this._nestedTreeLists.forEach((v, k, m) => {
                v.filterTree(predicate);
            });
        }

        private async updateQuantities(): Promise<void> {
            const dataSource = this.createDataSource();
            await dataSource.read();
            const items = dataSource.data<InventorySKUType>();
            this._gridTypes.wrapper.find(".inventory-sku-type-quantity").each((index, element) => {
                const skuTypeId = element.getAttribute("data-sku-type-id");
                const matchingItem = items.filter(x => x.Id === skuTypeId)[0] as InventorySKUType;
                if (matchingItem) {
                    element.innerText = `${matchingItem.SKUCount}`;
                } else {
                    element.innerText = `Unknown`;
                }
            });
        }

        private createGrid(): kendo.ui.Grid {
            const dataSource = this.createDataSource();
            const gridContainer = $(this._root).find(".grid-container");
            gridContainer.addClass("k-grid--dense");
            gridContainer.kendoGrid({
                dataSource: dataSource,
                columns: this.initializeColumns(),
                dataBound: (e: kendo.ui.GridDataBoundEvent) => {
                    this._nestedTreeLists.clear();
                    this._gridTypesSKUHeader.refresh();
                },
                detailInit: (e: kendo.ui.GridDetailInitEvent) => {
                    const dataItem = e.data as (kendo.data.ObservableObject & InventorySKUType);
                    const treeContainer = $("<div></div>");
                    e.detailCell.append(treeContainer);
                    
                    const nestedTree = new InventorySKUsTreeList(this, dataItem, treeContainer, this._canManage, this._canManageMovements);
                    this._nestedTreeLists.set(dataItem.Id, nestedTree);
                    e.detailRow.data("nestedTree", nestedTree);

                    if (this._nextedTreeFilter) {
                        setTimeout(() => {
                            nestedTree.filterTree(this._nextedTreeFilter);
                        })
                    }
                },
                detailExpand: (e: kendo.ui.GridDetailExpandEvent) => {
                },
                detailCollapse: (e: kendo.ui.GridDetailCollapseEvent) => {
                },
            });

            const grid = gridContainer.data("kendoGrid");

            this._gridTypesSKUHeader = new GridInnerHeader(grid);
            this._gridTypesSKUHeader.init();
            return grid;
        }

        private createDataSource(): kendo.data.DataSource {
            const dataSource = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: `/InventoryTypes/Read`,
                        data: () => {
                            const filterObj = { };
                            switch (this._statusFilter) {
                                case InventorySKUStatusFilter.WarningOnly:
                                    filterObj["warningOnly"] = true;
                                    break;
                            }

                            if (this._filterSKUName.value) {
                                filterObj["skuName"] = this._filterSKUName.value;
                            }

                            if (this._workshopFilter) {
                                filterObj["workshop"] = this._workshopFilter;
                            }

                            return filterObj;
                        }
                    },
                },
                schema: {
                    data: "Data",
                    total: "Total",
                    errors: "Errors",
                    model: {
                        id: "Id",
                        fields: {
                            "Name": { type: "string" },
                            "OrderNo": { type: "number" },
                            "HandpieceSpeedCompatibility": { type: "number" },
                            "SKUCount": { type: "number" },
                        },
                    },
                },
                sort: [
                    { field: "OrderNo", dir: "asc" }
                ],
                serverAggregates: true,
                serverFiltering: false,
                serverGrouping: true,
                serverPaging: true,
                serverSorting: true,
            });

            return dataSource;
        }

        private initializeColumns(): kendo.ui.GridColumn[] {
            const columns: kendo.ui.GridColumn[] = [];
            columns.push({
                title: "SKU Type",
                field: "Name",
                width: "860px",
                template: (data: InventorySKUType) => {
                    const span = document.createElement("span");
                    span.appendChild(document.createTextNode(`${data.Name}: `));
                    const qtySpan = span.appendChild(document.createElement("span"));
                    qtySpan.classList.add("inventory-sku-type-quantity");
                    qtySpan.setAttribute("data-sku-type-id", data.Id);
                    qtySpan.appendChild(document.createTextNode(`${data.SKUCount}`))
                    span.appendChild(document.createTextNode(` SKUs`));
                    return span.innerHTML;
                }
            });

            if (this._canManage) {
                columns.push({
                    title: "Actions",
                    width: "300px",
                    command: [
                        {
                            name: "CustomCreateChild",
                            iconClass: "fas fa-plus",
                            text: "&nbsp;Add SKU",
                            click: GridHandlers.createButtonClickPopupHandler<InventorySKUType>(
                                item => new DevGuild.AspNet.Routing.Uri(`/Inventory/Create/?parentId=${item.Id}&hierarchyParentId=`),
                                item => ({
                                    title: `Add SKU of type ${item.Name}`,
                                    width: `800px`,
                                    height: `auto`,
                                    refresh: (e: kendo.ui.WindowEvent) => {
                                        e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                            clickEvent.preventDefault();
                                            e.sender.close();
                                            e.sender.destroy();
                                        });
                                        e.sender.center();
                                    },
                                    open: async (e: kendo.ui.WindowEvent) => {
                                        await AjaxFormsManager.waitFor("InventoryCreate");

                                        const treeList = this._nestedTreeLists.get(item.Id);
                                        if (treeList) {
                                            await treeList.refresh();
                                        }
                                        
                                        e.sender.close();
                                        e.sender.destroy();
                                    }
                                })),
                        },
                        {
                            name: "CustomEdit",
                            iconClass: "fas fa-pencil-alt",
                            text: "&nbsp;",
                            click: GridHandlers.createButtonClickPopupHandler<InventorySKUType>(
                                item => new DevGuild.AspNet.Routing.Uri(`/InventoryTypes/Edit/${item.Id}`),
                                item => ({
                                    title: `Edit SKU Type ${item.Name}`,
                                    width: `1000px`,
                                    height: `auto`,
                                    refresh: (e: kendo.ui.WindowEvent) => {
                                        e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                            clickEvent.preventDefault();
                                            e.sender.close();
                                            e.sender.destroy();
                                        });
                                        e.sender.center();
                                    },
                                    open: async (e: kendo.ui.WindowEvent) => {
                                        await AjaxFormsManager.waitFor("InventoryTypesEdit");
                                        this._gridTypes.persistExpandedState("Id");
                                        await this._gridTypes.dataSource.read();
                                        e.sender.close();
                                        e.sender.destroy();
                                    }
                                })),
                        },
                        {
                            name: "CustomMoveUp",
                            iconClass: "fas fa-arrow-up",
                            text: "&nbsp;",
                            click: async (e: JQueryEventObject) => {
                                e.preventDefault();
                                const dataItem = this._gridTypes.dataItem<InventorySKUType>(e.currentTarget.closest("tr"));
                                await fetch(`/InventoryTypes/MoveUp/${dataItem.Id}`, {
                                    method: "POST",
                                    credentials: "same-origin",
                                    headers: {
                                        "X-Requested-With": "XMLHttpRequest"
                                    },
                                    body: ""
                                });
                                this._gridTypes.persistExpandedState("Id");
                                await this._gridTypes.dataSource.read();
                            }
                        },
                        {
                            name: "CustomMoveDown",
                            iconClass: "fas fa-arrow-down",
                            text: "&nbsp;",
                            click: async (e: JQueryEventObject) => {
                                e.preventDefault();
                                const dataItem = this._gridTypes.dataItem<InventorySKUType>(e.currentTarget.closest("tr"));
                                await fetch(`/InventoryTypes/MoveDown/${dataItem.Id}`, {
                                    method: "POST",
                                    credentials: "same-origin",
                                    headers: {
                                        "X-Requested-With": "XMLHttpRequest"
                                    },
                                    body: ""
                                });
                                this._gridTypes.persistExpandedState("Id");
                                await this._gridTypes.dataSource.read();
                            }
                        },
                        {
                            name: "CustomSort",
                            iconClass: "fas fa-sort-alpha-down",
                            text: "&nbsp;",
                            click: GridHandlers.createButtonClickPopupHandler<InventorySKUType>(
                                item => new DevGuild.AspNet.Routing.Uri(`/InventoryTypes/Sort/${item.Id}`),
                                item => ({
                                    title: `Sort SKU Type ${item.Name}`,
                                    width: `1000px`,
                                    height: `auto`,
                                    refresh: (e: kendo.ui.WindowEvent) => {
                                        e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                            clickEvent.preventDefault();
                                            e.sender.close();
                                            e.sender.destroy();
                                        });
                                        e.sender.center();
                                    },
                                    open: async (e: kendo.ui.WindowEvent) => {
                                        await AjaxFormsManager.waitFor("InventoryTypesSort");
                                        this._gridTypes.persistExpandedState("Id");
                                        await this._gridTypes.dataSource.read();
                                        e.sender.close();
                                        e.sender.destroy();
                                    }
                                })),
                        },
                        {
                            name: "CustomDelete",
                            iconClass: "fas fa-trash-alt",
                            text: "&nbsp;",
                            click: GridHandlers.createButtonClickPopupHandler<InventorySKUType>(
                                item => new DevGuild.AspNet.Routing.Uri(`/InventoryTypes/Delete/${item.Id}`),
                                item => ({
                                    title: `Delete SKU Type ${item.Name}`,
                                    width: `1000px`,
                                    height: `auto`,
                                    refresh: (e: kendo.ui.WindowEvent) => {
                                        e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                            clickEvent.preventDefault();
                                            e.sender.close();
                                            e.sender.destroy();
                                        });
                                        e.sender.center();
                                    },
                                    open: async (e: kendo.ui.WindowEvent) => {
                                        await AjaxFormsManager.waitFor("InventoryTypesDelete");
                                        this._gridTypes.persistExpandedState("Id");
                                        await this._gridTypes.dataSource.read();
                                        e.sender.close();
                                        e.sender.destroy();
                                    }
                                })),
                        },
                    ]
                });
            }

            return columns;
        }
    }

    export class InventorySKUsTreeList {
        private readonly _page: InventoryIndexPage;
        private readonly _skuType: InventorySKUType;
        private readonly _root: JQuery<HTMLElement>;
        private readonly _canManage: boolean;
        private readonly _canManageMovements: boolean;

        private readonly _dataSource: kendo.data.DataSource;
        private readonly _treeList: kendo.ui.TreeList;

        constructor(page: InventoryIndexPage, skuType: InventorySKUType, root: JQuery<HTMLElement>, canManage: boolean, canManageMovements: boolean) {
            this._page = page;
            this._skuType = skuType;
            this._root = root;
            this._canManage = canManage;
            this._canManageMovements = canManageMovements;
            
            this._dataSource = this.createDataSource();
            this._treeList = this.initTree();
        }

        async refresh(): Promise<void> {
            this._treeList.persistExpandedState("Id");
            await this._treeList.dataSource.read();
        }

        private initTree(): kendo.ui.TreeList {
            this._root.addClass("inventory-skus-treelist");
            this._root.addClass("k-treelist--dense");

            this._root.kendoTreeList({
                columns: this.initializeColumns(),
                dataSource: this._dataSource,
                pageable: false,
                scrollable: false,
                dataBound: (e: kendo.ui.TreeListDataBoundEvent) => {
                    const treeList = e.sender;
                    treeList.tbody.find("tr[role='row']").each((index, element) => {
                        const node = $(element);
                        const model = treeList.dataItem(node) as kendo.data.TreeListModel & InventorySKU;
                        if (model.NodeType === InventorySKUNodeType.Leaf) {
                            node.find(".k-button[data-command=customcreatechild]").addClass("k-state-disabled").prop("disabled", true);
                            node.find(".k-button[data-command=customcreatemovement").removeClass("k-state-disabled").prop("disabled", false);
                        } else {
                            node.find(".k-button[data-command=customcreatechild]").removeClass("k-state-disabled").prop("disabled", false);
                            node.find(".k-button[data-command=customcreatemovement").addClass("k-state-disabled").prop("disabled", true);
                        }

                        if (model.IsDefaultChild) {
                            node.find("[data-column=Name]").addClass("font-weight-bold");
                        } else {
                            node.find("[data-column=Name]").removeClass("font-weight-bold");
                        }
                    });

                    treeList.tbody.find(".inventory-movements-link").each((index, element) => {
                        const existing = $(element).data("kendoTooltip") as kendo.ui.Tooltip;
                        if (existing) {
                            existing.destroy();
                        }

                        const sku = element.getAttribute("data-sku");
                        const tab = element.getAttribute("data-tab");
                        $(element).kendoTooltip({
                            position: "top",
                            content: {
                                url: `/InventoryMovements/Preview?sku=${sku}&tab=${tab}`,
                            },
                            width: 950,
                            height: 300,
                        });
                    })

                    treeList.tbody.find(".inventory-sku-name").each((index, element) => {
                        const existing = $(element).data("kendoTooltip") as kendo.ui.Tooltip;
                        if (existing) {
                            existing.destroy();
                        }

                        $(element).kendoTooltip();
                    });
                },
            });

            const treeList = this._root.data("kendoTreeList");
            treeList["_button"] = (command) => {
                var icon = [];
                if (command.imageClass) {
                    icon.push(kendo.dom.element('span', {
                        className: command.imageClass
                    }));
                }
                return kendo.dom.element('button', {
                    'type': 'button',
                    'data-command': command.name.toLowerCase(),
                    className: [
                        'k-button',
                        'k-button-icontext',
                        command.className
                    ].join(' '),
                    'disabled': command.className && command.className.indexOf('disabled') >= 0 ? 'disabled' : undefined,
                }, icon.concat([
                    kendo.dom.html("&nbsp;"),
                    kendo.dom.text(command.text)
                ])); 
            }


            return treeList;
        }

        private createDataSource(): kendo.data.TreeListDataSource {
            const dataSource = new kendo.data.TreeListDataSource ({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: `/Inventory/Read?parentId=${this._skuType.Id}`,
                        data: () => {
                            const filterObj = { };
                            switch (this._page.statusFilter) {
                                case InventorySKUStatusFilter.WarningOnly:
                                    filterObj["warningOnly"] = true;
                                    break;
                            }

                            if (this._page.workshopFilter) {
                                filterObj["workshop"] = this._page.workshopFilter;
                            }

                            return filterObj;
                        }
                    },
                },
                schema: {
                    data: "Data",
                    total: "Total",
                    errors: "Errors",
                    model: {
                        id: "Id",
                        parentId: "ParentId",
                        fields: {
                            "TypeId": { type: "string" },
                            "ParentId": { type: "string", nullable: true },
                            "OrderNo": { type: "number" },
                            "Name": { type: "string" },
                            "TotalQuantity": { type: "number" },
                            "ShelfQuantity": { type: "number" },
                            "TrayQuantity": { type: "number" },
                            "OrderedQuantity": { type: "number" },
                            "RequestedQuantity": { type: "number" },
                            "AveragePrice": { type: "number" },
                            "TotalPrice": { type: "number" },
                            "Description": { type: "string" },
                            "NodeType": { type: "number" },
                        },
                    },
                },
                sort: [
                    { field: "OrderNo", dir: "asc" }
                ]
            });

            return dataSource;
        }

        filterTree(predicate?: (item: InventorySKU) => boolean) {
            if (predicate === undefined || predicate === null) {
                this._dataSource.filter([]);
                return;
            }

            if (typeof predicate !== "function") {
                throw new Error("Invalid predicate");
            }

            const dataSource = this._dataSource;
            const recursivePredicate: (item: InventorySKU) => boolean = item => {
                if (predicate(item)) {
                    return true;
                }

                const children = dataSource.data<InventorySKU>().filter(x => x.ParentId === item.Id) as InventorySKU[];
                for (let child of children) {
                    if (recursivePredicate(child)) {
                        return true;
                    }
                }

                return false;
            }

            dataSource.filter([{ operator: recursivePredicate }]);
        }

        private initializeColumns(): kendo.ui.TreeListColumn[] {
            const columns: kendo.ui.TreeListColumn[] = [];
            columns.push({
                title: "Name",
                field: "Name",
                width: "300px",
                attributes: {
                    "data-column": "Name"
                },
                template: (item: InventorySKU) => {
                    const container = document.createElement("span");
                    if (item.HasWarning) {
                        const icon = document.createElement("span");
                        icon.classList.add("fas");
                        icon.classList.add("fa-exclamation-triangle");
                        icon.classList.add("text-warning");
                        container.appendChild(icon);
                        container.appendChild(document.createTextNode("\u00a0"));
                    } else if (item.HasDescendantsWithWarning) {
                        const icon = document.createElement("span");
                        icon.classList.add("fas");
                        icon.classList.add("fa-exclamation-triangle");
                        icon.classList.add("text-muted");
                        container.appendChild(icon);
                        container.appendChild(document.createTextNode("\u00a0"));
                    }

                    const nameSpan = container.appendChild(document.createElement("span"));
                    nameSpan.classList.add("inventory-sku-name");
                    nameSpan.setAttribute("title", item.Name);
                    nameSpan.appendChild(document.createTextNode(item.Name));

                    return container.innerHTML;
                },
            });
            columns.push({
                title: "QTY",
                field: "TotalQuantity",
                width: "45px",
            });
            columns.push({
                title: "Shelf",
                field: "ShelfQuantity",
                width: "45px",
            });
            columns.push({
                title: "Tray",
                field: "TrayQuantity",
                width: "45px",
                template: (item: InventorySKU) => {
                    const link = document.createElement("a");
                    link.classList.add("inventory-movements-link");
                    link.setAttribute("data-sku", item.Id);
                    link.setAttribute("data-tab", "Tray");
                    link.style.color = `#007bff`;
                    link.href = `/InventoryMovements?sku=${item.Id}&tab=Tray&group=false`;
                    link.target = `__blank`;
                    link.appendChild(document.createTextNode(kendo.toString(item.TrayQuantity, "#,##0.##")));
                    return link.outerHTML;
                },
            });
            columns.push({
                title: "Ord",
                field: "OrderedQuantity",
                width: "45px",
                template: (item: InventorySKU) => {
                    const link = document.createElement("a");
                    link.classList.add("inventory-movements-link");
                    link.setAttribute("data-sku", item.Id);
                    link.setAttribute("data-tab", "Ordered");
                    link.style.color = `#007bff`;
                    link.href = `/InventoryMovements?sku=${item.Id}&tab=Ordered&group=false`;
                    link.target = `__blank`;
                    link.appendChild(document.createTextNode(kendo.toString(item.OrderedQuantity, "#,##0.##")));
                    return link.outerHTML;
                },
            });
            columns.push({
                title: "Req",
                field: "RequestedQuantity",
                width: "45px",
                template: (item: InventorySKU) => {
                    const link = document.createElement("a");
                    link.classList.add("inventory-movements-link");
                    link.setAttribute("data-sku", item.Id);
                    link.setAttribute("data-tab", "ApprovedAndRequested");
                    link.style.color = `#007bff`;
                    link.href = `/InventoryMovements?sku=${item.Id}&tab=Approved&group=false`;
                    link.target = `__blank`;
                    link.appendChild(document.createTextNode(kendo.toString(item.RequestedQuantity, "#,##0.##")));
                    return link.outerHTML;
                },
            });
            columns.push({
                title: "Price (AUD)",
                field: "AveragePrice",
                width: "70px",
                template: (item: InventorySKU) => {
                    if (item.AveragePrice !== undefined && item.AveragePrice !== null) {
                        return `$${item.AveragePrice}`;
                    } else {
                        return ``;
                    }
                },
            });
            columns.push({
                title: "Total (AUD)",
                field: "TotalPrice",
                width: "70px",
                template: (item: InventorySKU) => {
                    if (item.TotalPrice !== undefined && item.TotalPrice !== null) {
                        return `$${item.TotalPrice}`;
                    } else {
                        return ``;
                    }
                },
            });
            columns.push({
                title: "Description",
                field: "Description",
                width: "200px",
            });

            if (this._canManage) {
                columns.push({
                    title: "Actions",
                    width: "300px",
                    command: [
                        {
                            name: "CustomCreateChild",
                            text: " Add SKU",
                            imageClass: "fas fa-plus",
                            click: GridHandlers.createButtonClickPopupHandler<InventorySKU>(
                                item => new DevGuild.AspNet.Routing.Uri(`/Inventory/Create/?parentId=${item.TypeId}&hierarchyParentId=${item.Id}`),
                                item => ({
                                    title: `Add SKU to group ${item.Name}`,
                                    width: `800px`,
                                    height: `auto`,
                                    refresh: (e: kendo.ui.WindowEvent) => {
                                        e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                            clickEvent.preventDefault();
                                            e.sender.close();
                                            e.sender.destroy();
                                        });
                                        e.sender.center();
                                    },
                                    open: async (e: kendo.ui.WindowEvent) => {
                                        await AjaxFormsManager.waitFor("InventoryCreate");
                                        await this.refresh();
                                        e.sender.close();
                                        e.sender.destroy();
                                    }
                                })),
                        },
                        {
                            name: "CustomCreateMovement",
                            text: " Move",
                            imageClass: "fas fa-exchange-alt",
                            click: GridHandlers.createButtonClickPopupHandler<InventorySKU>(
                                item => this._page.workshopFilter
                                    ? new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/CreateForSKU?workshop=${this._page.workshopFilter}&sku=${item.Id}`)
                                    : new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/CreateForSKU?sku=${item.Id}`),
                                item => ({
                                    title: `Move SKU ${item.Name}`,
                                    width: `800px`,
                                    height: `auto`,
                                    refresh: (e: kendo.ui.WindowEvent) => {
                                        e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                            clickEvent.preventDefault();
                                            e.sender.close();
                                            e.sender.destroy();
                                        });
                                        e.sender.center();
                                    },
                                    open: async (e: kendo.ui.WindowEvent) => {
                                        await AjaxFormsManager.waitFor("InventoryMovementsCreateForSKU");
                                        await this.refresh();
                                        e.sender.close();
                                        e.sender.destroy();
                                    }
                                })),
                        },
                        {
                            name: "CustomEdit",
                            text: "",
                            imageClass: "fas fa-pencil-alt",
                            click: GridHandlers.createButtonClickPopupHandler<InventorySKU>(
                                item => {
                                    switch (item.NodeType) {
                                    case InventorySKUNodeType.Leaf:
                                        return new DevGuild.AspNet.Routing.Uri(`/Inventory/Edit/${item.Id}`);
                                    case InventorySKUNodeType.Group:
                                        return new DevGuild.AspNet.Routing.Uri(`/Inventory/EditGroup/${item.Id}`);
                                    default:
                                        throw new Error("Invalid SKU node type");
                                    }
                                },
                                item => {
                                    switch (item.NodeType) {
                                    case InventorySKUNodeType.Leaf:
                                        return {
                                            title: `Edit SKU ${item.Name}`,
                                            width: `800px`,
                                            height: `auto`,
                                            refresh: (e: kendo.ui.WindowEvent) => {
                                                e.sender.wrapper.on("click", ".inventory-edit__convert-to-group", clickEvent => {
                                                    clickEvent.preventDefault();
                                                    e.sender.close();
                                                    e.sender.destroy();
                                                    this.openConvertToGroup(item);
                                                });
                                                e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                                    clickEvent.preventDefault();
                                                    e.sender.close();
                                                    e.sender.destroy();
                                                });
                                                e.sender.center();
                                            },
                                            open: async (e: kendo.ui.WindowEvent) => {
                                                await AjaxFormsManager.waitFor("InventoryEdit");
                                                await this.refresh();
                                                e.sender.close();
                                                e.sender.destroy();
                                            }
                                        };
                                    case InventorySKUNodeType.Group:
                                        return {
                                            title: `Edit SKU Group ${item.Name}`,
                                            width: `800px`,
                                            height: `auto`,
                                            refresh: (e: kendo.ui.WindowEvent) => {
                                                e.sender.wrapper.on("click", ".inventory-edit__convert-to-leaf", clickEvent => {
                                                    clickEvent.preventDefault();
                                                    e.sender.close();
                                                    e.sender.destroy();
                                                    this.openConvertToLeaf(item);
                                                });
                                                e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                                    clickEvent.preventDefault();
                                                    e.sender.close();
                                                    e.sender.destroy();
                                                });
                                                e.sender.center();
                                            },
                                            open: async (e: kendo.ui.WindowEvent) => {
                                                await AjaxFormsManager.waitFor("InventoryEditGroup");
                                                await this.refresh();
                                                e.sender.close();
                                                e.sender.destroy();
                                            }
                                        };
                                    default:
                                        throw new Error("Invalid SKU node type");
                                    }

                                }),
                        },
                        {
                            name: "CustomMoveUp",
                            text: "",
                            imageClass: "fas fa-arrow-up",
                            click: async (e: JQueryEventObject) => {
                                e.preventDefault();
                                const dataItem = this._treeList.dataItem(e.currentTarget.closest("tr")) as kendo.data.TreeListModel & InventorySKUType;
                                await fetch(`/Inventory/MoveUp/${dataItem.Id}`, {
                                    method: "POST",
                                    credentials: "same-origin",
                                    headers: {
                                        "X-Requested-With": "XMLHttpRequest"
                                    },
                                    body: ""
                                });
                                await this.refresh();
                            }
                        },
                        {
                            name: "CustomMoveDown",
                            text: "",
                            imageClass: "fas fa-arrow-down",
                            click: async (e: JQueryEventObject) => {
                                e.preventDefault();
                                const dataItem = this._treeList.dataItem(e.currentTarget.closest("tr")) as kendo.data.TreeListModel & InventorySKUType;
                                await fetch(`/Inventory/MoveDown/${dataItem.Id}`, {
                                    method: "POST",
                                    credentials: "same-origin",
                                    headers: {
                                        "X-Requested-With": "XMLHttpRequest"
                                    },
                                    body: ""
                                });
                                await this.refresh();
                            }
                        },
                        {
                            name: "CustomDelete",
                            text: "",
                            imageClass: "fas fa-trash-alt",
                            click: GridHandlers.createButtonClickPopupHandler<InventorySKU>(
                                item => new DevGuild.AspNet.Routing.Uri(`/Inventory/Delete/${item.Id}`),
                                item => ({
                                    title: `Delete SKU ${item.Name}`,
                                    width: `800px`,
                                    height: `auto`,
                                    refresh: (e: kendo.ui.WindowEvent) => {
                                        e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                            clickEvent.preventDefault();
                                            e.sender.close();
                                            e.sender.destroy();
                                        });
                                        e.sender.center();
                                    },
                                    open: async (e: kendo.ui.WindowEvent) => {
                                        await AjaxFormsManager.waitFor("InventoryDelete");
                                        await this.refresh();
                                        e.sender.close();
                                        e.sender.destroy();
                                    }
                                })),
                        },
                    ]
                });
            } else if (this._canManageMovements) {
                columns.push({
                    title: "Actions",
                    width: "300px",
                    command: [
                        {
                            name: "CustomCreateMovement",
                            text: " Move",
                            imageClass: "fas fa-exchange-alt",
                            click: GridHandlers.createButtonClickPopupHandler<InventorySKU>(
                                item => new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/CreateForSKU?sku=${item.Id}`),
                                item => ({
                                    title: `Move SKU ${item.Name}`,
                                    width: `800px`,
                                    height: `auto`,
                                    refresh: (e: kendo.ui.WindowEvent) => {
                                        e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                            clickEvent.preventDefault();
                                            e.sender.close();
                                            e.sender.destroy();
                                        });
                                        e.sender.center();
                                    },
                                    open: async (e: kendo.ui.WindowEvent) => {
                                        await AjaxFormsManager.waitFor("InventoryMovementsCreateForSKU");
                                        await this.refresh();
                                        e.sender.close();
                                        e.sender.destroy();
                                    }
                                })),
                        },
                    ]
                });
            }

            return columns;
        }

        private openConvertToGroup(item: InventorySKU) : void {
            const route = new DevGuild.AspNet.Routing.Uri(`Inventory/ConvertToGroup/${item.Id}`);
            const dialogRoot = $("<div></div>");
            const dialogOptions = {
                title: `Convert SKU ${item.Name} to group`,
                actions: ["close"],
                content: route.value,
                width: "800px",
                height: "auto",
                modal: true,
                visible: false,
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("InventoryConvertToGroup");
                    await this.refresh();
                    e.sender.close();
                    e.sender.destroy();
                },
                close: (e: kendo.ui.WindowCloseEvent) => {
                    e.sender.destroy();
                }
            };

            dialogRoot.kendoWindow(dialogOptions);

            const dialog = dialogRoot.data("kendoWindow");
            dialog.center();
            dialog.open();

            $(window).on("resize", e => {
                if (!(dialog.element.closest("html").length === 0 || dialog.element.is(":hidden"))) {
                    dialog.center();
                }
            });
        }

        private openConvertToLeaf(item: InventorySKU) : void {
            const route = new DevGuild.AspNet.Routing.Uri(`Inventory/ConvertToLeaf/${item.Id}`);
            const dialogRoot = $("<div></div>");
            const dialogOptions = {
                title: `Convert SKU ${item.Name} to leaf`,
                actions: ["close"],
                content: route.value,
                width: "800px",
                height: "auto",
                modal: true,
                visible: false,
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("InventoryConvertToLeaf");
                    await this.refresh();
                    e.sender.close();
                    e.sender.destroy();
                },
                close: (e: kendo.ui.WindowCloseEvent) => {
                    e.sender.destroy();
                }
            };

            dialogRoot.kendoWindow(dialogOptions);

            const dialog = dialogRoot.data("kendoWindow");
            dialog.center();
            dialog.open();

            $(window).on("resize", e => {
                if (!(dialog.element.closest("html").length === 0 || dialog.element.is(":hidden"))) {
                    dialog.center();
                }
            });
        }
    }
}