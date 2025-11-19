var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var InventoryMovements;
            (function (InventoryMovements) {
                var Shared;
                (function (Shared) {
                    var InventoryMovementBulkCollection = /** @class */ (function () {
                        function InventoryMovementBulkCollection() {
                            this._direction = undefined;
                            this._type = undefined;
                            this._status = undefined;
                            this._averagePrice = undefined;
                            this._extraInitialQuantity = undefined;
                            this._list = [];
                        }
                        Object.defineProperty(InventoryMovementBulkCollection.prototype, "direction", {
                            get: function () {
                                return this._direction;
                            },
                            set: function (val) {
                                this._direction = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkCollection.prototype, "type", {
                            get: function () {
                                return this._type;
                            },
                            set: function (val) {
                                this._type = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkCollection.prototype, "status", {
                            get: function () {
                                return this._status;
                            },
                            set: function (val) {
                                this._status = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkCollection.prototype, "averagePrice", {
                            get: function () {
                                return this._averagePrice;
                            },
                            set: function (val) {
                                this._averagePrice = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkCollection.prototype, "extraInitialQuantity", {
                            get: function () {
                                return this._extraInitialQuantity;
                            },
                            set: function (val) {
                                this._extraInitialQuantity = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        InventoryMovementBulkCollection.prototype.add = function (movement) {
                            this._list.push(movement);
                        };
                        InventoryMovementBulkCollection.prototype.getBulkMovements = function () {
                            var result = [];
                            for (var _i = 0, _a = this._list; _i < _a.length; _i++) {
                                var item = _a[_i];
                                result.push({
                                    Id: item.Id,
                                    Direction: item.Direction,
                                    Type: item.Type,
                                    Status: item.Status,
                                    HandpieceId: item.HandpieceId,
                                    HandpieceNumber: item.HandpieceNumber,
                                    HandpieceStatus: item.HandpieceStatus,
                                    RequiredQuantity: item.RequiredQuantity,
                                    Quantity: item.Quantity,
                                    QuantityAbsolute: item.QuantityAbsolute,
                                    AveragePrice: item.AveragePrice,
                                    Price: item.Price,
                                    CreatedOn: this.parseDate(item.CreatedOn),
                                    CompletedOn: this.parseDate(item.CompletedOn),
                                    PartsComment: item.PartsComment,
                                    MovementComment: item.MovementComment,
                                    BulkEditStatus: Shared.BulkEditStatus.Normal,
                                });
                            }
                            return result;
                        };
                        InventoryMovementBulkCollection.prototype.parseDate = function (value) {
                            if (value === undefined || value === null) {
                                return null;
                            }
                            if (typeof value === "string") {
                                return new Date(value + "Z");
                            }
                            if (typeof value === "object") {
                                if (value instanceof Date) {
                                    return value;
                                }
                                else {
                                    throw new Error("Invalid date object type");
                                }
                            }
                            throw new Error("Invalid date type");
                        };
                        return InventoryMovementBulkCollection;
                    }());
                    Shared.InventoryMovementBulkCollection = InventoryMovementBulkCollection;
                })(Shared = InventoryMovements.Shared || (InventoryMovements.Shared = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=InventoryMovementBulkCollection.js.map