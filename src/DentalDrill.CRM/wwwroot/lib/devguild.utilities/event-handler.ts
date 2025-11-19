namespace DevGuild.Utilities {
    export class EventHandler<T> {
        private _handlers: ((sender: object, eventArgs: T) => void)[] = [];

        raise(sender: object, eventArgs: T): void {
            for (let handler of this._handlers) {
                handler(sender, eventArgs);
            }
        }

        subscribe(handler: (sender: object, eventArgs: T) => void): void {
            for (let i = 0; i < this._handlers.length; i++) {
                if (this._handlers[i] === handler) {
                    return;
                }
            }

            this._handlers.push(handler);
        }

        unsubscribe(handler: (sender: object, eventArgs: T) => void): void {
            for (let i = 0; i < this._handlers.length; i++) {
                if (this._handlers[i] === handler) {
                    this._handlers.splice(i, 1);
                    return;
                }
            }
        }
    }
}