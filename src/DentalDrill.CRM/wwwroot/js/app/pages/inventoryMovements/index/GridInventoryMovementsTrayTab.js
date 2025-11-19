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
            var InventoryMovements;
            (function (InventoryMovements) {
                var Index;
                (function (Index) {
                    var GridInventoryMovementsTrayTab = /** @class */ (function (_super) {
                        __extends(GridInventoryMovementsTrayTab, _super);
                        function GridInventoryMovementsTrayTab(id, root, options) {
                            return _super.call(this, id, root, options) || this;
                        }
                        GridInventoryMovementsTrayTab.prototype.getEndpointUrl = function () {
                            return "/InventoryMovements/ReadTray";
                        };
                        GridInventoryMovementsTrayTab.prototype.initializeCommands = function () {
                            var _this = this;
                            var commands = [];
                            commands.push({
                                name: "CustomOpenJob",
                                iconClass: "fas fa-link",
                                text: "&nbsp; Job",
                                click: function (e) {
                                    e.preventDefault();
                                    var dataItem = _this.grid.dataItem(e.currentTarget.closest("tr"));
                                    if (!dataItem) {
                                        return;
                                    }
                                    if (dataItem.HandpieceId) {
                                        var url = routes.handpieces.edit(dataItem.HandpieceId);
                                        url.open();
                                    }
                                }
                            });
                            return commands;
                        };
                        return GridInventoryMovementsTrayTab;
                    }(Index.GridInventoryMovementsTabBase));
                    Index.GridInventoryMovementsTrayTab = GridInventoryMovementsTrayTab;
                })(Index = InventoryMovements.Index || (InventoryMovements.Index = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=GridInventoryMovementsTrayTab.js.map