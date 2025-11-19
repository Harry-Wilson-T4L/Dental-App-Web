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
            var Jobs;
            (function (Jobs) {
                var Details;
                (function (Details) {
                    var GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
                    var AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;
                    var HandpiecesGrid = /** @class */ (function () {
                        function HandpiecesGrid() {
                        }
                        HandpiecesGrid.getInstance = function (jobId) {
                            return $("#JobHandpiecesGrid_" + jobId).data("kendoGrid");
                        };
                        HandpiecesGrid.statusTemplate = function (dataItem, options, statuses) {
                            var option = options.filter(function (x) { return x.source === dataItem.StatusId; })[0];
                            if (!option) {
                                return dataItem.Status;
                            }
                            var id = "Item_" + dataItem.Id.replace(/\-/g, "") + "_Status";
                            var html = "<select id=\"" + id + "\" class=\"handpiece-status-change__select\" data-item-id=\"" + dataItem.Id + "\">";
                            var _loop_1 = function (destination) {
                                var status_1 = statuses.filter(function (x) { return x.value === destination; })[0];
                                if (destination === dataItem.StatusId) {
                                    html += "<option value=\"" + destination + "\" selected=\"selected\">" + status_1.text + "</option>";
                                }
                                else {
                                    html += "<option value=\"" + destination + "\">" + status_1.text + "</option>";
                                }
                            };
                            for (var _i = 0, _b = option.destinations; _i < _b.length; _i++) {
                                var destination = _b[_i];
                                _loop_1(destination);
                            }
                            html += "</select>";
                            setTimeout(function () {
                                $("#" + id).kendoDropDownList();
                            });
                            return "" + html;
                        };
                        HandpiecesGrid.handleStatusSubmit = function (event) {
                            var inputsRoot = $(".handpiece-status-change__inputs");
                            inputsRoot.empty();
                            $("select.handpiece-status-change__select").each(function (index, element) {
                                var node = $(element);
                                var dropDown = node.data("kendoDropDownList");
                                var value = dropDown.value();
                                inputsRoot.append($("<input type=\"hidden\" />").attr("name", "Items[" + index + "].Id").attr("value", node.attr("data-item-id")));
                                inputsRoot.append($("<input type=\"hidden\" />").attr("name", "Items[" + index + "].Status").attr("value", value));
                            });
                            return true;
                        };
                        HandpiecesGrid.handleRead = function () {
                            var jobNumber = $("#JobNumberFilter").val();
                            var client = $("#ClientFilter").val();
                            var receivedFrom = $("#ReceivedFromFilter").data("kendoDatePicker").value();
                            var receivedTo = $("#ReceivedToFilter").data("kendoDatePicker").value();
                            var serial = $("#SerialFilter").val();
                            var makeAndModel = $("#MakeAndModelFilter").val();
                            var type = $("#TypeFilter").data("kendoDropDownList").dataItem();
                            return {
                                jobNumber: jobNumber,
                                client: client !== "None" ? client : null,
                                receivedFrom: receivedFrom ? receivedFrom.toISOString() : null,
                                receivedTo: receivedTo ? receivedTo.toISOString() : null,
                                serial: serial,
                                makeAndModel: makeAndModel !== "Any" ? makeAndModel : null,
                                type: type ? type.Value : null,
                            };
                        };
                        HandpiecesGrid.handleDataBound = function (e) {
                            e.sender.element.find("[data-toggle='tooltip']").tooltip();
                        };
                        var _a;
                        _a = HandpiecesGrid;
                        HandpiecesGrid.handleDetails = GridHandlers.createButtonClickNavigationHandler(function (item) { return routes.handpieces.details(item.Id); });
                        HandpiecesGrid.handleEdit = GridHandlers.createButtonClickNavigationHandler(function (item) { return routes.handpieces.edit(item.Id); });
                        HandpiecesGrid.handleDelete = GridHandlers.createButtonClickPopupHandler(function (item) { return routes.handpieces.delete(item.Id); }, function (item) { return ({
                            title: "Delete Handpiece " + item.Serial,
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
                                        case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("HandpiecesDelete")];
                                        case 1:
                                            _b.sent();
                                            return [4 /*yield*/, HandpiecesGrid.getInstance(item.JobId).dataSource.read()];
                                        case 2:
                                            _b.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        HandpiecesGrid.handleCreate = GridHandlers.createGridButtonClickPopupHandler(".job-handpieces-grid .k-grid-CustomCreate2", function (target) {
                            var gridElement = target.closest(".job-handpieces-grid");
                            return routes.handpieces.create(gridElement.attr("data-job-id"));
                        }, function (target) {
                            var gridElement = target.closest(".job-handpieces-grid");
                            var grid = gridElement.data("kendoGrid");
                            return {
                                title: "Add Handpiece",
                                width: "1000px",
                                height: "auto",
                                refresh: function (e) {
                                    e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                        clickEvent.preventDefault();
                                        e.sender.close();
                                        e.sender.destroy();
                                    });
                                    var form = document.querySelector("#HandpiecesCreate");
                                    if (form) {
                                        DentalDrill.CRM.Pages.Handpieces.Edit.HandpiecesEditForm.initialize(form);
                                    }
                                    e.sender.center();
                                },
                                open: function (e) { return __awaiter(_a, void 0, void 0, function () {
                                    return __generator(_a, function (_b) {
                                        switch (_b.label) {
                                            case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("HandpiecesCreate")];
                                            case 1:
                                                _b.sent();
                                                return [4 /*yield*/, grid.dataSource.read()];
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
                        return HandpiecesGrid;
                    }());
                    Details.HandpiecesGrid = HandpiecesGrid;
                    var JobInvoicesGrid = /** @class */ (function () {
                        function JobInvoicesGrid() {
                        }
                        JobInvoicesGrid.getInstance = function (jobId) {
                            return $("#JobInvoicesGrid_" + jobId).data("kendoGrid");
                        };
                        var _b;
                        _b = JobInvoicesGrid;
                        JobInvoicesGrid.handleDownload = GridHandlers.createButtonClickNavigationHandler(function (item) { return routes.jobInvoices.download(item.Id); });
                        JobInvoicesGrid.handleDelete = GridHandlers.createButtonClickPopupHandler(function (item) { return routes.jobInvoices.delete(item.Id); }, function (item) { return ({
                            title: "Delete Invoice " + item.FullInvoiceNumber,
                            width: "800px",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                e.sender.center();
                            },
                            open: function (e) { return __awaiter(_b, void 0, void 0, function () {
                                return __generator(_b, function (_c) {
                                    switch (_c.label) {
                                        case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("JobInvoicesDelete")];
                                        case 1:
                                            _c.sent();
                                            return [4 /*yield*/, JobInvoicesGrid.getInstance(item.JobId).dataSource.read()];
                                        case 2:
                                            _c.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        JobInvoicesGrid.handleCreate = GridHandlers.createGridButtonClickPopupHandler(".job-invoices-grid .k-grid-CustomCreate2", function (target) {
                            var gridElement = target.closest(".job-invoices-grid");
                            return routes.jobInvoices.create(gridElement.attr("data-job-id"));
                        }, function (target) {
                            var gridElement = target.closest(".job-invoices-grid");
                            var grid = gridElement.data("kendoGrid");
                            return {
                                title: "Upload Invoice",
                                width: "800px",
                                height: "auto",
                                refresh: function (e) {
                                    e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                        clickEvent.preventDefault();
                                        e.sender.close();
                                        e.sender.destroy();
                                    });
                                    e.sender.center();
                                },
                                open: function (e) { return __awaiter(_b, void 0, void 0, function () {
                                    return __generator(_b, function (_c) {
                                        switch (_c.label) {
                                            case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("JobInvoicesCreate")];
                                            case 1:
                                                _c.sent();
                                                return [4 /*yield*/, grid.dataSource.read()];
                                            case 2:
                                                _c.sent();
                                                e.sender.close();
                                                e.sender.destroy();
                                                return [2 /*return*/];
                                        }
                                    });
                                }); }
                            };
                        });
                        return JobInvoicesGrid;
                    }());
                    Details.JobInvoicesGrid = JobInvoicesGrid;
                    $(function () {
                        $("[data-toggle='tooltip']").tooltip();
                        //$("[data-toggle='tooltip']").tooltip({ trigger: 'manual' }).tooltip('show');
                        //$("[data-toggle='tooltip']")
                        //    .on("mouseenter", function () {
                        //        var _this = this;
                        //        $(this).tooltip("show");
                        //        $(".tooltip").on("mouseleave", function () {
                        //            $(_this).tooltip('hide');
                        //        });
                        //    }).on("mouseleave", function () {
                        //        var _this = this;
                        //        setTimeout(function () {
                        //            if (!$(".tooltip:hover").length) {
                        //                $(_this).tooltip("hide");
                        //            }
                        //        }, 300);
                        //    });
                    });
                })(Details = Jobs.Details || (Jobs.Details = {}));
            })(Jobs = Pages.Jobs || (Pages.Jobs = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=details.js.map