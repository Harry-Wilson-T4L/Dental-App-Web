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
        var Controls;
        (function (Controls) {
            var PdfReports;
            (function (PdfReports) {
                var PdfPageBuilder = /** @class */ (function () {
                    function PdfPageBuilder(container) {
                        this._page = $("<div class=\"pdf-report__page pdf-report__page--size-a4\"></div>");
                        container.append(this._page);
                    }
                    PdfPageBuilder.prototype.appendJQuery = function (node) {
                        this._page.append(node);
                    };
                    PdfPageBuilder.prototype.appendHTML = function (node) {
                        this._page.append($(node));
                    };
                    PdfPageBuilder.prototype.appendText = function (text) {
                        this._page.append(document.createTextNode(text));
                    };
                    PdfPageBuilder.prototype.exportGroups = function () {
                        return __awaiter(this, void 0, void 0, function () {
                            var group;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0: return [4 /*yield*/, kendo.drawing.drawDOM(this._page, {})];
                                    case 1:
                                        group = _a.sent();
                                        return [2 /*return*/, [group]];
                                }
                            });
                        });
                    };
                    PdfPageBuilder.prototype.removeHeightOverflow = function () {
                        var containerHeight = this._page.height();
                        var consumedHeight = 0;
                        var overflown = false;
                        var children = this._page.children();
                        if (children.length <= 1) {
                            return [];
                        }
                        var removed = [];
                        for (var i = 0; i < children.length; i++) {
                            var child = children[i];
                            var childHeight = child.scrollHeight;
                            if (overflown || consumedHeight + childHeight > containerHeight) {
                                overflown = true;
                                removed.push(child);
                            }
                            else {
                                consumedHeight += childHeight;
                            }
                        }
                        for (var i = 0; i < removed.length; i++) {
                            removed[i].parentElement.removeChild(removed[i]);
                        }
                        return removed;
                    };
                    PdfPageBuilder.prototype.usedHeight = function () {
                        var containerHeight = this._page.height();
                        var consumedHeight = 0;
                        var children = this._page.children();
                        for (var i = 0; i < children.length; i++) {
                            var child = children[i];
                            var childHeight = child.scrollHeight;
                            if (consumedHeight + childHeight > containerHeight) {
                                return containerHeight;
                            }
                            else {
                                consumedHeight += childHeight;
                            }
                        }
                        return consumedHeight;
                    };
                    PdfPageBuilder.prototype.remainingHeight = function () {
                        var containerHeight = this._page.height();
                        var consumedHeight = 0;
                        var children = this._page.children();
                        for (var i = 0; i < children.length; i++) {
                            var child = children[i];
                            var childHeight = child.scrollHeight;
                            if (consumedHeight + childHeight > containerHeight) {
                                return 0;
                            }
                            else {
                                consumedHeight += childHeight;
                            }
                        }
                        return containerHeight - consumedHeight;
                    };
                    PdfPageBuilder.prototype.remove = function () {
                        this._page.remove();
                    };
                    return PdfPageBuilder;
                }());
                PdfReports.PdfPageBuilder = PdfPageBuilder;
            })(PdfReports = Controls.PdfReports || (Controls.PdfReports = {}));
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=PdfPageBuilder.js.map