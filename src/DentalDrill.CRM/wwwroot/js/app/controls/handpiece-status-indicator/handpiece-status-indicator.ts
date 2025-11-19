namespace DentalDrill.CRM.Controls {
    export class HandpieceStatusIndicator {
        private _max: number = 7;
        private _value: number = 0;
        private _danger: boolean = false;
        private _overrides: boolean[] = [];
        private _counts: number[] = [];

        constructor() {
        }

        get max(): number {
            return this._max;
        }

        get value(): number {
            return this._value;
        }

        set value(val: number) {
            if (val < 0 || val > this._max) {
                throw new Error("Invalid value");
            }

            this._value = val;
        }

        get danger(): boolean {
            return this._danger;
        }

        set danger(val: boolean) {
            this._danger = val;
        }

        getOverride(index: number): boolean {
            if (index < 1 || index > this._max) {
                throw new Error("Invalid index");
            }

            const val = this._overrides[index];
            return val ? val : false;
        }

        setOverride(index: number, val: boolean): void {
            if (index < 1 || index > this._max) {
                throw new Error("Invalid index");
            }

            this._overrides[index] = val;
        }

        getCount(index: number): number {
            if (index < 1 || index > this._max) {
                throw new Error("Invalid index");
            }

            const val = this._counts[index];
            return val ? val : 0;
        }

        setCount(index: number, val: number): void {
            if (index < 1 || index > this._max) {
                throw new Error("Invalid index");
            }

            this._counts[index] = val;
        }

        render(): HTMLElement {
            const mainDiv = this.createMainDiv();
            mainDiv.appendChild(this.createProgress());
            mainDiv.appendChild(this.createPoints());
            return mainDiv;
        }

        private createMainDiv(): HTMLDivElement {
            const div = document.createElement("div");
            div.classList.add("handpiece-status-indicator");
            div.setAttribute("data-max", `${this._max}`);
            div.setAttribute("data-value", `${this._value}`);
            div.setAttribute("data-danger", `${this._danger ? "True" : "False"}`);
            div.setAttribute("data-override", this.computeOverrides());

            return div;
        }

        private computeOverrides(): string {
            let result = "";
            for (let i = 1; i <= this._max; i++) {
                if (this.getOverride(i)) {
                    result += ` ${i}`;
                }
            }

            return result.trim();
        }

        private createProgress(): HTMLDivElement {
            const progress = document.createElement("div");
            progress.classList.add("progress");
            progress.classList.add("handpiece-status-indicator__progress");
            progress.appendChild(this.createProgressBar());
            return progress;
        }

        private createProgressBar(): HTMLDivElement {
            const progressBar = document.createElement("div");
            progressBar.classList.add("progress-bar");
            return progressBar;
        }

        private createPoints(): HTMLDivElement {
            const points = document.createElement("div");
            points.classList.add("handpiece-status-indicator__points");
            for (let i = 1; i <= this._max; i++) {
                points.appendChild(this.createPoint(i));
            }

            return points;
        }

        private createPoint(index: number): HTMLDivElement {
            const point = document.createElement("div");
            point.classList.add("handpiece-status-indicator__points__point");
            const count = this.getCount(index);
            if (count > 0) {
                point.innerText = count.toString();
            }

            return point;
        }
    }
}