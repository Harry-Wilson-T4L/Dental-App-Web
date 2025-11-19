var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var CorporatePricing;
            (function (CorporatePricing) {
                var Index;
                (function (Index) {
                    var CorporatePricingPage = /** @class */ (function () {
                        function CorporatePricingPage(root, canEdit) {
                            this._root = root;
                            this._canEdit = canEdit;
                        }
                        CorporatePricingPage.prototype.setCategories = function (categories) {
                            this._categories = categories;
                        };
                        CorporatePricingPage.prototype.init = function () {
                            var _this = this;
                            this._grid = this.createGrid();
                            window.addEventListener("resize", function (e) {
                                _this._grid.setOptions({ height: "100px" });
                                _this._grid.resize(true);
                                _this._grid.setOptions({ height: "100%" });
                                _this._grid.resize(true);
                            });
                        };
                        CorporatePricingPage.prototype.createGrid = function () {
                            var dataSource = this.createDataSource();
                            var gridContainer = $(this._root).find(".grid-container");
                            gridContainer.kendoGrid({
                                dataSource: dataSource,
                                columns: this.initializeColumns(),
                                toolbar: this._canEdit ? ["save"] : undefined,
                                editable: this._canEdit ? {
                                    mode: "incell"
                                } : undefined,
                                pageable: true,
                            });
                            var grid = gridContainer.data("kendoGrid");
                            return grid;
                        };
                        CorporatePricingPage.prototype.createDataSource = function () {
                            var dataSource = new kendo.data.DataSource({
                                type: "aspnetmvc-ajax",
                                transport: {
                                    read: {
                                        url: "/CorporatePricingServiceLevels/Read"
                                    },
                                    update: this._canEdit ? {
                                        url: "/CorporatePricingServiceLevels/Update"
                                    } : undefined,
                                },
                                pageSize: 20,
                                schema: {
                                    data: "Data",
                                    total: "Total",
                                    errors: "Errors",
                                    model: {
                                        id: "Id",
                                        fields: this.initializeSchemaModel()
                                    }
                                }
                            });
                            return dataSource;
                        };
                        CorporatePricingPage.prototype.initializeColumns = function () {
                            var columns = [];
                            columns.push({
                                field: "ServiceLevelName",
                                title: "Service Level"
                            });
                            for (var _i = 0, _a = this._categories; _i < _a.length; _i++) {
                                var category = _a[_i];
                                var fieldName = "CategoriesPricing_" + category.Id.replace(/\-/g, "");
                                columns.push({
                                    field: fieldName,
                                    title: category.Name,
                                });
                            }
                            return columns;
                        };
                        CorporatePricingPage.prototype.initializeSchemaModel = function () {
                            var fields = {
                                ServiceLevelName: { type: "string", editable: "false" }
                            };
                            for (var _i = 0, _a = this._categories; _i < _a.length; _i++) {
                                var category = _a[_i];
                                var fieldName = "CategoriesPricing_" + category.Id.replace(/\-/g, "");
                                var fieldSource = "CategoriesPricing[\"" + category.Id + "\"]";
                                fields[fieldName] = {
                                    type: "number",
                                    from: fieldSource
                                };
                            }
                            return fields;
                        };
                        return CorporatePricingPage;
                    }());
                    Index.CorporatePricingPage = CorporatePricingPage;
                })(Index = CorporatePricing.Index || (CorporatePricing.Index = {}));
            })(CorporatePricing = Pages.CorporatePricing || (Pages.CorporatePricing = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=index.js.map