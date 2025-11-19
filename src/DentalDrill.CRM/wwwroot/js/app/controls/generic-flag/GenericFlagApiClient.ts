namespace DentalDrill.CRM.Controls.GenericFlag {
    export enum GenericFlagState {
        NotSet = 0,
        PartiallySet = 50,
        Set = 100,
    }

    export class GenericFlagsApiClient {
        async getState(flagId: string): Promise<GenericFlagState> {
            const response = await fetch(`/GenericFlags/GetValue?flagId=${encodeURIComponent(flagId)}`, {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    "X-Requested-With": "XMLHttpRequest",
                },
            });

            if (response.status !== 200) {
                throw new Error("Invalid response status code");
            }

            const responseBody = await response.json();
            if (responseBody && responseBody.Flag && responseBody.Flag === flagId) {
                return responseBody.State;
            } else {
                throw new Error("Invalid response");
            }
        }

        async setState(flagId: string, state: GenericFlagState): Promise<void> {
            const response = await fetch(`/GenericFlags/SetValue?flagId=${encodeURIComponent(flagId)}&state=${state}`, {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    "X-Requested-With": "XMLHttpRequest",
                },
            });

            if (response.status !== 200) {
                throw new Error("Invalid response status code");
            }

            const responseBody = await response.json();
            if (responseBody && responseBody.Flag && responseBody.Flag === flagId) {
                return;
            } else {
                throw new Error("Invalid response");
            }
        }

        async bulkGetValue(flags: string[]): Promise<Map<string, GenericFlagState>> {
            const response = await fetch(`/GenericFlags/BulkGetValue`, {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    "Content-Type": "application/json",
                    "X-Requested-With": "XMLHttpRequest",
                },
                body: JSON.stringify({
                    Flags: flags
                }),
            });

            if (response.status !== 200) {
                throw new Error("Invalid response status code");
            }

            const responseBody = await response.json();
            if (responseBody && responseBody.States && typeof responseBody.States === "object") {
                const map = new Map<string, GenericFlagState>();
                for (let key of Object.keys(responseBody.States)) {
                    map.set(key, responseBody.States[key]);
                }

                return map;
            } else {
                throw new Error("Invalid response");
            }
        }
    }
}