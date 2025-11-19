namespace DentalDrill.CRM.Pages.InventoryMovements.Shared {
    export enum BulkEditStatus {
        Normal = 0,
        Postponed = 1,
        Cancelled = 2
    }

    export interface InventoryMovementBulkModel {
        Id: string;
        Direction: InventoryMovementDirection;
        Type: InventoryMovementType;
        Status: InventoryMovementStatus;
        HandpieceId?: string;
        HandpieceNumber?: string;
        HandpieceStatus?: HandpieceStatus;
        RequiredQuantity?: number;
        Quantity: number;
        QuantityAbsolute: number;
        AveragePrice?: number;
        Price?: number;
        CreatedOn: Date;
        CompletedOn?: Date;
        PartsComment: string;
        MovementComment: string;
        BulkEditStatus?: BulkEditStatus;
    }

    export class InventoryMovementBulkModelWrapper implements InventoryMovementBulkModel {
        private readonly _inner: kendo.data.ObservableObject & InventoryMovementBulkModel;

        constructor(inner: kendo.data.ObservableObject & InventoryMovementBulkModel) {
            this._inner = inner;
        }

        get Id(): string {
            return this._inner.Id;
        }

        set Id(val: string) {
            this._inner.Id = val;
            this._inner.set("Id", val);
        }

        get Direction(): InventoryMovementDirection {
            return this._inner.Direction;
        }

        set Direction(val: InventoryMovementDirection) {
            this._inner.Direction = val;
            this._inner.set("Direction", val);
        }

        get Type(): InventoryMovementType {
            return this._inner.Type;
        }

        set Type(val: InventoryMovementType) {
            this._inner.Type = val;
            this._inner.set("Type", val);
        }

        get Status(): InventoryMovementStatus {
            return this._inner.Status;
        }

        set Status(val: InventoryMovementStatus) {
            this._inner.Status = val;
            this._inner.set("Status", val);
        }

        get HandpieceId(): string {
            return this._inner.HandpieceId;
        }

        set HandpieceId(val: string) {
            this._inner.HandpieceId = val;
            this._inner.set("HandpieceId", val);
        }

        get HandpieceNumber(): string {
            return this._inner.HandpieceNumber;
        }

        set HandpieceNumber(val: string) {
            this._inner.HandpieceNumber = val;
            this._inner.set("HandpieceNumber", val);
        }

        get HandpieceStatus(): HandpieceStatus {
            return this._inner.HandpieceStatus;
        }

        set HandpieceStatus(val: HandpieceStatus) {
            this._inner.HandpieceStatus = val;
            this._inner.set("HandpieceStatus", val);
        }

        get RequiredQuantity(): number {
            return this._inner.RequiredQuantity;
        }

        set RequiredQuantity(val: number) {
            this._inner.RequiredQuantity = val;
            this._inner.set("RequiredQuantity", val);
        }

        get Quantity(): number {
            return this._inner.Quantity;
        }

        set Quantity(val: number) {
            this._inner.Quantity = val;
            this._inner.set("Quantity", val);
        }

        get QuantityAbsolute(): number {
            return this._inner.QuantityAbsolute;
        }

        set QuantityAbsolute(val: number) {
            this._inner.QuantityAbsolute = val;
            this._inner.set("QuantityAbsolute", val);
        }

        get AveragePrice(): number {
            return this._inner.AveragePrice;
        }

        set AveragePrice(val: number) {
            this._inner.AveragePrice = val;
            this._inner.set("AveragePrice", val);
        }

        get Price(): number {
            return this._inner.Price;
        }

        set Price(val: number) {
            this._inner.Price = val;
            this._inner.set("Price", val);
        }

        get CreatedOn(): Date {
            return this._inner.CreatedOn;
        }

        set CreatedOn(val: Date) {
            this._inner.CreatedOn = val;
            this._inner.set("CreatedOn", val);
        }

        get CompletedOn(): Date {
            return this._inner.CompletedOn;
        }

        set CompletedOn(val: Date) {
            this._inner.CompletedOn = val;
            this._inner.set("CompletedOn", val);
        }

        get PartsComment(): string {
            return this._inner.PartsComment;
        }

        set PartsComment(val: string) {
            this._inner.PartsComment = val;
            this._inner.set("PartsComment", val);
        }

        get MovementComment(): string {
            return this._inner.MovementComment;
        }

        set MovementComment(val: string) {
            this._inner.MovementComment = val;
            this._inner.set("MovementComment", val);
        }

        get BulkEditStatus(): BulkEditStatus {
            return this._inner.BulkEditStatus;
        }

        set BulkEditStatus(val: BulkEditStatus) {
            this._inner.BulkEditStatus = val;
            this._inner.set("BulkEditStatus", val);
        }
    }
}