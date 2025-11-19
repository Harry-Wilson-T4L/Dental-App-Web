namespace DentalDrill.CRM.Pages.InventoryMovements.Shared {
    export enum HandpieceStatus {
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

    export class HandpieceStatusHelper {
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
            }
        }

        static createDataSource(): kendo.data.DataSource {
            const items: HandpieceStatus[] = [
                HandpieceStatus.Received,
                HandpieceStatus.BeingEstimated,
                HandpieceStatus.WaitingForApproval,
                HandpieceStatus.TbcHoldOn,
                HandpieceStatus.NeedsReApproval,
                HandpieceStatus.EstimateSent,
                HandpieceStatus.BeingRepaired,
                HandpieceStatus.WaitingForParts,
                HandpieceStatus.TradeIn,
                HandpieceStatus.ReadyToReturn,
                HandpieceStatus.ReturnUnrepaired,
                HandpieceStatus.SentComplete,
                HandpieceStatus.Cancelled,
                HandpieceStatus.Unrepairable,
            ];

            return new kendo.data.DataSource({
                data: items.map(x => ({ name: HandpieceStatusHelper.toDisplayString(x), value: x }))
            });
        }
    }
}