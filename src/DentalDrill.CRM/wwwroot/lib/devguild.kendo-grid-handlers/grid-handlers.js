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
var DevGuild;
(function (DevGuild) {
    var AspNet;
    (function (AspNet) {
        var Controls;
        (function (Controls) {
            var Grids;
            (function (Grids) {
                var Handlers;
                (function (Handlers) {
                    var GridHandlers = /** @class */ (function () {
                        function GridHandlers() {
                        }
                        GridHandlers.createButtonClickHandler = function (action) {
                            return function (e, original) {
                                e.preventDefault();
                                var dataItem = this.dataItem(e.currentTarget.closest("tr"));
                                action(dataItem);
                            };
                        };
                        GridHandlers.createButtonClickNavigationHandler = function (routeSelector) {
                            return function (e, original) {
                                e.preventDefault();
                                var dataItem = this.dataItem(e.currentTarget.closest("tr"));
                                var url = routeSelector(dataItem);
                                url.execute(e.ctrlKey || original && original.which === 2);
                            };
                        };
                        GridHandlers.createGridButtonClickNavigationHandler = function (buttonSelector, routeSelector) {
                            var _this = this;
                            $(document).on("click", buttonSelector, function (e) { return __awaiter(_this, void 0, void 0, function () {
                                var target, url;
                                return __generator(this, function (_a) {
                                    e.preventDefault();
                                    target = $(e.target);
                                    url = routeSelector(target);
                                    url.navigate();
                                    return [2 /*return*/];
                                });
                            }); });
                            return {};
                        };
                        GridHandlers.createGridButtonClickPopupHandler = function (buttonSelector, routeSelector, options) {
                            var _this = this;
                            $(document).on("click", buttonSelector, function (e) { return __awaiter(_this, void 0, void 0, function () {
                                var target, url, dialogRoot_1, dialogOptions, dialog;
                                return __generator(this, function (_a) {
                                    e.preventDefault();
                                    target = $(e.target);
                                    url = routeSelector(target);
                                    if (e.ctrlKey) {
                                        url.open();
                                    }
                                    else {
                                        dialogRoot_1 = $("<div></div>");
                                        dialogOptions = {
                                            title: "",
                                            actions: ["close"],
                                            content: url.value,
                                            width: "800px",
                                            height: "600px",
                                            modal: true,
                                            visible: false,
                                            close: function () { return dialogRoot_1.data("kendoWindow").destroy(); }
                                        };
                                        if (options) {
                                            $.extend(dialogOptions, options(target));
                                        }
                                        dialogRoot_1.kendoWindow(dialogOptions);
                                        dialog = dialogRoot_1.data("kendoWindow");
                                        dialog.center();
                                        dialog.open();
                                    }
                                    return [2 /*return*/];
                                });
                            }); });
                            $(document).on("auxclick", buttonSelector, function (e) {
                                if (e.which === 2) {
                                    e.preventDefault();
                                    var target = $(e.target);
                                    var url = routeSelector(target);
                                    url.open();
                                }
                            });
                            return {};
                        };
                        GridHandlers.createButtonClickPopupHandler = function (routeSelector, options) {
                            return function (e, original) {
                                e.preventDefault();
                                var dataItem = this.dataItem(e.currentTarget.closest("tr"));
                                var url = routeSelector(dataItem);
                                if (url === undefined || url === null) {
                                    return;
                                }
                                if (e.ctrlKey || original && original.which === 2) {
                                    url.open();
                                }
                                else {
                                    var dialogRoot_2 = $("<div></div>");
                                    var dialogOptions = {
                                        title: "",
                                        actions: ["close"],
                                        content: url.value,
                                        width: "800px",
                                        height: "600px",
                                        modal: true,
                                        visible: false,
                                        close: function () { return dialogRoot_2.data("kendoWindow").destroy(); }
                                    };
                                    if (options) {
                                        $.extend(dialogOptions, options(dataItem));
                                    }
                                    dialogRoot_2.kendoWindow(dialogOptions);
                                    var dialog_1 = dialogRoot_2.data("kendoWindow");
                                    dialog_1.center();
                                    dialog_1.open();
                                    $(window).on("resize", function (e) {
                                        if (!(dialog_1.element.closest("html").length === 0 || dialog_1.element.is(":hidden"))) {
                                            dialog_1.center();
                                        }
                                    });
                                }
                            };
                        };
                        return GridHandlers;
                    }());
                    Handlers.GridHandlers = GridHandlers;
                })(Handlers = Grids.Handlers || (Grids.Handlers = {}));
            })(Grids = Controls.Grids || (Controls.Grids = {}));
        })(Controls = AspNet.Controls || (AspNet.Controls = {}));
    })(AspNet = DevGuild.AspNet || (DevGuild.AspNet = {}));
})(DevGuild || (DevGuild = {}));
//# sourceMappingURL=grid-handlers.js.map