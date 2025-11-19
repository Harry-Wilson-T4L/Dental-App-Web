var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var HandpieceStoreOrders;
            (function (HandpieceStoreOrders) {
                var Index;
                (function (Index) {
                    var GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
                    var HandpieceStoreOrdersGrid = /** @class */ (function () {
                        function HandpieceStoreOrdersGrid() {
                        }
                        HandpieceStoreOrdersGrid.handleDetails = GridHandlers.createButtonClickPopupHandler(function (item) { return routes.handpieceStoreOrders.details(item.Id); }, function (item) { return ({
                            title: "Handpiece Order " + item.OrderNumber,
                            width: "1000px",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                e.sender.center();
                            }
                        }); });
                        return HandpieceStoreOrdersGrid;
                    }());
                    Index.HandpieceStoreOrdersGrid = HandpieceStoreOrdersGrid;
                })(Index = HandpieceStoreOrders.Index || (HandpieceStoreOrders.Index = {}));
            })(HandpieceStoreOrders = Pages.HandpieceStoreOrders || (Pages.HandpieceStoreOrders = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=index.js.map