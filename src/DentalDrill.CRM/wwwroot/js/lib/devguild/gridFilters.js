var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var DevGuild;
(function (DevGuild) {
    var Filters;
    (function (Filters) {
        var Grids;
        (function (Grids) {
            var GridFilterField = /** @class */ (function () {
                function GridFilterField() {
                }
                return GridFilterField;
            }());
            Grids.GridFilterField = GridFilterField;
            var IntSelectGridFilterField = /** @class */ (function (_super) {
                __extends(IntSelectGridFilterField, _super);
                function IntSelectGridFilterField(root, fieldName) {
                    var _this = _super.call(this) || this;
                    _this._root = root;
                    _this._fieldName = fieldName;
                    return _this;
                }
                IntSelectGridFilterField.prototype.apply = function (filters, exceptions) {
                    var _this = this;
                    var value = this._root.val();
                    if (Array.isArray(value)) {
                        throw new Error('Value cannot be array');
                    }
                    if (typeof value === 'string') {
                        value = parseInt(value);
                    }
                    if ((!exceptions || exceptions.every(function (x) { return x !== _this._fieldName; })) && value) {
                        filters.push({
                            field: this._fieldName,
                            operator: "eq",
                            value: value
                        });
                    }
                };
                IntSelectGridFilterField.prototype.reset = function () {
                    this._root.val("").trigger("change");
                };
                Object.defineProperty(IntSelectGridFilterField.prototype, "control", {
                    get: function () {
                        return this._root;
                    },
                    enumerable: false,
                    configurable: true
                });
                return IntSelectGridFilterField;
            }(GridFilterField));
            Grids.IntSelectGridFilterField = IntSelectGridFilterField;
            var IdDropDownListGridFilterField = /** @class */ (function (_super) {
                __extends(IdDropDownListGridFilterField, _super);
                function IdDropDownListGridFilterField(root, fieldName, operator) {
                    if (operator === void 0) { operator = "eq"; }
                    var _this = _super.call(this) || this;
                    _this._root = root;
                    _this._fieldName = fieldName;
                    _this._operator = operator;
                    return _this;
                }
                IdDropDownListGridFilterField.prototype.apply = function (filters, exceptions) {
                    var _this = this;
                    var value = this._root.data("kendoDropDownList").dataItem().Id;
                    if ((!exceptions || exceptions.every(function (x) { return x !== _this._fieldName; })) && value && value !== "00000000-0000-0000-0000-000000000000") {
                        filters.push({
                            field: this._fieldName,
                            operator: this._operator,
                            value: value
                        });
                    }
                };
                IdDropDownListGridFilterField.prototype.reset = function () {
                    this._root.data('kendoDropDownList').trigger("change");
                };
                Object.defineProperty(IdDropDownListGridFilterField.prototype, "control", {
                    get: function () {
                        return this._root;
                    },
                    enumerable: false,
                    configurable: true
                });
                return IdDropDownListGridFilterField;
            }(GridFilterField));
            Grids.IdDropDownListGridFilterField = IdDropDownListGridFilterField;
            var DropDownListGridFilterField = /** @class */ (function (_super) {
                __extends(DropDownListGridFilterField, _super);
                function DropDownListGridFilterField(root, fieldName, _a) {
                    var _b = _a === void 0 ? {} : _a, _c = _b.defaultValue, defaultValue = _c === void 0 ? null : _c, _d = _b.operator, operator = _d === void 0 ? "eq" : _d, _e = _b.value, value = _e === void 0 ? function (a) { return a; } : _e;
                    var _this = _super.call(this) || this;
                    _this._root = root;
                    _this._fieldName = fieldName;
                    _this._operator = operator;
                    _this._defaultValue = defaultValue;
                    _this._getValue = value;
                    return _this;
                }
                DropDownListGridFilterField.prototype.apply = function (filters, exceptions) {
                    var _this = this;
                    var value = this._getValue(this._root.data("kendoDropDownList").dataItem());
                    if ((!exceptions || exceptions.every(function (x) { return x !== _this._fieldName; })) && value && (!this._defaultValue || value !== this._defaultValue)) {
                        if (this._applyValueDelegate) {
                            this._applyValueDelegate(filters, value);
                        }
                        else {
                            filters.push({
                                field: this._fieldName,
                                operator: this._operator,
                                value: value
                            });
                        }
                    }
                };
                DropDownListGridFilterField.prototype.reset = function () {
                    var dropDown = this._root.data("kendoDropDownList");
                    if (this._resetValueDelegate) {
                        dropDown.value(this._resetValueDelegate());
                    }
                    else {
                        dropDown.value(this._defaultValue);
                    }
                    dropDown.trigger("change");
                };
                Object.defineProperty(DropDownListGridFilterField.prototype, "control", {
                    get: function () {
                        return this._root;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(DropDownListGridFilterField.prototype, "kendoControl", {
                    get: function () {
                        return this._root.data("kendoDropDownList");
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(DropDownListGridFilterField.prototype, "applyValueDelegate", {
                    get: function () {
                        return this._applyValueDelegate;
                    },
                    set: function (val) {
                        this._applyValueDelegate = val;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(DropDownListGridFilterField.prototype, "resetValueDelegate", {
                    get: function () {
                        return this._resetValueDelegate;
                    },
                    set: function (val) {
                        this._resetValueDelegate = val;
                    },
                    enumerable: false,
                    configurable: true
                });
                return DropDownListGridFilterField;
            }(GridFilterField));
            Grids.DropDownListGridFilterField = DropDownListGridFilterField;
            var ComplexDropDownListGridFilterField = /** @class */ (function (_super) {
                __extends(ComplexDropDownListGridFilterField, _super);
                function ComplexDropDownListGridFilterField(root, fieldName, _a) {
                    var _b = _a === void 0 ? {} : _a, _c = _b.defaultValue, defaultValue = _c === void 0 ? null : _c, _d = _b.operator, operator = _d === void 0 ? "eq" : _d, _e = _b.value, value = _e === void 0 ? function (a) { return a; } : _e;
                    var _this = _super.call(this) || this;
                    _this._root = root;
                    _this._fieldName = fieldName;
                    _this._operator = operator;
                    _this._defaultValue = defaultValue;
                    _this._getValue = value;
                    return _this;
                }
                ComplexDropDownListGridFilterField.prototype.apply = function (filters, exceptions) {
                    var _this = this;
                    var value = this._getValue(this._root.data("kendoDropDownList").dataItem());
                    if ((!exceptions || exceptions.every(function (x) { return x !== _this._fieldName; })) && value && (this._defaultValue === undefined || value.Value !== this._defaultValue)) {
                        filters.push({
                            field: this._fieldName,
                            operator: this._operator,
                            value: value.Value
                        });
                    }
                };
                ComplexDropDownListGridFilterField.prototype.reset = function () {
                    this._root.data('kendoDropDownList').trigger("change");
                };
                Object.defineProperty(ComplexDropDownListGridFilterField.prototype, "control", {
                    get: function () {
                        return this._root;
                    },
                    enumerable: false,
                    configurable: true
                });
                return ComplexDropDownListGridFilterField;
            }(GridFilterField));
            Grids.ComplexDropDownListGridFilterField = ComplexDropDownListGridFilterField;
            var MultiSelectGridFilterField = /** @class */ (function (_super) {
                __extends(MultiSelectGridFilterField, _super);
                function MultiSelectGridFilterField(root, fieldName) {
                    var _this = _super.call(this) || this;
                    _this._root = root;
                    _this._fieldName = fieldName;
                    return _this;
                }
                MultiSelectGridFilterField.prototype.apply = function (filters, exceptions) {
                    var _this = this;
                    var values = this._root.data('kendoMultiSelect').value();
                    var innerFilters = [];
                    for (var i = 0; i < values.length; i++) {
                        var value = values[i];
                        if ((!exceptions || exceptions.every(function (x) { return x !== _this._fieldName; })) && value) {
                            innerFilters.push({
                                field: this._fieldName,
                                operator: "eq",
                                value: value
                            });
                        }
                    }
                    if (innerFilters.length) {
                        filters.push({
                            logic: "or",
                            filters: innerFilters
                        });
                    }
                };
                MultiSelectGridFilterField.prototype.reset = function () {
                    this._root.data('kendoMultiSelect').trigger("change");
                };
                Object.defineProperty(MultiSelectGridFilterField.prototype, "control", {
                    get: function () {
                        return this._root;
                    },
                    enumerable: false,
                    configurable: true
                });
                return MultiSelectGridFilterField;
            }(GridFilterField));
            Grids.MultiSelectGridFilterField = MultiSelectGridFilterField;
            var BooleanSelectGridFilterField = /** @class */ (function (_super) {
                __extends(BooleanSelectGridFilterField, _super);
                function BooleanSelectGridFilterField(root, fieldName) {
                    var _this = _super.call(this) || this;
                    _this._root = root;
                    _this._fieldName = fieldName;
                    return _this;
                }
                BooleanSelectGridFilterField.prototype.apply = function (filters, exceptions) {
                    var _this = this;
                    var value = this._root.val();
                    if ((!exceptions || exceptions.every(function (x) { return x !== _this._fieldName; })) && value) {
                        switch (value) {
                            case "true":
                                filters.push({
                                    field: this._fieldName,
                                    operator: "eq",
                                    value: "true"
                                });
                                break;
                            case "false":
                                filters.push({
                                    field: this._fieldName,
                                    operator: "eq",
                                    value: "false"
                                });
                                break;
                        }
                    }
                };
                BooleanSelectGridFilterField.prototype.reset = function () {
                    this._root.val("").trigger("change");
                };
                Object.defineProperty(BooleanSelectGridFilterField.prototype, "control", {
                    get: function () {
                        return this._root;
                    },
                    enumerable: false,
                    configurable: true
                });
                return BooleanSelectGridFilterField;
            }(GridFilterField));
            Grids.BooleanSelectGridFilterField = BooleanSelectGridFilterField;
            var StringInputGridFilterField = /** @class */ (function (_super) {
                __extends(StringInputGridFilterField, _super);
                function StringInputGridFilterField(root, fieldName, operator) {
                    if (operator === void 0) { operator = "contains"; }
                    var _this = _super.call(this) || this;
                    _this._root = root;
                    _this._fieldName = fieldName;
                    _this._operator = operator;
                    return _this;
                }
                StringInputGridFilterField.prototype.apply = function (filters, exceptions) {
                    var _this = this;
                    var value = this._root.val();
                    if ((!exceptions || exceptions.every(function (x) { return x !== _this._fieldName; })) && value) {
                        filters.push({
                            field: this._fieldName,
                            operator: this._operator,
                            value: value
                        });
                    }
                };
                StringInputGridFilterField.prototype.reset = function () {
                    this._root.val("").trigger("change");
                };
                Object.defineProperty(StringInputGridFilterField.prototype, "control", {
                    get: function () {
                        return this._root;
                    },
                    enumerable: false,
                    configurable: true
                });
                return StringInputGridFilterField;
            }(GridFilterField));
            Grids.StringInputGridFilterField = StringInputGridFilterField;
            var DatePickerFilterField = /** @class */ (function (_super) {
                __extends(DatePickerFilterField, _super);
                function DatePickerFilterField(root, fieldName, operator) {
                    if (operator === void 0) { operator = "eq"; }
                    var _this = _super.call(this) || this;
                    _this._root = root;
                    _this._fieldName = fieldName;
                    _this._operator = operator;
                    return _this;
                }
                DatePickerFilterField.prototype.apply = function (filters, exceptions) {
                    var _this = this;
                    var value = this._root.data("kendoDatePicker").value();
                    if ((!exceptions || exceptions.every(function (x) { return x !== _this._fieldName; })) && value) {
                        filters.push({
                            field: this._fieldName,
                            operator: this._operator,
                            value: value
                        });
                    }
                };
                DatePickerFilterField.prototype.reset = function () {
                    this._root.data("kendoDatePicker").value(null);
                    this._root.data("kendoDatePicker").trigger("change");
                };
                Object.defineProperty(DatePickerFilterField.prototype, "control", {
                    get: function () {
                        return this._root;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(DatePickerFilterField.prototype, "kendoControl", {
                    get: function () {
                        return this._root.data("kendoDatePicker");
                    },
                    enumerable: false,
                    configurable: true
                });
                return DatePickerFilterField;
            }(GridFilterField));
            Grids.DatePickerFilterField = DatePickerFilterField;
            var StringInputGridFilterMultiField = /** @class */ (function (_super) {
                __extends(StringInputGridFilterMultiField, _super);
                function StringInputGridFilterMultiField(root, fieldNames) {
                    var _this = _super.call(this) || this;
                    _this._root = root;
                    _this._fieldNames = fieldNames;
                    return _this;
                }
                StringInputGridFilterMultiField.prototype.apply = function (filters, exceptions) {
                    var value = this._root.val();
                    var innerFilters = [];
                    var _loop_1 = function () {
                        var fieldName = this_1._fieldNames[i];
                        if ((!exceptions || exceptions.every(function (x) { return x !== fieldName; })) && value) {
                            innerFilters.push({
                                field: fieldName,
                                operator: "contains",
                                value: value
                            });
                        }
                    };
                    var this_1 = this;
                    for (var i = 0; i < this._fieldNames.length; i++) {
                        _loop_1();
                    }
                    if (innerFilters.length) {
                        filters.push({
                            logic: "or",
                            filters: innerFilters
                        });
                    }
                };
                StringInputGridFilterMultiField.prototype.reset = function () {
                    this._root.val("").trigger("change");
                };
                Object.defineProperty(StringInputGridFilterMultiField.prototype, "control", {
                    get: function () {
                        return this._root;
                    },
                    enumerable: false,
                    configurable: true
                });
                return StringInputGridFilterMultiField;
            }(GridFilterField));
            Grids.StringInputGridFilterMultiField = StringInputGridFilterMultiField;
            var IntInputGridFilterField = /** @class */ (function (_super) {
                __extends(IntInputGridFilterField, _super);
                function IntInputGridFilterField(root, fieldName, operator) {
                    if (operator === void 0) { operator = 'eq'; }
                    var _this = _super.call(this) || this;
                    _this._root = root;
                    _this._operator = operator;
                    _this._fieldName = fieldName;
                    return _this;
                }
                IntInputGridFilterField.prototype.apply = function (filters, exceptions) {
                    var _this = this;
                    var value = this._root.val();
                    var operator = this._operator;
                    if ((!exceptions || exceptions.every(function (x) { return x !== _this._fieldName; })) && value) {
                        filters.push({
                            field: this._fieldName,
                            operator: operator,
                            value: value
                        });
                    }
                };
                IntInputGridFilterField.prototype.reset = function () {
                    this._root.val("").trigger("change");
                };
                return IntInputGridFilterField;
            }(GridFilterField));
            Grids.IntInputGridFilterField = IntInputGridFilterField;
            var IntInputGridFilterFieldWithOperator = /** @class */ (function (_super) {
                __extends(IntInputGridFilterFieldWithOperator, _super);
                function IntInputGridFilterFieldWithOperator(root, operator, fieldName) {
                    var _this = _super.call(this) || this;
                    _this._root = root;
                    _this._operator = operator;
                    _this._fieldName = fieldName;
                    return _this;
                }
                IntInputGridFilterFieldWithOperator.prototype.apply = function (filters, exceptions) {
                    var _this = this;
                    var value = this._root.val();
                    var operator = this._operator.val();
                    if (typeof operator !== 'string') {
                        throw new Error('Operator must be function');
                    }
                    operator = this.convertOperatorToFilter(operator);
                    if ((!exceptions || exceptions.every(function (x) { return x !== _this._fieldName; })) && value && operator) {
                        filters.push({
                            field: this._fieldName,
                            operator: operator,
                            value: value
                        });
                    }
                };
                IntInputGridFilterFieldWithOperator.prototype.reset = function () {
                    this._root.val("").trigger("change");
                    this._operator.val("EqualsTo").trigger("change");
                };
                IntInputGridFilterFieldWithOperator.prototype.convertOperatorToFilter = function (operator) {
                    if (operator) {
                        switch (operator) {
                            case "EqualsTo":
                                return "eq";
                            case "NotEqualsTo":
                                return "neq";
                            case "GreaterThan":
                                return "gt";
                            case "GreaterThanOrEqualsTo":
                                return "gte";
                            case "LessThan":
                                return "lt";
                            case "LessThanOrEqualsTo":
                                return "lte";
                        }
                    }
                    return undefined;
                };
                return IntInputGridFilterFieldWithOperator;
            }(GridFilterField));
            Grids.IntInputGridFilterFieldWithOperator = IntInputGridFilterFieldWithOperator;
            var NumberInputGridFilterField = /** @class */ (function (_super) {
                __extends(NumberInputGridFilterField, _super);
                function NumberInputGridFilterField(root, fieldName, operator) {
                    if (operator === void 0) { operator = "eq"; }
                    var _this = _super.call(this) || this;
                    _this._root = root;
                    _this._operator = operator;
                    _this._fieldName = fieldName;
                    return _this;
                }
                NumberInputGridFilterField.prototype.apply = function (filters, exceptions) {
                    var _this = this;
                    var value = this._root.val();
                    var operator = this._operator;
                    if ((!exceptions || exceptions.every(function (x) { return x !== _this._fieldName; })) && value && operator) {
                        filters.push({
                            field: this._fieldName,
                            operator: operator,
                            value: value
                        });
                    }
                };
                NumberInputGridFilterField.prototype.reset = function () {
                    this._root.val("").trigger("change");
                };
                return NumberInputGridFilterField;
            }(GridFilterField));
            Grids.NumberInputGridFilterField = NumberInputGridFilterField;
            var GridFilterFieldsCollection = /** @class */ (function () {
                function GridFilterFieldsCollection(root) {
                    this._root = root;
                }
                Object.defineProperty(GridFilterFieldsCollection.prototype, "root", {
                    get: function () {
                        return this._root;
                    },
                    enumerable: false,
                    configurable: true
                });
                return GridFilterFieldsCollection;
            }());
            Grids.GridFilterFieldsCollection = GridFilterFieldsCollection;
            var DynamicGridFilter = /** @class */ (function () {
                function DynamicGridFilter(root) {
                    this._root = root;
                }
                Object.defineProperty(DynamicGridFilter.prototype, "root", {
                    get: function () {
                        return this._root;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(DynamicGridFilter.prototype, "fields", {
                    get: function () {
                        return this._fields;
                    },
                    enumerable: false,
                    configurable: true
                });
                DynamicGridFilter.prototype.initialize = function () {
                    this._fields = this.createFields(this.root);
                    this.additionalInitialization();
                };
                DynamicGridFilter.prototype.getFilters = function (exceptions) {
                    var filters = [];
                    this._fields.applyAll(filters, exceptions);
                    return filters;
                };
                DynamicGridFilter.prototype.additionalInitialization = function () {
                };
                DynamicGridFilter.prototype.apply = function (grid, exceptions) {
                    grid.dataSource.filter({ logic: "and", filters: this.getFilters(exceptions) });
                    grid.dataSource.read();
                };
                DynamicGridFilter.prototype.reset = function (grid) {
                    this._fields.resetAll();
                    this.apply(grid);
                };
                return DynamicGridFilter;
            }());
            Grids.DynamicGridFilter = DynamicGridFilter;
            var GridFilterCore = /** @class */ (function () {
                function GridFilterCore(root) {
                    this._root = root;
                }
                Object.defineProperty(GridFilterCore.prototype, "root", {
                    get: function () {
                        return this._root;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(GridFilterCore.prototype, "fields", {
                    get: function () {
                        return this._fields;
                    },
                    enumerable: false,
                    configurable: true
                });
                GridFilterCore.prototype.initialize = function () {
                    this._fields = this.createFields(this.root);
                    this.additionalInitialization();
                };
                GridFilterCore.prototype.getFilters = function (exceptions) {
                    if (!this._fields) {
                        this.initialize();
                    }
                    var filters = [];
                    this._fields.applyAll(filters, exceptions);
                    return filters;
                };
                GridFilterCore.prototype.additionalInitialization = function () {
                };
                GridFilterCore.prototype.apply = function () {
                    this.applyFilter({ logic: "and", filters: this.getFilters() });
                };
                GridFilterCore.prototype.reset = function () {
                    this._fields.resetAll();
                    this.apply();
                };
                return GridFilterCore;
            }());
            Grids.GridFilterCore = GridFilterCore;
            var GridFilter = /** @class */ (function () {
                function GridFilter(root, grid) {
                    this._root = root;
                    this._grid = grid;
                }
                Object.defineProperty(GridFilter.prototype, "root", {
                    get: function () {
                        return this._root;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(GridFilter.prototype, "grid", {
                    get: function () {
                        return this._grid;
                    },
                    set: function (newGrid) {
                        this._grid = newGrid;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(GridFilter.prototype, "fields", {
                    get: function () {
                        return this._fields;
                    },
                    enumerable: false,
                    configurable: true
                });
                GridFilter.prototype.initialize = function () {
                    this._fields = this.createFields(this.root);
                    this.additionalInitialization();
                };
                GridFilter.prototype.getFilters = function (exceptions) {
                    var filters = [];
                    this._fields.applyAll(filters, exceptions);
                    return filters;
                };
                GridFilter.prototype.additionalInitialization = function () {
                };
                GridFilter.prototype.apply = function () {
                    this.grid.dataSource.filter({ logic: "and", filters: this.getFilters() });
                };
                GridFilter.prototype.reset = function () {
                    this._fields.resetAll();
                    this.apply();
                };
                return GridFilter;
            }());
            Grids.GridFilter = GridFilter;
            var GridsFilter = /** @class */ (function () {
                function GridsFilter(root, grids) {
                    this._root = root;
                    this._grids = grids;
                }
                Object.defineProperty(GridsFilter.prototype, "root", {
                    get: function () {
                        return this._root;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(GridsFilter.prototype, "grids", {
                    get: function () {
                        return this._grids;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(GridsFilter.prototype, "fields", {
                    get: function () {
                        return this._fields;
                    },
                    enumerable: false,
                    configurable: true
                });
                GridsFilter.prototype.initialize = function () {
                    this._fields = this.createFields(this.root);
                    this.additionalInitialization();
                };
                GridsFilter.prototype.getFilters = function (n, exceptions) {
                    var filters = [];
                    this._fields[n].applyAll(filters, exceptions);
                    return filters;
                };
                GridsFilter.prototype.additionalInitialization = function () {
                };
                GridsFilter.prototype.apply = function () {
                    var grids = this.grids;
                    for (var i = 0; i < grids.length; i++) {
                        var filters = this.getFilters(i);
                        var result = { logic: "and", filters: filters };
                        grids[i].dataSource.filter(result);
                        this.additionalApply(result, i);
                    }
                };
                GridsFilter.prototype.reset = function () {
                    this._fields.forEach(function (field) { return field.resetAll(); });
                    this.apply();
                };
                return GridsFilter;
            }());
            Grids.GridsFilter = GridsFilter;
        })(Grids = Filters.Grids || (Filters.Grids = {}));
    })(Filters = DevGuild.Filters || (DevGuild.Filters = {}));
})(DevGuild || (DevGuild = {}));
//# sourceMappingURL=gridFilters.js.map