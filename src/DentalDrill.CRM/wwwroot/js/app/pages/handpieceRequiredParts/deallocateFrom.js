var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var HandpieceRequiredParts;
            (function (HandpieceRequiredParts) {
                var DeallocateFrom;
                (function (DeallocateFrom) {
                    var HandpieceStatus;
                    (function (HandpieceStatus) {
                        HandpieceStatus[HandpieceStatus["None"] = 0] = "None";
                        HandpieceStatus[HandpieceStatus["Received"] = 10] = "Received";
                        HandpieceStatus[HandpieceStatus["BeingEstimated"] = 20] = "BeingEstimated";
                        HandpieceStatus[HandpieceStatus["WaitingForApproval"] = 30] = "WaitingForApproval";
                        HandpieceStatus[HandpieceStatus["TbcHoldOn"] = 31] = "TbcHoldOn";
                        HandpieceStatus[HandpieceStatus["NeedsReApproval"] = 32] = "NeedsReApproval";
                        HandpieceStatus[HandpieceStatus["EstimateSent"] = 35] = "EstimateSent";
                        HandpieceStatus[HandpieceStatus["BeingRepaired"] = 40] = "BeingRepaired";
                        HandpieceStatus[HandpieceStatus["WaitingForParts"] = 41] = "WaitingForParts";
                        HandpieceStatus[HandpieceStatus["TradeIn"] = 42] = "TradeIn";
                        HandpieceStatus[HandpieceStatus["ReadyToReturn"] = 50] = "ReadyToReturn";
                        HandpieceStatus[HandpieceStatus["ReturnUnrepaired"] = 51] = "ReturnUnrepaired";
                        HandpieceStatus[HandpieceStatus["SentComplete"] = 60] = "SentComplete";
                        HandpieceStatus[HandpieceStatus["Cancelled"] = 70] = "Cancelled";
                        HandpieceStatus[HandpieceStatus["Unrepairable"] = 71] = "Unrepairable";
                    })(HandpieceStatus = DeallocateFrom.HandpieceStatus || (DeallocateFrom.HandpieceStatus = {}));
                    var HandpieceStatusHelper = /** @class */ (function () {
                        function HandpieceStatusHelper() {
                        }
                        HandpieceStatusHelper.toDisplayString = function (value) {
                            switch (value) {
                                case HandpieceStatus.None:
                                    return "None";
                                case HandpieceStatus.Received:
                                    return "Received";
                                case HandpieceStatus.BeingEstimated:
                                    return "Being estimated";
                                case HandpieceStatus.WaitingForApproval:
                                    return "Estimate Complete";
                                case HandpieceStatus.TbcHoldOn:
                                    return "Tbc hold on";
                                case HandpieceStatus.NeedsReApproval:
                                    return "Needs approval";
                                case HandpieceStatus.EstimateSent:
                                    return "Estimate sent";
                                case HandpieceStatus.BeingRepaired:
                                    return "Being repaired";
                                case HandpieceStatus.WaitingForParts:
                                    return "Waiting for parts";
                                case HandpieceStatus.TradeIn:
                                    return "Trade-in";
                                case HandpieceStatus.ReadyToReturn:
                                    return "Ready to return";
                                case HandpieceStatus.ReturnUnrepaired:
                                    return "Return unrepaired";
                                case HandpieceStatus.SentComplete:
                                    return "Sent complete";
                                case HandpieceStatus.Cancelled:
                                    return "Cancelled";
                                case HandpieceStatus.Unrepairable:
                                    return "Unrepairable";
                            }
                        };
                        return HandpieceStatusHelper;
                    }());
                    DeallocateFrom.HandpieceStatusHelper = HandpieceStatusHelper;
                    var HiddenInputList = /** @class */ (function () {
                        function HiddenInputList(root) {
                            this._root = root;
                        }
                        HiddenInputList.prototype.clear = function () {
                            this._root.innerHTML = "";
                        };
                        HiddenInputList.prototype.add = function (name, value) {
                            var input = document.createElement("input");
                            input.type = "hidden";
                            input.name = name;
                            input.value = value;
                            this._root.appendChild(input);
                        };
                        return HiddenInputList;
                    }());
                    DeallocateFrom.HiddenInputList = HiddenInputList;
                    var DeallocateFromOptions = /** @class */ (function () {
                        function DeallocateFromOptions() {
                            this._allocatedQuantity = 0;
                            this._shelfQuantity = 0;
                        }
                        Object.defineProperty(DeallocateFromOptions.prototype, "handpieceId", {
                            get: function () {
                                return this._handpieceId;
                            },
                            set: function (val) {
                                this._handpieceId = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(DeallocateFromOptions.prototype, "skuId", {
                            get: function () {
                                return this._skuId;
                            },
                            set: function (val) {
                                this._skuId = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(DeallocateFromOptions.prototype, "allocatedQuantity", {
                            get: function () {
                                return this._allocatedQuantity;
                            },
                            set: function (val) {
                                this._allocatedQuantity = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(DeallocateFromOptions.prototype, "shelfQuantity", {
                            get: function () {
                                return this._shelfQuantity;
                            },
                            set: function (val) {
                                this._shelfQuantity = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return DeallocateFromOptions;
                    }());
                    DeallocateFrom.DeallocateFromOptions = DeallocateFromOptions;
                    var DeallocateFromEditor = /** @class */ (function () {
                        function DeallocateFromEditor(root, options) {
                            this._previousSelection = undefined;
                            this._bypassChange = false;
                            this._root = root;
                            this._options = options;
                        }
                        DeallocateFromEditor.prototype.init = function () {
                            var _this = this;
                            this._dataSource = this.createDataSource();
                            this._grid = this.createGrid();
                            this._form = this._root.querySelector("form");
                            this._formData = new HiddenInputList(this._root.querySelector(".editor__data"));
                            this._fieldMoveToHandpiecesValue = this._root.querySelector(".editor__field[data-field=MoveToHandpieces] .editor__field__value");
                            this._fieldMoveToShelfValue = this._root.querySelector(".editor__field[data-field=MoveToShelf] .editor__field__value");
                            this._form.addEventListener("submit", function (e) {
                                _this._formData.clear();
                                var selectedItems = _this._grid.select();
                                selectedItems.each(function (index, element) {
                                    var dataItem = _this._grid.dataItem(element);
                                    _this._formData.add("ReallocateTo[" + index + "]", "" + dataItem.Id);
                                });
                            });
                        };
                        DeallocateFromEditor.prototype.processChange = function (e) {
                            var _this = this;
                            if (this._bypassChange) {
                                return;
                            }
                            var selected = this._grid.select();
                            var selectedQuantity = 0;
                            selected.each(function (index, element) {
                                var dataItem = _this._grid.dataItem(element);
                                selectedQuantity += dataItem.QuantityAbsolute;
                            });
                            if (selectedQuantity > (this._options.allocatedQuantity + this._options.shelfQuantity)) {
                                this.restorePreviousSelection();
                                return;
                            }
                            this.updatePreviousSelection();
                            this._fieldMoveToHandpiecesValue.textContent = selectedQuantity.toString();
                            this._fieldMoveToShelfValue.textContent = (this._options.allocatedQuantity - selectedQuantity).toString();
                        };
                        DeallocateFromEditor.prototype.updatePreviousSelection = function () {
                            this._previousSelection = this._grid.select();
                        };
                        DeallocateFromEditor.prototype.restorePreviousSelection = function () {
                            this._bypassChange = true;
                            try {
                                if (this._previousSelection) {
                                    this._grid.clearSelection();
                                    this._grid.select(this._previousSelection);
                                }
                                else {
                                    this._grid.clearSelection();
                                }
                            }
                            finally {
                                this._bypassChange = false;
                            }
                        };
                        DeallocateFromEditor.prototype.createGrid = function () {
                            var container = this._root.querySelector(".grid-container");
                            container.style.height = "200px";
                            container.classList.add("k-grid--dense");
                            return $(container).kendoGrid({
                                dataSource: this._dataSource,
                                selectable: "multiple, row",
                                persistSelection: true,
                                change: this.processChange.bind(this),
                                columns: [
                                    {
                                        selectable: true,
                                        width: "50px",
                                    },
                                    {
                                        field: "CreatedOn",
                                        title: "Date",
                                        width: "100px",
                                        template: function (data) {
                                            return kendo.toString(data.CreatedOn, "d");
                                        },
                                    },
                                    {
                                        field: "QuantityAbsolute",
                                        title: "QTY",
                                        width: "100px",
                                    },
                                    {
                                        field: "HandpieceNumber",
                                        title: "Handpiece",
                                        width: "150px",
                                        template: function (data) {
                                            if (!(data.HandpieceId && data.HandpieceNumber)) {
                                                return "";
                                            }
                                            var link = document.createElement("a");
                                            link.href = "/Handpieces/Edit/" + data.HandpieceId;
                                            link.appendChild(document.createTextNode(data.HandpieceNumber));
                                            return link.outerHTML;
                                        },
                                    },
                                    {
                                        field: "HandpieceStatus",
                                        title: "Status",
                                        width: "150px",
                                        template: function (data) {
                                            if (data.HandpieceStatus === undefined || data.HandpieceStatus === null) {
                                                return "";
                                            }
                                            return HandpieceStatusHelper.toDisplayString(data.HandpieceStatus);
                                        },
                                    }
                                ]
                            }).data("kendoGrid");
                        };
                        DeallocateFromEditor.prototype.createDataSource = function () {
                            var dataSource = new kendo.data.DataSource({
                                type: "aspnetmvc-ajax",
                                transport: {
                                    read: {
                                        url: "/HandpieceRequiredParts/DeallocateFromRead?handpiece=" + this._options.handpieceId + "&sku=" + this._options.skuId
                                    },
                                },
                                schema: {
                                    data: "Data",
                                    total: "Total",
                                    errors: "Errors",
                                    model: {
                                        id: "Id",
                                        fields: {
                                            "CreatedOn": { type: "Date" },
                                            "QuantityAbsolute": { type: "number" },
                                            "HandpieceId": { type: "string" },
                                            "HandpieceNumber": { type: "string" },
                                            "HandpieceStatus": { type: "number" },
                                        },
                                    },
                                }
                            });
                            return dataSource;
                        };
                        return DeallocateFromEditor;
                    }());
                    DeallocateFrom.DeallocateFromEditor = DeallocateFromEditor;
                })(DeallocateFrom = HandpieceRequiredParts.DeallocateFrom || (HandpieceRequiredParts.DeallocateFrom = {}));
            })(HandpieceRequiredParts = Pages.HandpieceRequiredParts || (Pages.HandpieceRequiredParts = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=deallocateFrom.js.map