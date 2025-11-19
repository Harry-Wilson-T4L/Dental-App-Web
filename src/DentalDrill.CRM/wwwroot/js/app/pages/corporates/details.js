var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var Corporates;
            (function (Corporates) {
                var Details;
                (function (Details) {
                    var GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
                    var CorporateClientsGrid = /** @class */ (function () {
                        function CorporateClientsGrid() {
                        }
                        Object.defineProperty(CorporateClientsGrid, "instance", {
                            get: function () {
                                return $("#CorporateClientsGrid").data("kendoGrid");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        CorporateClientsGrid.handleDataBound = function (e) {
                            e.sender.element.find("[data-toggle='tooltip']").tooltip();
                        };
                        CorporateClientsGrid.handleDetailsClick = GridHandlers.createButtonClickNavigationHandler(function (item) { return routes.clients.details(item.Id); });
                        return CorporateClientsGrid;
                    }());
                    Details.CorporateClientsGrid = CorporateClientsGrid;
                })(Details = Corporates.Details || (Corporates.Details = {}));
            })(Corporates = Pages.Corporates || (Pages.Corporates = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=details.js.map