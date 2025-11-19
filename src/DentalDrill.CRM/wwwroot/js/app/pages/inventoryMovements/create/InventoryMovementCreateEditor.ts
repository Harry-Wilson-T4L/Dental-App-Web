namespace DentalDrill.CRM.Pages.InventoryMovements.Create {
    import InventoryMovementCreateType = Shared.InventoryMovementCreateType;
    import InventoryMovementTypeHelper = Shared.InventoryMovementTypeHelper;
    import InventoryMovementStatus = Shared.InventoryMovementStatus;
    import InventoryMovementStatusHelper = Shared.InventoryMovementStatusHelper;

    interface Workshop {
        Id: string;
        Name: string;
    }

    enum WorkshopDirection {
        From,
        To,
    }

    export class InventoryMovementCreateEditor {
        private readonly _root: HTMLElement;
        private readonly _rootNode: JQuery<HTMLElement>;

        private readonly _availableWorkshops: Workshop[];
        private readonly _preselectedWorkshop?: Workshop;

        private readonly _dropdownType: kendo.ui.DropDownList;
        private readonly _dropdownStatus: kendo.ui.DropDownList;
        private readonly _dropdownFromWorkshop: kendo.ui.DropDownList;
        private readonly _dropdownToWorkshop: kendo.ui.DropDownList;

        constructor(root: HTMLElement, availableWorkshops: Workshop[], preselectedWorkshop?: Workshop) {
            this._root = root;
            this._rootNode = $(root);

            this._availableWorkshops = availableWorkshops;
            this._preselectedWorkshop = preselectedWorkshop;

            this._dropdownType = this._rootNode.find("#Type").data("kendoDropDownList");
            this._dropdownStatus = this._rootNode.find("#Status").data("kendoDropDownList");
            this._dropdownFromWorkshop = this._rootNode.find("#FromWorkshop").data("kendoDropDownList");
            this._dropdownToWorkshop = this._rootNode.find("#ToWorkshop").data("kendoDropDownList");
        }

        init() {
            this._dropdownType.bind("change", this.processTypeChange.bind(this));
            this.processTypeChange();
        }

        private processTypeChange(): void {
            var type = parseInt(this._dropdownType.value()) as InventoryMovementCreateType;
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
                    } else {
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
                    } else {
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
                    } else {
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
                        this.configureWorkshops(WorkshopDirection.From, true, this._availableWorkshops.filter(x => x.Id !== this._preselectedWorkshop.Id), undefined);
                        this.configureWorkshops(WorkshopDirection.To, true, [this._preselectedWorkshop], this._preselectedWorkshop.Id);
                    } else {
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
                        this.configureWorkshops(WorkshopDirection.To, true, this._availableWorkshops.filter(x => x.Id !== this._preselectedWorkshop.Id), undefined);
                    } else {
                        this.configureWorkshops(WorkshopDirection.From, true, this._availableWorkshops, undefined);
                        this.configureWorkshops(WorkshopDirection.To, true, this._availableWorkshops, undefined);
                    }

                    break;
            }
        }

        private setAvailableStatuses(statuses: InventoryMovementStatus[]) {
            const oldStatus = this.getSelectedStatus();
            this._dropdownStatus.dataSource.data(statuses.map(x => ({ Value: x, Text: InventoryMovementStatusHelper.toDisplayString(x) })));
            if (statuses.indexOf(oldStatus) >= 0) {
                this._dropdownStatus.value(oldStatus.toString());
            } else {
                this._dropdownStatus.value(statuses[0].toString());
            }
        }

        private getSelectedStatus(): InventoryMovementStatus {
            const val = this._dropdownStatus.value();
            if (val === undefined || val === null || val === ``) {
                return undefined;
            }

            const valInt = parseInt(val);
            if (valInt === NaN) {
                return undefined;
            }

            return valInt as InventoryMovementStatus;
        }

        private configureWorkshops(direction: WorkshopDirection, enabled: boolean, available: Workshop[], selected?: string) {
            const workshopDropDownList = this.getWorkshopControl(direction);
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
        }

        private getWorkshopControl(direction: WorkshopDirection): kendo.ui.DropDownList {
            switch (direction) {
                case WorkshopDirection.From:
                    return this._dropdownFromWorkshop;
                case WorkshopDirection.To:
                    return this._dropdownToWorkshop;
                default:
                    throw new Error("Invalid workshop direction");
            }
        }
    }
}