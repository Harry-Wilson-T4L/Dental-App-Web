namespace DentalDrill.CRM.Pages.Handpieces.Edit {
    import EventHandler = DevGuild.Utilities.EventHandler;
    import HandpieceRequiredPartGrid = DentalDrill.CRM.Pages.HandpieceRequiredParts.Index.HandpieceRequiredPartGrid;
    import HandpieceRequiredPartStatusHelper = DentalDrill.CRM.Pages.HandpieceRequiredParts.Index.HandpieceRequiredPartStatusHelper;
    import HandpieceRequiredPartStatus = DentalDrill.CRM.Pages.HandpieceRequiredParts.Index.HandpieceRequiredPartStatus;
    import HandpieceRequiredPartReadModel = DentalDrill.CRM.Pages.HandpieceRequiredParts.Index.HandpieceRequiredPartReadModel;

    export class HandpiecesEditForm {
        static trackFormChanges(form: HTMLFormElement) {
            const formNode = $(form);
            formNode.on("change", "input", (e) => {
                if ($(e.target).attr("data-ignore-change") === "true") {
                    return;
                }

                formNode.data("hasChanges", true);
                console.log("Change tracked");
            });

            formNode.on("change", "select", (e) => {
                if ($(e.target).attr("data-ignore-change") === "true") {
                    return;
                }

                formNode.data("hasChanges", true);
                console.log("Change tracked");
            });

            formNode.on("change", "textarea", (e) => {
                if ($(e.target).attr("data-ignore-change") === "true") {
                    return;
                }

                formNode.data("hasChanges", true);
                console.log("Change tracked");
            });

            formNode.on("click", "input[type=checkbox]", (e) => {
                if ($(e.target).attr("data-ignore-change") === "true") {
                    return;
                }

                formNode.data("hasChanges", true);
                console.log("Change tracked");
            });

            window.addEventListener("beforeunload", (e) => {
                const hasChanges = formNode.data("hasChanges");
                if (hasChanges) {
                    e.returnValue = "Are you sure you want to leave this page? All changes will be lost.";
                    return "Are you sure you want to leave this page? All changes will be lost.";
                }

                return null;
            }, false);
        }

        static initialize(form: HTMLFormElement) {
            HandpiecesEditForm.trackFormChanges(form);
            const submitButtons = form.querySelectorAll("button[type='submit']");
            for (let i = 0; i < submitButtons.length; i++) {
                const submitButton = submitButtons[i] as HTMLButtonElement;
                submitButton.addEventListener("click", () => {
                    if (submitButton.getAttribute("name") === "Command") {
                        const input = document.querySelector("#CommandInput") as HTMLInputElement;
                        input.value = submitButton.value;
                    }
                }, false);
            }

            form.addEventListener("submit", (e) => {
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
            $(document).on("change", "input.diagnostic-check__selected", (e: JQueryEventObject) => {
                const target = e.target as HTMLInputElement;
                const container = $(target).closest(".handpiece-diagnostics");
                const matchingValue = container.find(`input.diagnostic-check__selected[value='${target.value}']`);
                matchingValue.each((index: number, element: HTMLInputElement) => {
                    if (element.id !== target.id) {
                        element.checked = target.checked;
                    }
                });

                const list = $(".handpiece-diagnostic-checked-list");
                const report = $(".handpiece-diagnostic-report");
                if (target.checked) {
                    if (list.find(`input[name=DiagnosticReportChecked][value=${target.value}]`).length === 0) {
                        list.append($(`<input />`)
                            .attr("id", `DiagnosticReportChecked_${target.value}`)
                            .attr("name", `DiagnosticReportChecked`)
                            .attr("type", "hidden")
                            .val(target.value));
                    }

                    if (report.find(`.handpiece-diagnostic-report__item[data-diagnostic-id=${target.value}]`).length === 0) {
                        const label = container.find(`label[for=${target.id}]`);
                        const reportItem = $(`<span><span>`)
                            .addClass(`handpiece-diagnostic-report__item`)
                            .addClass(`handpiece-diagnostic-report__predefined`)
                            .attr(`data-diagnostic-id`, target.value)
                            .text(label.text())
                            .append($(`<span></span>`)
                                .addClass(`handpiece-diagnostic-report__item__separator`)
                                .text(`, `));

                        const otherItem = report.find(`.handpiece-diagnostic-report__item.handpiece-diagnostic-report__other`);
                        if (otherItem.length !== 0) {
                            reportItem.insertBefore(otherItem);
                        } else {
                            reportItem.appendTo(report);
                        }
                    }
                } else {
                    list.find(`input[name=DiagnosticReportChecked][value=${target.value}]`).remove();
                    report.find(`.handpiece-diagnostic-report__item[data-diagnostic-id=${target.value}]`).remove();
                }
            });

            $(document).on("change", "input.diagnostic-check-other__selected", (e: JQueryEventObject) => {
                const target = e.target as HTMLInputElement;
                const report = $(".handpiece-diagnostic-report");
                if (target.checked) {
                    let otherItem = report.find(`.handpiece-diagnostic-report__item.handpiece-diagnostic-report__other`);
                    if (otherItem.length === 0) {
                        otherItem = $(`<span><span>`)
                            .addClass(`handpiece-diagnostic-report__item`)
                            .addClass(`handpiece-diagnostic-report__other`)
                            .text($(".diagnostic-check-other__text").val() as any);

                        otherItem.appendTo(report);
                    } else {
                        otherItem.text($(".diagnostic-check-other__text").val() as any);
                    }
                } else {
                    report.find(`.handpiece-diagnostic-report__item.handpiece-diagnostic-report__other`).remove();
                }
            })

            $(document).on("change", "input.diagnostic-check-other__text", (e: JQueryEventObject) => {
                const target = e.target as HTMLInputElement;
                const report = $(".handpiece-diagnostic-report");
                const otherItem = report.find(`.handpiece-diagnostic-report__item.handpiece-diagnostic-report__other`);
                if (otherItem.length > 0) {
                    otherItem.text(target.value);
                }
            });

            // Having input-validation-error class on kendo control would prevent their submit.
            $(".k-widget").removeClass("input-validation-error");

            // Link Service Levels and Cost of repairs
            const serviceLevel = $(form).find("#ServiceLevelId").data("kendoDropDownList");
            const costOfRepair = $(form).find("#CostOfRepair").data("kendoNumericTextBox");
            if (serviceLevel && costOfRepair) {
                let previousValue = serviceLevel.dataItem();
                serviceLevel.bind("change", (e: kendo.ui.DropDownListChangeEvent) => {
                    const selected = e.sender.dataItem() as { Id: string, Name: string, CostOfRepair: number };
                    if (selected && selected.CostOfRepair !== undefined && selected.CostOfRepair !== null) {
                        if (costOfRepair.value() === undefined || costOfRepair.value() === null || (previousValue && costOfRepair.value() === previousValue.CostOfRepair)) {
                            costOfRepair.value(selected.CostOfRepair);
                        }
                    }

                    previousValue = selected;
                });
            }

            $(form).on("click", ".handpiece-brand-model-serial__add-component", (e: JQueryEventObject) => {
                HandpiecesEditForm.addComponent(form, undefined);
            });

            $(form).on("click", ".handpiece-brand-model-serial__remove-component", (e: JQueryEventObject) => {
                $(e.target).parents(".handpiece-brand-model-serial--component").remove();
            });

            const enableAutoFullScreenExpand = function (kendo, dropDownList) {
                if (dropDownList === undefined) {
                    return;
                }

                dropDownList.popup.bind('open', e => {
                    e.sender.options.collision = 'none none';
                    setTimeout(() => {
                        const popup = e.sender, popupWrapper = e.sender.wrapper, listContainer = e.sender.element, listScroller = e.sender.element.find(".k-list-scroller");
                        const dropDownListOffset = kendo.getOffset(dropDownList.wrapper);
                        const headerHeight = kendo._outerHeight($("nav.navbar"));

                        const popupTop = window.scrollY + headerHeight + 16;
                        const popupLeft = dropDownListOffset.left;
                        const popupHeight = window.innerHeight - headerHeight - 32;
                        const scrollerHeight = window.innerHeight - headerHeight - 32 - 50;

                        popup._location({ isFixed: true, x: popupLeft, y: popupTop });
                        popup.wrapper.css({ top: `${popupTop}px`, left: `${popupLeft}px`, height: `${popupHeight}px` });
                        listContainer.css({ height: `${popupHeight}px` });
                        listScroller.css({ height: `${scrollerHeight}px` });
                    });
                });
            };

            enableAutoFullScreenExpand(kendo, $("#ServiceLevelId").data("kendoDropDownList"));
        }

        static addComponent(form: HTMLFormElement, item?: { Brand: string; Model: string; Serial: string }) {
            const container = $(form).find(".handpiece-components-wrapper");
            const itemId = Math.random().toString(10).substr(3, 12);
            const control = $(`<div></div>`)
                .addClass(`row handpiece-brand-model-serial handpiece-brand-model-serial--component`)
                .append($(`<div></div>`)
                    .addClass(`col-12 mb-2 handpiece-brand-model-serial__serial`)
                    .append($(`<div></div>`)
                        .addClass(`row`)
                        .append($(`<div></div>`)
                            .addClass(`col-sm-5 col-lg-3 col-xl-5`)
                            .append($(`<label></label>`)
                                .addClass(`col-form-label`)
                                .attr(`for`, `Components_${itemId}_Serial`)
                                .text(`Serial #`))
                            .append($(`<button></button>`)
                                .addClass(`k-button handpiece-brand-model-serial__remove-component`)
                                .attr(`type`, `button`)
                                .html(`<span class="fas fa-minus"></span>`)))
                        .append($(`<div></div>`)
                            .addClass(`col-sm-7 col-lg-9 col-xl-7`)
                            .append($(`<input />`)
                                .attr(`id`, `Components_${itemId}_Serial`)
                                .attr(`name`, `Components[${itemId}].Serial`)
                                .attr(`type`, `text`)
                                .val(item && item.Serial ? item.Serial : ``))
                            .append($(`<span></span>`)
                                .addClass(`field-validation-valid`)
                                .attr(`data-valmsg-for`, `Components[${itemId}].Serial`)
                                .attr(`data-valmsg-replace`, `true`)))))
                .append($(`<div></div>`)
                    .addClass(`col-12 mb-2 handpiece-brand-model-serial__brand`)
                    .append($(`<div></div>`)
                        .addClass(`row`)
                        .append($(`<div></div>`)
                            .addClass(`col-sm-5 col-lg-3 col-xl-3`)
                            .append($(`<label></label>`)
                                .addClass(`col-form-label`)
                                .attr(`for`, `Components_${itemId}_Brand`)
                                .text(`Brand`)))
                        .append($(`<div></div>`)
                            .addClass(`col-sm-7 col-lg-9 col-xl-9`)
                            .append($(`<input />`)
                                .attr(`id`, `Components_${itemId}_Brand`)
                                .attr(`name`, `Components[${itemId}].Brand`)
                                .attr(`type`, `text`)
                                .val(item && item.Brand ? item.Brand : ``))
                            .append($(`<span></span>`)
                                .addClass(`field-validation-valid`)
                                .attr(`data-valmsg-for`, `Components[${itemId}].Brand`)
                                .attr(`data-valmsg-replace`, `true`)))))
                .append($(`<div></div>`)
                    .addClass(`col-12 mb-2 handpiece-brand-model-serial__model`)
                    .append($(`<div></div>`)
                        .addClass(`row`)
                        .append($(`<div></div>`)
                            .addClass(`col-sm-5 col-lg-3 col-xl-3`)
                            .append($(`<label></label>`)
                                .addClass(`col-form-label`)
                                .attr(`for`, `Components_${itemId}_MakeAndModel`)
                                .text(`Model`)))
                        .append($(`<div></div>`)
                            .addClass(`col-sm-7 col-lg-9 col-xl-9`)
                            .append($(`<input />`)
                                .attr(`id`, `Components_${itemId}_MakeAndModel`)
                                .attr(`name`, `Components[${itemId}].MakeAndModel`)
                                .attr(`type`, `text`)
                                .val(item && item.Model ? item.Model : ``))
                            .append($(`<span></span>`)
                                .addClass(`field-validation-valid`)
                                .attr(`data-valmsg-for`, `Components[${itemId}].MakeAndModel`)
                                .attr(`data-valmsg-replace`, `true`)))));

            container.append(control);
            $(`#Components_${itemId}_Serial`).kendoTextBox({
            });
            $(`#Components_${itemId}_Brand`).kendoTextBox({
            });
            $(`#Components_${itemId}_MakeAndModel`).kendoTextBox({
            });
        }

        static handleExistingHandpieceChange(e: kendo.ui.DropDownListChangeEvent) {
            const form = e.sender.wrapper.closest("form")[0] as HTMLFormElement;
            const selectedItem = e.sender.dataItem();
            if (selectedItem) {
                if (selectedItem.Type === 0) {
                    $(form).find("#Brand").data("kendoTextBox").value(selectedItem.Brand);
                    $(form).find("#MakeAndModel").data("kendoTextBox").value(selectedItem.Model);
                    $(form).find("#Serial").data("kendoTextBox").value(selectedItem.Serial);
                    $(form).find(".handpiece-brand-model-serial--component").each((index, element) => {
                        $(element).remove();
                    });

                    for (let component of selectedItem.Components) {
                        HandpiecesEditForm.addComponent(form, component);
                    }
                }
            }
        }

        static tryUpdateComponents(form: HTMLFormElement) {
            const componentsNodes = form.querySelectorAll(".handpiece-brand-model-serial--component");
            for (let i = 0; i < componentsNodes.length; i++) {
                const inputs = componentsNodes[i].querySelectorAll("input[name]");
                for (let j = 0; j < inputs.length; j++) {
                    const input = inputs[j] as HTMLInputElement;
                    input.name = input.name.replace(/Components\[[-0-9]+\]/, `Components[${i}]`)
                }
            }
        }

        static initDiagnostics() {
            $("input[type=checkbox].diagnostic-check__selected").each((index: number, element: HTMLInputElement) => {
                element.checked = false;
            })

            const container = $(".handpiece-diagnostics");
            const list = $(".handpiece-diagnostic-checked-list");
            const checkedItems = list.find("input[name=DiagnosticReportChecked]");
            checkedItems.each((index: number, element: HTMLInputElement) => {
                const itemId = element.value;
                const itemInputs = container.find(`input.diagnostic-check__selected[value='${itemId}']`);
                itemInputs.each((index: number, element: HTMLInputElement) => {
                    element.checked = true;
                });
            });

            const otherTextInput = document.querySelector("#DiagnosticReportOther") as HTMLInputElement;
            const otherCheck = document.querySelector(".handpiece-diagnostics").querySelector(".diagnostic-check-other") as HTMLDivElement;
            const otherCheckSelectedCheckbox = otherCheck.querySelector(".diagnostic-check-other__selected") as HTMLInputElement;
            const otherCheckTextInput = otherCheck.querySelector(".diagnostic-check-other__text") as HTMLInputElement;
            if (otherTextInput.value) {
                otherCheckTextInput.value = otherTextInput.value;
                otherCheckTextInput.classList.remove("d-none");
                otherCheckSelectedCheckbox.checked = true;
            } else {
                otherCheckTextInput.value = ``;
                otherCheckTextInput.classList.add("d-none");
                otherCheckSelectedCheckbox.checked = false;
            }
        }

        static tryUpdateDiagnostics(form: HTMLFormElement) {
            const otherTextInput = form.querySelector("#DiagnosticReportOther") as HTMLInputElement;
            const itemsContainer = document.querySelector("#DiagnosticReportItems") as HTMLDivElement;

            if (otherTextInput && itemsContainer) {
                const otherCheck = itemsContainer.querySelector(".diagnostic-check-other") as HTMLDivElement;
                const otherCheckSelectedCheckbox = otherCheck.querySelector(".diagnostic-check-other__selected") as HTMLInputElement;
                const otherCheckTextInput = otherCheck.querySelector(".diagnostic-check-other__text") as HTMLInputElement;

                if (otherCheckSelectedCheckbox.checked) {
                    otherTextInput.value = otherCheckTextInput.value;
                } else {
                    otherTextInput.value = "";
                }
            }
        }
    }

    $(() => {
        const form = document.querySelector("#HandpiecesEditForm") as HTMLFormElement;
        if (form) {
            HandpiecesEditForm.initialize(form);
            $("[data-toggle='tooltip']").tooltip();

            const partsMultiselect = $("#HandpieceRequiredPartsMultiSelect");
            if (partsMultiselect.length > 0) {
                const handpieceId = partsMultiselect.attr("data-handpiece");
                const isReadOnly = partsMultiselect.attr("data-readonly") === "true" || partsMultiselect.attr("data-readonly") === "True";
                const isRemoveOnly = partsMultiselect.attr("data-removeonly") === "true" || partsMultiselect.attr("data-removeonly") === "True";
                const multiselect = new HandpieceRequiredPartsMultiSelect(handpieceId, partsMultiselect, !isReadOnly && !isRemoveOnly, !isReadOnly);

                const changeStatusSelect = $(form).find("#ChangeStatus").data("kendoDropDownList");
                if (changeStatusSelect !== undefined) {
                    multiselect.changed.subscribe((sender, e) => {
                        multiselect.api.checkStatus(handpieceId).then(response => {
                            if (changeStatusSelect.value() === "BeingRepaired" && (response.StockStatus === HandpiecePartsStockStatus.OutOfStock || response.StockStatus === HandpiecePartsStockStatus.PartialStock)) {
                                changeStatusSelect.value("WaitingForParts");
                            } else if (changeStatusSelect.value() === "WaitingForParts" && (response.StockStatus === HandpiecePartsStockStatus.InStock)) {
                                changeStatusSelect.value("BeingRepaired");
                            }
                        });
                    });
                }
            }
        }
    });

    export class AlertsContainer {
        private readonly _root: JQuery;

        constructor(root: JQuery) {
            this._root = root;
        }

        primary(text: string, dismissAfter?: number, manualDismissal?: boolean) {
            this.render("primary", text, dismissAfter, manualDismissal);
        }

        secondary(text: string, dismissAfter?: number, manualDismissal?: boolean) {
            this.render("secondary", text, dismissAfter, manualDismissal);
        }

        success(text: string, dismissAfter?: number, manualDismissal?: boolean) {
            this.render("success", text, dismissAfter, manualDismissal);
        }

        danger(text: string, dismissAfter?: number, manualDismissal?: boolean) {
            this.render("danger", text, dismissAfter, manualDismissal);
        }

        warning(text: string, dismissAfter?: number, manualDismissal?: boolean) {
            this.render("warning", text, dismissAfter, manualDismissal);
        }

        info(text: string, dismissAfter?: number, manualDismissal?: boolean) {
            this.render("info", text, dismissAfter, manualDismissal);
        }

        light(text: string, dismissAfter?: number, manualDismissal?: boolean) {
            this.render("light", text, dismissAfter, manualDismissal);
        }

        dark(text: string, dismissAfter?: number, manualDismissal?: boolean) {
            this.render("dark", text, dismissAfter, manualDismissal);
        }

        private render(type: string, text: string, dismissAfter?: number, manualDismissal?: boolean) {
            const node = $("<div></div>");
            node.addClass(`alert`);
            node.addClass(`alert-${type}`);
            node.attr("role", "alert");
            node.text(text);

            if (dismissAfter !== undefined && typeof dismissAfter == "number") {
                setTimeout(() => {
                    node.fadeOut("slow", () => {
                        node.remove();
                    });
                }, dismissAfter);
            }

            if (manualDismissal) {
                node.addClass("alert-dismissible");
                node.addClass("fade");
                node.addClass("show");

                const dismissButton = $(`<button type="button"></button>`);
                dismissButton.addClass("close");
                dismissButton.attr("data-dismiss", "alert");
                dismissButton.attr("aria-label", "Close");
                dismissButton.html(`<span aria-hidden="true">&times;</span>`);
                node.append(dismissButton);
            }

            this._root.append(node);
        }
    }

    export class HandpieceAssistant {
        private readonly _root: JQuery;
        private readonly _modelId: string;
        private readonly _listView: kendo.ui.ListView;
        private readonly _notesEditor: kendo.ui.Editor;

        private readonly _saveButton: JQuery;
        private readonly _cancelButton: JQuery;
        private readonly _notesAlerts: AlertsContainer;

        private _windowWrapper: JQuery;
        private _window: kendo.ui.Window;
        private _viewPortSize: { width: number, height: number };

        private _savedNotes: string;

        constructor(root: JQuery) {
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

        static init(root: JQuery) {
            const assistant = new HandpieceAssistant(root);
            root.data("HandpieceAssistant", assistant);
        }

        private async handleSaveNotes(e: JQueryEventObject): Promise<void> {
            e.preventDefault();
            try {
                this._savedNotes = this._notesEditor.value();
                const response = await fetch(`/HandpieceAssistant/UpdateNotes/${this._modelId}`, {
                    method: "POST",
                    credentials: "same-origin",
                    cache: "no-cache",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({ Notes: this._savedNotes })
                });

                if (response.status === 204) {
                    this._notesAlerts.success("Notes saved", 5000, true);
                } else {
                    this._notesAlerts.danger("Unable to save notes", undefined, true);
                }
            }
            catch (exception) {
                this._notesAlerts.danger("Unable to save notes", undefined, true);
            }
        }

        private handleCancelNotes(e: JQueryEventObject): void {
            e.preventDefault();
            this._notesEditor.value(this._savedNotes);
        }

        private handleSchematicsClick(e: JQueryEventObject): void {
            const target = $(e.target).closest(".handpiece-assistant-schematic");
            const id = target.attr("data-id");

            if (this.initWindow(id)) {
                this._window.open();
                this._window.center();
                this.selectPage(id);
            } else {
                this._window.open();
                this._window.center();
            }
        }

        private initWindow(id: string): boolean {
            if (this._window) {
                if (this.updateViewPortSize()) {
                    this._window.setOptions({
                        width: `${Math.round(this._viewPortSize.width * 0.9)}px`,
                        height: `${Math.round(this._viewPortSize.height * 0.9)}px`,
                    });
                }

                return true;
            }

            this.updateViewPortSize();
            const wrapper = this.getWindowWrapper();
            wrapper.kendoWindow({
                width: `${Math.round(this._viewPortSize.width * 0.9)}px`,
                height: `${Math.round(this._viewPortSize.height * 0.9)}px`,
                title: "Handpiece Assistant",
                actions: ["close"],
                modal: true,
                visible: false,
                content: `/HandpieceAssistant/Preview/${this._modelId}?schematicId=${id}`,
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.center();
                }
            });

            this._window = wrapper.data("kendoWindow");
            return false;
        }

        private updateViewPortSize(): boolean {
            const viewPortSize = {
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
        }

        private getWindowWrapper(): JQuery {
            if (!this._windowWrapper) {
                this._windowWrapper = $("<div></div>").addClass("handpiece-assistant-window");
            }

            return this._windowWrapper;
        }

        private selectPage(pageId: string) {
            const scrollViewWrapper = this._window.wrapper.find(".handpiece-assistant-scrollview");
            if (scrollViewWrapper.length === 0) {
                return;
            }

            const page = scrollViewWrapper.find(`.handpiece-assistant-schematic-page[data-id='${pageId}']`);
            if (page.length === 0) {
                return;
            }

            try {
                const pageNo = parseInt(page.attr("data-index"));
                const scrollView = scrollViewWrapper.data("kendoScrollView");
                scrollView.scrollTo(pageNo, false);
            } catch (exception) { }
        }
    }

    interface HandpieceRequiredPartAvailableSKUModel {
        Id: string;
        Name: string;
        ShelfQuantity: number;
        Price: number;
        IsDefaultChild: boolean;
    }

    interface HandpieceRequiredPartSelectedSKUModel {
        Id: string;
        SKUId: string;
        Name: string;
        RequiredQuantity: number;
        ShelfQuantity: number;
        Price: number;
        Status: HandpieceRequiredPartStatus;
    }

    enum HandpiecePartsStockStatus {
        InStock = 0,
        OutOfStock = 1,
        PartialStock = 2,
    }

    interface HandpieceCheckStatusResponse {
        StockStatus: HandpiecePartsStockStatus;
    }

    class HandpieceRequiredPartsApi {
        async add(handpieceId: string, sku: string, quantity: number): Promise<HandpieceRequiredPartReadModel> {
            const response = await fetch(`/HandpieceRequiredParts/ApiAdd?parentId=${handpieceId}`, {
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
            });

            if (response.status !== 200) {
                throw new Error("Failed to add required part");
            }

            const json = await response.json();
            return json;
        }

        async update(handpieceId: string, id: string, oldQuanity: number, newQuantity: number): Promise<HandpieceRequiredPartReadModel> {
            const response = await fetch(`/HandpieceRequiredParts/ApiUpdate/${id}?parentId=${handpieceId}`, {
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
            });

            if (response.status !== 200) {
                throw new Error("Failed to update required part");
            }

            const json = await response.json();
            return json;
        }

        async checkStatus(handpieceId: string): Promise<HandpieceCheckStatusResponse> {
            const response = await fetch(`/HandpieceRequiredParts/ApiCheckStatus?parentId=${handpieceId}`, {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    "Content-Type": "application/json",
                    "X-Requested-With": "XMLHttpRequest"
                },
                body: JSON.stringify({ }),
            });

            if (response.status !== 200) {
                throw new Error("Failed to check handpiece status");
            }

            const json = await response.json();
            return json;
        }
    }

    export class HandpieceRequiredPartsMultiSelect {
        private readonly _api: HandpieceRequiredPartsApi;
        private readonly _handpieceId: string;
        private readonly _multiselectWrapper: JQuery<HTMLElement>;
        private readonly _multiselect: kendo.ui.MultiSelect;
        private readonly _canAdd: boolean;
        private readonly _canRemove: boolean;
        private readonly _changed: EventHandler<any> = new EventHandler<any>();

        private _grid: HandpieceRequiredPartGrid;
        private _globalValues: Map<string, HandpieceRequiredPartSelectedSKUModel> = new Map<string, HandpieceRequiredPartSelectedSKUModel>();
        private _globalValuesBySKUId: Map<string, HandpieceRequiredPartSelectedSKUModel> = new Map<string, HandpieceRequiredPartSelectedSKUModel>();

        constructor(handpieceId: string, multiselectWrapper: JQuery<HTMLElement>, canAdd: boolean, canRemove: boolean) {
            this._api = new HandpieceRequiredPartsApi();
            this._handpieceId = handpieceId;
            this._multiselectWrapper = multiselectWrapper;
            this._canAdd = canAdd;
            this._canRemove = canRemove;
            this._multiselect = this.initMultiSelect();
            
            this._multiselectWrapper.data("HandpieceRequiredPartsMultiSelect", this);
        }

        get api(): HandpieceRequiredPartsApi {
            return this._api;
        }

        get changed(): EventHandler<any> {
            return this._changed;
        }

        private refresh(): void {
            const selectedItems = this._multiselect.dataItems();
            this._multiselect.tagList.html("");
            for (let item of selectedItems) {
                this._multiselect.tagList.append(this._multiselect["tagTemplate"](item));
            }
        }

        private initMultiSelect(): kendo.ui.MultiSelect {
            const values: object[] = [];
            const preloadedOptions = this._multiselectWrapper.find("option");
            preloadedOptions.each((index, element) => {
                const value: HandpieceRequiredPartSelectedSKUModel = {
                    Id: $(element).attr("data-id"),
                    SKUId: $(element).attr("data-sku-id"),
                    Name: $(element).attr("data-sku-name"),
                    RequiredQuantity: parseFloat($(element).attr("data-quantity")),
                    ShelfQuantity: parseFloat($(element).attr("data-shelf-quantity")),
                    Price: parseFloat($(element).attr("data-price")),
                    Status: parseInt($(element).attr("data-status")),
                }

                values.push(value);
                this._globalValues.set(value.Id, value);
                this._globalValuesBySKUId.set(value.SKUId, value);
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

                headerTemplate: () => {
                    const headerRoot = document.createElement("div");
                    const gridLabel = headerRoot.appendChild(document.createElement("div"));
                    gridLabel.classList.add("parts-selector-header");
                    gridLabel.style.paddingLeft = "1.5rem";
                    gridLabel.style.fontWeight = "bold";
                    gridLabel.innerText = "Required Parts";

                    const gridWrapper = headerRoot.appendChild(document.createElement("div"));
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

                    const selectLabel = headerRoot.appendChild(document.createElement("div"));
                    selectLabel.classList.add(`parts-selector-header`);
                    selectLabel.style.paddingLeft = "1.5rem";
                    selectLabel.style.fontWeight = "bold";
                    selectLabel.innerText = "Add new part:";

                    return headerRoot.innerHTML;
                },

                itemTemplate: (item: HandpieceRequiredPartAvailableSKUModel) => {
                    const span = document.createElement("span");
                    if (item.IsDefaultChild) {
                        const quantityWrapper = span.appendChild(document.createElement("strong"));
                        quantityWrapper.appendChild(document.createTextNode(`[${item.ShelfQuantity}]`));
                        quantityWrapper.appendChild(document.createTextNode(` `));
                        quantityWrapper.appendChild(document.createTextNode(item.Name));
                        span.appendChild(document.createTextNode(` `));
                        const defaultPill = span.appendChild(document.createElement(`span`));
                        defaultPill.classList.add(`badge`, `badge-pill`, `badge-primary`);
                        defaultPill.appendChild(document.createTextNode(`default`));
                    } else {
                        span.appendChild(document.createTextNode(`[${item.ShelfQuantity}]`));
                        span.appendChild(document.createTextNode(` `));
                        span.appendChild(document.createTextNode(item.Name));
                    }
                    
                    return span.outerHTML;
                },

                open: (e: kendo.ui.MultiSelectOpenEvent) => {
                    if (this._grid) {
                        return;
                    }

                    const gridWrapper = e.sender.popup.element.find(".multiselect-grid-wrapper");
                    if (gridWrapper.length) {
                        this._grid = new HandpieceRequiredPartGrid(this._handpieceId, gridWrapper);
                        this._grid.updated.subscribe((sender, data) => {
                            for (let i = 0; i < data.length; i++) {
                                const item = this._globalValuesBySKUId.get(data[i].SKUId);
                                if (item !== undefined) {
                                    item.RequiredQuantity = data[i].RequiredQuantity;
                                    item.ShelfQuantity = data[i].ShelfQuantity;
                                    item.Status = data[i].Status;
                                }
                            }

                            this.refresh();
                        });

                        this._grid.init();
                    }
                },

                virtual: {
                    itemHeight: 32,
                    mapValueTo: "dataItem",
                    valueMapper: (options) => {
                        const resolved = options.value.map(x => this._globalValues.get(x));
                        options.success(resolved);
                    }
                },

                select: async (e: kendo.ui.MultiSelectSelectEvent) => {
                    e.preventDefault();
                    e.sender.input.val("");

                    if (!this._canAdd) {
                        return;
                    }

                    const existing = this._globalValuesBySKUId.get(e.dataItem.Id);
                    if (existing) {
                        const oldQuantity = existing.RequiredQuantity;
                        const newQuantity = existing.RequiredQuantity + 1;

                        var updated = await this._api.update(this._handpieceId, existing.Id, oldQuantity, newQuantity);
                        existing.RequiredQuantity = updated.RequiredQuantity;
                        existing.ShelfQuantity = updated.ShelfQuantity;
                        existing.Price = updated.Price;
                        existing.Status = updated.Status;

                        this.refresh();
                    } else {
                        const newPart = await this._api.add(this._handpieceId, e.dataItem.Id, 1);
                        const newValue: HandpieceRequiredPartSelectedSKUModel = {
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

                        const value = e.sender.value();
                        value.push(newValue.Id);
                        e.sender.value(value);
                        e.sender.refresh();
                    }
                    
                    if (this._grid) {
                        this._grid.refresh();
                    }

                    this._changed.raise(this, undefined);
                },

                deselect: async (e: kendo.ui.MultiSelectDeselectEvent) => {
                    if (!this._canRemove) {
                        e.preventDefault();
                        return;
                    }

                    const partId: string = e.dataItem.Id;
                    const fullItem = this._globalValues.get(partId);
                    if (!fullItem) {
                        e.preventDefault();
                        return;
                    }

                    const oldQuantity = fullItem.RequiredQuantity;
                    const newQuantity = Math.abs(fullItem.RequiredQuantity - 1) > 0.000001 ? fullItem.RequiredQuantity - 1 : 0;
                    if (newQuantity > 0) {
                        e.preventDefault();
                    }

                    const updated = await this._api.update(this._handpieceId, fullItem.Id, oldQuantity, newQuantity);
                    if (updated.RequiredQuantity > 0) {
                        fullItem.RequiredQuantity = updated.RequiredQuantity;
                        fullItem.ShelfQuantity = updated.ShelfQuantity;
                        fullItem.Price = updated.Price;
                        fullItem.Status = updated.Status;

                        this.refresh();
                    } else {
                        fullItem.RequiredQuantity = 0;
                        fullItem.Status = HandpieceRequiredPartStatus.Unknown;
                        this._globalValues.delete(fullItem.Id);
                        this._globalValuesBySKUId.delete(fullItem.SKUId);
                    }
                    
                    if (this._grid) {
                        this._grid.refresh();
                    }

                    this._changed.raise(this, undefined);
                }
            });

            const multiSelect = this._multiselectWrapper.data("kendoMultiSelect");

            multiSelect["tagTemplate"] = (data: HandpieceRequiredPartSelectedSKUModel) => {
                let style = HandpieceRequiredPartStatusHelper.toDisplayColor(data.Status);

                let numberTag = ``;
                if (data.RequiredQuantity !== undefined && data.ShelfQuantity !== undefined) {
                    numberTag = `[${data.RequiredQuantity}/${data.ShelfQuantity}] `;
                } else if (data.RequiredQuantity !== undefined) {
                    numberTag = `[${data.RequiredQuantity}] `;
                }

                let html = `<li role="option" aria-selected="true" class="k-button" unselectable="on" style="${style}" data-id="${data.Id}">`;
                html += `<span unselectable="on"><span>${numberTag}${data.Name}</span></span>`;
                if (this._canAdd) {
                    html += `<span aria-hidden="true" unselectable="on" aria-label="add" title="add" class="handpiece-part-increase-quantity" style="align-self: stretch; display: flex; margin-left: 0.5rem;"><span class="k-icon k-i-plus"></span></span>`;
                }
                if (this._canRemove) {
                    html += `<span aria-hidden="true" unselectable="on" aria-label="delete" title="delete" class="k-select" style="margin-left: 0"><span class="k-icon k-i-close"></span></span>`;
                }
                html += '</li>';
                return html;
            };

            multiSelect.input.on("keydown", (e: JQuery.KeyDownEvent) => {
                if (e.which === 8 && !e.target.value.length) {
                    e.stopImmediatePropagation();
                    e.preventDefault();
                }
            });
            jQuery["_data"](multiSelect.input[0]).events.keydown.reverse();

            multiSelect.wrapper.on("click", ".handpiece-part-increase-quantity", async e => {
                if (!this._canAdd) {
                    e.preventDefault();
                    return;
                }

                const li = e.target.closest("li");
                const dataId = li.getAttribute("data-id");
                const existing = this._globalValues.get(dataId);
                if (existing) {
                    const oldQuantity = existing.RequiredQuantity;
                    const newQuantity = existing.RequiredQuantity + 1;

                    var updated = await this._api.update(this._handpieceId, existing.Id, oldQuantity, newQuantity);
                    existing.RequiredQuantity = updated.RequiredQuantity;
                    existing.ShelfQuantity = updated.ShelfQuantity;
                    existing.Price = updated.Price;
                    existing.Status = updated.Status;

                    this.refresh();

                    if (this._grid) {
                        this._grid.refresh();
                    }

                    this._changed.raise(this, undefined);
                }
            });

            multiSelect.popup.bind("open", e => {
                ////window.scrollTo({
                ////    behavior: "auto",
                ////    left: window.scrollX,
                ////    top: $("#handpiece-parts-section").offset().top - $("nav.navbar").outerHeight() - 8,
                ////});

                setTimeout(() => {
                    const popup = e.sender as any;
                    const availableSpaceUp = multiSelect.wrapper.offset().top - window.scrollY - $("nav.navbar").outerHeight() - 16;
                    const availableSpaceDown = window.innerHeight - (multiSelect.wrapper.offset().top + multiSelect.wrapper.outerHeight() - window.scrollY) - 16;

                    console.log(`Available Space UP: ${availableSpaceUp} DOWN: ${availableSpaceDown}`);

                    if (availableSpaceDown >= availableSpaceUp) {
                        // Dropping down
                        const popupHeight = availableSpaceDown;
                        const popupLeft = multiSelect.wrapper.offset().left;
                        const popupTop = multiSelect.wrapper.offset().top + multiSelect.wrapper.outerHeight();

                        popup._location({ isFixed: true, x: popupLeft, y: popupTop });

                        let extraHeight = 0;
                        popup.element.find(".parts-selector-header").each((index, element) => extraHeight += $(element).height());

                        popup.wrapper.css({ height: `${popupHeight}px`, left: `${popupLeft}px`, top: `${popupTop}px` });
                        popup.element.css({ height: `${popupHeight}px` });
                        popup.element.find(".k-virtual-wrap > .k-virtual-content").css({ maxHeight: `${popupHeight - extraHeight - 16}px` });
                    }
                    else {
                        // Dropping up
                        const popupHeight = availableSpaceUp;
                        const popupLeft = multiSelect.wrapper.offset().left;
                        const popupTop = multiSelect.wrapper.offset().top - popupHeight;

                        popup._location({ isFixed: true, x: popupLeft, y: popupTop });

                        let extraHeight = 0;
                        popup.element.find(".parts-selector-header").each((index, element) => extraHeight += $(element).height());

                        popup.wrapper.css({ height: `${popupHeight}px`, left: `${popupLeft}px`, top: `${popupTop}px` });
                        popup.element.css({ height: `${popupHeight}px` });
                        popup.element.find(".k-virtual-wrap > .k-virtual-content").css({ maxHeight: `${popupHeight - extraHeight - 16}px` });
                    }
                });
            });

            return multiSelect;
        }

        private createDataSource(): kendo.data.DataSource {
            const dataSource = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: `/HandpieceRequiredParts/ReadAvailable?parentId=${this._handpieceId}`
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
        }
    }
}