var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
kendo.ui.Grid.prototype["saveExpandedState"] = function (field, customSave) {
    var grid = this;
    var expanded = [];
    grid.tbody.children(":has(> .k-hierarchy-cell > .k-i-collapse)").each(function (index, row) {
        var item = grid.dataItem(row);
        var fieldValue = item.get(field);
        var info = {};
        info[field] = fieldValue;
        if (customSave) {
            customSave(info, row, item);
        }
        expanded.push(info);
    });
    return expanded;
};
kendo.ui.Grid.prototype["restoreExpandedState"] = function (field, expanded, customLoad) {
    var grid = this;
    var rows = [];
    grid.tbody.children().each(function (index, row) {
        var item = grid.dataItem(row);
        var fieldValue = item.get(field);
        var matches = expanded.filter(function (x) { return x[field] == fieldValue; });
        if (matches.length > 0) {
            var match = matches[0];
            rows.push({
                src: match,
                row: row,
                dataItem: item,
            });
        }
    });
    for (var _i = 0, rows_1 = rows; _i < rows_1.length; _i++) {
        var row = rows_1[_i];
        grid.expandRow(row.row);
        if (customLoad) {
            customLoad(row.src, row.row, row.dataItem);
        }
    }
};
kendo.ui.Grid.prototype["persistExpandedState"] = function (field, customSave, customLoad) {
    var grid = this;
    var state = grid.saveExpandedState(field, customSave);
    grid.one("dataBound", function (e) {
        grid.restoreExpandedState(field, state, customLoad);
    });
};
kendo.ui.TreeList.prototype["saveExpandedState"] = function (field) {
    var treeList = this;
    var expanded = [];
    treeList.tbody.children(":has(> td:first-child > .k-i-collapse)").each(function (index, row) {
        var level = $(row).find("td:first-child > .k-i-none").length;
        var item = treeList.dataItem(row);
        var fieldValue = item.get(field);
        var info = {};
        info[field] = fieldValue;
        info["__level"] = level;
        expanded.push(info);
    });
    return expanded;
};
kendo.ui.TreeList.prototype["restoreExpandedState"] = function (field, expanded) {
    return __awaiter(this, void 0, void 0, function () {
        var treeList, level, expandedOfLevel, _loop_1, _i, expandedOfLevel_1, match;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0:
                    treeList = this;
                    level = 0;
                    expandedOfLevel = expanded.filter(function (x) { return x["__level"] === level; });
                    _a.label = 1;
                case 1:
                    if (!(expandedOfLevel.length > 0)) return [3 /*break*/, 6];
                    _loop_1 = function (match) {
                        var expectedFieldValue, rows;
                        return __generator(this, function (_b) {
                            switch (_b.label) {
                                case 0:
                                    expectedFieldValue = match[field];
                                    rows = treeList.tbody.children().filter(function (index, row) {
                                        var item = treeList.dataItem(row);
                                        var fieldValue = item.get(field);
                                        return fieldValue === expectedFieldValue;
                                    });
                                    if (!(rows.length > 0)) return [3 /*break*/, 2];
                                    return [4 /*yield*/, treeList.expand(rows[0])];
                                case 1:
                                    _b.sent();
                                    _b.label = 2;
                                case 2: return [2 /*return*/];
                            }
                        });
                    };
                    _i = 0, expandedOfLevel_1 = expandedOfLevel;
                    _a.label = 2;
                case 2:
                    if (!(_i < expandedOfLevel_1.length)) return [3 /*break*/, 5];
                    match = expandedOfLevel_1[_i];
                    return [5 /*yield**/, _loop_1(match)];
                case 3:
                    _a.sent();
                    _a.label = 4;
                case 4:
                    _i++;
                    return [3 /*break*/, 2];
                case 5:
                    level++;
                    expandedOfLevel = expanded.filter(function (x) { return x["__level"] === level; });
                    return [3 /*break*/, 1];
                case 6: return [2 /*return*/];
            }
        });
    });
};
kendo.ui.TreeList.prototype["persistExpandedState"] = function (field) {
    var _this = this;
    var treeList = this;
    var state = treeList.saveExpandedState(field);
    treeList.one("dataBound", function (e) { return __awaiter(_this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, treeList.restoreExpandedState(field, state)];
                case 1:
                    _a.sent();
                    return [2 /*return*/];
            }
        });
    }); });
};
kendo.ui.Grid.prototype["autoResize"] = function (resizer) {
    this.autoResizeWhen(function () { return true; }, resizer);
};
kendo.ui.Grid.prototype["autoResizeWhen"] = function (predicate, resizer) {
    var grid = this;
    var lastKnownHeight = document.documentElement.clientHeight;
    var timeoutHandle = undefined;
    var handler = function () {
        if (lastKnownHeight != document.documentElement.clientHeight) {
            lastKnownHeight = document.documentElement.clientHeight;
            if (predicate()) {
                if (resizer !== undefined) {
                    resizer(grid);
                }
                else {
                    grid.setOptions({ height: "100px" });
                    grid.resize();
                    grid.setOptions({ height: "100%" });
                    grid.resize();
                }
            }
        }
        timeoutHandle = undefined;
    };
    $(window).on("resize", function (e) {
        if (timeoutHandle !== undefined) {
            clearTimeout(timeoutHandle);
        }
        timeoutHandle = setTimeout(handler, 500);
    });
};
$(function () {
    setTimeout(function () {
        var autoResizeGrids = document.querySelectorAll(".k-grid.k-grid--auto-resize");
        for (var i = 0; i < autoResizeGrids.length; i++) {
            var grid = $(autoResizeGrids[i]).data("kendoGrid");
            if (grid) {
                grid.autoResize();
            }
        }
    });
});
//# sourceMappingURL=kendo-grid-extensions.js.map