var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var InventoryMovements;
            (function (InventoryMovements) {
                var Preview;
                (function (Preview) {
                    var GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
                    var HandpieceStatusHelper = InventoryMovements.Shared.HandpieceStatusHelper;
                    var InventoryMovementsPreviewPage = /** @class */ (function () {
                        function InventoryMovementsPreviewPage(root, options) {
                            this._root = root;
                            this._options = options;
                        }
                        InventoryMovementsPreviewPage.prototype.init = function () {
                            this._dataSource = this.createDataSource(this.getUrlFromTab(), this._options.sku, this._options.workshop);
                            this._grid = this.createGrid();
                        };
                        InventoryMovementsPreviewPage.prototype.createGrid = function () {
                            var container = this._root.querySelector(".grid-container");
                            var grid = $(container)
                                .css("height", "100%")
                                .addClass("k-grid--dense")
                                .addClass("k-grid--small-text")
                                .kendoGrid({
                                dataSource: this._dataSource,
                                columns: [
                                    {
                                        title: "Date",
                                        field: "Date",
                                        width: "50px",
                                        template: function (data) { return "" + kendo.toString(data.Date, "d"); },
                                    },
                                    {
                                        title: "SKU",
                                        field: "SKUName",
                                        width: "150px",
                                    },
                                    {
                                        title: "QTY",
                                        field: "QuantityAbsolute",
                                        width: "50px",
                                    },
                                    {
                                        title: "Client",
                                        field: "ClientId",
                                        width: "100px",
                                        template: function (data) {
                                            if (!data.ClientId || !data.ClientFullName) {
                                                return "";
                                            }
                                            var link = document.createElement("a");
                                            link.href = "/Clients/Details/" + data.ClientId;
                                            link.appendChild(document.createTextNode(data.ClientFullName));
                                            return link.outerHTML;
                                        }
                                    },
                                    {
                                        title: "Job",
                                        field: "HandpieceNumber",
                                        width: "50px",
                                        template: function (data) {
                                            if (!data.HandpieceId) {
                                                return "";
                                            }
                                            return "<a style=\"color: #007bff;\" href=\"/Handpieces/Edit/" + data.HandpieceId + "\">" + data.HandpieceNumber + "</a> ";
                                        },
                                    },
                                    {
                                        title: "Job Status",
                                        field: "HandpieceStatus",
                                        width: "150px",
                                        template: function (data) {
                                            if (!data.HandpieceId) {
                                                return "";
                                            }
                                            return HandpieceStatusHelper.toDisplayString(data.HandpieceStatus);
                                        },
                                    },
                                    {
                                        title: "Price",
                                        field: "FinalPrice",
                                        width: "50px",
                                        template: function (data) {
                                            if (data.FinalPrice === undefined || data.FinalPrice === null) {
                                                return "";
                                            }
                                            if (data.MovementPrice === undefined || data.MovementPrice === null) {
                                                return "<i>$" + kendo.toString(data.FinalPrice, "#,##0.##") + "</i>";
                                            }
                                            return "$" + kendo.toString(data.FinalPrice, "#,##0.##");
                                        }
                                    },
                                    {
                                        title: "Total",
                                        field: "TotalPriceAbsolute",
                                        width: "50px",
                                        template: function (data) {
                                            if (data.TotalPriceAbsolute === undefined || data.TotalPriceAbsolute === null) {
                                                return "";
                                            }
                                            if (data.MovementPrice === undefined || data.MovementPrice === null) {
                                                return "<i>$" + kendo.toString(data.TotalPriceAbsolute, "#,##0.##") + "</i>";
                                            }
                                            return "$" + kendo.toString(data.TotalPriceAbsolute, "#,##0.##");
                                        }
                                    },
                                    {
                                        title: "Actions",
                                        width: "50px",
                                        command: {
                                            name: "CustomHistory",
                                            iconClass: "fas fa-history",
                                            text: "&nbsp;",
                                            click: GridHandlers.createButtonClickPopupHandler(function (item) { return new DevGuild.AspNet.Routing.Uri("/InventoryMovements/History/" + item.Id); }, function (item) { return ({
                                                title: "Move history",
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
                                            }); }),
                                        },
                                    }
                                ]
                            }).data("kendoGrid");
                            return grid;
                        };
                        InventoryMovementsPreviewPage.prototype.getUrlFromTab = function () {
                            switch (this._options.tab) {
                                case "All":
                                    return "/InventoryMovements/ReadAll";
                                case "Tray":
                                    return "/InventoryMovements/ReadTray";
                                case "Ordered":
                                    return "/InventoryMovements/ReadOrdered";
                                case "ApprovedAndRequested":
                                    return "/InventoryMovements/ReadApprovedAndRequested";
                                case "Approved":
                                    return "/InventoryMovements/ReadApproved";
                                case "Requested":
                                    return "/InventoryMovements/ReadRequested";
                                case "Complete":
                                    return "/InventoryMovements/ReadComplete";
                                default:
                                    throw new Error("Unknown tab " + this._options.tab);
                            }
                        };
                        InventoryMovementsPreviewPage.prototype.createDataSource = function (url, sku, workshop) {
                            var dataSource = new kendo.data.DataSource({
                                type: "aspnetmvc-ajax",
                                transport: {
                                    read: {
                                        url: url,
                                        data: function () { return ({ sku: sku, workshop: workshop }); },
                                    },
                                },
                                schema: {
                                    data: "Data",
                                    total: "Total",
                                    errors: "Errors",
                                    model: {
                                        id: "Id",
                                        fields: {
                                            "Date": { type: "Date" },
                                            "SKUId": { type: "string" },
                                            "SKUName": { type: "string" },
                                            "Quantity": { type: "number" },
                                            "QuantityAbsolute": { type: "number" },
                                            "Type": { type: "number" },
                                            "Status": { type: "number" },
                                            "HandpieceId": { type: "string" },
                                            "HandpieceNumber": { type: "string" },
                                            "HandpieceStatus": { type: "number" },
                                            "ClientId": { type: "string" },
                                            "ClientFullName": { type: "string" },
                                            "AveragePrice": { type: "number" },
                                            "MovementPrice": { type: "number" },
                                            "FinalPrice": { type: "number" },
                                            "TotalPrice": { type: "number" },
                                            "TotalPriceAbsolute": { type: "number" },
                                        },
                                    },
                                },
                                sort: [
                                    { field: "SKUTypeOrderNo", dir: "asc" },
                                    { field: "SKUOrderNo", dir: "asc" },
                                    { field: "Date", dir: "asc" },
                                ],
                                serverAggregates: true,
                                serverFiltering: true,
                                serverGrouping: true,
                                serverPaging: true,
                                serverSorting: true,
                            });
                            return dataSource;
                        };
                        return InventoryMovementsPreviewPage;
                    }());
                    Preview.InventoryMovementsPreviewPage = InventoryMovementsPreviewPage;
                })(Preview = InventoryMovements.Preview || (InventoryMovements.Preview = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=InventoryMovementsPreviewPage.js.map