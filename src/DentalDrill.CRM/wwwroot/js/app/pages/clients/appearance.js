var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var Clients;
            (function (Clients) {
                var Appearance;
                (function (Appearance) {
                    var ColorPickerEx = /** @class */ (function () {
                        function ColorPickerEx(root) {
                            var _this = this;
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
                            this._input.addEventListener("change", function (e) {
                                if (kendo.parseColor(_this._input.value, true)) {
                                    _this._colorPicker.value(_this._input.value);
                                }
                            });
                            $(root).data("ColorPickerEx", this);
                        }
                        ColorPickerEx.prototype.configureColorPicker = function () {
                            var _this = this;
                            var existing = $(this._colorPickerElement).data("kendoFlatColorPicker");
                            if (existing) {
                                existing.bind("change", function (e) {
                                    _this._input.value = e.sender.value();
                                });
                                return existing;
                            }
                            else {
                                return undefined;
                            }
                        };
                        ColorPickerEx.prototype.createColorPicker = function () {
                            var _this = this;
                            var node = $(this._colorPickerElement);
                            var colorValue = kendo.parseColor(this._input.value, true) ? this._input.value : "#000000";
                            node.kendoFlatColorPicker({
                                preview: false,
                                value: colorValue,
                                change: function (e) {
                                    _this._input.value = e.sender.value();
                                }
                            });
                            var colorPicker = node.data("kendoFlatColorPicker");
                            if (this._input.readOnly) {
                                colorPicker.enable(false);
                            }
                            return colorPicker;
                        };
                        return ColorPickerEx;
                    }());
                    Appearance.ColorPickerEx = ColorPickerEx;
                    var AppearanceEditor = /** @class */ (function () {
                        function AppearanceEditor() {
                        }
                        AppearanceEditor.initForm = function (form) {
                            var colorPickers = form.querySelectorAll(".color-picker-ex");
                            for (var i = 0; i < colorPickers.length; i++) {
                                var element = colorPickers[i];
                                var picker = new ColorPickerEx(element);
                            }
                        };
                        return AppearanceEditor;
                    }());
                    Appearance.AppearanceEditor = AppearanceEditor;
                })(Appearance = Clients.Appearance || (Clients.Appearance = {}));
            })(Clients = Pages.Clients || (Pages.Clients = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=appearance.js.map