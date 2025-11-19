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
var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            var MapCounter = /** @class */ (function () {
                function MapCounter() {
                    this._map = new Map();
                }
                MapCounter.prototype.increment = function (key) {
                    if (this._map.has(key)) {
                        var value = this._map.get(key) + 1;
                        this._map.set(key, value);
                        return value;
                    }
                    else {
                        this._map.set(key, 1);
                        return 1;
                    }
                };
                MapCounter.prototype.count = function (key) {
                    if (this._map.has(key)) {
                        return this._map.get(key);
                    }
                    else {
                        return 0;
                    }
                };
                return MapCounter;
            }());
            var MultiSelectTogglerItem = /** @class */ (function () {
                function MultiSelectTogglerItem(root) {
                    this._root = root;
                    this._root.data("MultiSelectTogglerItem", this);
                }
                MultiSelectTogglerItem.prototype.activate = function () {
                    this._root.removeClass("multi-select-toggler__item--partial");
                    this._root.addClass("multi-select-toggler__item--active");
                };
                MultiSelectTogglerItem.prototype.deactivate = function () {
                    this._root.removeClass("multi-select-toggler__item--partial");
                    this._root.removeClass("multi-select-toggler__item--active");
                };
                MultiSelectTogglerItem.prototype.partial = function () {
                    this._root.addClass("multi-select-toggler__item--partial");
                    this._root.removeClass("multi-select-toggler__item--active");
                };
                return MultiSelectTogglerItem;
            }());
            Controls.MultiSelectTogglerItem = MultiSelectTogglerItem;
            var MultiSelectTogglerSpecificItem = /** @class */ (function (_super) {
                __extends(MultiSelectTogglerSpecificItem, _super);
                function MultiSelectTogglerSpecificItem(root, id) {
                    var _this = _super.call(this, root) || this;
                    _this._id = id;
                    return _this;
                }
                Object.defineProperty(MultiSelectTogglerSpecificItem.prototype, "id", {
                    get: function () {
                        return this._id;
                    },
                    enumerable: false,
                    configurable: true
                });
                return MultiSelectTogglerSpecificItem;
            }(MultiSelectTogglerItem));
            Controls.MultiSelectTogglerSpecificItem = MultiSelectTogglerSpecificItem;
            var MultiSelectToggler = /** @class */ (function () {
                function MultiSelectToggler(root) {
                    var _this = this;
                    this._root = root;
                    var multiSelectElement = $(root.attr("data-target"));
                    this._multiSelect = multiSelectElement.data("kendoMultiSelect");
                    this._propertyName = root.attr("data-property");
                    this._selectAllItem = this.initSelectAll(root);
                    this._toggleItems = this.initSpecific(root);
                    this._multiSelect.bind("change", function (e) {
                        _this.handleMultiSelectChange();
                    });
                    this.handleMultiSelectChange();
                }
                MultiSelectToggler.prototype.initSelectAll = function (root) {
                    var _this = this;
                    var item = root.find(".multi-select-toggler__item[data-toggle-all='true']");
                    if (item.length !== 0) {
                        item.on("click", function (e) {
                            var allActivated = _this.getAllActivated();
                            if (allActivated) {
                                _this._multiSelect.value([]);
                            }
                            else {
                                var dataItems = _this._multiSelect.dataSource.data();
                                var propertyId_1 = _this._multiSelect.options.dataValueField;
                                var ids = dataItems.map(function (x) { return x[propertyId_1]; });
                                _this._multiSelect.value(ids);
                            }
                            _this.handleMultiSelectChange();
                        });
                        return new MultiSelectTogglerItem(item);
                    }
                    else {
                        return undefined;
                    }
                };
                MultiSelectToggler.prototype.initSpecific = function (root) {
                    var _this = this;
                    var items = root.find(".multi-select-toggler__item[data-toggle-id]");
                    var result = [];
                    items.each(function (index, element) {
                        var item = $(element);
                        var itemId = item.attr("data-toggle-id");
                        item.on("click", function (e) {
                            var allActivated = _this.getAllActivatedOf(itemId);
                            if (allActivated) {
                                var ids_2 = _this.getAllIdsOf(itemId);
                                var selected = _this._multiSelect.value();
                                var updated = selected.filter(function (x) { return ids_2.every(function (y) { return y !== x; }); });
                                _this._multiSelect.value(updated);
                            }
                            else {
                                var ids = _this.getAllIdsOf(itemId);
                                var selected = _this._multiSelect.value();
                                var _loop_1 = function (id) {
                                    if (selected.every(function (x) { return x !== id; })) {
                                        selected.push(id);
                                    }
                                };
                                for (var _i = 0, ids_1 = ids; _i < ids_1.length; _i++) {
                                    var id = ids_1[_i];
                                    _loop_1(id);
                                }
                                _this._multiSelect.value(selected);
                            }
                            _this.handleMultiSelectChange();
                        });
                        result.push(new MultiSelectTogglerSpecificItem(item, itemId));
                    });
                    return result;
                };
                MultiSelectToggler.prototype.getAllIds = function () {
                    var result = [];
                    var dataItems = this._multiSelect.dataSource.data();
                    var propertyId = this._multiSelect.options.dataValueField;
                    for (var i = 0; i < dataItems.length; i++) {
                        var dataItem = dataItems[i];
                        result.push(dataItem[propertyId]);
                    }
                    return result;
                };
                MultiSelectToggler.prototype.getAllIdsOf = function (groupId) {
                    var result = [];
                    var dataItems = this._multiSelect.dataSource.data();
                    var propertyId = this._multiSelect.options.dataValueField;
                    for (var i = 0; i < dataItems.length; i++) {
                        var dataItem = dataItems[i];
                        if (dataItem[this._propertyName] === groupId) {
                            result.push(dataItem[propertyId]);
                        }
                    }
                    return result;
                };
                MultiSelectToggler.prototype.getAllActivated = function () {
                    var totalCount = 0;
                    var selectedCount = 0;
                    var selected = this._multiSelect.value();
                    var allIds = this.getAllIds();
                    var _loop_2 = function (i) {
                        totalCount++;
                        if (selected.some(function (x) { return x === allIds[i]; })) {
                            selectedCount++;
                        }
                    };
                    for (var i = 0; i < allIds.length; i++) {
                        _loop_2(i);
                    }
                    if (totalCount > 0 && totalCount === selectedCount) {
                        return true;
                    }
                    else if (totalCount > 0 && selectedCount < totalCount) {
                        return undefined;
                    }
                    else {
                        return false;
                    }
                };
                MultiSelectToggler.prototype.getAllActivatedOf = function (groupId) {
                    var totalCount = 0;
                    var selectedCount = 0;
                    var selected = this._multiSelect.value();
                    var allIds = this.getAllIdsOf(groupId);
                    var _loop_3 = function (i) {
                        totalCount++;
                        if (selected.some(function (x) { return x === allIds[i]; })) {
                            selectedCount++;
                        }
                    };
                    for (var i = 0; i < allIds.length; i++) {
                        _loop_3(i);
                    }
                    if (totalCount > 0 && totalCount === selectedCount) {
                        return true;
                    }
                    else if (totalCount > 0 && selectedCount < totalCount) {
                        return undefined;
                    }
                    else {
                        return false;
                    }
                };
                MultiSelectToggler.prototype.handleMultiSelectChange = function () {
                    var selected = this._multiSelect.value();
                    if (selected && selected.length > 0) {
                        var dataItems = this._multiSelect.dataSource.data();
                        var totalsCounter = new MapCounter();
                        var selectedCounter = new MapCounter();
                        var _loop_4 = function (i) {
                            var dataItem = dataItems[i];
                            totalsCounter.increment(dataItem.StateId);
                            if (selected.some(function (x) { return x === dataItem.Id; })) {
                                selectedCounter.increment(dataItem.StateId);
                            }
                        };
                        for (var i = 0; i < dataItems.length; i++) {
                            _loop_4(i);
                        }
                        var anyActivated = false;
                        var anyDeactivated = false;
                        for (var _i = 0, _a = this._toggleItems; _i < _a.length; _i++) {
                            var item = _a[_i];
                            var totalCount = totalsCounter.count(item.id);
                            if (totalCount === 0) {
                                continue;
                            }
                            var selectedCount = selectedCounter.count(item.id);
                            if (selectedCount === totalCount) {
                                item.activate();
                                anyActivated = true;
                            }
                            else if (selectedCount > 0 && selectedCount < totalCount) {
                                item.partial();
                                anyActivated = true;
                                anyDeactivated = true;
                            }
                            else {
                                item.deactivate();
                                anyDeactivated = true;
                            }
                        }
                        if (this._selectAllItem) {
                            if (anyActivated && anyDeactivated) {
                                this._selectAllItem.partial();
                            }
                            else if (anyActivated) {
                                this._selectAllItem.activate();
                            }
                            else {
                                this._selectAllItem.deactivate();
                            }
                        }
                    }
                    else {
                        this._selectAllItem.deactivate();
                        for (var _b = 0, _c = this._toggleItems; _b < _c.length; _b++) {
                            var item = _c[_b];
                            item.deactivate();
                        }
                    }
                };
                return MultiSelectToggler;
            }());
            Controls.MultiSelectToggler = MultiSelectToggler;
            $(function () {
                $(".multi-select-toggler[data-init='true']").each(function (index, element) {
                    var item = $(element);
                    var toggler = new MultiSelectToggler(item);
                });
            });
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=multiselect-toggler.js.map