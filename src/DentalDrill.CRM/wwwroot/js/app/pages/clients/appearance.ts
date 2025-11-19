namespace DentalDrill.CRM.Pages.Clients.Appearance {
    export class ColorPickerEx {
        private readonly _root: HTMLElement;
        private readonly _colorPickerElement: HTMLElement;
        private readonly _colorPicker: kendo.ui.FlatColorPicker;
        private readonly _input: HTMLInputElement;

        constructor(root: HTMLElement) {
            this._root = root;
            this._colorPickerElement = this._root.querySelector(".color-picker-ex__picker");
            this._input = this._root.querySelector(".color-picker-ex__input");
            
            if (!this._colorPickerElement || !this._input) {
                throw new Error("Unable to locate required elements");
            }

            this._colorPicker = this.configureColorPicker();
            if (!this._colorPicker) {
                this._colorPicker = this.createColorPicker();
            }

            this._input.addEventListener("change", e => {
                if (kendo.parseColor(this._input.value, true)) {
                    this._colorPicker.value(this._input.value);
                }
            });

            $(root).data("ColorPickerEx", this);
        }

        private configureColorPicker(): kendo.ui.FlatColorPicker {
            const existing = $(this._colorPickerElement).data("kendoFlatColorPicker");
            if (existing) {
                existing.bind("change", (e: kendo.ui.FlatColorPickerChangeEvent) => {
                    this._input.value = e.sender.value();
                });

                return existing;
            } else {
                return undefined;
            }
        }

        private createColorPicker(): kendo.ui.FlatColorPicker {
            const node = $(this._colorPickerElement);
            const colorValue = kendo.parseColor(this._input.value, true) ? this._input.value : "#000000";

            node.kendoFlatColorPicker({
                preview: false,
                value: colorValue,
                change: (e: kendo.ui.FlatColorPickerChangeEvent) => {
                    this._input.value = e.sender.value();
                }
            });

            const colorPicker = node.data("kendoFlatColorPicker");
            if (this._input.readOnly) {
                colorPicker.enable(false);
            }

            return colorPicker;
        }
    }

    export class AppearanceEditor {
        static initForm(form: HTMLElement): void {
            const colorPickers = form.querySelectorAll(".color-picker-ex");
            for (let i = 0; i < colorPickers.length; i++) {
                const element = colorPickers[i] as HTMLElement;
                const picker = new ColorPickerEx(element);
            }
        }
    }
}