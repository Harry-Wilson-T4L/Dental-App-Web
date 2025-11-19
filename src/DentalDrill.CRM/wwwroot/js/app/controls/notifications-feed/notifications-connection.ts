namespace DentalDrill.CRM.Controls.NotificationsFeed {
    import EventHandler = DevGuild.Utilities.EventHandler;

    export class NotificationsConnection {
        private static _instance: NotificationsConnection;

        private readonly _connection: signalR.HubConnection;
        private readonly _connected: EventHandler<object> = new EventHandler<object>();
        private readonly _disconnected: EventHandler<object> = new EventHandler<object>();
        private readonly _updated: EventHandler<object> = new EventHandler<object>();
        private readonly _callbackUpdated: EventHandler<object> = new EventHandler<object>();

        private _isConnected: boolean = false;

        private constructor() {
            this._connection = new signalR.HubConnectionBuilder().withUrl("/hub/Notifications").build();
            this._connection.on("ReceiveUpdate", () => {
                this._updated.raise(this, { });
            });
            this._connection.on("CallbackUpdated", () => {
                this._callbackUpdated.raise(this, { });
            });
            this._connection.onclose(() => {
                this._isConnected = false;
                this._disconnected.raise(this, { });
            });
        }

        static get instance(): NotificationsConnection {
            if (NotificationsConnection._instance === undefined) {
                NotificationsConnection._instance = new NotificationsConnection();
                NotificationsConnection._instance.connect();
            }

            return NotificationsConnection._instance;
        }

        get isConnected(): boolean {
            return this._isConnected;
        }

        get connected(): EventHandler<object> {
            return this._connected;
        }

        get disconnected(): EventHandler<object> {
            return this._disconnected;
        }

        get updated(): EventHandler<object> {
            return this._updated;
        }

        get callbackUpdated(): EventHandler<object> {
            return this._callbackUpdated;
        }

        async connect(): Promise<void> {
            await this._connection.start();
            this._isConnected = true;
            this._connected.raise(this, { });
        }
    }
}