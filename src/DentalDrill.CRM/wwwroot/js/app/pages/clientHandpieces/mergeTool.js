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
var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var ClientHandpieces;
            (function (ClientHandpieces) {
                var MergeTool;
                (function (MergeTool) {
                    var ClientHandpieceItemsGrid = /** @class */ (function () {
                        function ClientHandpieceItemsGrid() {
                        }
                        ClientHandpieceItemsGrid.handleDataBound = function (e) {
                            var grid = e.sender;
                            grid.wrapper.find(".client-handpiece-items__draggable").each(function (index, element) {
                                var draggableNode = $(element);
                                if (draggableNode.attr("data-draggable-initialized") === undefined) {
                                    draggableNode.attr("data-draggable-initialized", "true");
                                }
                                draggableNode.kendoDraggable({
                                    hint: function () {
                                        var row = draggableNode.closest("tr");
                                        var dataItem = grid.dataItem(row);
                                        draggableNode.attr("data-client-handpiece-id", grid.wrapper.attr("data-chid"));
                                        draggableNode.attr("data-handpiece-id", dataItem.Id);
                                        var nodeClone = draggableNode.clone();
                                        return nodeClone;
                                    },
                                    dragstart: function (e) { return ClientHandpieceItemsGrid.draggableStart(grid, e); },
                                    dragend: function (e) { return ClientHandpieceItemsGrid.draggableEnd(grid, e); },
                                });
                            });
                            if (grid.wrapper.attr("data-drop-initialized") === undefined) {
                                grid.wrapper.attr("data-drop-initialized", "true");
                                grid.wrapper.kendoDropTarget({
                                    drop: function (e) { return ClientHandpieceItemsGrid.drop(grid, e); },
                                });
                            }
                        };
                        ClientHandpieceItemsGrid.draggableStart = function (grid, e) {
                            var handpieceId = e.sender.element.attr("data-handpiece-id");
                        };
                        ClientHandpieceItemsGrid.draggableEnd = function (grid, e) {
                            var handpieceId = e.sender.element.attr("data-handpiece-id");
                        };
                        ClientHandpieceItemsGrid.drop = function (grid, e) {
                            return __awaiter(this, void 0, void 0, function () {
                                var sourceClientHandpieceId, destinationClientHandpieceId, handpieceId, response, responseContent, relatedSelectors, i, gridNode;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            sourceClientHandpieceId = e.draggable.element.attr("data-client-handpiece-id");
                                            destinationClientHandpieceId = grid.wrapper.attr("data-chid");
                                            handpieceId = e.draggable.element.attr("data-handpiece-id");
                                            if (!(sourceClientHandpieceId !== destinationClientHandpieceId)) return [3 /*break*/, 9];
                                            return [4 /*yield*/, fetch("/ClientHandpieces/MoveToExisting/?handpieceId=" + handpieceId + "&clientHandpieceId=" + destinationClientHandpieceId, {
                                                    method: "POST",
                                                    credentials: "same-origin",
                                                    headers: { "X-Requested-With": "XMLHttpRequest" },
                                                    body: ""
                                                })];
                                        case 1:
                                            response = _a.sent();
                                            if (!(response.status === 200)) return [3 /*break*/, 9];
                                            return [4 /*yield*/, response.json()];
                                        case 2:
                                            responseContent = _a.sent();
                                            if (!(responseContent && responseContent.Success === true)) return [3 /*break*/, 9];
                                            if (!(responseContent.RefreshAll == true)) return [3 /*break*/, 5];
                                            return [4 /*yield*/, $("#RepairedItemsFirstGrid").data("kendoGrid").dataSource.read()];
                                        case 3:
                                            _a.sent();
                                            return [4 /*yield*/, $("#RepairedItemsSecondGrid").data("kendoGrid").dataSource.read()];
                                        case 4:
                                            _a.sent();
                                            return [3 /*break*/, 9];
                                        case 5:
                                            relatedSelectors = [
                                                "#ClientHandpiece_" + sourceClientHandpieceId + "_FirstGrid",
                                                "#ClientHandpiece_" + sourceClientHandpieceId + "_SecondGrid",
                                                "#ClientHandpiece_" + destinationClientHandpieceId + "_FirstGrid",
                                                "#ClientHandpiece_" + destinationClientHandpieceId + "_SecondGrid"
                                            ];
                                            i = 0;
                                            _a.label = 6;
                                        case 6:
                                            if (!(i < relatedSelectors.length)) return [3 /*break*/, 9];
                                            gridNode = $(relatedSelectors[i]);
                                            if (!(gridNode.length > 0)) return [3 /*break*/, 8];
                                            return [4 /*yield*/, gridNode.data("kendoGrid").dataSource.read()];
                                        case 7:
                                            _a.sent();
                                            _a.label = 8;
                                        case 8:
                                            i++;
                                            return [3 /*break*/, 6];
                                        case 9: return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        ClientHandpieceItemsGrid.dropOnNew = function (e) {
                            return __awaiter(this, void 0, void 0, function () {
                                var sourceClientHandpieceId, handpieceId, response, responseContent;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            sourceClientHandpieceId = e.draggable.element.attr("data-client-handpiece-id");
                                            handpieceId = e.draggable.element.attr("data-handpiece-id");
                                            return [4 /*yield*/, fetch("/ClientHandpieces/MoveToNew/?handpieceId=" + handpieceId, {
                                                    method: "POST",
                                                    credentials: "same-origin",
                                                    headers: { "X-Requested-With": "XMLHttpRequest" },
                                                    body: ""
                                                })];
                                        case 1:
                                            response = _a.sent();
                                            if (!(response.status === 200)) return [3 /*break*/, 5];
                                            return [4 /*yield*/, response.json()];
                                        case 2:
                                            responseContent = _a.sent();
                                            if (!(responseContent && responseContent.Success === true)) return [3 /*break*/, 5];
                                            // Refresh related grids
                                            return [4 /*yield*/, $("#RepairedItemsFirstGrid").data("kendoGrid").dataSource.read()];
                                        case 3:
                                            // Refresh related grids
                                            _a.sent();
                                            return [4 /*yield*/, $("#RepairedItemsSecondGrid").data("kendoGrid").dataSource.read()];
                                        case 4:
                                            _a.sent();
                                            _a.label = 5;
                                        case 5: return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        return ClientHandpieceItemsGrid;
                    }());
                    MergeTool.ClientHandpieceItemsGrid = ClientHandpieceItemsGrid;
                    $(function () {
                        $("#ClientHandpieceNewArea").kendoDropTarget({
                            drop: function (e) { return ClientHandpieceItemsGrid.dropOnNew(e); },
                        });
                    });
                })(MergeTool = ClientHandpieces.MergeTool || (ClientHandpieces.MergeTool = {}));
            })(ClientHandpieces = Pages.ClientHandpieces || (Pages.ClientHandpieces = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=mergeTool.js.map