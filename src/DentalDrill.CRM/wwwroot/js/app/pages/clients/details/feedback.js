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
                    var FeedbackFormStatus;
                    (function (FeedbackFormStatus) {
                        FeedbackFormStatus[FeedbackFormStatus["New"] = 0] = "New";
                        FeedbackFormStatus[FeedbackFormStatus["Completed"] = 1] = "Completed";
                        FeedbackFormStatus[FeedbackFormStatus["Expired"] = 2] = "Expired";
                        FeedbackFormStatus[FeedbackFormStatus["Cancelled"] = 3] = "Cancelled";
                    })(FeedbackFormStatus || (FeedbackFormStatus = {}));
                    var ClientFeedbackFormsGrid = /** @class */ (function () {
                        function ClientFeedbackFormsGrid(root, clientId, questions) {
                            this._root = root;
                            this._clientId = clientId;
                            this._questions = questions;
                        }
                        ClientFeedbackFormsGrid.prototype.init = function () {
                            this._grid = this.createGrid();
                        };
                        ClientFeedbackFormsGrid.initialize = function (root, clientId, questions) {
                            var obj = new ClientFeedbackFormsGrid(root, clientId, questions);
                            obj.init();
                            ClientFeedbackFormsGrid._instance = obj;
                            return obj;
                        };
                        Object.defineProperty(ClientFeedbackFormsGrid, "instance", {
                            get: function () {
                                return ClientFeedbackFormsGrid._instance;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        ClientFeedbackFormsGrid.prototype.createGrid = function () {
                            var dataSource = this.createDataSource();
                            var gridContainer = $(this._root).find(".grid-container");
                            gridContainer.kendoGrid({
                                dataSource: dataSource,
                                columns: this.initializeColumns(),
                                pageable: true,
                            });
                            var grid = gridContainer.data("kendoGrid");
                            return grid;
                        };
                        ClientFeedbackFormsGrid.prototype.initializeColumns = function () {
                            var columns = [];
                            columns.push({
                                field: "CreatedOn",
                                title: "Created On",
                                template: "#if (data.CreatedOn) {# #:kendo.toString(data.CreatedOn, \"d\")#<br />#:kendo.toString(data.CreatedOn, \"t\")# #}#",
                            });
                            columns.push({
                                field: "SentOn",
                                title: "Sent On",
                                template: "#if (data.SentOn) {# #:kendo.toString(data.SentOn, \"d\")#<br />#:kendo.toString(data.SentOn, \"t\")# #}#",
                            });
                            columns.push({
                                field: "Status",
                                title: "Status",
                                template: function (data) {
                                    if (data === undefined || data === null) {
                                        return "";
                                    }
                                    switch (data.Status) {
                                        case FeedbackFormStatus.New:
                                            return "New";
                                        case FeedbackFormStatus.Completed:
                                            return "Completed";
                                        case FeedbackFormStatus.Expired:
                                            return "Expired";
                                        case FeedbackFormStatus.Cancelled:
                                            return "Cancelled";
                                    }
                                }
                            });
                            columns.push({
                                field: "TotalRating",
                                title: "Total Rating",
                            });
                            for (var _i = 0, _b = this._questions; _i < _b.length; _i++) {
                                var question = _b[_i];
                                var fieldName = "Answers_" + question.id.replace(/\-/g, "");
                                switch (question.type) {
                                    case 0:
                                        columns.push({
                                            field: fieldName,
                                            title: question.shortName
                                        });
                                        break;
                                    case 1:
                                        columns.push({
                                            field: fieldName,
                                            title: question.shortName
                                        });
                                        break;
                                }
                            }
                            columns.push({
                                command: [
                                    {
                                        name: "CustomDetails",
                                        text: "<span class=\"fas fa-fw fa-external-link-alt\"></span>",
                                        click: GridHandlers.createButtonClickPopupHandler(function (item) { return routes.clientFeedback.details(item.Id); }, function (item) { return ({
                                            title: "Feedback form",
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
                                        }); })
                                    }
                                ]
                            });
                            return columns;
                        };
                        ClientFeedbackFormsGrid.prototype.createDataSource = function () {
                            var dataSource = new kendo.data.DataSource({
                                type: "aspnetmvc-ajax",
                                transport: {
                                    read: {
                                        url: "/ClientFeedbackForms/Read?parentId=" + this._clientId
                                    }
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
                                },
                                sort: [
                                    { field: "CreatedOn", dir: "desc" }
                                ]
                            });
                            return dataSource;
                        };
                        ClientFeedbackFormsGrid.prototype.initializeSchemaModel = function () {
                            var fields = {
                                CreatedOn: { type: "Date" },
                                SentOn: { type: "Date" },
                                Status: { type: "number" },
                                TotalRating: { type: "number" },
                            };
                            for (var _i = 0, _b = this._questions; _i < _b.length; _i++) {
                                var question = _b[_i];
                                var fieldName = "Answers_" + question.id.replace(/\-/g, "");
                                var fieldSource = "Answers[\"" + question.id + "\"]";
                                switch (question.type) {
                                    case 0:
                                        fields[fieldName] = {
                                            type: "number",
                                            from: fieldSource + ".IntegerValue"
                                        };
                                        break;
                                    case 1:
                                        fields[fieldName] = {
                                            type: "string",
                                            from: fieldSource + ".StringValue"
                                        };
                                        break;
                                }
                            }
                            return fields;
                        };
                        var _a;
                        _a = ClientFeedbackFormsGrid;
                        ClientFeedbackFormsGrid.handleSendButton = GridHandlers.createGridButtonClickPopupHandler(".client-feedback-send-button", function (target) { return new DevGuild.AspNet.Routing.Uri(target.attr("href")); }, function (target) {
                            return {
                                title: "Send new feedback form",
                                width: "1000px",
                                height: "auto",
                                refresh: function (e) {
                                    e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                        clickEvent.preventDefault();
                                        e.sender.close();
                                        e.sender.destroy();
                                    });
                                    e.sender.center();
                                },
                                open: function (e) { return __awaiter(_a, void 0, void 0, function () {
                                    return __generator(_a, function (_b) {
                                        switch (_b.label) {
                                            case 0: return [4 /*yield*/, DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("ClientFeedbackFormsSendNewForm")];
                                            case 1:
                                                _b.sent();
                                                return [4 /*yield*/, ClientFeedbackFormsGrid.instance._grid.dataSource.read()];
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
                        return ClientFeedbackFormsGrid;
                    }());
                    Details.ClientFeedbackFormsGrid = ClientFeedbackFormsGrid;
                })(Details = Clients.Details || (Clients.Details = {}));
            })(Clients = Pages.Clients || (Pages.Clients = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=feedback.js.map