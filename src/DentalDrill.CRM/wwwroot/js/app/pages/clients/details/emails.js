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
            var Clients;
            (function (Clients) {
                var Details;
                (function (Details) {
                    var GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
                    var ClientEmailsGrid = /** @class */ (function () {
                        function ClientEmailsGrid() {
                        }
                        Object.defineProperty(ClientEmailsGrid, "instance", {
                            get: function () {
                                return $("#ClientEmailsGrid").data("kendoGrid");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        var _a;
                        _a = ClientEmailsGrid;
                        ClientEmailsGrid.handleDetails = GridHandlers.createButtonClickPopupHandler(function (item) { return routes.clientEmails.details(item.Id); }, function (item) { return ({
                            title: item.Subject,
                            width: "1000px",
                            height: "800px",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                e.sender.center();
                            }
                        }); });
                        ClientEmailsGrid.handleSendButton = GridHandlers.createGridButtonClickPopupHandler(".client-email-send-button", function (target) { return new DevGuild.AspNet.Routing.Uri(target.attr("href")); }, function (target) {
                            var windowTitle = target.attr("data-title");
                            var hybridFormId = target.attr("data-hybrid-id");
                            var fixedHeight = target.attr("data-height");
                            return {
                                title: windowTitle,
                                width: "1000px",
                                height: fixedHeight ? fixedHeight : "auto",
                                refresh: function (e) {
                                    e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                        clickEvent.preventDefault();
                                        e.sender.close();
                                        e.sender.destroy();
                                    });
                                    if (!fixedHeight) {
                                        e.sender.center();
                                    }
                                },
                                open: function (e) { return __awaiter(_a, void 0, void 0, function () {
                                    return __generator(_a, function (_b) {
                                        switch (_b.label) {
                                            case 0: return [4 /*yield*/, DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor(hybridFormId)];
                                            case 1:
                                                _b.sent();
                                                return [4 /*yield*/, ClientEmailsGrid.instance.dataSource.read()];
                                            case 2:
                                                _b.sent();
                                                e.sender.close();
                                                e.sender.destroy();
                                                return [2 /*return*/];
                                        }
                                    });
                                }); }
                            };
                        });
                        return ClientEmailsGrid;
                    }());
                    Details.ClientEmailsGrid = ClientEmailsGrid;
                })(Details = Clients.Details || (Clients.Details = {}));
            })(Clients = Pages.Clients || (Pages.Clients = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=emails.js.map