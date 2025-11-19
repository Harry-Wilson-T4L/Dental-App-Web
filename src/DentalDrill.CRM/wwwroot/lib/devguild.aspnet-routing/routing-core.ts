namespace DevGuild.AspNet.Routing {
    export class Uri {
        private _value: string;

        constructor(value: string) {
            this._value = value;
        }

        get value(): string {
            return this._value;
        }

        navigate(): void {
            window.location.href = this._value;
        }

        open(): void {
            window.open(this._value, "_blank");
        }

        execute(open: boolean): void {
            if (open) {
                this.open();
            } else {
                this.navigate();
            }
        }
    }

    export abstract class ControllerRoute {
        private _prefix: string;

        constructor(prefix: string) {
            this._prefix = prefix;
        }

        protected get prefix(): string {
            return this._prefix;
        }

        protected buildUri(uri: string): Uri {
            return new Uri(this._prefix + uri);
        }
    }
}