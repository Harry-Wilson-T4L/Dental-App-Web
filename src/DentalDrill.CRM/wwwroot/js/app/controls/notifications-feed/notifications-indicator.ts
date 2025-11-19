namespace DentalDrill.CRM.Controls.NotificationsFeed {
    import Dropdown = DentalDrill.CRM.Controls.Dropdown;

    export class ClassToggler {
        private readonly _root: HTMLElement;
        private readonly _className: string;

        constructor(root: HTMLElement, className: string) {
            this._root = root;
            this._className = className;
        }

        get value(): boolean {
            return this._root.classList.contains(this._className);
        }

        set value(val: boolean) {
            if (val && !this._root.classList.contains(this._className)) {
                this._root.classList.add(this._className);
            } else if (!val && this._root.classList.contains(this._className)) {
                this._root.classList.remove(this._className);
            }
        }
    }

    export class NotificationsIndicatorNumber {
        private readonly _root: HTMLSpanElement;
        private _value: number;

        constructor(root: HTMLSpanElement) {
            this._root = root;

            this._value = parseInt(this._root.innerText);
            if (isNaN(this._value)) {
                this._value = undefined;
            }
        }

        get value(): number {
            return this._value;
        }

        set value(val: number) {
            this._value = val;
            this._root.innerText = val.toString();
        }
    }

    export class NotificationsIndicator {
        private readonly _root: HTMLElement;
        private readonly _hasUnresolved: ClassToggler;
        private readonly _hasUnread: ClassToggler;
        private readonly _disconnected: ClassToggler;

        private readonly _dropdown: Dropdown;
        private readonly _numberContainer: NotificationsIndicatorNumber;

        constructor(root: HTMLElement) {
            this._root = root;
            this._hasUnresolved = new ClassToggler(this._root, "notifications-indicator--has-unresolved");
            this._hasUnread = new ClassToggler(this._root, "notifications-indicator--has-unread");
            this._disconnected = new ClassToggler(this._root, "notifications-indicator--disconnected");

            this._dropdown = new Dropdown(root);
            this._numberContainer = new NotificationsIndicatorNumber(this._root.querySelector(".notifications-indicator__number") as HTMLSpanElement);
            
            const connection = NotificationsConnection.instance;

            connection.connected.subscribe((sender, args) => {
                this.disconnected = false;
            });
            connection.disconnected.subscribe((sender, args) => {
                this.disconnected = true;
            });
            connection.updated.subscribe(async (sender, args) => {
                const totals = await this.loadTotals();
                const previousUnread = this._numberContainer.value;
                this._numberContainer.value = totals.unread;
                if ((previousUnread === undefined || totals.unread > previousUnread) && !this._dropdown.isShown()) {
                    this.hasUnread = true;
                }
            });

            this._dropdown.preventContentClickHide();
            this._dropdown.onShown.subscribe((sender, args) => {
                const component = this._root.querySelector(".notifications-feed") as HTMLElement;
                if (component) {
                    const componentNode = $(component);
                    if (!componentNode.data("NotificationsListView")) {
                        NotificationsListView.create(componentNode);
                    }
                }

                if (this.hasUnread) {
                    this.hasUnread = false;
                }
            });

            this.disconnected = !connection.isConnected;
        }

        get hasUnresolved(): boolean {
            return this._hasUnresolved.value;
        }

        set hasUnresolved(val: boolean) {
            this._hasUnresolved.value = val;
        }

        get hasUnread(): boolean {
            return this._hasUnread.value;
        }

        set hasUnread(val: boolean) {
            this._hasUnread.value = val;
        }

        get disconnected(): boolean {
            return this._disconnected.value;
        }

        set disconnected(val: boolean) {
            this._disconnected.value = val;
        }

        async initialize(): Promise<void> {
            const totals = await this.loadTotals();
            this._numberContainer.value = totals.unread;
        }

        private async loadTotals(): Promise<{ unread: number, read: number, resolved: number, all: number }> {
            const response = await $.ajax({
                url: "/Notifications/Total",
                method: "POST"
            });

            return response;
        }
    }

    $(() => {
        const indicatorElements = document.querySelectorAll(".notifications-indicator");
        for (let i = 0; i < indicatorElements.length; i++) {
            const element = indicatorElements[i] as HTMLElement;
            const indicator = new NotificationsIndicator(element);
            $(element).data("NotificationsIndicator", element);
            indicator.initialize();
        }
    });
}