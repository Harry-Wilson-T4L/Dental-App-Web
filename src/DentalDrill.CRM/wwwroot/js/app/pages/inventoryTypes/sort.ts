namespace DentalDrill.CRM.Pages.InventoryTypes.Sort {
    export class InventoryTypeSortEditor {
        private readonly _root: HTMLElement;

        private readonly _scopeLabel: HTMLLabelElement;
        private readonly _scopeRadioGroup: kendo.ui.RadioGroup;

        private readonly _specificSKULabel: HTMLLabelElement;
        private readonly _specificSKUDropDown: kendo.ui.DropDownList;

        private readonly _methodLabel: HTMLLabelElement;
        private readonly _methodRadioGroup: kendo.ui.RadioGroup;

        constructor(root: HTMLElement) {
            this._root = root;

            this._scopeLabel = root.querySelector("label[for=Scope]");
            this._scopeRadioGroup = $(root.querySelector(".k-radio-list[name=Scope]")).data("kendoRadioGroup");

            this._specificSKULabel = root.querySelector("label[for=SpecificSKU]");
            this._specificSKUDropDown = $(root.querySelector(".k-dropdown input[name=SpecificSKU]")).data("kendoDropDownList");

            this._methodLabel = root.querySelector("label[for=Method]");
            this._methodRadioGroup = $(root.querySelector(".k-radio-list[name=Method]")).data("kendoRadioGroup");
        }

        init() {
            this._scopeRadioGroup.bind("change", (e: kendo.ui.RadioGroupChangeEvent) => {
                const newValue = e.sender.value();
                if (newValue == "Specific" || newValue == "SpecificRecursive") {
                    this._specificSKULabel.classList.remove("k-disabled");
                    this._specificSKUDropDown.enable(true);
                } else {
                    this._specificSKULabel.classList.add("k-disabled");
                    this._specificSKUDropDown.enable(false);
                }
            });
        }
    }
}