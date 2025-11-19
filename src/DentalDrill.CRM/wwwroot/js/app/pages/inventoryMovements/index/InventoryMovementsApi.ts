namespace DentalDrill.CRM.Pages.InventoryMovements.Index {
    import InventoryMovementStatus = Shared.InventoryMovementStatus;
    
    export interface IInventoryMovementApi {
        approve(id: string): Promise<void>;
        order(id: string): Promise<void>;
        verify(id: string): Promise<void>;
        cancel(id: string): Promise<void>;

        approveGroup(sku: string): Promise<void>;
        orderGroup(sku: string): Promise<void>;
        verifyGroup(sku: string): Promise<void>;
        cancelGroup(sku: string, status: InventoryMovementStatus): Promise<void>;
    }

    export class InventoryMovementApi implements IInventoryMovementApi {
        private readonly _options: InventoryMovementsIndexOptions;

        constructor(options: InventoryMovementsIndexOptions) {
            this._options = options;
        }

        async approve(id: string): Promise<void> {
            const response = await fetch(`/InventoryMovements/Approve/${id}`,
                {
                    method: "POST",
                    credentials: "same-origin",
                    cache: "no-cache",
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded;charset=UTF-8",
                        "X-Requested-With": "XMLHttpRequest",
                    },
                    body: this.toFormUrlEncoded({
                        "__RequestVerificationToken": this._options.antiForgeryToken
                    })
                });

            if (response.status !== 200) {
                throw new Error("API call failed");
            }
        }
        
        async order(id: string): Promise<void> {
            const response = await fetch(`/InventoryMovements/Order/${id}`,
                {
                    method: "POST",
                    credentials: "same-origin",
                    cache: "no-cache",
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded;charset=UTF-8",
                        "X-Requested-With": "XMLHttpRequest",
                    },
                    body: this.toFormUrlEncoded({
                        "__RequestVerificationToken": this._options.antiForgeryToken
                    })
                });

            if (response.status !== 200) {
                throw new Error("API call failed");
            }
        }
        
        async verify(id: string): Promise<void> {
            const response = await fetch(`/InventoryMovements/Verify/${id}`,
                {
                    method: "POST",
                    credentials: "same-origin",
                    cache: "no-cache",
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded;charset=UTF-8",
                        "X-Requested-With": "XMLHttpRequest",
                    },
                    body: this.toFormUrlEncoded({
                        "__RequestVerificationToken": this._options.antiForgeryToken
                    })
                });

            if (response.status !== 200) {
                throw new Error("API call failed");
            }
        }

        async cancel(id: string): Promise<void> {
            const response = await fetch(`/InventoryMovements/Cancel/${id}`,
                {
                    method: "POST",
                    credentials: "same-origin",
                    cache: "no-cache",
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded;charset=UTF-8",
                        "X-Requested-With": "XMLHttpRequest",
                    },
                    body: this.toFormUrlEncoded({
                        "__RequestVerificationToken": this._options.antiForgeryToken
                    })
                });

            if (response.status !== 200) {
                throw new Error("API call failed");
            }
        }

        async approveGroup(id: string): Promise<void> {
            const response = await fetch(`/InventoryMovements/GroupApprove?sku=${id}`,
                {
                    method: "POST",
                    credentials: "same-origin",
                    cache: "no-cache",
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded;charset=UTF-8",
                        "X-Requested-With": "XMLHttpRequest",
                    },
                    body: this.toFormUrlEncoded({
                        "__RequestVerificationToken": this._options.antiForgeryToken
                    })
                });

            if (response.status !== 200) {
                throw new Error("API call failed");
            }
        }
        
        async orderGroup(id: string): Promise<void> {
            const response = await fetch(`/InventoryMovements/GroupOrder?sku=${id}`,
                {
                    method: "POST",
                    credentials: "same-origin",
                    cache: "no-cache",
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded;charset=UTF-8",
                        "X-Requested-With": "XMLHttpRequest",
                    },
                    body: this.toFormUrlEncoded({
                        "__RequestVerificationToken": this._options.antiForgeryToken
                    })
                });

            if (response.status !== 200) {
                throw new Error("API call failed");
            }
        }
        
        async verifyGroup(id: string): Promise<void> {
            const response = await fetch(`/InventoryMovements/GroupVerify?sku=${id}`,
                {
                    method: "POST",
                    credentials: "same-origin",
                    cache: "no-cache",
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded;charset=UTF-8",
                        "X-Requested-With": "XMLHttpRequest",
                    },
                    body: this.toFormUrlEncoded({
                        "__RequestVerificationToken": this._options.antiForgeryToken
                    })
                });

            if (response.status !== 200) {
                throw new Error("API call failed");
            }
        }

        async cancelGroup(id: string, status: InventoryMovementStatus): Promise<void> {
            const response = await fetch(`/InventoryMovements/GroupCancel?sku=${id}&status=${status}`,
                {
                    method: "POST",
                    credentials: "same-origin",
                    cache: "no-cache",
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded;charset=UTF-8",
                        "X-Requested-With": "XMLHttpRequest",
                    },
                    body: this.toFormUrlEncoded({
                        "__RequestVerificationToken": this._options.antiForgeryToken
                    })
                });

            if (response.status !== 200) {
                throw new Error("API call failed");
            }
        }

        private toFormUrlEncoded(obj: object): string {
            let fields: string[] = [];
            for (let key of Object.keys(obj)) {
                fields.push(`${encodeURIComponent(key)}=${encodeURIComponent(obj[key])}`);
            }

            return fields.join("&");
        }
    }
}