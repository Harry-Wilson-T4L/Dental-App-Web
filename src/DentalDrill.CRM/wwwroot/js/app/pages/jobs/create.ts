namespace DentalDrill.CRM.Pages.Jobs.Create {
    interface HandpieceGridItemComponent {
        Brand: string;
        Model: string;
        Serial: string;
    }

    interface HandpieceGridItem {
        Position: number;
        ClientHandpieceId: string;
        Brand: string;
        Model: string;
        Serial: string;
        Components: HandpieceGridItemComponent[];
        ProblemDescription: string;
    }

    interface ClientExistingHandpiece {
        Id: string;
        Brand: string;
        Model: string;
        Serial: string;
        Components: HandpieceGridItemComponent[];
    }

    function levenshtein(a: string, b: string): number {
        function _min(d0, d1, d2, bx, ay) {
            return d0 < d1 || d2 < d1
                ? d0 > d2
                ? d2 + 1
                : d0 + 1
                : bx === ay
                ? d1
                : d1 + 1;
        }

        if (a === b) {
            return 0;
        }

        if (a.length > b.length) {
            const swap = a;
            a = b;
            b = swap;
        }

        let la = a.length;
        let lb = b.length;
        while (la > 0 && (a.charCodeAt(la - 1) === b.charCodeAt(lb - 1))) {
            la--;
            lb--;
        }

        let offset = 0;
        while (offset < la && (a.charCodeAt(offset) === b.charCodeAt(offset))) {
            offset++;
        }

        la -= offset;
        lb -= offset;
        if (la === 0 || lb < 3) {
            return lb;
        }

        const vector: number[] = [];
        for (let y = 0; y < la; y++) {
            vector.push(y + 1);
            vector.push(a.charCodeAt(offset + y));
        }

        let dd;
        const len = vector.length - 1;
        let x = 0;
        for (; x < lb - 3;) {
            let d0: number;
            let d1: number;
            let d2: number;
            let d3: number;
            const bx0 = b.charCodeAt(offset + (d0 = x));
            const bx1 = b.charCodeAt(offset + (d1 = x + 1));
            const bx2 = b.charCodeAt(offset + (d2 = x + 2));
            const bx3 = b.charCodeAt(offset + (d3 = x + 3));
            dd = (x += 4);
            for (let y = 0; y < len; y += 2) {
                const dy = vector[y];
                const ay = vector[y + 1];
                d0 = _min(dy, d0, d1, bx0, ay);
                d1 = _min(d0, d1, d2, bx1, ay);
                d2 = _min(d1, d2, d3, bx2, ay);
                dd = _min(d2, d3, dd, bx3, ay);
                vector[y] = dd;
                d3 = d2;
                d2 = d1;
                d1 = d0;
                d0 = dy;
            }
        }

        for (; x < lb;) {
            let d0: number;
            const bx0 = b.charCodeAt(offset + (d0 = x));
            dd = ++x;
            for (let y = 0; y < len; y += 2) {
                const dy = vector[y];
                vector[y] = dd = _min(dy, d0, dd, bx0, vector[y + 1]);
                d0 = dy;
            }
        }

        return dd;
    }

    interface EditorControl {
        value: string;
        disabled: boolean;
        readOnly: boolean;

        onChange(handler: (e: Event) => void);
        addClass(className: string): void;
        removeClass(className: string): void;
    }

    class BasicInputEditorControl implements EditorControl {
        private readonly _input: HTMLInputElement;

        constructor(input: HTMLInputElement) {
            this._input = input;
        }

        get value(): string {
            return this._input.value;
        }

        set value(val: string) {
            this._input.value = val;
        }

        get disabled(): boolean {
            return this._input.disabled;
        }

        set disabled(val: boolean) {
            this._input.disabled = val;
        }

        get readOnly(): boolean {
            return this._input.readOnly;
        }

        set readOnly(val: boolean) {
            this._input.readOnly = val;
        }

        onChange(handler: (e: Event) => void) {
            this._input.addEventListener("change", e => handler(e));
        }

        addClass(className: string): void {
            this._input.classList.add(className);
        }

        removeClass(className: string): void {
            this._input.classList.remove(className);
        }
    }

    class AutoCompleteEditorControl implements EditorControl {
        private readonly _control: kendo.ui.AutoComplete;

        constructor(control: kendo.ui.AutoComplete) {
            this._control = control;
        }

        get value(): string {
            return this._control.value();
        }

        set value(val: string) {
            this._control.value(val);
        }

        get disabled(): boolean {
            return !this._control.options.enable;
        }

        set disabled(val: boolean) {
            this._control.enable(!val);
        }

        get readOnly(): boolean {
            return undefined;
        }

        set readOnly(val: boolean) {
            this._control.readonly(val);
        }

        onChange(handler: (e: Event) => void) {
            this._control.bind("change", e => handler(e));
        }

        addClass(className: string): void {
            this._control.wrapper.addClass(className);
        }

        removeClass(className: string): void {
            this._control.wrapper.removeClass(className);
        }
    }

    export class HandpieceEditorControl {
        private static _instance: HandpieceEditorControl;
        private static _instanceRoot: HTMLElement;

        private readonly _root: HTMLElement;

        private readonly _componentsContainer: HTMLDivElement;
        private readonly _componentsAddButton: HTMLButtonElement;

        private readonly _warningContainer: HTMLDivElement;
        
        private readonly _exitingSelector: kendo.ui.DropDownList;
        private readonly _change: HTMLInputElement;
        private readonly _serial: EditorControl;
        private readonly _brand: EditorControl;
        private readonly _model: EditorControl;
        private readonly _problem: EditorControl;

        private readonly _saveButton: HTMLButtonElement;
        private readonly _cancelButton: HTMLButtonElement;

        private _warnedItem: HandpieceGridItem;
        private _editedItem: HandpieceGridItem;

        constructor(root: HTMLElement) {
            console.log("Initialized");
            this._root = root;

            this._componentsContainer = root.querySelector(".handpiece-editor-control__components");
            this._componentsAddButton = root.querySelector(".handpiece-editor-control__add-component");

            this._warningContainer = root.querySelector(".handpiece-editor-control__warning-container");
            
            this._exitingSelector = $(root).find("#ExistingClientHandpiece").data("kendoDropDownList");
            this._change = root.querySelector("[name=EditorChange]");
            this._serial = new BasicInputEditorControl(root.querySelector("[name=EditorSerial]"));
            this._brand = new AutoCompleteEditorControl(HandpieceEditorControl.createAutoComplete(root.querySelector("[name=EditorBrand]"), "/Jobs/BrandsAutoComplete"));
            this._model = new AutoCompleteEditorControl(HandpieceEditorControl.createAutoComplete(root.querySelector("[name=EditorModel]"), "/Jobs/ModelsAutoComplete", () => ({ brand: this._brand.value })));
            this._problem = new AutoCompleteEditorControl(HandpieceEditorControl.createAutoComplete(root.querySelector("[name=EditorProblem]"), "/Jobs/ProblemsAutoComplete"));

            this._saveButton = root.querySelector(".handpiece-editor-control-save");
            this._cancelButton = root.querySelector(".handpiece-editor-control-cancel");

            this._componentsAddButton.addEventListener("click", e => this.addComponent());

            this._change.addEventListener("click", e => {
                if (this._change.disabled) {
                    return;
                }

                if (this._change.checked) {
                    this.enableEditing();
                } else {
                    this.disableEditing();
                }
            });

            this._serial.onChange(e => this._serial.removeClass("manual-error"));
            this._brand.onChange(e => this._brand.removeClass("manual-error"));
            this._model.onChange(e => this._model.removeClass("manual-error"));
            this._problem.onChange(e => this._problem.removeClass("manual-error"));

            this._saveButton.addEventListener("click", e => this.onSave());
            this._cancelButton.addEventListener("click", e => this.onCancel());
        }

        static get instance(): HandpieceEditorControl {
            const root = document.getElementById("HandpieceEditorControl");
            if (HandpieceEditorControl._instance === undefined || HandpieceEditorControl._instanceRoot !== root) {
                HandpieceEditorControl._instanceRoot = root;
                HandpieceEditorControl._instance = new HandpieceEditorControl(root);
            }
            
            return HandpieceEditorControl._instance;
        }

        static handleClientChange(e: kendo.ui.DropDownListChangeEvent) {
            HandpieceEditorControl.instance.onClientChange(e);
        }

        static handleExistingChange(e: kendo.ui.DropDownListChangeEvent) {
            HandpieceEditorControl.instance.onChange();
        }

        static compareItems(first: HandpieceGridItem, second: HandpieceGridItem): { MainDistance: number, ComponentsDistance: number } {
            const max: (first: number, second: number) => number = (first, second) => first > second ? first : second;

            let mainDistance = 0;
            mainDistance = max(mainDistance, levenshtein(first.Serial, second.Serial));
            mainDistance = max(mainDistance, levenshtein(first.Brand, second.Brand));
            mainDistance = max(mainDistance, levenshtein(first.Model, second.Model));

            let componentsDistance = 0;
            for (let i = 0; i < max(first.Components.length, second.Components.length); i++) {
                componentsDistance = max(componentsDistance, levenshtein(i < first.Components.length ? first.Components[i].Serial : ``, i < second.Components.length ? second.Components[i].Serial : ``));
                componentsDistance = max(componentsDistance, levenshtein(i < first.Components.length ? first.Components[i].Brand : ``, i < second.Components.length ? second.Components[i].Brand : ``));
                componentsDistance = max(componentsDistance, levenshtein(i < first.Components.length ? first.Components[i].Model : ``, i < second.Components.length ? second.Components[i].Model : ``));
            }

            return { MainDistance: mainDistance, ComponentsDistance: componentsDistance };
        }

        public init() {
        }

        static createAutoComplete(input: HTMLInputElement, url: string, urlData?: () => object): kendo.ui.AutoComplete {
            let isDisabled = false;
            let isReadOnly = false;

            if (input.disabled) {
                input.disabled = false;
                isDisabled = true;
            }

            if (input.readOnly) {
                input.readOnly = false;
                isReadOnly = false;
            }

            const node = $(input);
            node.kendoAutoComplete({
                dataSource: new kendo.data.DataSource({
                    type: "aspnetmvc-ajax",
                    transport: {
                        read: {
                            url: url,
                            data: urlData,
                        },
                    },
                    sort: [
                        { field: "Name", dir: "asc" },
                    ],
                    schema: {
                        data: "Data",
                        total: "Total",
                        errors: "Errors",
                    },
                    serverAggregates: true,
                    serverFiltering: true,
                    serverGrouping: true,
                    serverPaging: true,
                    serverSorting: true,
                }),
                dataTextField: "Name",
                filter: "startswith",
                valuePrimitive: true,
                minLength: 2,
            })

            const control = node.data("kendoAutoComplete");

            if (isDisabled) {
                control.enable(false);
            }

            if (isReadOnly) {
                control.readonly(true);
            }

            return control;
        }

        private onClientChange(e: kendo.ui.DropDownListChangeEvent) {
            this._exitingSelector.value("00000000-0000-0000-0000-000000000000");
            this._exitingSelector.one("dataBound", () => this.onChange());
        }

        private onChange() {
            const selectedItem = this._exitingSelector.dataItem();
            if (selectedItem !== undefined) {
                this._serial.disabled = false;
                this._brand.disabled = false;
                this._model.disabled = false;
                this._problem.disabled = false;

                if (selectedItem.Type === 0) {
                    this._change.disabled = false;
                    this._change.checked = false;

                    this.setHandpiece(selectedItem, false);
                    this.disableEditing();
                } else if (selectedItem.Type === 1) {
                    this._change.checked = false;
                    this._change.disabled = true;

                    this.enableEditing();
                }
            }
        }

        private enableEditing() {
            this._serial.readOnly = false;
            this._brand.readOnly = false;
            this._model.readOnly = false;
            this._componentsAddButton.disabled = false;
            this._componentsContainer.querySelectorAll(".handpiece-editor-control__remove-component").forEach((item: HTMLButtonElement) => item.disabled = false);
            this._componentsContainer.querySelectorAll("[data-name=Serial]").forEach((item: HTMLInputElement) => item.readOnly = false);
            this._componentsContainer.querySelectorAll("[data-name=Brand]").forEach((item: HTMLInputElement) => item.readOnly = false);
            this._componentsContainer.querySelectorAll("[data-name=Model]").forEach((item: HTMLInputElement) => item.readOnly = false);
        }

        private disableEditing() {
            this._serial.readOnly = true;
            this._brand.readOnly = true;
            this._model.readOnly = true;
            this._componentsAddButton.disabled = true;
            this._componentsContainer.querySelectorAll(".handpiece-editor-control__remove-component").forEach((item: HTMLButtonElement) => item.disabled = true);
            this._componentsContainer.querySelectorAll("[data-name=Serial]").forEach((item: HTMLInputElement) => item.readOnly = true);
            this._componentsContainer.querySelectorAll("[data-name=Brand]").forEach((item: HTMLInputElement) => item.readOnly = true);
            this._componentsContainer.querySelectorAll("[data-name=Model]").forEach((item: HTMLInputElement) => item.readOnly = true);
        }

        private parseItem(): HandpieceGridItem {
            const item: HandpieceGridItem = {
                Position: 0,
                ClientHandpieceId: this._exitingSelector.value(),
                Serial: this._serial.value,
                Brand: this._brand.value,
                Model: this._model.value,
                Components: [],
                ProblemDescription: this._problem.value,
            };

            for (let i = 0; i < this._componentsContainer.children.length; i++) {
                const containerControl = this._componentsContainer.children[i];
                const componentSerial = (containerControl.querySelector("[data-name=Serial]") as HTMLInputElement).value;
                const componentBrand = (containerControl.querySelector("[data-name=Brand]") as HTMLInputElement).value;
                const componentModel = (containerControl.querySelector("[data-name=Model]") as HTMLInputElement).value;
                item.Components.push({
                    Brand: componentBrand,
                    Model: componentModel,
                    Serial: componentSerial,
                });
            }

            return item;
        }

        private validate(): boolean {
            let isValid = true;

            if (!this._serial.value) {
                this._serial.addClass("manual-error");
                isValid = false;
            }

            if (!this._brand.value) {
                this._brand.addClass("manual-error");
                isValid = false;
            }

            if (!this._model.value) {
                this._model.addClass("manual-error");
                isValid = false;
            }

            for (let i = 0; i < this._componentsContainer.children.length; i++) {
                const containerControl = this._componentsContainer.children[i];
                const componentSerialInput = (containerControl.querySelector("[data-name=Serial]") as HTMLInputElement);
                const componentBrandInput = (containerControl.querySelector("[data-name=Brand]") as HTMLInputElement);
                const componentModelInput = (containerControl.querySelector("[data-name=Model]") as HTMLInputElement);

                if (!componentSerialInput.value) {
                    componentSerialInput.classList.add("manual-error");
                    isValid = false;
                }

                if (!componentBrandInput.value) {
                    componentBrandInput.closest(".k-widget").classList.add("manual-error");
                    isValid = false;
                }

                if (!componentModelInput.value) {
                    componentModelInput.closest(".k-widget").classList.add("manual-error");
                    isValid = false;
                }
            }

            return isValid;
        }

        private validateUnique(item: HandpieceGridItem): boolean {
            const selector = this._exitingSelector.value();
            if (selector && selector !== `00000000-0000-0000-0000-000000000000`) {
                return true;
            }

            if (this.hasWarning(item)) {
                return true;
            }

            const max: (first: number, second: number) => number = (first, second) => first > second ? first : second;

            const knownHandpieces = this._exitingSelector.dataSource.data<ClientExistingHandpiece>();
            const matchingHandpieces: { Handpiece: ClientExistingHandpiece; MainDistance: number; ComponentsDistance: number }[] = [];
            for (let i = 0; i < knownHandpieces.length; i++) {
                const knownHandpiece = knownHandpieces[i];
                const distance = HandpieceEditorControl.compareItems(item, knownHandpiece as any as HandpieceGridItem);
                
                if (distance.MainDistance <= 1) {
                    matchingHandpieces.push({
                        Handpiece: knownHandpiece,
                        MainDistance: distance.MainDistance,
                        ComponentsDistance: distance.ComponentsDistance,
                    });
                }
            }

            if (matchingHandpieces.length === 0) {
                return true;
            }

            if (matchingHandpieces.filter(x => x.MainDistance === 0 && x.ComponentsDistance === 0).length > 0) {
                const warning = 'One or more existing handpieces are matching entered data exactly. Click again to add anyway';
                this.showWarning(warning, item);
                return false;
            }

            if (matchingHandpieces.filter(x => x.MainDistance <= 1).length > 0) {
                const warning = 'One or more existing handpieces are almost matching entered data. Click again to add anyway';
                this.showWarning(warning, item);
                return false;
            }

            return true;
        }

        private showWarning(warning: string, item: HandpieceGridItem) {
            this.clearWarning();
            this._warnedItem = item;
            const warningSpan = this._warningContainer.appendChild(document.createElement("div"));
            warningSpan.classList.add("alert");
            warningSpan.classList.add("alert-warning");
            warningSpan.innerText = warning;
        }

        private hasWarning(item: HandpieceGridItem): boolean {
            if (this._warningContainer.innerText.trim().length > 0) {
                const distance = HandpieceEditorControl.compareItems(item, this._warnedItem);
                return distance.MainDistance === 0 && distance.ComponentsDistance === 0;
            }

            return false;
        }

        private clearWarning() {
            this._warnedItem = undefined;
            while (this._warningContainer.children.length > 0) {
                this._warningContainer.children[0].remove();
            }
        }

        private onSave() {
            if (!this.validate()) {
                return;
            }

            const parsedItem = this.parseItem();
            if (this._editedItem === undefined) {
                if (!this.validateUnique(parsedItem)) {
                    return;
                }
                
                const newItem: HandpieceGridItem = {
                    Position: HandpiecesGrid.instance.dataSource.data().length + 1,
                    ClientHandpieceId: parsedItem.ClientHandpieceId,
                    Serial: parsedItem.Serial,
                    Brand: parsedItem.Brand,
                    Model: parsedItem.Model,
                    Components: [],
                    ProblemDescription: parsedItem.ProblemDescription,
                };

                for (let i = 0; i < parsedItem.Components.length; i++) {
                    newItem.Components.push(parsedItem.Components[i]);
                }

                HandpiecesGrid.instance.dataSource.add(newItem);
            } else {
                this._editedItem.ClientHandpieceId = parsedItem.ClientHandpieceId;
                this._editedItem.Serial = parsedItem.Serial;
                this._editedItem.Brand = parsedItem.Brand;
                this._editedItem.Model = parsedItem.Model;
                this._editedItem.Components.splice(0, this._editedItem.Components.length);
                this._editedItem.ProblemDescription = parsedItem.ProblemDescription;

                for (let i = 0; i < parsedItem.Components.length; i++) {
                    this._editedItem.Components.push(parsedItem.Components[i]);
                }

                HandpiecesGrid.instance.dataSource.sync();
            }

            this._exitingSelector.value("00000000-0000-0000-0000-000000000000");
            this.onChange()
            this.clear();
            this.clearWarning();
            this._editedItem = undefined;
            this._saveButton.innerText = `Add Handpiece`;
        }

        private onCancel() {
            this._exitingSelector.value("00000000-0000-0000-0000-000000000000");
            this.onChange()
            this.clear();
            this.clearWarning();
            this._editedItem = undefined;
            this._saveButton.innerText = `Add Handpiece`;
        }

        public editItem(item: HandpieceGridItem) {
            this.setHandpiece(item, true);
            this._editedItem = item;
            this._saveButton.innerText = `Save`;
            this.clearWarning();
        }

        private setHandpiece(item: HandpieceGridItem, setExisting: boolean) {
            if (setExisting) {
                if (item.ClientHandpieceId) {
                    this._exitingSelector.value(item.ClientHandpieceId);
                } else {
                    this._exitingSelector.value("00000000-0000-0000-0000-000000000000");
                }
            }

            this._serial.value = item.Serial ? item.Serial : ``;
            this._brand.value = item.Brand ? item.Brand : ``;
            this._model.value = item.Model ? item.Model : ``;
            this._problem.value = item.ProblemDescription ? item.ProblemDescription : ``;

            while (this._componentsContainer.childNodes.length > 0) {
                this._componentsContainer.childNodes[0].remove();
            }

            for (let component of item.Components) {
                this.addComponent(component);
            }
        }

        private clear() {
            this._serial.value = ``;
            this._brand.value = ``;
            this._model.value = ``;
            this._problem.value = ``;

            while (this._componentsContainer.childNodes.length > 0) {
                this._componentsContainer.childNodes[0].remove();
            }
        }

        private addComponent(component?: HandpieceGridItemComponent) {
            const componentWrapper = document.createElement("div");
            componentWrapper.classList.add("row");
            componentWrapper.classList.add("handpiece-editor-control-component");

            const createField = (name: string, labelText: string, includeRemoveButton: boolean, inputConstructor: (input: HTMLInputElement) => void) => {
                const wrapper = document.createElement("div");
                wrapper.classList.add("col-md-4");
                wrapper.classList.add("mb-2");

                const row = wrapper.appendChild(document.createElement("div"));
                row.classList.add("row");

                const labelWrapper = row.appendChild(document.createElement("div"));
                labelWrapper.classList.add("col-sm-4");
                const label = labelWrapper.appendChild(document.createElement("label"));
                label.innerText = labelText;
                if (includeRemoveButton) {
                    const removeButton = labelWrapper.appendChild(document.createElement("button"));
                    removeButton.type = "button";
                    removeButton.classList.add("k-button");
                    removeButton.classList.add("handpiece-editor-control__remove-component")
                    removeButton.innerHTML = `<span class="fas fa-minus"></span>`;
                    removeButton.addEventListener("click", e => {
                        componentWrapper.remove();
                    });
                }

                const inputWrapper = row.appendChild(document.createElement("div"));
                inputWrapper.classList.add("col-sm-8");
                const input = inputWrapper.appendChild(document.createElement("input"));
                input.setAttribute(`data-name`, name);
                inputConstructor(input);
                input.type = "text";

                return wrapper;
            };

            componentWrapper.appendChild(createField(`Serial`, `Serial`, true, input => {
                input.addEventListener("change", e => input.classList.remove("manual-error"));
                input.classList.add("k-textbox");
                input.type = "text";
                if (component && component.Serial) {
                    input.value = component.Serial;
                }
            }));

            let brandAutoComplete: kendo.ui.AutoComplete;
            componentWrapper.appendChild(createField(`Brand`, `Brand`, false, input => {
                input.classList.add("w-100");
                input.type = "text";
                brandAutoComplete = HandpieceEditorControl.createAutoComplete(input, "/Jobs/BrandsAutoComplete");
                if (component && component.Brand) {
                    brandAutoComplete.value(component.Brand);
                }

                brandAutoComplete.bind("change", e => {
                    brandAutoComplete.wrapper.removeClass("manual-error");
                });
            }));

            let modelAutoComplete: kendo.ui.AutoComplete;
            componentWrapper.appendChild(createField(`Model`, `Model`, false, input => {
                input.classList.add("w-100");
                input.type = "text";
                modelAutoComplete = HandpieceEditorControl.createAutoComplete(input, "/Jobs/ModelsAutoComplete", () => ({ brand: brandAutoComplete.value() }));
                if (component && component.Model) {
                    modelAutoComplete.value(component.Model);
                }

                modelAutoComplete.bind("change", e => {
                    modelAutoComplete.wrapper.removeClass("manual-error");
                });
            }));

            this._componentsContainer.appendChild(componentWrapper);
        }
    }

    export class HandpiecesGrid {
        static get instance(): kendo.ui.Grid {
            return $("#HandpiecesGrid").data("kendoGrid");
        }

        static init(): void {
            const grid = HandpiecesGrid.instance;
            const form = grid.element.closest("form");
            form.on("submit", (e: JQueryEventObject) => {
                const gridData = grid.dataSource.data<HandpieceGridItem>();
                let inputsHost = form.find(".form-pre-submit-inputs");
                if (inputsHost.length === 0) {
                    inputsHost = $("<div class=\"form-pre-submit-inputs\" style=\"display: none;\"></div>");
                    form.append(inputsHost);
                } else {
                    inputsHost.empty();
                }

                const submittedData = gridData.map(x => ({
                    Position: x.Position,
                    ClientHandpieceId: x.ClientHandpieceId,
                    Brand: x.Brand,
                    Model: x.Model,
                    Serial: x.Serial,
                    Components: x.Components.map(y => ({
                        Brand: y.Brand,
                        Model: y.Model,
                        Serial: y.Serial,
                    })),
                    ProblemDescription: x.ProblemDescription,
                }));

                inputsHost.append($("<input />")
                    .attr("type", "hidden")
                    .attr("name", `Handpieces`)
                    .attr("value", JSON.stringify(submittedData)));
            });

            HandpieceEditorControl.instance.init();

            const clientSelector = $("#ClientId").data("kendoDropDownList");
            if (clientSelector !== undefined) {
                const clientId = clientSelector.value()
                if (clientId && clientId !== '00000000-0000-0000-0000-000000000000') {
                    HandpieceEditorControl.handleClientChange(undefined);
                }
            }
        }

        static handleEditClick(this: kendo.ui.Grid, e: JQueryEventObject) {
            e.preventDefault();
            const dataItem = this.dataItem<HandpieceGridItem>(e.currentTarget.closest("tr"));
            HandpieceEditorControl.instance.editItem(dataItem);
        }

        static handleDeleteClick(this: kendo.ui.Grid, e: JQueryEventObject) {
            e.preventDefault();
            const dataItem = this.dataItem<HandpieceGridItem>(e.currentTarget.closest("tr"));
            this.dataSource.remove(dataItem);
            const data = this.dataSource.data<HandpieceGridItem>();
            let position = 1;
            for (let i = 0; i < data.length; i++) {
                if (data[i].uid !== dataItem.uid) {
                    data[i].Position = position++;
                }
            }

            this.dataSource.sync();
        }
    }
}