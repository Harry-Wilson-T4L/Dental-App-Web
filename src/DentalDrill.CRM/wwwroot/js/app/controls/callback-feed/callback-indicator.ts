namespace DentalDrill.CRM.Controls.CallbackFeed {
    import Dropdown = DentalDrill.CRM.Controls.Dropdown;
    import NotificationsConnection = DentalDrill.CRM.Controls.NotificationsFeed.NotificationsConnection;

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

    export class CallbackIndicatorNumber {
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

    export class CallbackIndicator {
        private readonly _root: HTMLElement;
        private readonly _hasUnresolved: ClassToggler;
        private readonly _hasUnread: ClassToggler;
        private readonly _disconnected: ClassToggler;

        private readonly _dropdown: Dropdown;
        private readonly _numberContainer: CallbackIndicatorNumber;

        constructor(root: HTMLElement) {
            this._root = root;
            this._hasUnresolved = new ClassToggler(this._root, "callback-indicator--has-unresolved");
            this._hasUnread = new ClassToggler(this._root, "callback-indicator--has-unread");
            this._disconnected = new ClassToggler(this._root, "callback-indicator--disconnected");

            this._dropdown = new Dropdown(root);
            this._numberContainer = new CallbackIndicatorNumber(this._root.querySelector(".callback-indicator__number") as HTMLSpanElement);

            const connection = NotificationsConnection.instance;

            connection.connected.subscribe((sender, args) => {
                this.disconnected = false;
            });
            connection.disconnected.subscribe((sender, args) => {
                this.disconnected = true;
            });
            connection.callbackUpdated.subscribe(async (sender, args) => {
                const totals = await this.loadTotals();
                const previousUnread = this._numberContainer.value;
                this._numberContainer.value = totals.new;
                if ((previousUnread === undefined || totals.new > previousUnread) && !this._dropdown.isShown()) {
                    this.hasUnread = true;
                }
            });

            this._dropdown.preventContentClickHide();
            this._dropdown.onShown.subscribe((sender, args) => {
                const component = this._root.querySelector(".callback-feed") as HTMLElement;
                if (component) {
                    const componentNode = $(component);
                    if (!componentNode.data("CallbackFeedListView")) {
                        CallbackFeedListView.create(componentNode);
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
            this._numberContainer.value = totals.new;
        }

        private async loadTotals(): Promise<{ new: number, done: number, total: number }> {
            const response = await fetch("/Callback/Total", {
                method: "POST",
                credentials: "same-origin",
                cache: "no-cache",
                body: ""
            });

            return await response.json();
        }
    }

    $(() => {
        const indicatorElements = document.querySelectorAll(".callback-indicator");
        for (let i = 0; i < indicatorElements.length; i++) {
            const element = indicatorElements[i] as HTMLElement;
            const indicator = new CallbackIndicator(element);
            $(element).data("CallbackIndicator", element);
            indicator.initialize();
        }
    });
}