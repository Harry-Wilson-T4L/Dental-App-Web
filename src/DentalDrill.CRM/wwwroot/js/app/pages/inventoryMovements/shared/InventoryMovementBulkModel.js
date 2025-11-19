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
                    var BulkEditStatus;
                    (function (BulkEditStatus) {
                        BulkEditStatus[BulkEditStatus["Normal"] = 0] = "Normal";
                        BulkEditStatus[BulkEditStatus["Postponed"] = 1] = "Postponed";
                        BulkEditStatus[BulkEditStatus["Cancelled"] = 2] = "Cancelled";
                    })(BulkEditStatus = Shared.BulkEditStatus || (Shared.BulkEditStatus = {}));
                    var InventoryMovementBulkModelWrapper = /** @class */ (function () {
                        function InventoryMovementBulkModelWrapper(inner) {
                            this._inner = inner;
                        }
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "Id", {
                            get: function () {
                                return this._inner.Id;
                            },
                            set: function (val) {
                                this._inner.Id = val;
                                this._inner.set("Id", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "Direction", {
                            get: function () {
                                return this._inner.Direction;
                            },
                            set: function (val) {
                                this._inner.Direction = val;
                                this._inner.set("Direction", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "Type", {
                            get: function () {
                                return this._inner.Type;
                            },
                            set: function (val) {
                                this._inner.Type = val;
                                this._inner.set("Type", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "Status", {
                            get: function () {
                                return this._inner.Status;
                            },
                            set: function (val) {
                                this._inner.Status = val;
                                this._inner.set("Status", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "HandpieceId", {
                            get: function () {
                                return this._inner.HandpieceId;
                            },
                            set: function (val) {
                                this._inner.HandpieceId = val;
                                this._inner.set("HandpieceId", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "HandpieceNumber", {
                            get: function () {
                                return this._inner.HandpieceNumber;
                            },
                            set: function (val) {
                                this._inner.HandpieceNumber = val;
                                this._inner.set("HandpieceNumber", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "HandpieceStatus", {
                            get: function () {
                                return this._inner.HandpieceStatus;
                            },
                            set: function (val) {
                                this._inner.HandpieceStatus = val;
                                this._inner.set("HandpieceStatus", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "RequiredQuantity", {
                            get: function () {
                                return this._inner.RequiredQuantity;
                            },
                            set: function (val) {
                                this._inner.RequiredQuantity = val;
                                this._inner.set("RequiredQuantity", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "Quantity", {
                            get: function () {
                                return this._inner.Quantity;
                            },
                            set: function (val) {
                                this._inner.Quantity = val;
                                this._inner.set("Quantity", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "QuantityAbsolute", {
                            get: function () {
                                return this._inner.QuantityAbsolute;
                            },
                            set: function (val) {
                                this._inner.QuantityAbsolute = val;
                                this._inner.set("QuantityAbsolute", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "AveragePrice", {
                            get: function () {
                                return this._inner.AveragePrice;
                            },
                            set: function (val) {
                                this._inner.AveragePrice = val;
                                this._inner.set("AveragePrice", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "Price", {
                            get: function () {
                                return this._inner.Price;
                            },
                            set: function (val) {
                                this._inner.Price = val;
                                this._inner.set("Price", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "CreatedOn", {
                            get: function () {
                                return this._inner.CreatedOn;
                            },
                            set: function (val) {
                                this._inner.CreatedOn = val;
                                this._inner.set("CreatedOn", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "CompletedOn", {
                            get: function () {
                                return this._inner.CompletedOn;
                            },
                            set: function (val) {
                                this._inner.CompletedOn = val;
                                this._inner.set("CompletedOn", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "PartsComment", {
                            get: function () {
                                return this._inner.PartsComment;
                            },
                            set: function (val) {
                                this._inner.PartsComment = val;
                                this._inner.set("PartsComment", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "MovementComment", {
                            get: function () {
                                return this._inner.MovementComment;
                            },
                            set: function (val) {
                                this._inner.MovementComment = val;
                                this._inner.set("MovementComment", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementBulkModelWrapper.prototype, "BulkEditStatus", {
                            get: function () {
                                return this._inner.BulkEditStatus;
                            },
                            set: function (val) {
                                this._inner.BulkEditStatus = val;
                                this._inner.set("BulkEditStatus", val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return InventoryMovementBulkModelWrapper;
                    }());
                    Shared.InventoryMovementBulkModelWrapper = InventoryMovementBulkModelWrapper;
                })(Shared = InventoryMovements.Shared || (InventoryMovements.Shared = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=InventoryMovementBulkModel.js.map