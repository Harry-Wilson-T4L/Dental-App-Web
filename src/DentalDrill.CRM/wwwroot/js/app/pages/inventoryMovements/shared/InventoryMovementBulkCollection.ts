namespace DentalDrill.CRM.Pages.InventoryMovements.Shared {
    export class InventoryMovementBulkCollection {
        private _direction: InventoryMovementDirection = undefined;
        private _type: InventoryMovementType = undefined;
        private _status: InventoryMovementStatus = undefined;
        private _averagePrice: number = undefined;
        private _extraInitialQuantity: number = undefined;
        private _list: InventoryMovementBulkModel[] = [];

        constructor() {
        }

        get direction(): InventoryMovementDirection {
            return this._direction;
        }

        set direction(val: InventoryMovementDirection) {
            this._direction = val;
        }

        get type(): InventoryMovementType {
            return this._type;
        }

        set type(val: InventoryMovementType) {
            this._type = val;
        }

        get status(): InventoryMovementStatus {
            return this._status;
        }

        set status(val: InventoryMovementStatus) {
            this._status = val;
        }

        get averagePrice(): number {
            return this._averagePrice;
        }

        set averagePrice(val: number) {
            this._averagePrice = val;
        }

        get extraInitialQuantity(): number {
            return this._extraInitialQuantity;
        }

        set extraInitialQuantity(val: number) {
            this._extraInitialQuantity = val;
        }

        add(movement: InventoryMovementBulkModel) {
            this._list.push(movement);
        }

        getBulkMovements(): InventoryMovementBulkModel[] {
            const result: InventoryMovementBulkModel[] = [];
            for (let item of this._list) {
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
                    BulkEditStatus: BulkEditStatus.Normal,
                })
            }
            return result;
        }

        private parseDate(value: any): Date {
            if (value === undefined || value === null) {
                return null;
            }

            if (typeof value === "string") {
                return new Date(value + "Z");
            }

            if (typeof value === "object") {
                if (value instanceof Date) {
                    return value;
                } else {
                    throw new Error("Invalid date object type");
                }
            }

            throw new Error("Invalid date type");
        }
    }
}