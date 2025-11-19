namespace DentalDrill.CRM.Controls.GenericFlag {
    export class GenericFlagControl {
        private readonly _api: GenericFlagsApiClient;
        private readonly _checkbox: HTMLInputElement;
        private readonly _flagId: string;
        private _state: GenericFlagState;
        private _changing: boolean;

        constructor(checkbox: HTMLInputElement, flagId: string, initialState: GenericFlagState) {
            this._api = new GenericFlagsApiClient();
            this._checkbox = checkbox;
            this._flagId = flagId;
            this._changing = false;

            this.applyState(initialState);
            this._checkbox.addEventListener("click", this.checkboxOnChange.bind(this));
        }

        static initContainer(container: HTMLElement) {
            if (!container) {
                throw new Error("no container provided");
            }

            const flagId = container.getAttribute("data-flag");
            if (!flagId) {
                throw new Error("flag id is missing");
            }

            container.innerHTML = ``;
            setTimeout(async () => {
                const api = new GenericFlagsApiClient();
                const state = await api.getState(flagId);
                
                const checkbox = container.appendChild(document.createElement("input"));
                checkbox.type = "checkbox";
                checkbox.classList.add("generic-flag");
                checkbox.classList.add("k-checkbox");

                const control = new GenericFlagControl(checkbox, flagId, state);
                checkbox["__control"] = control;
            }, 0);
        }

        static initContainers(containers: HTMLElement[]) {
            if (containers === undefined || containers === null || !Array.isArray(containers)) {
                throw new Error("invalid containers collection");
            }

            if (containers.length === 0) {
                return;
            }

            const flagsToInit: string[] = [];
            const containersToInit = new Map<string, HTMLElement>();

            for (let container of containers) {
                const flagId = container.getAttribute("data-flag");
                if (!flagId) {
                    throw new Error("flag id is missing");
                }

                container.innerHTML = ``;

                flagsToInit.push(flagId);
                containersToInit.set(flagId, container);
            }

            setTimeout(async () => {
                const api = new GenericFlagsApiClient();
                const states = await api.bulkGetValue(flagsToInit);
                for (let flagId of flagsToInit) {
                    const container = containersToInit.get(flagId);
                    let state = states.get(flagId);
                    if (state === undefined || state === null) {
                        state = GenericFlagState.NotSet;
                    }
                    
                    const checkbox = container.appendChild(document.createElement("input"));
                    checkbox.type = "checkbox";
                    checkbox.classList.add("generic-flag");
                    checkbox.classList.add("k-checkbox");

                    const control = new GenericFlagControl(checkbox, flagId, state);
                    checkbox["__control"] = control;
                }
            });
        }

        private checkboxOnChange(e: Event): void {
            e.preventDefault();
            e.stopPropagation();

            if (this._changing) {
                return;
            }

            this._changing = true;
            setTimeout(async () => {
                try {
                    const nextState = this.getNextState();
                    await this._api.setState(this._flagId, nextState);
                    this.applyState(nextState);
                } 
                finally {
                    this._changing = false;
                }
            }, 0);
        }

        private getNextState(): GenericFlagState {
            switch (this._state) {
                case GenericFlagState.Set:
                    return GenericFlagState.NotSet;
                case GenericFlagState.NotSet:
                case GenericFlagState.PartiallySet:
                default:
                    return GenericFlagState.Set;
            }
        }

        private applyState(state: GenericFlagState) {
            this._state = state;
            switch (this._state) {
                case GenericFlagState.Set:
                    this._checkbox.indeterminate = false;
                    this._checkbox.checked = true;
                    break;
                case GenericFlagState.PartiallySet:
                    this._checkbox.checked = false;
                    this._checkbox.indeterminate = true;
                    break;
                case GenericFlagState.NotSet:
                default:
                    this._checkbox.checked = false;
                    this._checkbox.indeterminate = false;
                    break;
            }
        }
    }
}