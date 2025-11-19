var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var InventoryMovements;
            (function (InventoryMovements) {
                var Create;
                (function (Create) {
                    var InventoryMovementCreateType = InventoryMovements.Shared.InventoryMovementCreateType;
                    var InventoryMovementStatus = InventoryMovements.Shared.InventoryMovementStatus;
                    var InventoryMovementStatusHelper = InventoryMovements.Shared.InventoryMovementStatusHelper;
                    var WorkshopDirection;
                    (function (WorkshopDirection) {
                        WorkshopDirection[WorkshopDirection["From"] = 0] = "From";
                        WorkshopDirection[WorkshopDirection["To"] = 1] = "To";
                    })(WorkshopDirection || (WorkshopDirection = {}));
                    var InventoryMovementCreateEditor = /** @class */ (function () {
                        function InventoryMovementCreateEditor(root, availableWorkshops, preselectedWorkshop) {
                            this._root = root;
                            this._rootNode = $(root);
                            this._availableWorkshops = availableWorkshops;
                            this._preselectedWorkshop = preselectedWorkshop;
                            this._dropdownType = this._rootNode.find("#Type").data("kendoDropDownList");
                            this._dropdownStatus = this._rootNode.find("#Status").data("kendoDropDownList");
                            this._dropdownFromWorkshop = this._rootNode.find("#FromWorkshop").data("kendoDropDownList");
                            this._dropdownToWorkshop = this._rootNode.find("#ToWorkshop").data("kendoDropDownList");
                        }
                        InventoryMovementCreateEditor.prototype.init = function () {
                            this._dropdownType.bind("change", this.processTypeChange.bind(this));
                            this.processTypeChange();
                        };
                        InventoryMovementCreateEditor.prototype.processTypeChange = function () {
                            var _this = this;
                            var type = parseInt(this._dropdownType.value());
                            switch (type) {
                                case InventoryMovementCreateType.Order:
                                    this.setAvailableStatuses([
                                        InventoryMovementStatus.Ordered,
                                        InventoryMovementStatus.Approved,
                                        InventoryMovementStatus.Requested
                                    ]);
                                    if (this._preselectedWorkshop) {
                                        this.configureWorkshops(WorkshopDirection.From, false, [], undefined);
                                        this.configureWorkshops(WorkshopDirection.To, true, [this._preselectedWorkshop], this._preselectedWorkshop.Id);
                                    }
                                    else {
                                        this.configureWorkshops(WorkshopDirection.From, false, [], undefined);
                                        this.configureWorkshops(WorkshopDirection.To, true, this._availableWorkshops, undefined);
                                    }
                                    break;
                                case InventoryMovementCreateType.Found:
                                    this.setAvailableStatuses([
                                        InventoryMovementStatus.Completed
                                    ]);
                                    if (this._preselectedWorkshop) {
                                        this.configureWorkshops(WorkshopDirection.From, false, [], undefined);
                                        this.configureWorkshops(WorkshopDirection.To, true, [this._preselectedWorkshop], this._preselectedWorkshop.Id);
                                    }
                                    else {
                                        this.configureWorkshops(WorkshopDirection.From, false, [], undefined);
                                        this.configureWorkshops(WorkshopDirection.To, true, this._availableWorkshops, undefined);
                                    }
                                    break;
                                case InventoryMovementCreateType.Lost:
                                    this.setAvailableStatuses([
                                        InventoryMovementStatus.Completed
                                    ]);
                                    if (this._preselectedWorkshop) {
                                        this.configureWorkshops(WorkshopDirection.From, true, [this._preselectedWorkshop], this._preselectedWorkshop.Id);
                                        this.configureWorkshops(WorkshopDirection.To, false, [], undefined);
                                    }
                                    else {
                                        this.configureWorkshops(WorkshopDirection.From, true, this._availableWorkshops, undefined);
                                        this.configureWorkshops(WorkshopDirection.To, false, [], undefined);
                                    }
                                    break;
                                case InventoryMovementCreateType.MoveBetweenWorkshops:
                                    this.setAvailableStatuses([
                                        InventoryMovementStatus.Completed
                                    ]);
                                    this.configureWorkshops(WorkshopDirection.From, true, this._availableWorkshops, undefined);
                                    this.configureWorkshops(WorkshopDirection.To, true, this._availableWorkshops, undefined);
                                    break;
                                case InventoryMovementCreateType.MoveFromAnotherWorkshop:
                                    this.setAvailableStatuses([
                                        InventoryMovementStatus.Completed
                                    ]);
                                    if (this._preselectedWorkshop) {
                                        this.configureWorkshops(WorkshopDirection.From, true, this._availableWorkshops.filter(function (x) { return x.Id !== _this._preselectedWorkshop.Id; }), undefined);
                                        this.configureWorkshops(WorkshopDirection.To, true, [this._preselectedWorkshop], this._preselectedWorkshop.Id);
                                    }
                                    else {
                                        this.configureWorkshops(WorkshopDirection.From, true, this._availableWorkshops, undefined);
                                        this.configureWorkshops(WorkshopDirection.To, true, this._availableWorkshops, undefined);
                                    }
                                    break;
                                case InventoryMovementCreateType.MoveToAnotherWorkshop:
                                    this.setAvailableStatuses([
                                        InventoryMovementStatus.Completed
                                    ]);
                                    if (this._preselectedWorkshop) {
                                        this.configureWorkshops(WorkshopDirection.From, true, [this._preselectedWorkshop], this._preselectedWorkshop.Id);
                                        this.configureWorkshops(WorkshopDirection.To, true, this._availableWorkshops.filter(function (x) { return x.Id !== _this._preselectedWorkshop.Id; }), undefined);
                                    }
                                    else {
                                        this.configureWorkshops(WorkshopDirection.From, true, this._availableWorkshops, undefined);
                                        this.configureWorkshops(WorkshopDirection.To, true, this._availableWorkshops, undefined);
                                    }
                                    break;
                            }
                        };
                        InventoryMovementCreateEditor.prototype.setAvailableStatuses = function (statuses) {
                            var oldStatus = this.getSelectedStatus();
                            this._dropdownStatus.dataSource.data(statuses.map(function (x) { return ({ Value: x, Text: InventoryMovementStatusHelper.toDisplayString(x) }); }));
                            if (statuses.indexOf(oldStatus) >= 0) {
                                this._dropdownStatus.value(oldStatus.toString());
                            }
                            else {
                                this._dropdownStatus.value(statuses[0].toString());
                            }
                        };
                        InventoryMovementCreateEditor.prototype.getSelectedStatus = function () {
                            var val = this._dropdownStatus.value();
                            if (val === undefined || val === null || val === "") {
                                return undefined;
                            }
                            var valInt = parseInt(val);
                            if (valInt === NaN) {
                                return undefined;
                            }
                            return valInt;
                        };
                        InventoryMovementCreateEditor.prototype.configureWorkshops = function (direction, enabled, available, selected) {
                            var workshopDropDownList = this.getWorkshopControl(direction);
                            workshopDropDownList.enable(enabled);
                            if (selected === undefined && available.length > 1) {
                                workshopDropDownList.dataSource.data([]);
                                workshopDropDownList.value(undefined);
                                workshopDropDownList.dataSource.data(available);
                            }
                            else if (selected === undefined && available.length == 1) {
                                workshopDropDownList.dataSource.data(available);
                                workshopDropDownList.value(available[0].Id);
                            }
                            else {
                                workshopDropDownList.dataSource.data(available);
                                workshopDropDownList.value(selected);
                            }
                        };
                        InventoryMovementCreateEditor.prototype.getWorkshopControl = function (direction) {
                            switch (direction) {
                                case WorkshopDirection.From:
                                    return this._dropdownFromWorkshop;
                                case WorkshopDirection.To:
                                    return this._dropdownToWorkshop;
                                default:
                                    throw new Error("Invalid workshop direction");
                            }
                        };
                        return InventoryMovementCreateEditor;
                    }());
                    Create.InventoryMovementCreateEditor = InventoryMovementCreateEditor;
                })(Create = InventoryMovements.Create || (InventoryMovements.Create = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=InventoryMovementCreateEditor.js.map