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
                    var GridInventoryGroupMovementsCompleteTab = /** @class */ (function (_super) {
                        __extends(GridInventoryGroupMovementsCompleteTab, _super);
                        function GridInventoryGroupMovementsCompleteTab(id, root, options) {
                            return _super.call(this, id, root, options) || this;
                        }
                        GridInventoryGroupMovementsCompleteTab.prototype.getTabName = function () {
                            return "Complete";
                        };
                        GridInventoryGroupMovementsCompleteTab.prototype.getEndpointUrl = function () {
                            return "/InventoryMovements/ReadGroupComplete";
                        };
                        GridInventoryGroupMovementsCompleteTab.prototype.initializeCommands = function () {
                            var _this = this;
                            var commands = [];
                            commands.push({
                                name: "CustomOpenJob",
                                iconClass: "fas fa-link",
                                text: "&nbsp; Jobs",
                                click: function (e) {
                                    e.preventDefault();
                                    var data = _this.grid.dataItem(e.target.closest("tr"));
                                    var url = new DevGuild.AspNet.Routing.Uri("/InventoryMovements?workshop=" + _this.workshopId + "&sku=" + data.SKUId + "&tab=" + _this.getTabName() + "&group=false");
                                    url.navigate();
                                }
                            });
                            return commands;
                        };
                        return GridInventoryGroupMovementsCompleteTab;
                    }(Index.GridInventoryGroupMovementsTabBase));
                    Index.GridInventoryGroupMovementsCompleteTab = GridInventoryGroupMovementsCompleteTab;
                })(Index = InventoryMovements.Index || (InventoryMovements.Index = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=GridInventoryGroupMovementsCompleteTab.js.map