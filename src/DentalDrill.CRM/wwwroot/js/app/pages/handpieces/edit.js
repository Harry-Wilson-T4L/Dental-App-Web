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
            var Handpieces;
            (function (Handpieces) {
                var Edit;
                (function (Edit) {
                    var EventHandler = DevGuild.Utilities.EventHandler;
                    var HandpieceRequiredPartGrid = DentalDrill.CRM.Pages.HandpieceRequiredParts.Index.HandpieceRequiredPartGrid;
                    var HandpieceRequiredPartStatusHelper = DentalDrill.CRM.Pages.HandpieceRequiredParts.Index.HandpieceRequiredPartStatusHelper;
                    var HandpieceRequiredPartStatus = DentalDrill.CRM.Pages.HandpieceRequiredParts.Index.HandpieceRequiredPartStatus;
                    var HandpiecesEditForm = /** @class */ (function () {
                        function HandpiecesEditForm() {
                        }
                        HandpiecesEditForm.trackFormChanges = function (form) {
                            var formNode = $(form);
                            formNode.on("change", "input", function (e) {
                                if ($(e.target).attr("data-ignore-change") === "true") {
                                    return;
                                }
                                formNode.data("hasChanges", true);
                                console.log("Change tracked");
                            });
                            formNode.on("change", "select", function (e) {
                                if ($(e.target).attr("data-ignore-change") === "true") {
                                    return;
                                }
                                formNode.data("hasChanges", true);
                                console.log("Change tracked");
                            });
                            formNode.on("change", "textarea", function (e) {
                                if ($(e.target).attr("data-ignore-change") === "true") {
                                    return;
                                }
                                formNode.data("hasChanges", true);
                                console.log("Change tracked");
                            });
                            formNode.on("click", "input[type=checkbox]", function (e) {
                                if ($(e.target).attr("data-ignore-change") === "true") {
                                    return;
                                }
                                formNode.data("hasChanges", true);
                                console.log("Change tracked");
                            });
                            window.addEventListener("beforeunload", function (e) {
                                var hasChanges = formNode.data("hasChanges");
                                if (hasChanges) {
                                    e.returnValue = "Are you sure you want to leave this page? All changes will be lost.";
                                    return "Are you sure you want to leave this page? All changes will be lost.";
                                }
                                return null;
                            }, false);
                        };
                        HandpiecesEditForm.initialize = function (form) {
                            HandpiecesEditForm.trackFormChanges(form);
                            var submitButtons = form.querySelectorAll("button[type='submit']");
                            var _loop_1 = function (i) {
                                var submitButton = submitButtons[i];
                                submitButton.addEventListener("click", function () {
                                    if (submitButton.getAttribute("name") === "Command") {
                                        var input = document.querySelector("#CommandInput");
                                        input.value = submitButton.value;
                                    }
                                }, false);
                            };
                            for (var i = 0; i < submitButtons.length; i++) {
                                _loop_1(i);
                            }
                            form.addEventListener("submit", function (e) {
                                e.preventDefault();
                                $(form).data("hasChanges", false);
                                HandpiecesEditForm.tryUpdateComponents(form);
                                HandpiecesEditForm.tryUpdateDiagnostics(form);
                                form.submit();
                            }, false);
                            $("input[type=text]").on("focus", function () {
                                var input = $(this);
                                clearTimeout(input.data("selectTimeId")); //stop started time out if any
                                var selectTimeId = setTimeout(function () {
                                    input.select();
                                    // To make this work on iOS, too, replace the above line with the following one. Discussed in https://stackoverflow.com/q/3272089
                                    // input[0].setSelectionRange(0, 9999);
                                });
                                input.data("selectTimeId", selectTimeId);
                            }).blur(function (e) {
                                clearTimeout($(this).data("selectTimeId")); //stop started timeout
                            });
                            // If same diagnostic check is present on multiple tabs - all instances would select/deselect automatically.
                            $(document).on("change", "input.diagnostic-check__selected", function (e) {
                                var target = e.target;
                                var container = $(target).closest(".handpiece-diagnostics");
                                var matchingValue = container.find("input.diagnostic-check__selected[value='" + target.value + "']");
                                matchingValue.each(function (index, element) {
                                    if (element.id !== target.id) {
                                        element.checked = target.checked;
                                    }
                                });
                                var list = $(".handpiece-diagnostic-checked-list");
                                var report = $(".handpiece-diagnostic-report");
                                if (target.checked) {
                                    if (list.find("input[name=DiagnosticReportChecked][value=" + target.value + "]").length === 0) {
                                        list.append($("<input />")
                                            .attr("id", "DiagnosticReportChecked_" + target.value)
                                            .attr("name", "DiagnosticReportChecked")
                                            .attr("type", "hidden")
                                            .val(target.value));
                                    }
                                    if (report.find(".handpiece-diagnostic-report__item[data-diagnostic-id=" + target.value + "]").length === 0) {
                                        var label = container.find("label[for=" + target.id + "]");
                                        var reportItem = $("<span><span>")
                                            .addClass("handpiece-diagnostic-report__item")
                                            .addClass("handpiece-diagnostic-report__predefined")
                                            .attr("data-diagnostic-id", target.value)
                                            .text(label.text())
                                            .append($("<span></span>")
                                            .addClass("handpiece-diagnostic-report__item__separator")
                                            .text(", "));
                                        var otherItem = report.find(".handpiece-diagnostic-report__item.handpiece-diagnostic-report__other");
                                        if (otherItem.length !== 0) {
                                            reportItem.insertBefore(otherItem);
                                        }
                                        else {
                                            reportItem.appendTo(report);
                                        }
                                    }
                                }
                                else {
                                    list.find("input[name=DiagnosticReportChecked][value=" + target.value + "]").remove();
                                    report.find(".handpiece-diagnostic-report__item[data-diagnostic-id=" + target.value + "]").remove();
                                }
                            });
                            $(document).on("change", "input.diagnostic-check-other__selected", function (e) {
                                var target = e.target;
                                var report = $(".handpiece-diagnostic-report");
                                if (target.checked) {
                                    var otherItem = report.find(".handpiece-diagnostic-report__item.handpiece-diagnostic-report__other");
                                    if (otherItem.length === 0) {
                                        otherItem = $("<span><span>")
                                            .addClass("handpiece-diagnostic-report__item")
                                            .addClass("handpiece-diagnostic-report__other")
                                            .text($(".diagnostic-check-other__text").val());
                                        otherItem.appendTo(report);
                                    }
                                    else {
                                        otherItem.text($(".diagnostic-check-other__text").val());
                                    }
                                }
                                else {
                                    report.find(".handpiece-diagnostic-report__item.handpiece-diagnostic-report__other").remove();
                                }
                            });
                            $(document).on("change", "input.diagnostic-check-other__text", function (e) {
                                var target = e.target;
                                var report = $(".handpiece-diagnostic-report");
                                var otherItem = report.find(".handpiece-diagnostic-report__item.handpiece-diagnostic-report__other");
                                if (otherItem.length > 0) {
                                    otherItem.text(target.value);
                                }
                            });
                            // Having input-validation-error class on kendo control would prevent their submit.
                            $(".k-widget").removeClass("input-validation-error");
                            // Link Service Levels and Cost of repairs
                            var serviceLevel = $(form).find("#ServiceLevelId").data("kendoDropDownList");
                            var costOfRepair = $(form).find("#CostOfRepair").data("kendoNumericTextBox");
                            if (serviceLevel && costOfRepair) {
                                var previousValue_1 = serviceLevel.dataItem();
                                serviceLevel.bind("change", function (e) {
                                    var selected = e.sender.dataItem();
                                    if (selected && selected.CostOfRepair !== undefined && selected.CostOfRepair !== null) {
                                        if (costOfRepair.value() === undefined || costOfRepair.value() === null || (previousValue_1 && costOfRepair.value() === previousValue_1.CostOfRepair)) {
                                            costOfRepair.value(selected.CostOfRepair);
                                        }
                                    }
                                    previousValue_1 = selected;
                                });
                            }
                            $(form).on("click", ".handpiece-brand-model-serial__add-component", function (e) {
                                HandpiecesEditForm.addComponent(form, undefined);
                            });
                            $(form).on("click", ".handpiece-brand-model-serial__remove-component", function (e) {
                                $(e.target).parents(".handpiece-brand-model-serial--component").remove();
                            });
                            var enableAutoFullScreenExpand = function (kendo, dropDownList) {
                                if (dropDownList === undefined) {
                                    return;
                                }
                                dropDownList.popup.bind('open', function (e) {
                                    e.sender.options.collision = 'none none';
                                    setTimeout(function () {
                                        var popup = e.sender, popupWrapper = e.sender.wrapper, listContainer = e.sender.element, listScroller = e.sender.element.find(".k-list-scroller");
                                        var dropDownListOffset = kendo.getOffset(dropDownList.wrapper);
                                        var headerHeight = kendo._outerHeight($("nav.navbar"));
                                        var popupTop = window.scrollY + headerHeight + 16;
                                        var popupLeft = dropDownListOffset.left;
                                        var popupHeight = window.innerHeight - headerHeight - 32;
                                        var scrollerHeight = window.innerHeight - headerHeight - 32 - 50;
                                        popup._location({ isFixed: true, x: popupLeft, y: popupTop });
                                        popup.wrapper.css({ top: popupTop + "px", left: popupLeft + "px", height: popupHeight + "px" });
                                        listContainer.css({ height: popupHeight + "px" });
                                        listScroller.css({ height: scrollerHeight + "px" });
                                    });
                                });
                            };
                            enableAutoFullScreenExpand(kendo, $("#ServiceLevelId").data("kendoDropDownList"));
                        };
                        HandpiecesEditForm.addComponent = function (form, item) {
                            var container = $(form).find(".handpiece-components-wrapper");
                            var itemId = Math.random().toString(10).substr(3, 12);
                            var control = $("<div></div>")
                                .addClass("row handpiece-brand-model-serial handpiece-brand-model-serial--component")
                                .append($("<div></div>")
                                .addClass("col-12 mb-2 handpiece-brand-model-serial__serial")
                                .append($("<div></div>")
                                .addClass("row")
                                .append($("<div></div>")
                                .addClass("col-sm-5 col-lg-3 col-xl-5")
                                .append($("<label></label>")
                                .addClass("col-form-label")
                                .attr("for", "Components_" + itemId + "_Serial")
                                .text("Serial #"))
                                .append($("<button></button>")
                                .addClass("k-button handpiece-brand-model-serial__remove-component")
                                .attr("type", "button")
                                .html("<span class=\"fas fa-minus\"></span>")))
                                .append($("<div></div>")
                                .addClass("col-sm-7 col-lg-9 col-xl-7")
                                .append($("<input />")
                                .attr("id", "Components_" + itemId + "_Serial")
                                .attr("name", "Components[" + itemId + "].Serial")
                                .attr("type", "text")
                                .val(item && item.Serial ? item.Serial : ""))
                                .append($("<span></span>")
                                .addClass("field-validation-valid")
                                .attr("data-valmsg-for", "Components[" + itemId + "].Serial")
                                .attr("data-valmsg-replace", "true")))))
                                .append($("<div></div>")
                                .addClass("col-12 mb-2 handpiece-brand-model-serial__brand")
                                .append($("<div></div>")
                                .addClass("row")
                                .append($("<div></div>")
                                .addClass("col-sm-5 col-lg-3 col-xl-3")
                                .append($("<label></label>")
                                .addClass("col-form-label")
                                .attr("for", "Components_" + itemId + "_Brand")
                                .text("Brand")))
                                .append($("<div></div>")
                                .addClass("col-sm-7 col-lg-9 col-xl-9")
                                .append($("<input />")
                                .attr("id", "Components_" + itemId + "_Brand")
                                .attr("name", "Components[" + itemId + "].Brand")
                                .attr("type", "text")
                                .val(item && item.Brand ? item.Brand : ""))
                                .append($("<span></span>")
                                .addClass("field-validation-valid")
                                .attr("data-valmsg-for", "Components[" + itemId + "].Brand")
                                .attr("data-valmsg-replace", "true")))))
                                .append($("<div></div>")
                                .addClass("col-12 mb-2 handpiece-brand-model-serial__model")
                                .append($("<div></div>")
                                .addClass("row")
                                .append($("<div></div>")
                                .addClass("col-sm-5 col-lg-3 col-xl-3")
                                .append($("<label></label>")
                                .addClass("col-form-label")
                                .attr("for", "Components_" + itemId + "_MakeAndModel")
                                .text("Model")))
                                .append($("<div></div>")
                                .addClass("col-sm-7 col-lg-9 col-xl-9")
                                .append($("<input />")
                                .attr("id", "Components_" + itemId + "_MakeAndModel")
                                .attr("name", "Components[" + itemId + "].MakeAndModel")
                                .attr("type", "text")
                                .val(item && item.Model ? item.Model : ""))
                                .append($("<span></span>")
                                .addClass("field-validation-valid")
                                .attr("data-valmsg-for", "Components[" + itemId + "].MakeAndModel")
                                .attr("data-valmsg-replace", "true")))));
                            container.append(control);
                            $("#Components_" + itemId + "_Serial").kendoTextBox({});
                            $("#Components_" + itemId + "_Brand").kendoTextBox({});
                            $("#Components_" + itemId + "_MakeAndModel").kendoTextBox({});
                        };
                        HandpiecesEditForm.handleExistingHandpieceChange = function (e) {
                            var form = e.sender.wrapper.closest("form")[0];
                            var selectedItem = e.sender.dataItem();
                            if (selectedItem) {
                                if (selectedItem.Type === 0) {
                                    $(form).find("#Brand").data("kendoTextBox").value(selectedItem.Brand);
                                    $(form).find("#MakeAndModel").data("kendoTextBox").value(selectedItem.Model);
                                    $(form).find("#Serial").data("kendoTextBox").value(selectedItem.Serial);
                                    $(form).find(".handpiece-brand-model-serial--component").each(function (index, element) {
                                        $(element).remove();
                                    });
                                    for (var _i = 0, _a = selectedItem.Components; _i < _a.length; _i++) {
                                        var component = _a[_i];
                                        HandpiecesEditForm.addComponent(form, component);
                                    }
                                }
                            }
                        };
                        HandpiecesEditForm.tryUpdateComponents = function (form) {
                            var componentsNodes = form.querySelectorAll(".handpiece-brand-model-serial--component");
                            for (var i = 0; i < componentsNodes.length; i++) {
                                var inputs = componentsNodes[i].querySelectorAll("input[name]");
                                for (var j = 0; j < inputs.length; j++) {
                                    var input = inputs[j];
                                    input.name = input.name.replace(/Components\[[-0-9]+\]/, "Components[" + i + "]");
                                }
                            }
                        };
                        HandpiecesEditForm.initDiagnostics = function () {
                            $("input[type=checkbox].diagnostic-check__selected").each(function (index, element) {
                                element.checked = false;
                            });
                            var container = $(".handpiece-diagnostics");
                            var list = $(".handpiece-diagnostic-checked-list");
                            var checkedItems = list.find("input[name=DiagnosticReportChecked]");
                            checkedItems.each(function (index, element) {
                                var itemId = element.value;
                                var itemInputs = container.find("input.diagnostic-check__selected[value='" + itemId + "']");
                                itemInputs.each(function (index, element) {
                                    element.checked = true;
                                });
                            });
                            var otherTextInput = document.querySelector("#DiagnosticReportOther");
                            var otherCheck = document.querySelector(".handpiece-diagnostics").querySelector(".diagnostic-check-other");
                            var otherCheckSelectedCheckbox = otherCheck.querySelector(".diagnostic-check-other__selected");
                            var otherCheckTextInput = otherCheck.querySelector(".diagnostic-check-other__text");
                            if (otherTextInput.value) {
                                otherCheckTextInput.value = otherTextInput.value;
                                otherCheckTextInput.classList.remove("d-none");
                                otherCheckSelectedCheckbox.checked = true;
                            }
                            else {
                                otherCheckTextInput.value = "";
                                otherCheckTextInput.classList.add("d-none");
                                otherCheckSelectedCheckbox.checked = false;
                            }
                        };
                        HandpiecesEditForm.tryUpdateDiagnostics = function (form) {
                            var otherTextInput = form.querySelector("#DiagnosticReportOther");
                            var itemsContainer = document.querySelector("#DiagnosticReportItems");
                            if (otherTextInput && itemsContainer) {
                                var otherCheck = itemsContainer.querySelector(".diagnostic-check-other");
                                var otherCheckSelectedCheckbox = otherCheck.querySelector(".diagnostic-check-other__selected");
                                var otherCheckTextInput = otherCheck.querySelector(".diagnostic-check-other__text");
                                if (otherCheckSelectedCheckbox.checked) {
                                    otherTextInput.value = otherCheckTextInput.value;
                                }
                                else {
                                    otherTextInput.value = "";
                                }
                            }
                        };
                        return HandpiecesEditForm;
                    }());
                    Edit.HandpiecesEditForm = HandpiecesEditForm;
                    $(function () {
                        var form = document.querySelector("#HandpiecesEditForm");
                        if (form) {
                            HandpiecesEditForm.initialize(form);
                            $("[data-toggle='tooltip']").tooltip();
                            var partsMultiselect = $("#HandpieceRequiredPartsMultiSelect");
                            if (partsMultiselect.length > 0) {
                                var handpieceId_1 = partsMultiselect.attr("data-handpiece");
                                var isReadOnly = partsMultiselect.attr("data-readonly") === "true" || partsMultiselect.attr("data-readonly") === "True";
                                var isRemoveOnly = partsMultiselect.attr("data-removeonly") === "true" || partsMultiselect.attr("data-removeonly") === "True";
                                var multiselect_1 = new HandpieceRequiredPartsMultiSelect(handpieceId_1, partsMultiselect, !isReadOnly && !isRemoveOnly, !isReadOnly);
                                var changeStatusSelect_1 = $(form).find("#ChangeStatus").data("kendoDropDownList");
                                if (changeStatusSelect_1 !== undefined) {
                                    multiselect_1.changed.subscribe(function (sender, e) {
                                        multiselect_1.api.checkStatus(handpieceId_1).then(function (response) {
                                            if (changeStatusSelect_1.value() === "BeingRepaired" && (response.StockStatus === HandpiecePartsStockStatus.OutOfStock || response.StockStatus === HandpiecePartsStockStatus.PartialStock)) {
                                                changeStatusSelect_1.value("WaitingForParts");
                                            }
                                            else if (changeStatusSelect_1.value() === "WaitingForParts" && (response.StockStatus === HandpiecePartsStockStatus.InStock)) {
                                                changeStatusSelect_1.value("BeingRepaired");
                                            }
                                        });
                                    });
                                }
                            }
                        }
                    });
                    var AlertsContainer = /** @class */ (function () {
                        function AlertsContainer(root) {
                            this._root = root;
                        }
                        AlertsContainer.prototype.primary = function (text, dismissAfter, manualDismissal) {
                            this.render("primary", text, dismissAfter, manualDismissal);
                        };
                        AlertsContainer.prototype.secondary = function (text, dismissAfter, manualDismissal) {
                            this.render("secondary", text, dismissAfter, manualDismissal);
                        };
                        AlertsContainer.prototype.success = function (text, dismissAfter, manualDismissal) {
                            this.render("success", text, dismissAfter, manualDismissal);
                        };
                        AlertsContainer.prototype.danger = function (text, dismissAfter, manualDismissal) {
                            this.render("danger", text, dismissAfter, manualDismissal);
                        };
                        AlertsContainer.prototype.warning = function (text, dismissAfter, manualDismissal) {
                            this.render("warning", text, dismissAfter, manualDismissal);
                        };
                        AlertsContainer.prototype.info = function (text, dismissAfter, manualDismissal) {
                            this.render("info", text, dismissAfter, manualDismissal);
                        };
                        AlertsContainer.prototype.light = function (text, dismissAfter, manualDismissal) {
                            this.render("light", text, dismissAfter, manualDismissal);
                        };
                        AlertsContainer.prototype.dark = function (text, dismissAfter, manualDismissal) {
                            this.render("dark", text, dismissAfter, manualDismissal);
                        };
                        AlertsContainer.prototype.render = function (type, text, dismissAfter, manualDismissal) {
                            var node = $("<div></div>");
                            node.addClass("alert");
                            node.addClass("alert-" + type);
                            node.attr("role", "alert");
                            node.text(text);
                            if (dismissAfter !== undefined && typeof dismissAfter == "number") {
                                setTimeout(function () {
                                    node.fadeOut("slow", function () {
                                        node.remove();
                                    });
                                }, dismissAfter);
                            }
                            if (manualDismissal) {
                                node.addClass("alert-dismissible");
                                node.addClass("fade");
                                node.addClass("show");
                                var dismissButton = $("<button type=\"button\"></button>");
                                dismissButton.addClass("close");
                                dismissButton.attr("data-dismiss", "alert");
                                dismissButton.attr("aria-label", "Close");
                                dismissButton.html("<span aria-hidden=\"true\">&times;</span>");
                                node.append(dismissButton);
                            }
                            this._root.append(node);
                        };
                        return AlertsContainer;
                    }());
                    Edit.AlertsContainer = AlertsContainer;
                    var HandpieceAssistant = /** @class */ (function () {
                        function HandpieceAssistant(root) {
                            this._root = root;
                            this._modelId = root.attr("data-model-id");
                            this._listView = this._root.find(".handpiece-assistant-list").data("kendoListView");
                            this._notesEditor = this._root.find(".handpiece-assistant-notes__editor").data("kendoEditor");
                            this._saveButton = this._root.find(".handpiece-assistant-notes__save");
                            this._cancelButton = this._root.find(".handpiece-assistant-notes__cancel");
                            this._notesAlerts = new AlertsContainer(this._root.find(".handpiece-assistant-notes__alerts"));
                            this._windowWrapper = undefined;
                            this._savedNotes = this._notesEditor.value();
                            this._saveButton.on("click", this.handleSaveNotes.bind(this));
                            this._cancelButton.on("click", this.handleCancelNotes.bind(this));
                            this._listView.wrapper.on("click", ".handpiece-assistant-schematic", this.handleSchematicsClick.bind(this));
                        }
                        HandpieceAssistant.init = function (root) {
                            var assistant = new HandpieceAssistant(root);
                            root.data("HandpieceAssistant", assistant);
                        };
                        HandpieceAssistant.prototype.handleSaveNotes = function (e) {
                            return __awaiter(this, void 0, void 0, function () {
                                var response, exception_1;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            e.preventDefault();
                                            _a.label = 1;
                                        case 1:
                                            _a.trys.push([1, 3, , 4]);
                                            this._savedNotes = this._notesEditor.value();
                                            return [4 /*yield*/, fetch("/HandpieceAssistant/UpdateNotes/" + this._modelId, {
                                                    method: "POST",
                                                    credentials: "same-origin",
                                                    cache: "no-cache",
                                                    headers: {
                                                        "Content-Type": "application/json"
                                                    },
                                                    body: JSON.stringify({ Notes: this._savedNotes })
                                                })];
                                        case 2:
                                            response = _a.sent();
                                            if (response.status === 204) {
                                                this._notesAlerts.success("Notes saved", 5000, true);
                                            }
                                            else {
                                                this._notesAlerts.danger("Unable to save notes", undefined, true);
                                            }
                                            return [3 /*break*/, 4];
                                        case 3:
                                            exception_1 = _a.sent();
                                            this._notesAlerts.danger("Unable to save notes", undefined, true);
                                            return [3 /*break*/, 4];
                                        case 4: return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        HandpieceAssistant.prototype.handleCancelNotes = function (e) {
                            e.preventDefault();
                            this._notesEditor.value(this._savedNotes);
                        };
                        HandpieceAssistant.prototype.handleSchematicsClick = function (e) {
                            var target = $(e.target).closest(".handpiece-assistant-schematic");
                            var id = target.attr("data-id");
                            if (this.initWindow(id)) {
                                this._window.open();
                                this._window.center();
                                this.selectPage(id);
                            }
                            else {
                                this._window.open();
                                this._window.center();
                            }
                        };
                        HandpieceAssistant.prototype.initWindow = function (id) {
                            if (this._window) {
                                if (this.updateViewPortSize()) {
                                    this._window.setOptions({
                                        width: Math.round(this._viewPortSize.width * 0.9) + "px",
                                        height: Math.round(this._viewPortSize.height * 0.9) + "px",
                                    });
                                }
                                return true;
                            }
                            this.updateViewPortSize();
                            var wrapper = this.getWindowWrapper();
                            wrapper.kendoWindow({
                                width: Math.round(this._viewPortSize.width * 0.9) + "px",
                                height: Math.round(this._viewPortSize.height * 0.9) + "px",
                                title: "Handpiece Assistant",
                                actions: ["close"],
                                modal: true,
                                visible: false,
                                content: "/HandpieceAssistant/Preview/" + this._modelId + "?schematicId=" + id,
                                refresh: function (e) {
                                    e.sender.center();
                                }
                            });
                            this._window = wrapper.data("kendoWindow");
                            return false;
                        };
                        HandpieceAssistant.prototype.updateViewPortSize = function () {
                            var viewPortSize = {
                                width: $(window).width(),
                                height: $(window).height(),
                            };
                            if (!this._viewPortSize) {
                                this._viewPortSize = viewPortSize;
                                return true;
                            }
                            if (this._viewPortSize.width === viewPortSize.width && this._viewPortSize.height == viewPortSize.height) {
                                return false;
                            }
                            this._viewPortSize = viewPortSize;
                            return true;
                        };
                        HandpieceAssistant.prototype.getWindowWrapper = function () {
                            if (!this._windowWrapper) {
                                this._windowWrapper = $("<div></div>").addClass("handpiece-assistant-window");
                            }
                            return this._windowWrapper;
                        };
                        HandpieceAssistant.prototype.selectPage = function (pageId) {
                            var scrollViewWrapper = this._window.wrapper.find(".handpiece-assistant-scrollview");
                            if (scrollViewWrapper.length === 0) {
                                return;
                            }
                            var page = scrollViewWrapper.find(".handpiece-assistant-schematic-page[data-id='" + pageId + "']");
                            if (page.length === 0) {
                                return;
                            }
                            try {
                                var pageNo = parseInt(page.attr("data-index"));
                                var scrollView = scrollViewWrapper.data("kendoScrollView");
                                scrollView.scrollTo(pageNo, false);
                            }
                            catch (exception) { }
                        };
                        return HandpieceAssistant;
                    }());
                    Edit.HandpieceAssistant = HandpieceAssistant;
                    var HandpiecePartsStockStatus;
                    (function (HandpiecePartsStockStatus) {
                        HandpiecePartsStockStatus[HandpiecePartsStockStatus["InStock"] = 0] = "InStock";
                        HandpiecePartsStockStatus[HandpiecePartsStockStatus["OutOfStock"] = 1] = "OutOfStock";
                        HandpiecePartsStockStatus[HandpiecePartsStockStatus["PartialStock"] = 2] = "PartialStock";
                    })(HandpiecePartsStockStatus || (HandpiecePartsStockStatus = {}));
                    var HandpieceRequiredPartsApi = /** @class */ (function () {
                        function HandpieceRequiredPartsApi() {
                        }
                        HandpieceRequiredPartsApi.prototype.add = function (handpieceId, sku, quantity) {
                            return __awaiter(this, void 0, void 0, function () {
                                var response, json;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, fetch("/HandpieceRequiredParts/ApiAdd?parentId=" + handpieceId, {
                                                method: "POST",
                                                credentials: "same-origin",
                                                headers: {
                                                    "Content-Type": "application/json",
                                                    "X-Requested-With": "XMLHttpRequest",
                                                },
                                                body: JSON.stringify({
                                                    SKU: sku,
                                                    Quantity: quantity,
                                                }),
                                            })];
                                        case 1:
                                            response = _a.sent();
                                            if (response.status !== 200) {
                                                throw new Error("Failed to add required part");
                                            }
                                            return [4 /*yield*/, response.json()];
                                        case 2:
                                            json = _a.sent();
                                            return [2 /*return*/, json];
                                    }
                                });
                            });
                        };
                        HandpieceRequiredPartsApi.prototype.update = function (handpieceId, id, oldQuanity, newQuantity) {
                            return __awaiter(this, void 0, void 0, function () {
                                var response, json;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, fetch("/HandpieceRequiredParts/ApiUpdate/" + id + "?parentId=" + handpieceId, {
                                                method: "POST",
                                                credentials: "same-origin",
                                                headers: {
                                                    "Content-Type": "application/json",
                                                    "X-Requested-With": "XMLHttpRequest"
                                                },
                                                body: JSON.stringify({
                                                    OldQuantity: oldQuanity,
                                                    NewQuantity: newQuantity,
                                                }),
                                            })];
                                        case 1:
                                            response = _a.sent();
                                            if (response.status !== 200) {
                                                throw new Error("Failed to update required part");
                                            }
                                            return [4 /*yield*/, response.json()];
                                        case 2:
                                            json = _a.sent();
                                            return [2 /*return*/, json];
                                    }
                                });
                            });
                        };
                        HandpieceRequiredPartsApi.prototype.checkStatus = function (handpieceId) {
                            return __awaiter(this, void 0, void 0, function () {
                                var response, json;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, fetch("/HandpieceRequiredParts/ApiCheckStatus?parentId=" + handpieceId, {
                                                method: "POST",
                                                credentials: "same-origin",
                                                headers: {
                                                    "Content-Type": "application/json",
                                                    "X-Requested-With": "XMLHttpRequest"
                                                },
                                                body: JSON.stringify({}),
                                            })];
                                        case 1:
                                            response = _a.sent();
                                            if (response.status !== 200) {
                                                throw new Error("Failed to check handpiece status");
                                            }
                                            return [4 /*yield*/, response.json()];
                                        case 2:
                                            json = _a.sent();
                                            return [2 /*return*/, json];
                                    }
                                });
                            });
                        };
                        return HandpieceRequiredPartsApi;
                    }());
                    var HandpieceRequiredPartsMultiSelect = /** @class */ (function () {
                        function HandpieceRequiredPartsMultiSelect(handpieceId, multiselectWrapper, canAdd, canRemove) {
                            this._changed = new EventHandler();
                            this._globalValues = new Map();
                            this._globalValuesBySKUId = new Map();
                            this._api = new HandpieceRequiredPartsApi();
                            this._handpieceId = handpieceId;
                            this._multiselectWrapper = multiselectWrapper;
                            this._canAdd = canAdd;
                            this._canRemove = canRemove;
                            this._multiselect = this.initMultiSelect();
                            this._multiselectWrapper.data("HandpieceRequiredPartsMultiSelect", this);
                        }
                        Object.defineProperty(HandpieceRequiredPartsMultiSelect.prototype, "api", {
                            get: function () {
                                return this._api;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(HandpieceRequiredPartsMultiSelect.prototype, "changed", {
                            get: function () {
                                return this._changed;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        HandpieceRequiredPartsMultiSelect.prototype.refresh = function () {
                            var selectedItems = this._multiselect.dataItems();
                            this._multiselect.tagList.html("");
                            for (var _i = 0, selectedItems_1 = selectedItems; _i < selectedItems_1.length; _i++) {
                                var item = selectedItems_1[_i];
                                this._multiselect.tagList.append(this._multiselect["tagTemplate"](item));
                            }
                        };
                        HandpieceRequiredPartsMultiSelect.prototype.initMultiSelect = function () {
                            var _this = this;
                            var values = [];
                            var preloadedOptions = this._multiselectWrapper.find("option");
                            preloadedOptions.each(function (index, element) {
                                var value = {
                                    Id: $(element).attr("data-id"),
                                    SKUId: $(element).attr("data-sku-id"),
                                    Name: $(element).attr("data-sku-name"),
                                    RequiredQuantity: parseFloat($(element).attr("data-quantity")),
                                    ShelfQuantity: parseFloat($(element).attr("data-shelf-quantity")),
                                    Price: parseFloat($(element).attr("data-price")),
                                    Status: parseInt($(element).attr("data-status")),
                                };
                                values.push(value);
                                _this._globalValues.set(value.Id, value);
                                _this._globalValuesBySKUId.set(value.SKUId, value);
                            });
                            preloadedOptions.remove();
                            ////let multiSelectHeight = 200;
                            ////if (window.innerHeight < 1000) {
                            ////    multiSelectHeight = 175;
                            ////}
                            ////if (window.innerHeight < 800) {
                            ////    multiSelectHeight = 150;
                            ////}
                            this._multiselectWrapper.kendoMultiSelect({
                                dataTextField: "Name",
                                dataValueField: "Id",
                                dataSource: this.createDataSource(),
                                filter: "contains",
                                autoClose: false,
                                value: values,
                                height: window.innerHeight - 32,
                                headerTemplate: function () {
                                    var headerRoot = document.createElement("div");
                                    var gridLabel = headerRoot.appendChild(document.createElement("div"));
                                    gridLabel.classList.add("parts-selector-header");
                                    gridLabel.style.paddingLeft = "1.5rem";
                                    gridLabel.style.fontWeight = "bold";
                                    gridLabel.innerText = "Required Parts";
                                    var gridWrapper = headerRoot.appendChild(document.createElement("div"));
                                    gridWrapper.classList.add("multiselect-grid-wrapper", "parts-selector-header", "k-grid--inline");
                                    gridWrapper.style.height = "200px";
                                    if (window.innerHeight < 1000) {
                                        gridWrapper.style.height = "175px";
                                    }
                                    if (window.innerHeight < 900) {
                                        gridWrapper.style.height = "150px";
                                    }
                                    if (window.innerHeight < 800) {
                                        gridWrapper.style.height = "125px";
                                    }
                                    var selectLabel = headerRoot.appendChild(document.createElement("div"));
                                    selectLabel.classList.add("parts-selector-header");
                                    selectLabel.style.paddingLeft = "1.5rem";
                                    selectLabel.style.fontWeight = "bold";
                                    selectLabel.innerText = "Add new part:";
                                    return headerRoot.innerHTML;
                                },
                                itemTemplate: function (item) {
                                    var span = document.createElement("span");
                                    if (item.IsDefaultChild) {
                                        var quantityWrapper = span.appendChild(document.createElement("strong"));
                                        quantityWrapper.appendChild(document.createTextNode("[" + item.ShelfQuantity + "]"));
                                        quantityWrapper.appendChild(document.createTextNode(" "));
                                        quantityWrapper.appendChild(document.createTextNode(item.Name));
                                        span.appendChild(document.createTextNode(" "));
                                        var defaultPill = span.appendChild(document.createElement("span"));
                                        defaultPill.classList.add("badge", "badge-pill", "badge-primary");
                                        defaultPill.appendChild(document.createTextNode("default"));
                                    }
                                    else {
                                        span.appendChild(document.createTextNode("[" + item.ShelfQuantity + "]"));
                                        span.appendChild(document.createTextNode(" "));
                                        span.appendChild(document.createTextNode(item.Name));
                                    }
                                    return span.outerHTML;
                                },
                                open: function (e) {
                                    if (_this._grid) {
                                        return;
                                    }
                                    var gridWrapper = e.sender.popup.element.find(".multiselect-grid-wrapper");
                                    if (gridWrapper.length) {
                                        _this._grid = new HandpieceRequiredPartGrid(_this._handpieceId, gridWrapper);
                                        _this._grid.updated.subscribe(function (sender, data) {
                                            for (var i = 0; i < data.length; i++) {
                                                var item = _this._globalValuesBySKUId.get(data[i].SKUId);
                                                if (item !== undefined) {
                                                    item.RequiredQuantity = data[i].RequiredQuantity;
                                                    item.ShelfQuantity = data[i].ShelfQuantity;
                                                    item.Status = data[i].Status;
                                                }
                                            }
                                            _this.refresh();
                                        });
                                        _this._grid.init();
                                    }
                                },
                                virtual: {
                                    itemHeight: 32,
                                    mapValueTo: "dataItem",
                                    valueMapper: function (options) {
                                        var resolved = options.value.map(function (x) { return _this._globalValues.get(x); });
                                        options.success(resolved);
                                    }
                                },
                                select: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                    var existing, oldQuantity, newQuantity, updated, newPart, newValue, value;
                                    return __generator(this, function (_a) {
                                        switch (_a.label) {
                                            case 0:
                                                e.preventDefault();
                                                e.sender.input.val("");
                                                if (!this._canAdd) {
                                                    return [2 /*return*/];
                                                }
                                                existing = this._globalValuesBySKUId.get(e.dataItem.Id);
                                                if (!existing) return [3 /*break*/, 2];
                                                oldQuantity = existing.RequiredQuantity;
                                                newQuantity = existing.RequiredQuantity + 1;
                                                return [4 /*yield*/, this._api.update(this._handpieceId, existing.Id, oldQuantity, newQuantity)];
                                            case 1:
                                                updated = _a.sent();
                                                existing.RequiredQuantity = updated.RequiredQuantity;
                                                existing.ShelfQuantity = updated.ShelfQuantity;
                                                existing.Price = updated.Price;
                                                existing.Status = updated.Status;
                                                this.refresh();
                                                return [3 /*break*/, 4];
                                            case 2: return [4 /*yield*/, this._api.add(this._handpieceId, e.dataItem.Id, 1)];
                                            case 3:
                                                newPart = _a.sent();
                                                newValue = {
                                                    Id: newPart.Id,
                                                    SKUId: newPart.SKUId,
                                                    Name: newPart.SKUName,
                                                    RequiredQuantity: newPart.RequiredQuantity,
                                                    ShelfQuantity: newPart.ShelfQuantity,
                                                    Price: newPart.Price,
                                                    Status: newPart.Status,
                                                };
                                                this._globalValues.set(newValue.Id, newValue);
                                                this._globalValuesBySKUId.set(newValue.SKUId, newValue);
                                                value = e.sender.value();
                                                value.push(newValue.Id);
                                                e.sender.value(value);
                                                e.sender.refresh();
                                                _a.label = 4;
                                            case 4:
                                                if (this._grid) {
                                                    this._grid.refresh();
                                                }
                                                this._changed.raise(this, undefined);
                                                return [2 /*return*/];
                                        }
                                    });
                                }); },
                                deselect: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                    var partId, fullItem, oldQuantity, newQuantity, updated;
                                    return __generator(this, function (_a) {
                                        switch (_a.label) {
                                            case 0:
                                                if (!this._canRemove) {
                                                    e.preventDefault();
                                                    return [2 /*return*/];
                                                }
                                                partId = e.dataItem.Id;
                                                fullItem = this._globalValues.get(partId);
                                                if (!fullItem) {
                                                    e.preventDefault();
                                                    return [2 /*return*/];
                                                }
                                                oldQuantity = fullItem.RequiredQuantity;
                                                newQuantity = Math.abs(fullItem.RequiredQuantity - 1) > 0.000001 ? fullItem.RequiredQuantity - 1 : 0;
                                                if (newQuantity > 0) {
                                                    e.preventDefault();
                                                }
                                                return [4 /*yield*/, this._api.update(this._handpieceId, fullItem.Id, oldQuantity, newQuantity)];
                                            case 1:
                                                updated = _a.sent();
                                                if (updated.RequiredQuantity > 0) {
                                                    fullItem.RequiredQuantity = updated.RequiredQuantity;
                                                    fullItem.ShelfQuantity = updated.ShelfQuantity;
                                                    fullItem.Price = updated.Price;
                                                    fullItem.Status = updated.Status;
                                                    this.refresh();
                                                }
                                                else {
                                                    fullItem.RequiredQuantity = 0;
                                                    fullItem.Status = HandpieceRequiredPartStatus.Unknown;
                                                    this._globalValues.delete(fullItem.Id);
                                                    this._globalValuesBySKUId.delete(fullItem.SKUId);
                                                }
                                                if (this._grid) {
                                                    this._grid.refresh();
                                                }
                                                this._changed.raise(this, undefined);
                                                return [2 /*return*/];
                                        }
                                    });
                                }); }
                            });
                            var multiSelect = this._multiselectWrapper.data("kendoMultiSelect");
                            multiSelect["tagTemplate"] = function (data) {
                                var style = HandpieceRequiredPartStatusHelper.toDisplayColor(data.Status);
                                var numberTag = "";
                                if (data.RequiredQuantity !== undefined && data.ShelfQuantity !== undefined) {
                                    numberTag = "[" + data.RequiredQuantity + "/" + data.ShelfQuantity + "] ";
                                }
                                else if (data.RequiredQuantity !== undefined) {
                                    numberTag = "[" + data.RequiredQuantity + "] ";
                                }
                                var html = "<li role=\"option\" aria-selected=\"true\" class=\"k-button\" unselectable=\"on\" style=\"" + style + "\" data-id=\"" + data.Id + "\">";
                                html += "<span unselectable=\"on\"><span>" + numberTag + data.Name + "</span></span>";
                                if (_this._canAdd) {
                                    html += "<span aria-hidden=\"true\" unselectable=\"on\" aria-label=\"add\" title=\"add\" class=\"handpiece-part-increase-quantity\" style=\"align-self: stretch; display: flex; margin-left: 0.5rem;\"><span class=\"k-icon k-i-plus\"></span></span>";
                                }
                                if (_this._canRemove) {
                                    html += "<span aria-hidden=\"true\" unselectable=\"on\" aria-label=\"delete\" title=\"delete\" class=\"k-select\" style=\"margin-left: 0\"><span class=\"k-icon k-i-close\"></span></span>";
                                }
                                html += '</li>';
                                return html;
                            };
                            multiSelect.input.on("keydown", function (e) {
                                if (e.which === 8 && !e.target.value.length) {
                                    e.stopImmediatePropagation();
                                    e.preventDefault();
                                }
                            });
                            jQuery["_data"](multiSelect.input[0]).events.keydown.reverse();
                            multiSelect.wrapper.on("click", ".handpiece-part-increase-quantity", function (e) { return __awaiter(_this, void 0, void 0, function () {
                                var li, dataId, existing, oldQuantity, newQuantity, updated;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            if (!this._canAdd) {
                                                e.preventDefault();
                                                return [2 /*return*/];
                                            }
                                            li = e.target.closest("li");
                                            dataId = li.getAttribute("data-id");
                                            existing = this._globalValues.get(dataId);
                                            if (!existing) return [3 /*break*/, 2];
                                            oldQuantity = existing.RequiredQuantity;
                                            newQuantity = existing.RequiredQuantity + 1;
                                            return [4 /*yield*/, this._api.update(this._handpieceId, existing.Id, oldQuantity, newQuantity)];
                                        case 1:
                                            updated = _a.sent();
                                            existing.RequiredQuantity = updated.RequiredQuantity;
                                            existing.ShelfQuantity = updated.ShelfQuantity;
                                            existing.Price = updated.Price;
                                            existing.Status = updated.Status;
                                            this.refresh();
                                            if (this._grid) {
                                                this._grid.refresh();
                                            }
                                            this._changed.raise(this, undefined);
                                            _a.label = 2;
                                        case 2: return [2 /*return*/];
                                    }
                                });
                            }); });
                            multiSelect.popup.bind("open", function (e) {
                                ////window.scrollTo({
                                ////    behavior: "auto",
                                ////    left: window.scrollX,
                                ////    top: $("#handpiece-parts-section").offset().top - $("nav.navbar").outerHeight() - 8,
                                ////});
                                setTimeout(function () {
                                    var popup = e.sender;
                                    var availableSpaceUp = multiSelect.wrapper.offset().top - window.scrollY - $("nav.navbar").outerHeight() - 16;
                                    var availableSpaceDown = window.innerHeight - (multiSelect.wrapper.offset().top + multiSelect.wrapper.outerHeight() - window.scrollY) - 16;
                                    console.log("Available Space UP: " + availableSpaceUp + " DOWN: " + availableSpaceDown);
                                    if (availableSpaceDown >= availableSpaceUp) {
                                        // Dropping down
                                        var popupHeight = availableSpaceDown;
                                        var popupLeft = multiSelect.wrapper.offset().left;
                                        var popupTop = multiSelect.wrapper.offset().top + multiSelect.wrapper.outerHeight();
                                        popup._location({ isFixed: true, x: popupLeft, y: popupTop });
                                        var extraHeight_1 = 0;
                                        popup.element.find(".parts-selector-header").each(function (index, element) { return extraHeight_1 += $(element).height(); });
                                        popup.wrapper.css({ height: popupHeight + "px", left: popupLeft + "px", top: popupTop + "px" });
                                        popup.element.css({ height: popupHeight + "px" });
                                        popup.element.find(".k-virtual-wrap > .k-virtual-content").css({ maxHeight: popupHeight - extraHeight_1 - 16 + "px" });
                                    }
                                    else {
                                        // Dropping up
                                        var popupHeight = availableSpaceUp;
                                        var popupLeft = multiSelect.wrapper.offset().left;
                                        var popupTop = multiSelect.wrapper.offset().top - popupHeight;
                                        popup._location({ isFixed: true, x: popupLeft, y: popupTop });
                                        var extraHeight_2 = 0;
                                        popup.element.find(".parts-selector-header").each(function (index, element) { return extraHeight_2 += $(element).height(); });
                                        popup.wrapper.css({ height: popupHeight + "px", left: popupLeft + "px", top: popupTop + "px" });
                                        popup.element.css({ height: popupHeight + "px" });
                                        popup.element.find(".k-virtual-wrap > .k-virtual-content").css({ maxHeight: popupHeight - extraHeight_2 - 16 + "px" });
                                    }
                                });
                            });
                            return multiSelect;
                        };
                        HandpieceRequiredPartsMultiSelect.prototype.createDataSource = function () {
                            var dataSource = new kendo.data.DataSource({
                                type: "aspnetmvc-ajax",
                                transport: {
                                    read: {
                                        url: "/HandpieceRequiredParts/ReadAvailable?parentId=" + this._handpieceId
                                    },
                                },
                                schema: {
                                    data: "Data",
                                    total: "Total",
                                    errors: "Errors",
                                    model: {
                                        id: "Id",
                                        fields: {
                                            "Name": { type: "string" },
                                            "ShelfQuantity": { type: "number" },
                                            "Price": { type: "number" },
                                        },
                                    },
                                },
                                serverAggregates: true,
                                serverFiltering: true,
                                serverGrouping: true,
                                serverPaging: true,
                                serverSorting: true,
                            });
                            return dataSource;
                        };
                        return HandpieceRequiredPartsMultiSelect;
                    }());
                    Edit.HandpieceRequiredPartsMultiSelect = HandpieceRequiredPartsMultiSelect;
                })(Edit = Handpieces.Edit || (Handpieces.Edit = {}));
            })(Handpieces = Pages.Handpieces || (Pages.Handpieces = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=edit.js.map