var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            var Reporting;
            (function (Reporting) {
                var ReportsPageDataSourceGroupHelper = /** @class */ (function () {
                    function ReportsPageDataSourceGroupHelper() {
                    }
                    ReportsPageDataSourceGroupHelper.getFirstItemFromGroup = function (group, field) {
                        if (group && group.items && group.items.length > 0) {
                            var firstItem = group.items[0];
                            if (firstItem[field]) {
                                return firstItem;
                            }
                            if (firstItem["aggregates"] && firstItem["items"]) {
                                return this.getFirstItemFromGroup(firstItem, field);
                            }
                        }
                        return undefined;
                    };
                    ReportsPageDataSourceGroupHelper.getAllItemsFromGroup = function (group) {
                        var result = [];
                        function processGroup(processedGroup) {
                            for (var i = 0; i < processedGroup.items.length; i++) {
                                var processedItem = processedGroup.items[i];
                                if (processedItem["aggregates"] && processedItem["items"]) {
                                    processGroup(processedItem);
                                }
                                else {
                                    result.push(processedItem);
                                }
                            }
                        }
                        processGroup(group);
                        return result;
                    };
                    return ReportsPageDataSourceGroupHelper;
                }());
                Reporting.ReportsPageDataSourceGroupHelper = ReportsPageDataSourceGroupHelper;
            })(Reporting = Controls.Reporting || (Controls.Reporting = {}));
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=ReportsPageDataSourceGroupHelper.js.map