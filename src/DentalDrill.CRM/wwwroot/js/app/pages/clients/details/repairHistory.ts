namespace DentalDrill.CRM.Pages.Clients.Details {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import HandpieceStatusIndicator = DentalDrill.CRM.Controls.HandpieceStatusIndicator;

    enum HandpieceStatus {
        None = 0,

        Received = 10,

        BeingEstimated = 20,

        WaitingForApproval = 30,
        TbcHoldOn = 31,
        NeedsReApproval = 32,

        EstimateSent = 35,

        BeingRepaired = 40,
        WaitingForParts = 41,
        TradeIn = 42,

        ReadyToReturn = 50,
        ReturnUnrepaired = 51,

        SentComplete = 60,

        Cancelled = 70,
        Unrepairable = 71,
    }

    class HandpieceStatusHelper {
        static toDisplayString(value: HandpieceStatus): string {
            switch (value) {
                case HandpieceStatus.None:
                    return `None`;
                case HandpieceStatus.Received:
                    return `Received`;
                case HandpieceStatus.BeingEstimated:
                    return `Being estimated`;
                case HandpieceStatus.WaitingForApproval:
                    return `Estimate Complete`;
                case HandpieceStatus.TbcHoldOn:
                    return `Tbc hold on`;
                case HandpieceStatus.NeedsReApproval:
                    return `Needs approval`;
                case HandpieceStatus.EstimateSent:
                    return `Estimate sent`;
                case HandpieceStatus.BeingRepaired:
                    return `Being repaired`;
                case HandpieceStatus.WaitingForParts:
                    return `Waiting for parts`;
                case HandpieceStatus.TradeIn:
                    return `Trade-in`;
                case HandpieceStatus.ReadyToReturn:
                    return `Ready to return`;
                case HandpieceStatus.ReturnUnrepaired:
                    return `Return unrepaired`;
                case HandpieceStatus.SentComplete:
                    return `Sent complete`;
                case HandpieceStatus.Cancelled:
                    return `Cancelled`;
                case HandpieceStatus.Unrepairable:
                    return `Unrepairable`;
                default:
                    return "Unknown";
            }
        }
    }

    enum ClientRepairedItemStatus {
        RequiresMaintenance = 0,
        RemindedRecently = 1,
        Complete = 2,
        Active = 3,
        ReminderExpired = 4,
        Cancelled = 5,
        Disabled = 6,
    }

    class ClientRepairedItemStatusHelper {
        static toDisplayString(value: ClientRepairedItemStatus): string {
            switch (value) {
                case ClientRepairedItemStatus.Active:
                    return "Active";
                case ClientRepairedItemStatus.Complete:
                    return "Complete";
                case ClientRepairedItemStatus.RequiresMaintenance:
                    return "Requires Maintenance";
                case ClientRepairedItemStatus.RemindedRecently:
                    return "Reminded Recently";
                case ClientRepairedItemStatus.ReminderExpired:
                    return "Reminder Expired";
                case ClientRepairedItemStatus.Cancelled:
                    return "Cancelled";
                case ClientRepairedItemStatus.Disabled:
                    return "Disabled";
                default:
                    return "Unknown";
            }
        }

        static toIcon(value: ClientRepairedItemStatus): string {
            switch (value) {
                case ClientRepairedItemStatus.Active:
                    return `<span class="fas fa-fw fa-wrench"></span>`;
                case ClientRepairedItemStatus.Complete:
                    return `<span class="fas fa-fw fa-check"></span>`;
                case ClientRepairedItemStatus.RequiresMaintenance:
                    return `<span class="fas fa-fw fa-bell"></span>`;
                case ClientRepairedItemStatus.RemindedRecently:
                    return `<span class="far fa-fw fa-bell-slash"></span>`;
                case ClientRepairedItemStatus.ReminderExpired:
                    return `<span class="fas fa-fw fa-bell-slash"></span>`;
                case ClientRepairedItemStatus.Cancelled:
                    return `<span class="fas fa-fw fa-ban"></span>`;
                case ClientRepairedItemStatus.Disabled:
                    return `<span class="fas fa-fw fa-power-off"></span>`;
                default:
                    return ``;
            }
        }
    }

    interface ClientRepairedItemViewModel {
        Id: string;
        ClientId: string;
        Brand: string;
        Model: string;
        Serial: string;
        LastRepair: string;
        LastRepairUrl: string;
        LastRepairStatus: HandpieceStatus;
        LastRepairDate: Date;
        Status: ClientRepairedItemStatus;
    }

    export class RepairHistoryGrid {
        static get instance(): kendo.ui.Grid {
            return $("#RepairedItemsGrid").data("kendoGrid");
        }

        static formatLastRepairStatus(data: ClientRepairedItemViewModel) {
            return `${HandpieceStatusHelper.toDisplayString(data.LastRepairStatus)}`;
        }

        static formatStatus(data: ClientRepairedItemViewModel) {
            return `${ClientRepairedItemStatusHelper.toIcon(data.Status)} ${ClientRepairedItemStatusHelper.toDisplayString(data.Status)}`;
        }

        static renderStatusIndicator(data: any): string {
            const config = (data.JobStatusConfig as string).split(";").map(x => parseInt(x));
            const indicator = new HandpieceStatusIndicator();

            const indicatorValue = Math.abs(config[0]);
            indicator.value = indicatorValue;
            indicator.danger = config[0] < 0;
            for (let i = 1; i <= 6; i++) {
                indicator.setOverride(i, config[i] > 0 && i < indicatorValue);
                indicator.setCount(i, i < indicatorValue ? config[i] : 0);
            }

            return indicator.render().outerHTML;
        }

        static async handleToggle(this: kendo.ui.Grid, e: JQueryEventObject) {
            e.preventDefault();
            const dataItem = this.dataItem<ClientRepairedItemViewModel>(e.currentTarget.closest("tr"));
            console.log(dataItem);

            await fetch(`/ClientRepairHistory/Toggle?parentId=${dataItem.ClientId}`, {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    "Content-Type": "application/json",
                    "X-Requested-With": "XMLHttpRequest"
                },
                body: JSON.stringify({
                    ClientHandpieceId: dataItem.Id,
                    Disable: dataItem.Status === ClientRepairedItemStatus.Disabled ? false : true,
                })
            });
            await this.dataSource.read();
        }

        static async handleResetCount(e: JQueryEventObject) {
            e.preventDefault();
            const grid = $("#RepairedItemsGrid").data("kendoGrid");
            const dataItem = grid.dataItem<ClientRepairedItemViewModel>(e.currentTarget.closest("tr"));
            console.log(dataItem);

            await fetch(`/ClientRepairHistory/ResetCount?parentId=${dataItem.ClientId}`, {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    "Content-Type": "application/json",
                    "X-Requested-With": "XMLHttpRequest"
                },
                body: JSON.stringify({
                    ClientHandpieceId: dataItem.Id,
                })
            });
            await grid.dataSource.read();
        }
    }

    $(() => {
        $(document).on("click", ".client-repair-history-reset-count", RepairHistoryGrid.handleResetCount);
    });
}
