namespace DentalDrill.CRM.Controls {
    class MapCounter<T> {
        private readonly _map: Map<T, number> = new Map<T, number>();

        increment(key: T): number {
            if (this._map.has(key)) {
                const value = this._map.get(key) + 1;
                this._map.set(key, value);
                return value;
            } else {
                this._map.set(key, 1);
                return 1;
            }
        }

        count(key: T): number {
            if (this._map.has(key)) {
                return this._map.get(key);
            } else {
                return 0;
            }
        }
    }

    export class MultiSelectTogglerItem {
        private readonly _root: JQuery;

        constructor(root: JQuery) {
            this._root = root;
            this._root.data("MultiSelectTogglerItem", this);
        }

        activate(): void {
            this._root.removeClass("multi-select-toggler__item--partial");
            this._root.addClass("multi-select-toggler__item--active");
        }

        deactivate(): void {
            this._root.removeClass("multi-select-toggler__item--partial");
            this._root.removeClass("multi-select-toggler__item--active");
        }

        partial(): void {
            this._root.addClass("multi-select-toggler__item--partial");
            this._root.removeClass("multi-select-toggler__item--active");
        }
    }

    export class MultiSelectTogglerSpecificItem extends MultiSelectTogglerItem {
        private readonly _id: string;

        constructor(root: JQuery, id: string) {
            super(root);
            this._id = id;
        }

        get id(): string {
            return this._id;
        }
    }

    export class MultiSelectToggler {
        private readonly _root: JQuery;
        private readonly _multiSelect: kendo.ui.MultiSelect;
        private readonly _propertyName: string;

        private readonly _selectAllItem: MultiSelectTogglerItem;
        private readonly _toggleItems: MultiSelectTogglerSpecificItem[];

        constructor(root: JQuery) {
            this._root = root;
            const multiSelectElement = $(root.attr("data-target"));
            this._multiSelect = multiSelectElement.data("kendoMultiSelect");
            this._propertyName = root.attr("data-property");

            this._selectAllItem = this.initSelectAll(root);
            this._toggleItems = this.initSpecific(root);

            this._multiSelect.bind("change", (e: kendo.ui.MultiSelectChangeEvent) => {
                this.handleMultiSelectChange();
            });

            this.handleMultiSelectChange();
        }

        private initSelectAll(root: JQuery): MultiSelectTogglerItem {
            const item = root.find(".multi-select-toggler__item[data-toggle-all='true']");
            if (item.length !== 0) {
                item.on("click", e => {
                    const allActivated = this.getAllActivated();
                    if (allActivated) {
                        this._multiSelect.value([]);
                    } else {
                        const dataItems = this._multiSelect.dataSource.data();
                        const propertyId = this._multiSelect.options.dataValueField;
                        const ids = dataItems.map(x => x[propertyId]);
                        this._multiSelect.value(ids);
                    }

                    this.handleMultiSelectChange();
                });

                return new MultiSelectTogglerItem(item);
            } else {
                return undefined;
            }
        }

        private initSpecific(root: JQuery): MultiSelectTogglerSpecificItem[] {
            const items = root.find(".multi-select-toggler__item[data-toggle-id]");
            const result: MultiSelectTogglerSpecificItem[] = [];
            items.each((index, element) => {
                const item = $(element);
                const itemId = item.attr("data-toggle-id");
                item.on("click", e => {
                    const allActivated = this.getAllActivatedOf(itemId);
                    if (allActivated) {
                        const ids = this.getAllIdsOf(itemId);
                        const selected = this._multiSelect.value() as string[];
                        const updated = selected.filter(x => ids.every(y => y !== x));
                        this._multiSelect.value(updated);
                    } else {
                        const ids = this.getAllIdsOf(itemId);
                        const selected = this._multiSelect.value() as string[];
                        for (let id of ids) {
                            if (selected.every(x => x !== id)) {
                                selected.push(id);
                            }
                        }

                        this._multiSelect.value(selected);
                    }

                    this.handleMultiSelectChange();
                });
                result.push(new MultiSelectTogglerSpecificItem(item, itemId));
            });

            return result;
        }

        private getAllIds(): string[] {
            const result: string[] = [];
            const dataItems = this._multiSelect.dataSource.data();
            const propertyId = this._multiSelect.options.dataValueField;
            for (let i = 0; i < dataItems.length; i++) {
                const dataItem = dataItems[i];
                result.push(dataItem[propertyId]);
            }

            return result;
        }

        private getAllIdsOf(groupId: string): string[] {
            const result: string[] = [];
            const dataItems = this._multiSelect.dataSource.data();
            const propertyId = this._multiSelect.options.dataValueField;
            for (let i = 0; i < dataItems.length; i++) {
                const dataItem = dataItems[i];
                if (dataItem[this._propertyName] === groupId) {
                    result.push(dataItem[propertyId]);
                }
            }

            return result;
        }

        private getAllActivated(): boolean {
            let totalCount = 0;
            let selectedCount = 0;
            const selected = this._multiSelect.value() as string[];
            const allIds = this.getAllIds();
            for (let i = 0; i < allIds.length; i++) {
                totalCount++;
                if (selected.some(x => x === allIds[i])) {
                    selectedCount++;
                }
            }

            if (totalCount > 0 && totalCount === selectedCount) {
                return true;
            } else if (totalCount > 0 && selectedCount < totalCount) {
                return undefined;
            } else {
                return false;
            }
        }

        private getAllActivatedOf(groupId: string): boolean {
            let totalCount = 0;
            let selectedCount = 0;
            const selected = this._multiSelect.value() as string[];
            const allIds = this.getAllIdsOf(groupId);
            for (let i = 0; i < allIds.length; i++) {
                totalCount++;
                if (selected.some(x => x === allIds[i])) {
                    selectedCount++;
                }
            }

            if (totalCount > 0 && totalCount === selectedCount) {
                return true;
            } else if (totalCount > 0 && selectedCount < totalCount) {
                return undefined;
            } else {
                return false;
            }
        }

        private handleMultiSelectChange() {
            const selected = this._multiSelect.value() as string[];
            if (selected && selected.length > 0) {
                const dataItems = this._multiSelect.dataSource.data<{ Id: string, Name: string, StateId: string }>();
                const totalsCounter = new MapCounter();
                const selectedCounter = new MapCounter();
                for (let i = 0; i < dataItems.length; i++) {
                    const dataItem = dataItems[i];
                    totalsCounter.increment(dataItem.StateId);
                    if (selected.some(x => x === dataItem.Id)) {
                        selectedCounter.increment(dataItem.StateId);
                    }
                }

                let anyActivated = false;
                let anyDeactivated = false;
                for (let item of this._toggleItems) {
                    const totalCount = totalsCounter.count(item.id);
                    if (totalCount === 0) {
                        continue;
                    }

                    const selectedCount = selectedCounter.count(item.id);
                    if (selectedCount === totalCount) {
                        item.activate();
                        anyActivated = true;
                    } else if (selectedCount > 0 && selectedCount < totalCount) {
                        item.partial();
                        anyActivated = true;
                        anyDeactivated = true;
                    } else {
                        item.deactivate();
                        anyDeactivated = true;
                    }
                }

                if (this._selectAllItem) {
                    if (anyActivated && anyDeactivated) {
                        this._selectAllItem.partial();
                    } else if (anyActivated) {
                        this._selectAllItem.activate();
                    } else {
                        this._selectAllItem.deactivate();
                    }
                }
            } else {
                this._selectAllItem.deactivate();
                for (let item of this._toggleItems) {
                    item.deactivate();
                }
            }
        }
    }

    $(() => {
        $(".multi-select-toggler[data-init='true']").each((index, element) => {
            const item = $(element);
            const toggler = new MultiSelectToggler(item);
        });
    })
}