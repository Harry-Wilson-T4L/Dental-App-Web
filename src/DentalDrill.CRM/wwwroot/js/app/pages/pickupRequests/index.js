var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var PickupRequests;
            (function (PickupRequests) {
                var Index;
                (function (Index) {
                    var GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
                    var PickupRequestsGrid = /** @class */ (function () {
                        function PickupRequestsGrid() {
                        }
                        Object.defineProperty(PickupRequestsGrid, "instance", {
                            get: function () {
                                return $("#PickupRequestsGrid").data("kendoGrid");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        PickupRequestsGrid.handleDetails = GridHandlers.createButtonClickPopupHandler(function (item) { return routes.pickupRequests.details(item.Id); }, function (item) { return ({
                            title: "Pickup request from " + item.PracticeName,
                            height: "auto",
                            refresh: function (e) {
                                e.sender.center();
                            },
                        }); });
                        return PickupRequestsGrid;
                    }());
                    Index.PickupRequestsGrid = PickupRequestsGrid;
                })(Index = PickupRequests.Index || (PickupRequests.Index = {}));
            })(PickupRequests = Pages.PickupRequests || (Pages.PickupRequests = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=index.js.map