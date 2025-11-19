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
    var Utilities;
    (function (Utilities) {
        var EqualityComparer = /** @class */ (function () {
            function EqualityComparer(comparer) {
                this._comparer = comparer;
            }
            EqualityComparer.prototype.areEquals = function (first, second) {
                return this._comparer(first, second);
            };
            return EqualityComparer;
        }());
        Utilities.EqualityComparer = EqualityComparer;
        var Collection = /** @class */ (function () {
            function Collection(items) {
                this._items = items ? items : [];
            }
            Object.defineProperty(Collection.prototype, "items", {
                get: function () {
                    return this._items;
                },
                enumerable: false,
                configurable: true
            });
            Collection.prototype.add = function (item) {
                this._items.push(item);
            };
            Collection.prototype.insert = function (index, item) {
                this._items.splice(index, 0, item);
            };
            Collection.prototype.remove = function (item) {
                var index = this._items.indexOf(item);
                if (index < 0) {
                    return false;
                }
                this._items.splice(index, 1);
                return true;
            };
            Collection.prototype.removeAt = function (index) {
                if (index >= 0 && this._items.length > index) {
                    this._items.splice(index, 1);
                }
            };
            Collection.prototype.toArray = function () {
                return this._items;
            };
            Collection.prototype.select = function (converter) {
                return new Collection(this._items.map(function (value, index) { return converter(value, index); }));
            };
            Collection.prototype.count = function (predicate) {
                if (predicate) {
                    return this._items.filter(function (value, index) { return predicate(value, index); }).length;
                }
                else {
                    return this._items.length;
                }
            };
            Collection.prototype.sum = function (field) {
                if (field) {
                    return this._items.reduce(function (acc, x, index) { return acc + field(x, index); }, 0);
                }
                else {
                    return this._items.reduce(function (acc, x) { return acc + x; }, 0);
                }
            };
            Collection.prototype.where = function (predicate) {
                return new Collection(this._items.filter(function (value, index) { return predicate(value, index); }));
            };
            Collection.prototype.groupBy = function (groupKey) {
                var groupedCollection = new GroupedCollection();
                for (var i = 0; i < this._items.length; i++) {
                    groupedCollection.getOrCreateGroup(groupKey(this._items[i], i)).items.add(this._items[i]);
                }
                return groupedCollection;
            };
            return Collection;
        }());
        Utilities.Collection = Collection;
        var GroupedCollection = /** @class */ (function (_super) {
            __extends(GroupedCollection, _super);
            function GroupedCollection(items, keyComparer) {
                var _this = _super.call(this, items) || this;
                _this._keyComparer = keyComparer ? keyComparer : new DefaultGroupKeyComparer();
                return _this;
            }
            GroupedCollection.prototype.getGroup = function (key) {
                for (var _i = 0, _a = this.items; _i < _a.length; _i++) {
                    var group = _a[_i];
                    if (this._keyComparer.areEquals(group.key, key)) {
                        return group;
                    }
                }
                return undefined;
            };
            GroupedCollection.prototype.getOrCreateGroup = function (key) {
                for (var _i = 0, _a = this.items; _i < _a.length; _i++) {
                    var group = _a[_i];
                    if (this._keyComparer.areEquals(group.key, key)) {
                        return group;
                    }
                }
                var newGroup = {
                    key: key,
                    items: new Collection()
                };
                this.items.push(newGroup);
                return newGroup;
            };
            return GroupedCollection;
        }(Collection));
        Utilities.GroupedCollection = GroupedCollection;
        var DefaultGroupKeyComparer = /** @class */ (function () {
            function DefaultGroupKeyComparer() {
            }
            DefaultGroupKeyComparer.prototype.areEquals = function (first, second) {
                return this.objectPropertyEquals(first, second);
            };
            DefaultGroupKeyComparer.prototype.objectEquals = function (first, second) {
                var firstKeys = Object.keys(first);
                var secondKeys = Object.keys(second);
                if (firstKeys.length !== secondKeys.length) {
                    return false;
                }
                for (var _i = 0, firstKeys_1 = firstKeys; _i < firstKeys_1.length; _i++) {
                    var key = firstKeys_1[_i];
                    var secondIndex = secondKeys.indexOf(key);
                    if (secondIndex < 0) {
                        return false;
                    }
                    secondKeys.splice(secondIndex, 1);
                }
                if (secondKeys.length > 0) {
                    return false;
                }
                for (var _a = 0, firstKeys_2 = firstKeys; _a < firstKeys_2.length; _a++) {
                    var key = firstKeys_2[_a];
                    if (!this.objectPropertyEquals(first[key], second[key])) {
                        return false;
                    }
                }
                return true;
            };
            DefaultGroupKeyComparer.prototype.objectPropertyEquals = function (first, second) {
                if (typeof first !== typeof second) {
                    return false;
                }
                if (typeof first === "object" && typeof second === "object") {
                    if (first === null && second === null) {
                        return true;
                    }
                    if (first === null || second === null) {
                        return false;
                    }
                    return this.objectEquals(first, second);
                }
                return first === second;
            };
            return DefaultGroupKeyComparer;
        }());
        Utilities.DefaultGroupKeyComparer = DefaultGroupKeyComparer;
    })(Utilities = DevGuild.Utilities || (DevGuild.Utilities = {}));
})(DevGuild || (DevGuild = {}));
//# sourceMappingURL=collections.js.map