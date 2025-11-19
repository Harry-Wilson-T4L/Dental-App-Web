var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            var Reporting;
            (function (Reporting) {
                var ReportsPageGridColumnHeaderGroupings = /** @class */ (function () {
                    function ReportsPageGridColumnHeaderGroupings() {
                    }
                    ReportsPageGridColumnHeaderGroupings.sum = function (fieldName) {
                        return function (dateSuffix) {
                            return function (group) {
                                var valueField = "" + fieldName + dateSuffix;
                                var groupItems = Reporting.ReportsPageDataSourceGroupHelper.getAllItemsFromGroup(group);
                                if (groupItems.length === 0) {
                                    return "";
                                }
                                var sumValue = 0;
                                for (var i = 0; i < groupItems.length; i++) {
                                    var item = groupItems[i];
                                    if (item[valueField] !== undefined && item[valueField] !== null) {
                                        sumValue += item[valueField];
                                    }
                                }
                                if (sumValue === 0) {
                                    return "";
                                }
                                return "" + sumValue.toFixed(0);
                            };
                        };
                    };
                    ReportsPageGridColumnHeaderGroupings.average = function (valueFieldName, countFieldName) {
                        return function (dateSuffix) {
                            return function (group) {
                                var valueField = "" + valueFieldName + dateSuffix;
                                var countField = "" + countFieldName + dateSuffix;
                                var groupItems = Reporting.ReportsPageDataSourceGroupHelper.getAllItemsFromGroup(group);
                                if (groupItems.length === 0) {
                                    return "";
                                }
                                var sumCount = 0;
                                var sumValue = 0;
                                for (var i = 0; i < groupItems.length; i++) {
                                    var item = groupItems[i];
                                    if (item[valueField] !== undefined && item[valueField] !== null && item[countField] !== undefined && item[countField] !== null) {
                                        sumCount += item[countField];
                                        sumValue += item[valueField] * item[countField];
                                    }
                                }
                                if (sumCount === 0 || sumValue === 0) {
                                    return "";
                                }
                                return "" + (sumValue / sumCount).toFixed(2);
                            };
                        };
                    };
                    ReportsPageGridColumnHeaderGroupings.averageFromSum = function (valueFieldName, countFieldName) {
                        return function (dateSuffix) {
                            return function (group) {
                                var valueField = "" + valueFieldName + dateSuffix;
                                var countField = "" + countFieldName + dateSuffix;
                                var groupItems = Reporting.ReportsPageDataSourceGroupHelper.getAllItemsFromGroup(group);
                                if (groupItems.length === 0) {
                                    return "";
                                }
                                var sumCount = 0;
                                var sumValue = 0;
                                for (var i = 0; i < groupItems.length; i++) {
                                    var item = groupItems[i];
                                    if (item[valueField] !== undefined && item[valueField] !== null && item[countField] !== undefined && item[countField] !== null) {
                                        sumCount += item[countField];
                                        sumValue += item[valueField];
                                    }
                                }
                                if (sumCount === 0 || sumValue === 0) {
                                    return "";
                                }
                                return "" + (sumValue / sumCount).toFixed(2);
                            };
                        };
                    };
                    ReportsPageGridColumnHeaderGroupings.percent = function (valueFieldName, countFieldName) {
                        return function (dateSuffix) {
                            return function (group) {
                                var valueField = "" + valueFieldName + dateSuffix;
                                var countField = "" + countFieldName + dateSuffix;
                                var groupItems = Reporting.ReportsPageDataSourceGroupHelper.getAllItemsFromGroup(group);
                                if (groupItems.length === 0) {
                                    return "";
                                }
                                var sumValue = 0;
                                var sumRating = 0;
                                for (var i = 0; i < groupItems.length; i++) {
                                    var item = groupItems[i];
                                    if (item[valueField] !== undefined && item[valueField] !== null && item[countField] !== undefined && item[countField] !== null) {
                                        sumValue += item[countField];
                                        sumRating += item[valueField] * item[countField];
                                    }
                                }
                                if (sumValue === 0 || sumRating === 0) {
                                    return "";
                                }
                                return (100 * sumRating / sumValue).toFixed(2) + "%";
                            };
                        };
                    };
                    return ReportsPageGridColumnHeaderGroupings;
                }());
                Reporting.ReportsPageGridColumnHeaderGroupings = ReportsPageGridColumnHeaderGroupings;
                var ReportsPageGridHandpiecesColumns = /** @class */ (function () {
                    function ReportsPageGridHandpiecesColumns() {
                    }
                    ReportsPageGridHandpiecesColumns.prototype.handpiecesCount = function (options) {
                        var column = {
                            field: "HandpiecesCount",
                            title: "Repairs",
                            format: "{0:n0}",
                            groupHeaderColumnTemplate: ReportsPageGridColumnHeaderGroupings.sum("HandpiecesCount")
                        };
                        if (options && typeof options === "object") {
                            $.extend(column, options);
                        }
                        return column;
                    };
                    ReportsPageGridHandpiecesColumns.prototype.ratingAverage = function (options) {
                        var column = {
                            field: "RatingAverage",
                            title: "Avg. Rating",
                            format: "{0:n}",
                            groupHeaderColumnTemplate: ReportsPageGridColumnHeaderGroupings.average("RatingAverage", "HandpiecesCount")
                        };
                        if (options && typeof options === "object") {
                            $.extend(column, options);
                        }
                        return column;
                    };
                    ReportsPageGridHandpiecesColumns.prototype.unrepairablePercent = function (options) {
                        var column = {
                            field: "UnrepairedPercent",
                            title: "Unrep.",
                            format: "{0:p2}",
                            groupHeaderColumnTemplate: ReportsPageGridColumnHeaderGroupings.percent("UnrepairedPercent", "HandpiecesCount")
                        };
                        if (options && typeof options === "object") {
                            $.extend(column, options);
                        }
                        return column;
                    };
                    ReportsPageGridHandpiecesColumns.prototype.returnUnrepairedPercent = function (options) {
                        var column = {
                            field: "ReturnUnrepairedPercent",
                            title: "Ret. Unrep.",
                            format: "{0:p2}",
                            groupHeaderColumnTemplate: ReportsPageGridColumnHeaderGroupings.percent("ReturnUnrepairedPercent", "HandpiecesCount")
                        };
                        if (options && typeof options === "object") {
                            $.extend(column, options);
                        }
                        return column;
                    };
                    ReportsPageGridHandpiecesColumns.prototype.turnaroundAverage = function (options) {
                        var column = {
                            field: "TurnaroundAverage",
                            title: "Avg. Turnaround",
                            format: "{0:2}",
                            groupHeaderColumnTemplate: ReportsPageGridColumnHeaderGroupings.average("TurnaroundAverage", "HandpiecesCount")
                        };
                        if (options && typeof options === "object") {
                            $.extend(column, options);
                        }
                        return column;
                    };
                    ReportsPageGridHandpiecesColumns.prototype.warrantyCount = function (options) {
                        var column = {
                            field: "WarrantyCount",
                            title: "Warranty",
                            format: "{0:2}",
                            groupHeaderColumnTemplate: ReportsPageGridColumnHeaderGroupings.sum("WarrantyCount")
                        };
                        if (options && typeof options === "object") {
                            $.extend(column, options);
                        }
                        return column;
                    };
                    ReportsPageGridHandpiecesColumns.prototype.costSum = function (options) {
                        var column = {
                            field: "CostSum",
                            title: "Cost",
                            format: "{0:c0}",
                            groupHeaderColumnTemplate: ReportsPageGridColumnHeaderGroupings.sum("CostSum")
                        };
                        if (options && typeof options === "object") {
                            $.extend(column, options);
                        }
                        return column;
                    };
                    ReportsPageGridHandpiecesColumns.prototype.costAverage = function (options) {
                        var column = {
                            field: "CostAverage",
                            title: "Avg. Cost",
                            format: "{0:c0}",
                            groupHeaderColumnTemplate: ReportsPageGridColumnHeaderGroupings.averageFromSum("CostSum", "HandpiecesCount")
                        };
                        if (options && typeof options === "object") {
                            $.extend(column, options);
                        }
                        return column;
                    };
                    return ReportsPageGridHandpiecesColumns;
                }());
                Reporting.ReportsPageGridHandpiecesColumns = ReportsPageGridHandpiecesColumns;
                var ReportsPageGridColumns = /** @class */ (function () {
                    function ReportsPageGridColumns() {
                    }
                    Object.defineProperty(ReportsPageGridColumns, "handpieces", {
                        get: function () {
                            return ReportsPageGridColumns._handpieces;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    ReportsPageGridColumns._handpieces = new ReportsPageGridHandpiecesColumns();
                    return ReportsPageGridColumns;
                }());
                Reporting.ReportsPageGridColumns = ReportsPageGridColumns;
            })(Reporting = Controls.Reporting || (Controls.Reporting = {}));
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=ReportsPageGridColumn.js.map