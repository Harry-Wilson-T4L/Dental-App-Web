var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var HandpieceRequiredParts;
            (function (HandpieceRequiredParts) {
                var AllocateTo;
                (function (AllocateTo) {
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
                    })(HandpieceStatus = AllocateTo.HandpieceStatus || (AllocateTo.HandpieceStatus = {}));
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
                    AllocateTo.HandpieceStatusHelper = HandpieceStatusHelper;
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
                    AllocateTo.HiddenInputList = HiddenInputList;
                    var AllocateToOptions = /** @class */ (function () {
                        function AllocateToOptions() {
                            this._requiredQuantity = 0;
                            this._shelfQuantity = 0;
                        }
                        Object.defineProperty(AllocateToOptions.prototype, "handpieceId", {
                            get: function () {
                                return this._handpieceId;
                            },
                            set: function (val) {
                                this._handpieceId = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(AllocateToOptions.prototype, "skuId", {
                            get: function () {
                                return this._skuId;
                            },
                            set: function (val) {
                                this._skuId = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(AllocateToOptions.prototype, "requiredQuantity", {
                            get: function () {
                                return this._requiredQuantity;
                            },
                            set: function (val) {
                                this._requiredQuantity = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(AllocateToOptions.prototype, "shelfQuantity", {
                            get: function () {
                                return this._shelfQuantity;
                            },
                            set: function (val) {
                                this._shelfQuantity = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return AllocateToOptions;
                    }());
                    AllocateTo.AllocateToOptions = AllocateToOptions;
                    var AllocateToEditor = /** @class */ (function () {
                        function AllocateToEditor(root, options) {
                            this._root = root;
                            this._options = options;
                        }
                        AllocateToEditor.prototype.init = function () {
                            var _this = this;
                            this._dataSource = this.createDataSource();
                            this._grid = this.createGrid();
                            this._form = this._root.querySelector("form");
                            this._formData = new HiddenInputList(this._root.querySelector(".editor__data"));
                            this._fieldMoveFromHandpiecesValue = this._root.querySelector(".editor__field[data-field=MoveFromHandpieces] .editor__field__value");
                            this._fieldMoveFromShelfValue = this._root.querySelector(".editor__field[data-field=MoveFromShelf] .editor__field__value");
                            this._buttonSubmit = this._root.querySelector("button[type=submit]");
                            this._form.addEventListener("submit", function (e) {
                                _this._formData.clear();
                                var selectedItems = _this._grid.select();
                                selectedItems.each(function (index, element) {
                                    var dataItem = _this._grid.dataItem(element);
                                    _this._formData.add("ReallocateFrom[" + index + "]", "" + dataItem.Id);
                                });
                            });
                        };
                        AllocateToEditor.prototype.processChange = function (e) {
                            var _this = this;
                            var selected = this._grid.select();
                            var selectedQuantity = 0;
                            selected.each(function (index, element) {
                                var dataItem = _this._grid.dataItem(element);
                                selectedQuantity += dataItem.QuantityAbsolute;
                            });
                            this._fieldMoveFromHandpiecesValue.textContent = selectedQuantity.toString();
                            this._fieldMoveFromShelfValue.textContent = (this._options.requiredQuantity - selectedQuantity).toString();
                            if ((selectedQuantity + this._options.shelfQuantity) >= this._options.requiredQuantity) {
                                this._fieldMoveFromShelfValue.classList.remove("text-danger");
                                this._buttonSubmit.disabled = false;
                            }
                            else {
                                this._fieldMoveFromShelfValue.classList.add("text-danger");
                                this._buttonSubmit.disabled = true;
                            }
                        };
                        AllocateToEditor.prototype.createGrid = function () {
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
                        AllocateToEditor.prototype.createDataSource = function () {
                            var dataSource = new kendo.data.DataSource({
                                type: "aspnetmvc-ajax",
                                transport: {
                                    read: {
                                        url: "/HandpieceRequiredParts/AllocateToRead?handpiece=" + this._options.handpieceId + "&sku=" + this._options.skuId
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
                        return AllocateToEditor;
                    }());
                    AllocateTo.AllocateToEditor = AllocateToEditor;
                })(AllocateTo = HandpieceRequiredParts.AllocateTo || (HandpieceRequiredParts.AllocateTo = {}));
            })(HandpieceRequiredParts = Pages.HandpieceRequiredParts || (Pages.HandpieceRequiredParts = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=allocateTo.js.map