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
        var Pages;
        (function (Pages) {
            var Inventory;
            (function (Inventory) {
                var Index;
                (function (Index) {
                    var GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
                    var AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;
                    var HandpieceSpeedCompatibility;
                    (function (HandpieceSpeedCompatibility) {
                        HandpieceSpeedCompatibility[HandpieceSpeedCompatibility["None"] = 0] = "None";
                        HandpieceSpeedCompatibility[HandpieceSpeedCompatibility["Other"] = 1] = "Other";
                        HandpieceSpeedCompatibility[HandpieceSpeedCompatibility["LowSpeed"] = 2] = "LowSpeed";
                        HandpieceSpeedCompatibility[HandpieceSpeedCompatibility["HighSpeed"] = 4] = "HighSpeed";
                        HandpieceSpeedCompatibility[HandpieceSpeedCompatibility["All"] = 7] = "All";
                    })(HandpieceSpeedCompatibility || (HandpieceSpeedCompatibility = {}));
                    var InventorySKUNodeType;
                    (function (InventorySKUNodeType) {
                        InventorySKUNodeType[InventorySKUNodeType["Leaf"] = 0] = "Leaf";
                        InventorySKUNodeType[InventorySKUNodeType["Group"] = 1] = "Group";
                    })(InventorySKUNodeType || (InventorySKUNodeType = {}));
                    var InventorySKUStatusFilter;
                    (function (InventorySKUStatusFilter) {
                        InventorySKUStatusFilter[InventorySKUStatusFilter["All"] = 0] = "All";
                        InventorySKUStatusFilter[InventorySKUStatusFilter["WarningOnly"] = 1] = "WarningOnly";
                    })(InventorySKUStatusFilter || (InventorySKUStatusFilter = {}));
                    var GridInnerHeader = /** @class */ (function () {
                        function GridInnerHeader(grid) {
                            this._grid = grid;
                        }
                        GridInnerHeader.prototype.init = function () {
                            this.resetHeaderElement();
                            this.setupVisibilityCalculatiuon();
                            this.calculateVisibility();
                        };
                        GridInnerHeader.prototype.refresh = function () {
                            this.resetHeaderElement();
                            this.setupVisibilityCalculatiuon();
                            this.calculateVisibility();
                        };
                        GridInnerHeader.prototype.setupVisibilityCalculatiuon = function () {
                            var _this = this;
                            var gridWrapper = this._grid.wrapper[0];
                            var gridContent = gridWrapper.querySelector(":scope > .k-grid-content");
                            if (!gridWrapper || !gridContent) {
                                throw new Error("Grid is not ready");
                            }
                            var $gridContent = $(gridContent);
                            if (!$gridContent.data("GridInnerHeader")) {
                                $gridContent.on("scroll", function (e) {
                                    _this.calculateVisibility();
                                });
                                $gridContent.data("GridInnerHeader", this);
                            }
                        };
                        GridInnerHeader.prototype.calculateVisibility = function () {
                            var gridWrapper = this._grid.wrapper[0];
                            var gridContent = gridWrapper.querySelector(":scope > .k-grid-content");
                            if (!gridWrapper || !gridContent) {
                                throw new Error("Grid is not ready");
                            }
                            var $gridContent = $(gridContent);
                            var headerVisible = false;
                            var $trees = $gridContent.find(".inventory-skus-treelist");
                            for (var i = 0; i < $trees.length; i++) {
                                var $tree = $trees.eq(i);
                                var treeOffsetTop = $tree.offset().top - $gridContent.offset().top;
                                var treeHeight = $tree.height();
                                if (treeOffsetTop < 0 && Math.abs(treeOffsetTop) < treeHeight) {
                                    headerVisible = true;
                                    console.log("Tree " + i + " Shall make header visible");
                                }
                            }
                            if (headerVisible) {
                                this._header.classList.add("k-grid-inner-header--visible");
                            }
                            else {
                                this._header.classList.remove("k-grid-inner-header--visible");
                            }
                        };
                        GridInnerHeader.prototype.resetHeaderElement = function () {
                            var gridWrapper = this._grid.wrapper[0];
                            var gridContent = gridWrapper.querySelector(":scope > .k-grid-content");
                            if (!gridWrapper || !gridContent) {
                                throw new Error("Grid is not ready");
                            }
                            var header = gridWrapper.querySelector(".k-grid-inner-header");
                            if (!header) {
                                header = document.createElement("div");
                                header.classList.add("k-grid-inner-header");
                                header.classList.add("k-grid-header");
                                header.classList.add("inventory-skus-treelist");
                                gridContent.insertAdjacentElement("beforebegin", header);
                            }
                            else {
                                header.innerHTML = "";
                            }
                            this._header = header;
                            var gridHeader = gridWrapper.children[0];
                            header.style.paddingLeft = $(gridHeader.querySelector("th.k-hierarchy-cell")).outerWidth() + "px";
                            header.style.paddingRight = gridHeader.style.paddingRight;
                            var headerWrap = header.appendChild(document.createElement("div"));
                            headerWrap.classList.add("k-grid-header-wrap");
                            headerWrap.classList.add("k-auto-scrollable");
                            headerWrap.style.paddingLeft = "0.25rem";
                            headerWrap.style.paddingRight = "0.25rem";
                            var headerTable = headerWrap.appendChild(document.createElement("table"));
                            var headerColGroup = headerTable.appendChild(document.createElement("colgroup"));
                            var headerTHead = headerTable.createTHead();
                            var headerRow = headerTHead.insertRow();
                            var addColumn = function (text, width, colInit, cellInit) {
                                var col = headerColGroup.appendChild(document.createElement("col"));
                                if (width) {
                                    col.style.width = width;
                                }
                                if (colInit) {
                                    colInit(col);
                                }
                                var th = headerRow.appendChild(document.createElement("th"));
                                th.classList.add("k-header");
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
                        };
                        return GridInnerHeader;
                    }());
                    Index.GridInnerHeader = GridInnerHeader;
                    var InventoryIndexPage = /** @class */ (function () {
                        function InventoryIndexPage(root) {
                            this._canManage = false;
                            this._canManageMovements = false;
                            this._statusFilter = InventorySKUStatusFilter.All;
                            this._workshopFilter = undefined;
                            this._root = root;
                            this._nestedTreeLists = new Map();
                        }
                        Object.defineProperty(InventoryIndexPage.prototype, "statusFilter", {
                            get: function () {
                                return this._statusFilter;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryIndexPage.prototype, "workshopFilter", {
                            get: function () {
                                return this._workshopFilter;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        InventoryIndexPage.prototype.setCanManage = function (canManage) {
                            this._canManage = canManage;
                        };
                        InventoryIndexPage.prototype.setCanManageMovements = function (canManageMovements) {
                            this._canManageMovements = canManageMovements;
                        };
                        InventoryIndexPage.prototype.init = function () {
                            var _this = this;
                            // Filter
                            setTimeout(function () {
                                _this._filterSKUStatus = $(_this._root).find("input.filter-sku-status").data("kendoDropDownList");
                                _this._filterSKUStatus.bind("change", function (e) {
                                    var statusFilter = InventorySKUStatusFilter[e.sender.value()];
                                    switch (statusFilter) {
                                        case InventorySKUStatusFilter.All:
                                        case InventorySKUStatusFilter.WarningOnly:
                                            if (_this._statusFilter !== statusFilter) {
                                                _this._statusFilter = statusFilter;
                                                _this._gridTypes.persistExpandedState("Id");
                                                _this._gridTypes.dataSource.read();
                                            }
                                            break;
                                        default:
                                            throw new Error("Invalid status filter");
                                    }
                                });
                                _this._filterSKUName = _this._root.querySelector(".filter-sku-name");
                                _this._filterSKUName.addEventListener("input", function (e) {
                                    var value = _this._filterSKUName.value;
                                    if (value === undefined || value === null || value === "") {
                                        _this.filterTrees();
                                    }
                                    else {
                                        _this.filterTrees(function (x) { return x.Name.toLowerCase().indexOf(value.toLowerCase()) >= 0; });
                                    }
                                    _this.updateQuantities();
                                });
                                _this._filterWorkshop = $(_this._root).find("input.filter-workshop").data("kendoDropDownList");
                                _this._filterWorkshop.bind("change", function (e) {
                                    var workshopFilter = e.sender.value() ? e.sender.value() : undefined;
                                    if (_this._workshopFilter !== workshopFilter) {
                                        _this._workshopFilter = workshopFilter;
                                        _this._gridTypes.persistExpandedState("Id");
                                        _this._gridTypes.dataSource.read();
                                    }
                                });
                                // Grid
                                _this._gridTypes = _this.createGrid();
                                window.addEventListener("resize", function (e) {
                                    _this._gridTypes.setOptions({ height: "100px" });
                                    _this._gridTypes.resize(true);
                                    _this._gridTypes.setOptions({ height: "100%" });
                                    _this._gridTypes.resize(true);
                                });
                            });
                            // Create Button
                            this._buttonCreateType = this._root.querySelector(".inventory-type-create-action");
                            GridHandlers.createGridButtonClickPopupHandler(".inventory-type-create-action", function (target) { return new DevGuild.AspNet.Routing.Uri("/InventoryTypes/Create"); }, function (target) { return ({
                                title: "Add SKU Type",
                                width: "1000px",
                                height: "auto",
                                refresh: function (e) {
                                    e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                        clickEvent.preventDefault();
                                        e.sender.close();
                                        e.sender.destroy();
                                    });
                                    e.sender.center();
                                },
                                open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                    return __generator(this, function (_a) {
                                        switch (_a.label) {
                                            case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryTypesCreate")];
                                            case 1:
                                                _a.sent();
                                                this._gridTypes.persistExpandedState("Id");
                                                return [4 /*yield*/, this._gridTypes.dataSource.read()];
                                            case 2:
                                                _a.sent();
                                                e.sender.close();
                                                e.sender.destroy();
                                                return [2 /*return*/];
                                        }
                                    });
                                }); }
                            }); });
                        };
                        InventoryIndexPage.prototype.filterTrees = function (predicate) {
                            this._nextedTreeFilter = predicate;
                            this._nestedTreeLists.forEach(function (v, k, m) {
                                v.filterTree(predicate);
                            });
                        };
                        InventoryIndexPage.prototype.updateQuantities = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                var dataSource, items;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            dataSource = this.createDataSource();
                                            return [4 /*yield*/, dataSource.read()];
                                        case 1:
                                            _a.sent();
                                            items = dataSource.data();
                                            this._gridTypes.wrapper.find(".inventory-sku-type-quantity").each(function (index, element) {
                                                var skuTypeId = element.getAttribute("data-sku-type-id");
                                                var matchingItem = items.filter(function (x) { return x.Id === skuTypeId; })[0];
                                                if (matchingItem) {
                                                    element.innerText = "" + matchingItem.SKUCount;
                                                }
                                                else {
                                                    element.innerText = "Unknown";
                                                }
                                            });
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        InventoryIndexPage.prototype.createGrid = function () {
                            var _this = this;
                            var dataSource = this.createDataSource();
                            var gridContainer = $(this._root).find(".grid-container");
                            gridContainer.addClass("k-grid--dense");
                            gridContainer.kendoGrid({
                                dataSource: dataSource,
                                columns: this.initializeColumns(),
                                dataBound: function (e) {
                                    _this._nestedTreeLists.clear();
                                    _this._gridTypesSKUHeader.refresh();
                                },
                                detailInit: function (e) {
                                    var dataItem = e.data;
                                    var treeContainer = $("<div></div>");
                                    e.detailCell.append(treeContainer);
                                    var nestedTree = new InventorySKUsTreeList(_this, dataItem, treeContainer, _this._canManage, _this._canManageMovements);
                                    _this._nestedTreeLists.set(dataItem.Id, nestedTree);
                                    e.detailRow.data("nestedTree", nestedTree);
                                    if (_this._nextedTreeFilter) {
                                        setTimeout(function () {
                                            nestedTree.filterTree(_this._nextedTreeFilter);
                                        });
                                    }
                                },
                                detailExpand: function (e) {
                                },
                                detailCollapse: function (e) {
                                },
                            });
                            var grid = gridContainer.data("kendoGrid");
                            this._gridTypesSKUHeader = new GridInnerHeader(grid);
                            this._gridTypesSKUHeader.init();
                            return grid;
                        };
                        InventoryIndexPage.prototype.createDataSource = function () {
                            var _this = this;
                            var dataSource = new kendo.data.DataSource({
                                type: "aspnetmvc-ajax",
                                transport: {
                                    read: {
                                        url: "/InventoryTypes/Read",
                                        data: function () {
                                            var filterObj = {};
                                            switch (_this._statusFilter) {
                                                case InventorySKUStatusFilter.WarningOnly:
                                                    filterObj["warningOnly"] = true;
                                                    break;
                                            }
                                            if (_this._filterSKUName.value) {
                                                filterObj["skuName"] = _this._filterSKUName.value;
                                            }
                                            if (_this._workshopFilter) {
                                                filterObj["workshop"] = _this._workshopFilter;
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
                        };
                        InventoryIndexPage.prototype.initializeColumns = function () {
                            var _this = this;
                            var columns = [];
                            columns.push({
                                title: "SKU Type",
                                field: "Name",
                                width: "860px",
                                template: function (data) {
                                    var span = document.createElement("span");
                                    span.appendChild(document.createTextNode(data.Name + ": "));
                                    var qtySpan = span.appendChild(document.createElement("span"));
                                    qtySpan.classList.add("inventory-sku-type-quantity");
                                    qtySpan.setAttribute("data-sku-type-id", data.Id);
                                    qtySpan.appendChild(document.createTextNode("" + data.SKUCount));
                                    span.appendChild(document.createTextNode(" SKUs"));
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
                                            click: GridHandlers.createButtonClickPopupHandler(function (item) { return new DevGuild.AspNet.Routing.Uri("/Inventory/Create/?parentId=" + item.Id + "&hierarchyParentId="); }, function (item) { return ({
                                                title: "Add SKU of type " + item.Name,
                                                width: "800px",
                                                height: "auto",
                                                refresh: function (e) {
                                                    e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                                        clickEvent.preventDefault();
                                                        e.sender.close();
                                                        e.sender.destroy();
                                                    });
                                                    e.sender.center();
                                                },
                                                open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                                    var treeList;
                                                    return __generator(this, function (_a) {
                                                        switch (_a.label) {
                                                            case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryCreate")];
                                                            case 1:
                                                                _a.sent();
                                                                treeList = this._nestedTreeLists.get(item.Id);
                                                                if (!treeList) return [3 /*break*/, 3];
                                                                return [4 /*yield*/, treeList.refresh()];
                                                            case 2:
                                                                _a.sent();
                                                                _a.label = 3;
                                                            case 3:
                                                                e.sender.close();
                                                                e.sender.destroy();
                                                                return [2 /*return*/];
                                                        }
                                                    });
                                                }); }
                                            }); }),
                                        },
                                        {
                                            name: "CustomEdit",
                                            iconClass: "fas fa-pencil-alt",
                                            text: "&nbsp;",
                                            click: GridHandlers.createButtonClickPopupHandler(function (item) { return new DevGuild.AspNet.Routing.Uri("/InventoryTypes/Edit/" + item.Id); }, function (item) { return ({
                                                title: "Edit SKU Type " + item.Name,
                                                width: "1000px",
                                                height: "auto",
                                                refresh: function (e) {
                                                    e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                                        clickEvent.preventDefault();
                                                        e.sender.close();
                                                        e.sender.destroy();
                                                    });
                                                    e.sender.center();
                                                },
                                                open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                                    return __generator(this, function (_a) {
                                                        switch (_a.label) {
                                                            case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryTypesEdit")];
                                                            case 1:
                                                                _a.sent();
                                                                this._gridTypes.persistExpandedState("Id");
                                                                return [4 /*yield*/, this._gridTypes.dataSource.read()];
                                                            case 2:
                                                                _a.sent();
                                                                e.sender.close();
                                                                e.sender.destroy();
                                                                return [2 /*return*/];
                                                        }
                                                    });
                                                }); }
                                            }); }),
                                        },
                                        {
                                            name: "CustomMoveUp",
                                            iconClass: "fas fa-arrow-up",
                                            text: "&nbsp;",
                                            click: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                                var dataItem;
                                                return __generator(this, function (_a) {
                                                    switch (_a.label) {
                                                        case 0:
                                                            e.preventDefault();
                                                            dataItem = this._gridTypes.dataItem(e.currentTarget.closest("tr"));
                                                            return [4 /*yield*/, fetch("/InventoryTypes/MoveUp/" + dataItem.Id, {
                                                                    method: "POST",
                                                                    credentials: "same-origin",
                                                                    headers: {
                                                                        "X-Requested-With": "XMLHttpRequest"
                                                                    },
                                                                    body: ""
                                                                })];
                                                        case 1:
                                                            _a.sent();
                                                            this._gridTypes.persistExpandedState("Id");
                                                            return [4 /*yield*/, this._gridTypes.dataSource.read()];
                                                        case 2:
                                                            _a.sent();
                                                            return [2 /*return*/];
                                                    }
                                                });
                                            }); }
                                        },
                                        {
                                            name: "CustomMoveDown",
                                            iconClass: "fas fa-arrow-down",
                                            text: "&nbsp;",
                                            click: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                                var dataItem;
                                                return __generator(this, function (_a) {
                                                    switch (_a.label) {
                                                        case 0:
                                                            e.preventDefault();
                                                            dataItem = this._gridTypes.dataItem(e.currentTarget.closest("tr"));
                                                            return [4 /*yield*/, fetch("/InventoryTypes/MoveDown/" + dataItem.Id, {
                                                                    method: "POST",
                                                                    credentials: "same-origin",
                                                                    headers: {
                                                                        "X-Requested-With": "XMLHttpRequest"
                                                                    },
                                                                    body: ""
                                                                })];
                                                        case 1:
                                                            _a.sent();
                                                            this._gridTypes.persistExpandedState("Id");
                                                            return [4 /*yield*/, this._gridTypes.dataSource.read()];
                                                        case 2:
                                                            _a.sent();
                                                            return [2 /*return*/];
                                                    }
                                                });
                                            }); }
                                        },
                                        {
                                            name: "CustomSort",
                                            iconClass: "fas fa-sort-alpha-down",
                                            text: "&nbsp;",
                                            click: GridHandlers.createButtonClickPopupHandler(function (item) { return new DevGuild.AspNet.Routing.Uri("/InventoryTypes/Sort/" + item.Id); }, function (item) { return ({
                                                title: "Sort SKU Type " + item.Name,
                                                width: "1000px",
                                                height: "auto",
                                                refresh: function (e) {
                                                    e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                                        clickEvent.preventDefault();
                                                        e.sender.close();
                                                        e.sender.destroy();
                                                    });
                                                    e.sender.center();
                                                },
                                                open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                                    return __generator(this, function (_a) {
                                                        switch (_a.label) {
                                                            case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryTypesSort")];
                                                            case 1:
                                                                _a.sent();
                                                                this._gridTypes.persistExpandedState("Id");
                                                                return [4 /*yield*/, this._gridTypes.dataSource.read()];
                                                            case 2:
                                                                _a.sent();
                                                                e.sender.close();
                                                                e.sender.destroy();
                                                                return [2 /*return*/];
                                                        }
                                                    });
                                                }); }
                                            }); }),
                                        },
                                        {
                                            name: "CustomDelete",
                                            iconClass: "fas fa-trash-alt",
                                            text: "&nbsp;",
                                            click: GridHandlers.createButtonClickPopupHandler(function (item) { return new DevGuild.AspNet.Routing.Uri("/InventoryTypes/Delete/" + item.Id); }, function (item) { return ({
                                                title: "Delete SKU Type " + item.Name,
                                                width: "1000px",
                                                height: "auto",
                                                refresh: function (e) {
                                                    e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                                        clickEvent.preventDefault();
                                                        e.sender.close();
                                                        e.sender.destroy();
                                                    });
                                                    e.sender.center();
                                                },
                                                open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                                    return __generator(this, function (_a) {
                                                        switch (_a.label) {
                                                            case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryTypesDelete")];
                                                            case 1:
                                                                _a.sent();
                                                                this._gridTypes.persistExpandedState("Id");
                                                                return [4 /*yield*/, this._gridTypes.dataSource.read()];
                                                            case 2:
                                                                _a.sent();
                                                                e.sender.close();
                                                                e.sender.destroy();
                                                                return [2 /*return*/];
                                                        }
                                                    });
                                                }); }
                                            }); }),
                                        },
                                    ]
                                });
                            }
                            return columns;
                        };
                        return InventoryIndexPage;
                    }());
                    Index.InventoryIndexPage = InventoryIndexPage;
                    var InventorySKUsTreeList = /** @class */ (function () {
                        function InventorySKUsTreeList(page, skuType, root, canManage, canManageMovements) {
                            this._page = page;
                            this._skuType = skuType;
                            this._root = root;
                            this._canManage = canManage;
                            this._canManageMovements = canManageMovements;
                            this._dataSource = this.createDataSource();
                            this._treeList = this.initTree();
                        }
                        InventorySKUsTreeList.prototype.refresh = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            this._treeList.persistExpandedState("Id");
                                            return [4 /*yield*/, this._treeList.dataSource.read()];
                                        case 1:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        InventorySKUsTreeList.prototype.initTree = function () {
                            this._root.addClass("inventory-skus-treelist");
                            this._root.addClass("k-treelist--dense");
                            this._root.kendoTreeList({
                                columns: this.initializeColumns(),
                                dataSource: this._dataSource,
                                pageable: false,
                                scrollable: false,
                                dataBound: function (e) {
                                    var treeList = e.sender;
                                    treeList.tbody.find("tr[role='row']").each(function (index, element) {
                                        var node = $(element);
                                        var model = treeList.dataItem(node);
                                        if (model.NodeType === InventorySKUNodeType.Leaf) {
                                            node.find(".k-button[data-command=customcreatechild]").addClass("k-state-disabled").prop("disabled", true);
                                            node.find(".k-button[data-command=customcreatemovement").removeClass("k-state-disabled").prop("disabled", false);
                                        }
                                        else {
                                            node.find(".k-button[data-command=customcreatechild]").removeClass("k-state-disabled").prop("disabled", false);
                                            node.find(".k-button[data-command=customcreatemovement").addClass("k-state-disabled").prop("disabled", true);
                                        }
                                        if (model.IsDefaultChild) {
                                            node.find("[data-column=Name]").addClass("font-weight-bold");
                                        }
                                        else {
                                            node.find("[data-column=Name]").removeClass("font-weight-bold");
                                        }
                                    });
                                    treeList.tbody.find(".inventory-movements-link").each(function (index, element) {
                                        var existing = $(element).data("kendoTooltip");
                                        if (existing) {
                                            existing.destroy();
                                        }
                                        var sku = element.getAttribute("data-sku");
                                        var tab = element.getAttribute("data-tab");
                                        $(element).kendoTooltip({
                                            position: "top",
                                            content: {
                                                url: "/InventoryMovements/Preview?sku=" + sku + "&tab=" + tab,
                                            },
                                            width: 950,
                                            height: 300,
                                        });
                                    });
                                    treeList.tbody.find(".inventory-sku-name").each(function (index, element) {
                                        var existing = $(element).data("kendoTooltip");
                                        if (existing) {
                                            existing.destroy();
                                        }
                                        $(element).kendoTooltip();
                                    });
                                },
                            });
                            var treeList = this._root.data("kendoTreeList");
                            treeList["_button"] = function (command) {
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
                            };
                            return treeList;
                        };
                        InventorySKUsTreeList.prototype.createDataSource = function () {
                            var _this = this;
                            var dataSource = new kendo.data.TreeListDataSource({
                                type: "aspnetmvc-ajax",
                                transport: {
                                    read: {
                                        url: "/Inventory/Read?parentId=" + this._skuType.Id,
                                        data: function () {
                                            var filterObj = {};
                                            switch (_this._page.statusFilter) {
                                                case InventorySKUStatusFilter.WarningOnly:
                                                    filterObj["warningOnly"] = true;
                                                    break;
                                            }
                                            if (_this._page.workshopFilter) {
                                                filterObj["workshop"] = _this._page.workshopFilter;
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
                        };
                        InventorySKUsTreeList.prototype.filterTree = function (predicate) {
                            if (predicate === undefined || predicate === null) {
                                this._dataSource.filter([]);
                                return;
                            }
                            if (typeof predicate !== "function") {
                                throw new Error("Invalid predicate");
                            }
                            var dataSource = this._dataSource;
                            var recursivePredicate = function (item) {
                                if (predicate(item)) {
                                    return true;
                                }
                                var children = dataSource.data().filter(function (x) { return x.ParentId === item.Id; });
                                for (var _i = 0, children_1 = children; _i < children_1.length; _i++) {
                                    var child = children_1[_i];
                                    if (recursivePredicate(child)) {
                                        return true;
                                    }
                                }
                                return false;
                            };
                            dataSource.filter([{ operator: recursivePredicate }]);
                        };
                        InventorySKUsTreeList.prototype.initializeColumns = function () {
                            var _this = this;
                            var columns = [];
                            columns.push({
                                title: "Name",
                                field: "Name",
                                width: "300px",
                                attributes: {
                                    "data-column": "Name"
                                },
                                template: function (item) {
                                    var container = document.createElement("span");
                                    if (item.HasWarning) {
                                        var icon = document.createElement("span");
                                        icon.classList.add("fas");
                                        icon.classList.add("fa-exclamation-triangle");
                                        icon.classList.add("text-warning");
                                        container.appendChild(icon);
                                        container.appendChild(document.createTextNode("\u00a0"));
                                    }
                                    else if (item.HasDescendantsWithWarning) {
                                        var icon = document.createElement("span");
                                        icon.classList.add("fas");
                                        icon.classList.add("fa-exclamation-triangle");
                                        icon.classList.add("text-muted");
                                        container.appendChild(icon);
                                        container.appendChild(document.createTextNode("\u00a0"));
                                    }
                                    var nameSpan = container.appendChild(document.createElement("span"));
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
                                template: function (item) {
                                    var link = document.createElement("a");
                                    link.classList.add("inventory-movements-link");
                                    link.setAttribute("data-sku", item.Id);
                                    link.setAttribute("data-tab", "Tray");
                                    link.style.color = "#007bff";
                                    link.href = "/InventoryMovements?sku=" + item.Id + "&tab=Tray&group=false";
                                    link.target = "__blank";
                                    link.appendChild(document.createTextNode(kendo.toString(item.TrayQuantity, "#,##0.##")));
                                    return link.outerHTML;
                                },
                            });
                            columns.push({
                                title: "Ord",
                                field: "OrderedQuantity",
                                width: "45px",
                                template: function (item) {
                                    var link = document.createElement("a");
                                    link.classList.add("inventory-movements-link");
                                    link.setAttribute("data-sku", item.Id);
                                    link.setAttribute("data-tab", "Ordered");
                                    link.style.color = "#007bff";
                                    link.href = "/InventoryMovements?sku=" + item.Id + "&tab=Ordered&group=false";
                                    link.target = "__blank";
                                    link.appendChild(document.createTextNode(kendo.toString(item.OrderedQuantity, "#,##0.##")));
                                    return link.outerHTML;
                                },
                            });
                            columns.push({
                                title: "Req",
                                field: "RequestedQuantity",
                                width: "45px",
                                template: function (item) {
                                    var link = document.createElement("a");
                                    link.classList.add("inventory-movements-link");
                                    link.setAttribute("data-sku", item.Id);
                                    link.setAttribute("data-tab", "ApprovedAndRequested");
                                    link.style.color = "#007bff";
                                    link.href = "/InventoryMovements?sku=" + item.Id + "&tab=Approved&group=false";
                                    link.target = "__blank";
                                    link.appendChild(document.createTextNode(kendo.toString(item.RequestedQuantity, "#,##0.##")));
                                    return link.outerHTML;
                                },
                            });
                            columns.push({
                                title: "Price (AUD)",
                                field: "AveragePrice",
                                width: "70px",
                                template: function (item) {
                                    if (item.AveragePrice !== undefined && item.AveragePrice !== null) {
                                        return "$" + item.AveragePrice;
                                    }
                                    else {
                                        return "";
                                    }
                                },
                            });
                            columns.push({
                                title: "Total (AUD)",
                                field: "TotalPrice",
                                width: "70px",
                                template: function (item) {
                                    if (item.TotalPrice !== undefined && item.TotalPrice !== null) {
                                        return "$" + item.TotalPrice;
                                    }
                                    else {
                                        return "";
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
                                            click: GridHandlers.createButtonClickPopupHandler(function (item) { return new DevGuild.AspNet.Routing.Uri("/Inventory/Create/?parentId=" + item.TypeId + "&hierarchyParentId=" + item.Id); }, function (item) { return ({
                                                title: "Add SKU to group " + item.Name,
                                                width: "800px",
                                                height: "auto",
                                                refresh: function (e) {
                                                    e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                                        clickEvent.preventDefault();
                                                        e.sender.close();
                                                        e.sender.destroy();
                                                    });
                                                    e.sender.center();
                                                },
                                                open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                                    return __generator(this, function (_a) {
                                                        switch (_a.label) {
                                                            case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryCreate")];
                                                            case 1:
                                                                _a.sent();
                                                                return [4 /*yield*/, this.refresh()];
                                                            case 2:
                                                                _a.sent();
                                                                e.sender.close();
                                                                e.sender.destroy();
                                                                return [2 /*return*/];
                                                        }
                                                    });
                                                }); }
                                            }); }),
                                        },
                                        {
                                            name: "CustomCreateMovement",
                                            text: " Move",
                                            imageClass: "fas fa-exchange-alt",
                                            click: GridHandlers.createButtonClickPopupHandler(function (item) { return _this._page.workshopFilter
                                                ? new DevGuild.AspNet.Routing.Uri("/InventoryMovements/CreateForSKU?workshop=" + _this._page.workshopFilter + "&sku=" + item.Id)
                                                : new DevGuild.AspNet.Routing.Uri("/InventoryMovements/CreateForSKU?sku=" + item.Id); }, function (item) { return ({
                                                title: "Move SKU " + item.Name,
                                                width: "800px",
                                                height: "auto",
                                                refresh: function (e) {
                                                    e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                                        clickEvent.preventDefault();
                                                        e.sender.close();
                                                        e.sender.destroy();
                                                    });
                                                    e.sender.center();
                                                },
                                                open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                                    return __generator(this, function (_a) {
                                                        switch (_a.label) {
                                                            case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryMovementsCreateForSKU")];
                                                            case 1:
                                                                _a.sent();
                                                                return [4 /*yield*/, this.refresh()];
                                                            case 2:
                                                                _a.sent();
                                                                e.sender.close();
                                                                e.sender.destroy();
                                                                return [2 /*return*/];
                                                        }
                                                    });
                                                }); }
                                            }); }),
                                        },
                                        {
                                            name: "CustomEdit",
                                            text: "",
                                            imageClass: "fas fa-pencil-alt",
                                            click: GridHandlers.createButtonClickPopupHandler(function (item) {
                                                switch (item.NodeType) {
                                                    case InventorySKUNodeType.Leaf:
                                                        return new DevGuild.AspNet.Routing.Uri("/Inventory/Edit/" + item.Id);
                                                    case InventorySKUNodeType.Group:
                                                        return new DevGuild.AspNet.Routing.Uri("/Inventory/EditGroup/" + item.Id);
                                                    default:
                                                        throw new Error("Invalid SKU node type");
                                                }
                                            }, function (item) {
                                                switch (item.NodeType) {
                                                    case InventorySKUNodeType.Leaf:
                                                        return {
                                                            title: "Edit SKU " + item.Name,
                                                            width: "800px",
                                                            height: "auto",
                                                            refresh: function (e) {
                                                                e.sender.wrapper.on("click", ".inventory-edit__convert-to-group", function (clickEvent) {
                                                                    clickEvent.preventDefault();
                                                                    e.sender.close();
                                                                    e.sender.destroy();
                                                                    _this.openConvertToGroup(item);
                                                                });
                                                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                                                    clickEvent.preventDefault();
                                                                    e.sender.close();
                                                                    e.sender.destroy();
                                                                });
                                                                e.sender.center();
                                                            },
                                                            open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                                                return __generator(this, function (_a) {
                                                                    switch (_a.label) {
                                                                        case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryEdit")];
                                                                        case 1:
                                                                            _a.sent();
                                                                            return [4 /*yield*/, this.refresh()];
                                                                        case 2:
                                                                            _a.sent();
                                                                            e.sender.close();
                                                                            e.sender.destroy();
                                                                            return [2 /*return*/];
                                                                    }
                                                                });
                                                            }); }
                                                        };
                                                    case InventorySKUNodeType.Group:
                                                        return {
                                                            title: "Edit SKU Group " + item.Name,
                                                            width: "800px",
                                                            height: "auto",
                                                            refresh: function (e) {
                                                                e.sender.wrapper.on("click", ".inventory-edit__convert-to-leaf", function (clickEvent) {
                                                                    clickEvent.preventDefault();
                                                                    e.sender.close();
                                                                    e.sender.destroy();
                                                                    _this.openConvertToLeaf(item);
                                                                });
                                                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                                                    clickEvent.preventDefault();
                                                                    e.sender.close();
                                                                    e.sender.destroy();
                                                                });
                                                                e.sender.center();
                                                            },
                                                            open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                                                return __generator(this, function (_a) {
                                                                    switch (_a.label) {
                                                                        case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryEditGroup")];
                                                                        case 1:
                                                                            _a.sent();
                                                                            return [4 /*yield*/, this.refresh()];
                                                                        case 2:
                                                                            _a.sent();
                                                                            e.sender.close();
                                                                            e.sender.destroy();
                                                                            return [2 /*return*/];
                                                                    }
                                                                });
                                                            }); }
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
                                            click: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                                var dataItem;
                                                return __generator(this, function (_a) {
                                                    switch (_a.label) {
                                                        case 0:
                                                            e.preventDefault();
                                                            dataItem = this._treeList.dataItem(e.currentTarget.closest("tr"));
                                                            return [4 /*yield*/, fetch("/Inventory/MoveUp/" + dataItem.Id, {
                                                                    method: "POST",
                                                                    credentials: "same-origin",
                                                                    headers: {
                                                                        "X-Requested-With": "XMLHttpRequest"
                                                                    },
                                                                    body: ""
                                                                })];
                                                        case 1:
                                                            _a.sent();
                                                            return [4 /*yield*/, this.refresh()];
                                                        case 2:
                                                            _a.sent();
                                                            return [2 /*return*/];
                                                    }
                                                });
                                            }); }
                                        },
                                        {
                                            name: "CustomMoveDown",
                                            text: "",
                                            imageClass: "fas fa-arrow-down",
                                            click: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                                var dataItem;
                                                return __generator(this, function (_a) {
                                                    switch (_a.label) {
                                                        case 0:
                                                            e.preventDefault();
                                                            dataItem = this._treeList.dataItem(e.currentTarget.closest("tr"));
                                                            return [4 /*yield*/, fetch("/Inventory/MoveDown/" + dataItem.Id, {
                                                                    method: "POST",
                                                                    credentials: "same-origin",
                                                                    headers: {
                                                                        "X-Requested-With": "XMLHttpRequest"
                                                                    },
                                                                    body: ""
                                                                })];
                                                        case 1:
                                                            _a.sent();
                                                            return [4 /*yield*/, this.refresh()];
                                                        case 2:
                                                            _a.sent();
                                                            return [2 /*return*/];
                                                    }
                                                });
                                            }); }
                                        },
                                        {
                                            name: "CustomDelete",
                                            text: "",
                                            imageClass: "fas fa-trash-alt",
                                            click: GridHandlers.createButtonClickPopupHandler(function (item) { return new DevGuild.AspNet.Routing.Uri("/Inventory/Delete/" + item.Id); }, function (item) { return ({
                                                title: "Delete SKU " + item.Name,
                                                width: "800px",
                                                height: "auto",
                                                refresh: function (e) {
                                                    e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                                        clickEvent.preventDefault();
                                                        e.sender.close();
                                                        e.sender.destroy();
                                                    });
                                                    e.sender.center();
                                                },
                                                open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                                    return __generator(this, function (_a) {
                                                        switch (_a.label) {
                                                            case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryDelete")];
                                                            case 1:
                                                                _a.sent();
                                                                return [4 /*yield*/, this.refresh()];
                                                            case 2:
                                                                _a.sent();
                                                                e.sender.close();
                                                                e.sender.destroy();
                                                                return [2 /*return*/];
                                                        }
                                                    });
                                                }); }
                                            }); }),
                                        },
                                    ]
                                });
                            }
                            else if (this._canManageMovements) {
                                columns.push({
                                    title: "Actions",
                                    width: "300px",
                                    command: [
                                        {
                                            name: "CustomCreateMovement",
                                            text: " Move",
                                            imageClass: "fas fa-exchange-alt",
                                            click: GridHandlers.createButtonClickPopupHandler(function (item) { return new DevGuild.AspNet.Routing.Uri("/InventoryMovements/CreateForSKU?sku=" + item.Id); }, function (item) { return ({
                                                title: "Move SKU " + item.Name,
                                                width: "800px",
                                                height: "auto",
                                                refresh: function (e) {
                                                    e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                                        clickEvent.preventDefault();
                                                        e.sender.close();
                                                        e.sender.destroy();
                                                    });
                                                    e.sender.center();
                                                },
                                                open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                                    return __generator(this, function (_a) {
                                                        switch (_a.label) {
                                                            case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryMovementsCreateForSKU")];
                                                            case 1:
                                                                _a.sent();
                                                                return [4 /*yield*/, this.refresh()];
                                                            case 2:
                                                                _a.sent();
                                                                e.sender.close();
                                                                e.sender.destroy();
                                                                return [2 /*return*/];
                                                        }
                                                    });
                                                }); }
                                            }); }),
                                        },
                                    ]
                                });
                            }
                            return columns;
                        };
                        InventorySKUsTreeList.prototype.openConvertToGroup = function (item) {
                            var _this = this;
                            var route = new DevGuild.AspNet.Routing.Uri("Inventory/ConvertToGroup/" + item.Id);
                            var dialogRoot = $("<div></div>");
                            var dialogOptions = {
                                title: "Convert SKU " + item.Name + " to group",
                                actions: ["close"],
                                content: route.value,
                                width: "800px",
                                height: "auto",
                                modal: true,
                                visible: false,
                                refresh: function (e) {
                                    e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                        clickEvent.preventDefault();
                                        e.sender.close();
                                        e.sender.destroy();
                                    });
                                    e.sender.center();
                                },
                                open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                    return __generator(this, function (_a) {
                                        switch (_a.label) {
                                            case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryConvertToGroup")];
                                            case 1:
                                                _a.sent();
                                                return [4 /*yield*/, this.refresh()];
                                            case 2:
                                                _a.sent();
                                                e.sender.close();
                                                e.sender.destroy();
                                                return [2 /*return*/];
                                        }
                                    });
                                }); },
                                close: function (e) {
                                    e.sender.destroy();
                                }
                            };
                            dialogRoot.kendoWindow(dialogOptions);
                            var dialog = dialogRoot.data("kendoWindow");
                            dialog.center();
                            dialog.open();
                            $(window).on("resize", function (e) {
                                if (!(dialog.element.closest("html").length === 0 || dialog.element.is(":hidden"))) {
                                    dialog.center();
                                }
                            });
                        };
                        InventorySKUsTreeList.prototype.openConvertToLeaf = function (item) {
                            var _this = this;
                            var route = new DevGuild.AspNet.Routing.Uri("Inventory/ConvertToLeaf/" + item.Id);
                            var dialogRoot = $("<div></div>");
                            var dialogOptions = {
                                title: "Convert SKU " + item.Name + " to leaf",
                                actions: ["close"],
                                content: route.value,
                                width: "800px",
                                height: "auto",
                                modal: true,
                                visible: false,
                                refresh: function (e) {
                                    e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                        clickEvent.preventDefault();
                                        e.sender.close();
                                        e.sender.destroy();
                                    });
                                    e.sender.center();
                                },
                                open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                    return __generator(this, function (_a) {
                                        switch (_a.label) {
                                            case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryConvertToLeaf")];
                                            case 1:
                                                _a.sent();
                                                return [4 /*yield*/, this.refresh()];
                                            case 2:
                                                _a.sent();
                                                e.sender.close();
                                                e.sender.destroy();
                                                return [2 /*return*/];
                                        }
                                    });
                                }); },
                                close: function (e) {
                                    e.sender.destroy();
                                }
                            };
                            dialogRoot.kendoWindow(dialogOptions);
                            var dialog = dialogRoot.data("kendoWindow");
                            dialog.center();
                            dialog.open();
                            $(window).on("resize", function (e) {
                                if (!(dialog.element.closest("html").length === 0 || dialog.element.is(":hidden"))) {
                                    dialog.center();
                                }
                            });
                        };
                        return InventorySKUsTreeList;
                    }());
                    Index.InventorySKUsTreeList = InventorySKUsTreeList;
                })(Index = Inventory.Index || (Inventory.Index = {}));
            })(Inventory = Pages.Inventory || (Pages.Inventory = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=index.js.map