namespace DentalDrill.CRM.Pages.HandpieceStore.Index {
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    export class PriceSlider {
        private readonly _root: JQuery<HTMLElement>;
        private readonly _valueNode: JQuery<HTMLElement>;
        private readonly _slider: kendo.ui.RangeSlider;

        constructor(root: JQuery<HTMLElement>) {
            this._root = root;
            this._valueNode = root.find(".price-slider__value");
            this._slider = root.find(".price-slider__slider .price-slider__slider__widget[data-role='rangeslider']").data("kendoRangeSlider");
            this._slider.bind("change", (e: kendo.ui.RangeSliderChangeEvent) => {
                const val = e.sender.value();
                if (val !== undefined && typeof val === "object" && Array.isArray(val)) {
                    const firstValue = val[0] as number;
                    const secondValue = val[1] as number;

                    this._valueNode.text(`$${kendo.format("{0:#,##0.##}", firstValue)} - $${kendo.format("{0:#,##0.##}", secondValue)}`);
                }
            });
        }

        get value(): number[] {
            return this._slider.value() as number[];
        }

        set value(val: number[]) {
            this._slider.value(val);
        }

        reset(): void {
            this._slider.value([this._slider.options.min, this._slider.options.max]);
        }
    }

    export class HandpieceStoreFilters {
        private readonly _page: HandpieceStorePage;
        private readonly _root: JQuery<HTMLElement>;

        private readonly _fieldSearch: JQuery<HTMLElement>;
        private readonly _fieldPrice: PriceSlider;
        private readonly _fieldBrand: kendo.ui.MultiSelect;
        private readonly _fieldModel: kendo.ui.MultiSelect;
        private readonly _fieldCoupling: kendo.ui.MultiSelect;
        private readonly _fieldType: kendo.ui.MultiSelect;

        constructor(page: HandpieceStorePage, root: JQuery<HTMLElement>) {
            this._page = page;
            this._root = root;
            this._root.find(".handpiece-store-filters__apply-button").on("click", e => {
                e.preventDefault();
                this.apply();
            });

            this._root.find(".handpiece-store-filters__reset-button").on("click", e => {
                e.preventDefault();
                this.reset();
            });

            this._fieldSearch = this._root.find(".handpiece-store-filters__fields__search");
            this._fieldPrice = new PriceSlider(this._root.find(".handpiece-store-filters__fields__price"));
            this._fieldBrand = this._root.find(".handpiece-store-filters__fields__brand[data-role='multiselect']").data("kendoMultiSelect");
            this._fieldModel = this._root.find(".handpiece-store-filters__fields__model[data-role='multiselect']").data("kendoMultiSelect");
            this._fieldCoupling = this._root.find(".handpiece-store-filters__fields__coupling[data-role='multiselect']").data("kendoMultiSelect");
            this._fieldType = this._root.find(".handpiece-store-filters__fields__type[data-role='multiselect']").data("kendoMultiSelect");
        }

        apply(): void {
            const filterValues = {
                Search: this._fieldSearch.val(),
                Price: this._fieldPrice.value,
                Brand: this._fieldBrand.value(),
                Model: this._fieldModel.value(),
                Coupling: this._fieldCoupling.value(),
                Type: this._fieldType.value(),
            };

            this._page.listView.readData = filterValues;
            this._page.listView.reload();
        }

        reset(): void {
            this._fieldSearch.val("");
            this._fieldPrice.reset();
            this._fieldBrand.value([]);
            this._fieldModel.value([]);
            this._fieldCoupling.value([]);
            this._fieldType.value([]);

            this._page.listView.readData = { };
            this._page.listView.reload();
        }
    }

    export class HandpieceStoreListView {
        private readonly _page: HandpieceStorePage;
        private readonly _root: JQuery<HTMLElement>;

        private _readData: object = { };

        constructor(page: HandpieceStorePage, root: JQuery<HTMLElement>) {
            this._page = page;
            this._root = root;
            this._root.on("click", ".handpiece-store-list__item__actions__buy", e => {
                e.preventDefault();
                const itemRoot = $(e.target).closest(".handpiece-store-list__item");
                if (itemRoot.length) {
                    const itemId = itemRoot.attr("data-id");
                    this.handleItemBuy(itemId);
                }
            });
        }

        get readData(): object {
            return this._readData;
        }

        set readData(data: object) {
            this._readData = data;
        }

        reload(): void {
            const listView = this._root.find(".handpiece-store-list__view[data-role=listview]").data("kendoListView");
            listView.dataSource.read();
        }

        private handleItemBuy(itemId: string) {
            const windowState = {
                open: true,
            };

            const url = routes.handpieceStore.buy(itemId);
            const dialogRoot = $("<div></div>");
            const dialogOptions = {
                title: "Buy Handpiece",
                actions: ["close"],
                content: url.value,
                width: "800px",
                height: "auto",
                modal: true,
                visible: false,
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("HandpieceStoryBuy");
                    await this.reload();
                    e.sender.close();
                    e.sender.destroy();
                    windowState.open = false;
                },
                close: () => {
                    dialogRoot.data("kendoWindow").destroy()
                    windowState.open = false;
                },
            };

            (dialogRoot as any).kendoWindow(dialogOptions);

            const dialog = dialogRoot.data("kendoWindow");
            dialog.center();
            dialog.open();

            $(window).on("resize", (e: JQueryEventObject) => {
                if (windowState.open) {
                    dialog.center();
                }
            });
        }
    }

    export class HandpieceStorePage {
        private readonly _root: JQuery<HTMLElement>;
        private readonly _filters: HandpieceStoreFilters;
        private readonly _listView: HandpieceStoreListView;

        constructor(root: JQuery<HTMLElement>) {
            this._root = root;
            this._filters = new HandpieceStoreFilters(this, root.find(".handpiece-store-filters"));
            this._listView = new HandpieceStoreListView(this, root.find(".handpiece-store-list"));
        }

        get filters(): HandpieceStoreFilters {
            return this._filters;
        }

        get listView(): HandpieceStoreListView {
            return this._listView;
        }
    }

    export class Global {
        private static _page: HandpieceStorePage;

        static linkPage(page: HandpieceStorePage) {
            Global._page = page;
        }

        static get page(): HandpieceStorePage {
            if (!Global._page) {
                throw "Page not linked";
            }

            return Global._page;
        }

        static getReadData(): object {
            return Global.page.listView.readData;
        }
    }

    $(() => {
        const page = new HandpieceStorePage($(".handpiece-store"));
        Global.linkPage(page);
    });
}