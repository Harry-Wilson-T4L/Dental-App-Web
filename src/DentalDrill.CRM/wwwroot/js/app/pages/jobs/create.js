var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var Jobs;
            (function (Jobs) {
                var Create;
                (function (Create) {
                    function levenshtein(a, b) {
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
                            var swap = a;
                            a = b;
                            b = swap;
                        }
                        var la = a.length;
                        var lb = b.length;
                        while (la > 0 && (a.charCodeAt(la - 1) === b.charCodeAt(lb - 1))) {
                            la--;
                            lb--;
                        }
                        var offset = 0;
                        while (offset < la && (a.charCodeAt(offset) === b.charCodeAt(offset))) {
                            offset++;
                        }
                        la -= offset;
                        lb -= offset;
                        if (la === 0 || lb < 3) {
                            return lb;
                        }
                        var vector = [];
                        for (var y = 0; y < la; y++) {
                            vector.push(y + 1);
                            vector.push(a.charCodeAt(offset + y));
                        }
                        var dd;
                        var len = vector.length - 1;
                        var x = 0;
                        for (; x < lb - 3;) {
                            var d0 = void 0;
                            var d1 = void 0;
                            var d2 = void 0;
                            var d3 = void 0;
                            var bx0 = b.charCodeAt(offset + (d0 = x));
                            var bx1 = b.charCodeAt(offset + (d1 = x + 1));
                            var bx2 = b.charCodeAt(offset + (d2 = x + 2));
                            var bx3 = b.charCodeAt(offset + (d3 = x + 3));
                            dd = (x += 4);
                            for (var y = 0; y < len; y += 2) {
                                var dy = vector[y];
                                var ay = vector[y + 1];
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
                            var d0 = void 0;
                            var bx0 = b.charCodeAt(offset + (d0 = x));
                            dd = ++x;
                            for (var y = 0; y < len; y += 2) {
                                var dy = vector[y];
                                vector[y] = dd = _min(dy, d0, dd, bx0, vector[y + 1]);
                                d0 = dy;
                            }
                        }
                        return dd;
                    }
                    var BasicInputEditorControl = /** @class */ (function () {
                        function BasicInputEditorControl(input) {
                            this._input = input;
                        }
                        Object.defineProperty(BasicInputEditorControl.prototype, "value", {
                            get: function () {
                                return this._input.value;
                            },
                            set: function (val) {
                                this._input.value = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(BasicInputEditorControl.prototype, "disabled", {
                            get: function () {
                                return this._input.disabled;
                            },
                            set: function (val) {
                                this._input.disabled = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(BasicInputEditorControl.prototype, "readOnly", {
                            get: function () {
                                return this._input.readOnly;
                            },
                            set: function (val) {
                                this._input.readOnly = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        BasicInputEditorControl.prototype.onChange = function (handler) {
                            this._input.addEventListener("change", function (e) { return handler(e); });
                        };
                        BasicInputEditorControl.prototype.addClass = function (className) {
                            this._input.classList.add(className);
                        };
                        BasicInputEditorControl.prototype.removeClass = function (className) {
                            this._input.classList.remove(className);
                        };
                        return BasicInputEditorControl;
                    }());
                    var AutoCompleteEditorControl = /** @class */ (function () {
                        function AutoCompleteEditorControl(control) {
                            this._control = control;
                        }
                        Object.defineProperty(AutoCompleteEditorControl.prototype, "value", {
                            get: function () {
                                return this._control.value();
                            },
                            set: function (val) {
                                this._control.value(val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(AutoCompleteEditorControl.prototype, "disabled", {
                            get: function () {
                                return !this._control.options.enable;
                            },
                            set: function (val) {
                                this._control.enable(!val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(AutoCompleteEditorControl.prototype, "readOnly", {
                            get: function () {
                                return undefined;
                            },
                            set: function (val) {
                                this._control.readonly(val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        AutoCompleteEditorControl.prototype.onChange = function (handler) {
                            this._control.bind("change", function (e) { return handler(e); });
                        };
                        AutoCompleteEditorControl.prototype.addClass = function (className) {
                            this._control.wrapper.addClass(className);
                        };
                        AutoCompleteEditorControl.prototype.removeClass = function (className) {
                            this._control.wrapper.removeClass(className);
                        };
                        return AutoCompleteEditorControl;
                    }());
                    var HandpieceEditorControl = /** @class */ (function () {
                        function HandpieceEditorControl(root) {
                            var _this = this;
                            console.log("Initialized");
                            this._root = root;
                            this._componentsContainer = root.querySelector(".handpiece-editor-control__components");
                            this._componentsAddButton = root.querySelector(".handpiece-editor-control__add-component");
                            this._warningContainer = root.querySelector(".handpiece-editor-control__warning-container");
                            this._exitingSelector = $(root).find("#ExistingClientHandpiece").data("kendoDropDownList");
                            this._change = root.querySelector("[name=EditorChange]");
                            this._serial = new BasicInputEditorControl(root.querySelector("[name=EditorSerial]"));
                            this._brand = new AutoCompleteEditorControl(HandpieceEditorControl.createAutoComplete(root.querySelector("[name=EditorBrand]"), "/Jobs/BrandsAutoComplete"));
                            this._model = new AutoCompleteEditorControl(HandpieceEditorControl.createAutoComplete(root.querySelector("[name=EditorModel]"), "/Jobs/ModelsAutoComplete", function () { return ({ brand: _this._brand.value }); }));
                            this._problem = new AutoCompleteEditorControl(HandpieceEditorControl.createAutoComplete(root.querySelector("[name=EditorProblem]"), "/Jobs/ProblemsAutoComplete"));
                            this._saveButton = root.querySelector(".handpiece-editor-control-save");
                            this._cancelButton = root.querySelector(".handpiece-editor-control-cancel");
                            this._componentsAddButton.addEventListener("click", function (e) { return _this.addComponent(); });
                            this._change.addEventListener("click", function (e) {
                                if (_this._change.disabled) {
                                    return;
                                }
                                if (_this._change.checked) {
                                    _this.enableEditing();
                                }
                                else {
                                    _this.disableEditing();
                                }
                            });
                            this._serial.onChange(function (e) { return _this._serial.removeClass("manual-error"); });
                            this._brand.onChange(function (e) { return _this._brand.removeClass("manual-error"); });
                            this._model.onChange(function (e) { return _this._model.removeClass("manual-error"); });
                            this._problem.onChange(function (e) { return _this._problem.removeClass("manual-error"); });
                            this._saveButton.addEventListener("click", function (e) { return _this.onSave(); });
                            this._cancelButton.addEventListener("click", function (e) { return _this.onCancel(); });
                        }
                        Object.defineProperty(HandpieceEditorControl, "instance", {
                            get: function () {
                                var root = document.getElementById("HandpieceEditorControl");
                                if (HandpieceEditorControl._instance === undefined || HandpieceEditorControl._instanceRoot !== root) {
                                    HandpieceEditorControl._instanceRoot = root;
                                    HandpieceEditorControl._instance = new HandpieceEditorControl(root);
                                }
                                return HandpieceEditorControl._instance;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        HandpieceEditorControl.handleClientChange = function (e) {
                            HandpieceEditorControl.instance.onClientChange(e);
                        };
                        HandpieceEditorControl.handleExistingChange = function (e) {
                            HandpieceEditorControl.instance.onChange();
                        };
                        HandpieceEditorControl.compareItems = function (first, second) {
                            var max = function (first, second) { return first > second ? first : second; };
                            var mainDistance = 0;
                            mainDistance = max(mainDistance, levenshtein(first.Serial, second.Serial));
                            mainDistance = max(mainDistance, levenshtein(first.Brand, second.Brand));
                            mainDistance = max(mainDistance, levenshtein(first.Model, second.Model));
                            var componentsDistance = 0;
                            for (var i = 0; i < max(first.Components.length, second.Components.length); i++) {
                                componentsDistance = max(componentsDistance, levenshtein(i < first.Components.length ? first.Components[i].Serial : "", i < second.Components.length ? second.Components[i].Serial : ""));
                                componentsDistance = max(componentsDistance, levenshtein(i < first.Components.length ? first.Components[i].Brand : "", i < second.Components.length ? second.Components[i].Brand : ""));
                                componentsDistance = max(componentsDistance, levenshtein(i < first.Components.length ? first.Components[i].Model : "", i < second.Components.length ? second.Components[i].Model : ""));
                            }
                            return { MainDistance: mainDistance, ComponentsDistance: componentsDistance };
                        };
                        HandpieceEditorControl.prototype.init = function () {
                        };
                        HandpieceEditorControl.createAutoComplete = function (input, url, urlData) {
                            var isDisabled = false;
                            var isReadOnly = false;
                            if (input.disabled) {
                                input.disabled = false;
                                isDisabled = true;
                            }
                            if (input.readOnly) {
                                input.readOnly = false;
                                isReadOnly = false;
                            }
                            var node = $(input);
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
                            });
                            var control = node.data("kendoAutoComplete");
                            if (isDisabled) {
                                control.enable(false);
                            }
                            if (isReadOnly) {
                                control.readonly(true);
                            }
                            return control;
                        };
                        HandpieceEditorControl.prototype.onClientChange = function (e) {
                            var _this = this;
                            this._exitingSelector.value("00000000-0000-0000-0000-000000000000");
                            this._exitingSelector.one("dataBound", function () { return _this.onChange(); });
                        };
                        HandpieceEditorControl.prototype.onChange = function () {
                            var selectedItem = this._exitingSelector.dataItem();
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
                                }
                                else if (selectedItem.Type === 1) {
                                    this._change.checked = false;
                                    this._change.disabled = true;
                                    this.enableEditing();
                                }
                            }
                        };
                        HandpieceEditorControl.prototype.enableEditing = function () {
                            this._serial.readOnly = false;
                            this._brand.readOnly = false;
                            this._model.readOnly = false;
                            this._componentsAddButton.disabled = false;
                            this._componentsContainer.querySelectorAll(".handpiece-editor-control__remove-component").forEach(function (item) { return item.disabled = false; });
                            this._componentsContainer.querySelectorAll("[data-name=Serial]").forEach(function (item) { return item.readOnly = false; });
                            this._componentsContainer.querySelectorAll("[data-name=Brand]").forEach(function (item) { return item.readOnly = false; });
                            this._componentsContainer.querySelectorAll("[data-name=Model]").forEach(function (item) { return item.readOnly = false; });
                        };
                        HandpieceEditorControl.prototype.disableEditing = function () {
                            this._serial.readOnly = true;
                            this._brand.readOnly = true;
                            this._model.readOnly = true;
                            this._componentsAddButton.disabled = true;
                            this._componentsContainer.querySelectorAll(".handpiece-editor-control__remove-component").forEach(function (item) { return item.disabled = true; });
                            this._componentsContainer.querySelectorAll("[data-name=Serial]").forEach(function (item) { return item.readOnly = true; });
                            this._componentsContainer.querySelectorAll("[data-name=Brand]").forEach(function (item) { return item.readOnly = true; });
                            this._componentsContainer.querySelectorAll("[data-name=Model]").forEach(function (item) { return item.readOnly = true; });
                        };
                        HandpieceEditorControl.prototype.parseItem = function () {
                            var item = {
                                Position: 0,
                                ClientHandpieceId: this._exitingSelector.value(),
                                Serial: this._serial.value,
                                Brand: this._brand.value,
                                Model: this._model.value,
                                Components: [],
                                ProblemDescription: this._problem.value,
                            };
                            for (var i = 0; i < this._componentsContainer.children.length; i++) {
                                var containerControl = this._componentsContainer.children[i];
                                var componentSerial = containerControl.querySelector("[data-name=Serial]").value;
                                var componentBrand = containerControl.querySelector("[data-name=Brand]").value;
                                var componentModel = containerControl.querySelector("[data-name=Model]").value;
                                item.Components.push({
                                    Brand: componentBrand,
                                    Model: componentModel,
                                    Serial: componentSerial,
                                });
                            }
                            return item;
                        };
                        HandpieceEditorControl.prototype.validate = function () {
                            var isValid = true;
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
                            for (var i = 0; i < this._componentsContainer.children.length; i++) {
                                var containerControl = this._componentsContainer.children[i];
                                var componentSerialInput = containerControl.querySelector("[data-name=Serial]");
                                var componentBrandInput = containerControl.querySelector("[data-name=Brand]");
                                var componentModelInput = containerControl.querySelector("[data-name=Model]");
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
                        };
                        HandpieceEditorControl.prototype.validateUnique = function (item) {
                            var selector = this._exitingSelector.value();
                            if (selector && selector !== "00000000-0000-0000-0000-000000000000") {
                                return true;
                            }
                            if (this.hasWarning(item)) {
                                return true;
                            }
                            var max = function (first, second) { return first > second ? first : second; };
                            var knownHandpieces = this._exitingSelector.dataSource.data();
                            var matchingHandpieces = [];
                            for (var i = 0; i < knownHandpieces.length; i++) {
                                var knownHandpiece = knownHandpieces[i];
                                var distance = HandpieceEditorControl.compareItems(item, knownHandpiece);
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
                            if (matchingHandpieces.filter(function (x) { return x.MainDistance === 0 && x.ComponentsDistance === 0; }).length > 0) {
                                var warning = 'One or more existing handpieces are matching entered data exactly. Click again to add anyway';
                                this.showWarning(warning, item);
                                return false;
                            }
                            if (matchingHandpieces.filter(function (x) { return x.MainDistance <= 1; }).length > 0) {
                                var warning = 'One or more existing handpieces are almost matching entered data. Click again to add anyway';
                                this.showWarning(warning, item);
                                return false;
                            }
                            return true;
                        };
                        HandpieceEditorControl.prototype.showWarning = function (warning, item) {
                            this.clearWarning();
                            this._warnedItem = item;
                            var warningSpan = this._warningContainer.appendChild(document.createElement("div"));
                            warningSpan.classList.add("alert");
                            warningSpan.classList.add("alert-warning");
                            warningSpan.innerText = warning;
                        };
                        HandpieceEditorControl.prototype.hasWarning = function (item) {
                            if (this._warningContainer.innerText.trim().length > 0) {
                                var distance = HandpieceEditorControl.compareItems(item, this._warnedItem);
                                return distance.MainDistance === 0 && distance.ComponentsDistance === 0;
                            }
                            return false;
                        };
                        HandpieceEditorControl.prototype.clearWarning = function () {
                            this._warnedItem = undefined;
                            while (this._warningContainer.children.length > 0) {
                                this._warningContainer.children[0].remove();
                            }
                        };
                        HandpieceEditorControl.prototype.onSave = function () {
                            if (!this.validate()) {
                                return;
                            }
                            var parsedItem = this.parseItem();
                            if (this._editedItem === undefined) {
                                if (!this.validateUnique(parsedItem)) {
                                    return;
                                }
                                var newItem = {
                                    Position: HandpiecesGrid.instance.dataSource.data().length + 1,
                                    ClientHandpieceId: parsedItem.ClientHandpieceId,
                                    Serial: parsedItem.Serial,
                                    Brand: parsedItem.Brand,
                                    Model: parsedItem.Model,
                                    Components: [],
                                    ProblemDescription: parsedItem.ProblemDescription,
                                };
                                for (var i = 0; i < parsedItem.Components.length; i++) {
                                    newItem.Components.push(parsedItem.Components[i]);
                                }
                                HandpiecesGrid.instance.dataSource.add(newItem);
                            }
                            else {
                                this._editedItem.ClientHandpieceId = parsedItem.ClientHandpieceId;
                                this._editedItem.Serial = parsedItem.Serial;
                                this._editedItem.Brand = parsedItem.Brand;
                                this._editedItem.Model = parsedItem.Model;
                                this._editedItem.Components.splice(0, this._editedItem.Components.length);
                                this._editedItem.ProblemDescription = parsedItem.ProblemDescription;
                                for (var i = 0; i < parsedItem.Components.length; i++) {
                                    this._editedItem.Components.push(parsedItem.Components[i]);
                                }
                                HandpiecesGrid.instance.dataSource.sync();
                            }
                            this._exitingSelector.value("00000000-0000-0000-0000-000000000000");
                            this.onChange();
                            this.clear();
                            this.clearWarning();
                            this._editedItem = undefined;
                            this._saveButton.innerText = "Add Handpiece";
                        };
                        HandpieceEditorControl.prototype.onCancel = function () {
                            this._exitingSelector.value("00000000-0000-0000-0000-000000000000");
                            this.onChange();
                            this.clear();
                            this.clearWarning();
                            this._editedItem = undefined;
                            this._saveButton.innerText = "Add Handpiece";
                        };
                        HandpieceEditorControl.prototype.editItem = function (item) {
                            this.setHandpiece(item, true);
                            this._editedItem = item;
                            this._saveButton.innerText = "Save";
                            this.clearWarning();
                        };
                        HandpieceEditorControl.prototype.setHandpiece = function (item, setExisting) {
                            if (setExisting) {
                                if (item.ClientHandpieceId) {
                                    this._exitingSelector.value(item.ClientHandpieceId);
                                }
                                else {
                                    this._exitingSelector.value("00000000-0000-0000-0000-000000000000");
                                }
                            }
                            this._serial.value = item.Serial ? item.Serial : "";
                            this._brand.value = item.Brand ? item.Brand : "";
                            this._model.value = item.Model ? item.Model : "";
                            this._problem.value = item.ProblemDescription ? item.ProblemDescription : "";
                            while (this._componentsContainer.childNodes.length > 0) {
                                this._componentsContainer.childNodes[0].remove();
                            }
                            for (var _i = 0, _a = item.Components; _i < _a.length; _i++) {
                                var component = _a[_i];
                                this.addComponent(component);
                            }
                        };
                        HandpieceEditorControl.prototype.clear = function () {
                            this._serial.value = "";
                            this._brand.value = "";
                            this._model.value = "";
                            this._problem.value = "";
                            while (this._componentsContainer.childNodes.length > 0) {
                                this._componentsContainer.childNodes[0].remove();
                            }
                        };
                        HandpieceEditorControl.prototype.addComponent = function (component) {
                            var componentWrapper = document.createElement("div");
                            componentWrapper.classList.add("row");
                            componentWrapper.classList.add("handpiece-editor-control-component");
                            var createField = function (name, labelText, includeRemoveButton, inputConstructor) {
                                var wrapper = document.createElement("div");
                                wrapper.classList.add("col-md-4");
                                wrapper.classList.add("mb-2");
                                var row = wrapper.appendChild(document.createElement("div"));
                                row.classList.add("row");
                                var labelWrapper = row.appendChild(document.createElement("div"));
                                labelWrapper.classList.add("col-sm-4");
                                var label = labelWrapper.appendChild(document.createElement("label"));
                                label.innerText = labelText;
                                if (includeRemoveButton) {
                                    var removeButton = labelWrapper.appendChild(document.createElement("button"));
                                    removeButton.type = "button";
                                    removeButton.classList.add("k-button");
                                    removeButton.classList.add("handpiece-editor-control__remove-component");
                                    removeButton.innerHTML = "<span class=\"fas fa-minus\"></span>";
                                    removeButton.addEventListener("click", function (e) {
                                        componentWrapper.remove();
                                    });
                                }
                                var inputWrapper = row.appendChild(document.createElement("div"));
                                inputWrapper.classList.add("col-sm-8");
                                var input = inputWrapper.appendChild(document.createElement("input"));
                                input.setAttribute("data-name", name);
                                inputConstructor(input);
                                input.type = "text";
                                return wrapper;
                            };
                            componentWrapper.appendChild(createField("Serial", "Serial", true, function (input) {
                                input.addEventListener("change", function (e) { return input.classList.remove("manual-error"); });
                                input.classList.add("k-textbox");
                                input.type = "text";
                                if (component && component.Serial) {
                                    input.value = component.Serial;
                                }
                            }));
                            var brandAutoComplete;
                            componentWrapper.appendChild(createField("Brand", "Brand", false, function (input) {
                                input.classList.add("w-100");
                                input.type = "text";
                                brandAutoComplete = HandpieceEditorControl.createAutoComplete(input, "/Jobs/BrandsAutoComplete");
                                if (component && component.Brand) {
                                    brandAutoComplete.value(component.Brand);
                                }
                                brandAutoComplete.bind("change", function (e) {
                                    brandAutoComplete.wrapper.removeClass("manual-error");
                                });
                            }));
                            var modelAutoComplete;
                            componentWrapper.appendChild(createField("Model", "Model", false, function (input) {
                                input.classList.add("w-100");
                                input.type = "text";
                                modelAutoComplete = HandpieceEditorControl.createAutoComplete(input, "/Jobs/ModelsAutoComplete", function () { return ({ brand: brandAutoComplete.value() }); });
                                if (component && component.Model) {
                                    modelAutoComplete.value(component.Model);
                                }
                                modelAutoComplete.bind("change", function (e) {
                                    modelAutoComplete.wrapper.removeClass("manual-error");
                                });
                            }));
                            this._componentsContainer.appendChild(componentWrapper);
                        };
                        return HandpieceEditorControl;
                    }());
                    Create.HandpieceEditorControl = HandpieceEditorControl;
                    var HandpiecesGrid = /** @class */ (function () {
                        function HandpiecesGrid() {
                        }
                        Object.defineProperty(HandpiecesGrid, "instance", {
                            get: function () {
                                return $("#HandpiecesGrid").data("kendoGrid");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        HandpiecesGrid.init = function () {
                            var grid = HandpiecesGrid.instance;
                            var form = grid.element.closest("form");
                            form.on("submit", function (e) {
                                var gridData = grid.dataSource.data();
                                var inputsHost = form.find(".form-pre-submit-inputs");
                                if (inputsHost.length === 0) {
                                    inputsHost = $("<div class=\"form-pre-submit-inputs\" style=\"display: none;\"></div>");
                                    form.append(inputsHost);
                                }
                                else {
                                    inputsHost.empty();
                                }
                                var submittedData = gridData.map(function (x) { return ({
                                    Position: x.Position,
                                    ClientHandpieceId: x.ClientHandpieceId,
                                    Brand: x.Brand,
                                    Model: x.Model,
                                    Serial: x.Serial,
                                    Components: x.Components.map(function (y) { return ({
                                        Brand: y.Brand,
                                        Model: y.Model,
                                        Serial: y.Serial,
                                    }); }),
                                    ProblemDescription: x.ProblemDescription,
                                }); });
                                inputsHost.append($("<input />")
                                    .attr("type", "hidden")
                                    .attr("name", "Handpieces")
                                    .attr("value", JSON.stringify(submittedData)));
                            });
                            HandpieceEditorControl.instance.init();
                            var clientSelector = $("#ClientId").data("kendoDropDownList");
                            if (clientSelector !== undefined) {
                                var clientId = clientSelector.value();
                                if (clientId && clientId !== '00000000-0000-0000-0000-000000000000') {
                                    HandpieceEditorControl.handleClientChange(undefined);
                                }
                            }
                        };
                        HandpiecesGrid.handleEditClick = function (e) {
                            e.preventDefault();
                            var dataItem = this.dataItem(e.currentTarget.closest("tr"));
                            HandpieceEditorControl.instance.editItem(dataItem);
                        };
                        HandpiecesGrid.handleDeleteClick = function (e) {
                            e.preventDefault();
                            var dataItem = this.dataItem(e.currentTarget.closest("tr"));
                            this.dataSource.remove(dataItem);
                            var data = this.dataSource.data();
                            var position = 1;
                            for (var i = 0; i < data.length; i++) {
                                if (data[i].uid !== dataItem.uid) {
                                    data[i].Position = position++;
                                }
                            }
                            this.dataSource.sync();
                        };
                        return HandpiecesGrid;
                    }());
                    Create.HandpiecesGrid = HandpiecesGrid;
                })(Create = Jobs.Create || (Jobs.Create = {}));
            })(Jobs = Pages.Jobs || (Pages.Jobs = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=create.js.map