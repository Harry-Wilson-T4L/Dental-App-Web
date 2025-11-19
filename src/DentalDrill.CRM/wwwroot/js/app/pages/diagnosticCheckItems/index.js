var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var DiagnosticCheckItems;
            (function (DiagnosticCheckItems) {
                var Index;
                (function (Index) {
                    var DiagnosticCheckItemsGrid = /** @class */ (function () {
                        function DiagnosticCheckItemsGrid() {
                        }
                        DiagnosticCheckItemsGrid.setTypes = function (types) {
                            DiagnosticCheckItemsGrid._types = [];
                            for (var _i = 0, types_1 = types; _i < types_1.length; _i++) {
                                var type = types_1[_i];
                                DiagnosticCheckItemsGrid._types.push({ Id: type.Id, Name: type.Name });
                            }
                        };
                        DiagnosticCheckItemsGrid.resolveType = function (id) {
                            return DiagnosticCheckItemsGrid._types.filter(function (x) { return x.Id === id; })[0];
                        };
                        DiagnosticCheckItemsGrid.resolveTypeName = function (id) {
                            var type = DiagnosticCheckItemsGrid.resolveType(id);
                            return type ? type.Name : "Unknown";
                        };
                        DiagnosticCheckItemsGrid.renderTypesColumn = function (data) {
                            if (!DiagnosticCheckItemsGrid._types) {
                                return "N/A";
                            }
                            if (data.Types) {
                                return data.Types.map(function (x) { return DiagnosticCheckItemsGrid.resolveTypeName(x); }).join(", ");
                            }
                            else {
                                return "";
                            }
                        };
                        DiagnosticCheckItemsGrid.handleBeforeEdit = function (e) {
                            if (e.model.isNew()) {
                                var types = e.model["Types"];
                                if (types === undefined || types === null || types === "") {
                                    e.model["Types"] = [];
                                }
                            }
                        };
                        return DiagnosticCheckItemsGrid;
                    }());
                    Index.DiagnosticCheckItemsGrid = DiagnosticCheckItemsGrid;
                })(Index = DiagnosticCheckItems.Index || (DiagnosticCheckItems.Index = {}));
            })(DiagnosticCheckItems = Pages.DiagnosticCheckItems || (Pages.DiagnosticCheckItems = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=index.js.map