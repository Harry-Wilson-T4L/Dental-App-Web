namespace DentalDrill.CRM.Pages.HandpieceRequiredParts.AllocateTo {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    export enum HandpieceStatus {
        None = 0,
        Received = 10,
        BeingEstimated = 20,
        WaitingForApproval = 30,
        TbcHoldOn = 31,
        NeedsReApproval = 32,
        EstimateSent = 35,
        BeingRepaired = 40,
        WaitingForParts = 41,
        TradeIn = 42,
        ReadyToReturn = 50,
        ReturnUnrepaired = 51,
        SentComplete = 60,
        Cancelled = 70,
        Unrepairable = 71,
    }

    export class HandpieceStatusHelper {
        static toDisplayString(value: HandpieceStatus): string {
            switch (value) {
            case HandpieceStatus.None:
                return `None`;
            case HandpieceStatus.Received:
                return `Received`;
            case HandpieceStatus.BeingEstimated:
                return `Being estimated`;
            case HandpieceStatus.WaitingForApproval:
                return `Estimate Complete`;
            case HandpieceStatus.TbcHoldOn:
                return `Tbc hold on`;
            case HandpieceStatus.NeedsReApproval:
                return `Needs approval`;
            case HandpieceStatus.EstimateSent:
                return `Estimate sent`;
            case HandpieceStatus.BeingRepaired:
                return `Being repaired`;
            case HandpieceStatus.WaitingForParts:
                return `Waiting for parts`;
            case HandpieceStatus.TradeIn:
                return `Trade-in`;
            case HandpieceStatus.ReadyToReturn:
                return `Ready to return`;
            case HandpieceStatus.ReturnUnrepaired:
                return `Return unrepaired`;
            case HandpieceStatus.SentComplete:
                return `Sent complete`;
            case HandpieceStatus.Cancelled:
                return `Cancelled`;
            case HandpieceStatus.Unrepairable:
                return `Unrepairable`;
            }
        }
    }

    export class HiddenInputList {
        private readonly _root: HTMLElement;

        constructor(root: HTMLElement) {
            this._root = root;
        }

        clear() {
            this._root.innerHTML = ``;
        }

        add(name: string, value: string) {
            const input = document.createElement("input");
            input.type = "hidden";
            input.name = name;
            input.value = value;

            this._root.appendChild(input);
        }
    }

    export interface HandpieceRequiredPartReallocateReadModel {
        Id: string;
        CreatedOn: Date;
        QuantityAbsolute: number;
        HandpieceId: string;
        HandpieceNumber: string;
        HandpieceStatus: HandpieceStatus;
    }

    export class AllocateToOptions {
        private _handpieceId: string;
        private _skuId: string;
        private _requiredQuantity: number = 0;
        private _shelfQuantity: number = 0;

        get handpieceId(): string {
            return this._handpieceId;
        }

        set handpieceId(val: string) {
            this._handpieceId = val;
        }

        get skuId(): string {
            return this._skuId;
        }

        set skuId(val: string) {
            this._skuId = val;
        }

        get requiredQuantity(): number {
            return this._requiredQuantity;
        }

        set requiredQuantity(val: number) {
            this._requiredQuantity = val;
        }

        get shelfQuantity(): number {
            return this._shelfQuantity;
        }

        set shelfQuantity(val: number) {
            this._shelfQuantity = val;
        }
    }

    export class AllocateToEditor {
        private readonly _root: HTMLElement;
        private readonly _options: AllocateToOptions;

        private _grid: kendo.ui.Grid;
        private _dataSource: kendo.data.DataSource;

        private _fieldMoveFromHandpiecesValue: HTMLElement;
        private _fieldMoveFromShelfValue: HTMLElement;
        private _buttonSubmit: HTMLButtonElement;

        private _form: HTMLFormElement;
        private _formData: HiddenInputList;

        constructor(root: HTMLElement, options: AllocateToOptions) {
            this._root = root;
            this._options = options;
        }

        init() {
            this._dataSource = this.createDataSource();
            this._grid = this.createGrid();

            this._form = this._root.querySelector("form");
            this._formData = new HiddenInputList(this._root.querySelector(".editor__data"));

            this._fieldMoveFromHandpiecesValue = this._root.querySelector(".editor__field[data-field=MoveFromHandpieces] .editor__field__value");
            this._fieldMoveFromShelfValue = this._root.querySelector(".editor__field[data-field=MoveFromShelf] .editor__field__value");
            this._buttonSubmit = this._root.querySelector("button[type=submit]");

            this._form.addEventListener("submit", e => {
                this._formData.clear();
                const selectedItems = this._grid.select();
                selectedItems.each((index, element) => {
                    const dataItem = this._grid.dataItem<HandpieceRequiredPartReallocateReadModel>(element);
                    this._formData.add(`ReallocateFrom[${index}]`, `${dataItem.Id}`);
                });
            });
        }

        private processChange(e: kendo.ui.GridChangeEvent) {
            const selected = this._grid.select();

            let selectedQuantity = 0;
            selected.each((index, element) => {
                const dataItem = this._grid.dataItem<HandpieceRequiredPartReallocateReadModel>(element);
                selectedQuantity += dataItem.QuantityAbsolute;
            })
            
            this._fieldMoveFromHandpiecesValue.textContent = selectedQuantity.toString();
            this._fieldMoveFromShelfValue.textContent = (this._options.requiredQuantity - selectedQuantity).toString();

            if ((selectedQuantity + this._options.shelfQuantity) >= this._options.requiredQuantity) {
                this._fieldMoveFromShelfValue.classList.remove("text-danger");
                this._buttonSubmit.disabled = false;
            } else {
                this._fieldMoveFromShelfValue.classList.add("text-danger");
                this._buttonSubmit.disabled = true;
            }
        }

        private createGrid(): kendo.ui.Grid {
            const container = this._root.querySelector(".grid-container") as HTMLElement;
            container.style.height = "200px";
            container.classList.add("k-grid--dense");

            return $(container).kendoGrid({
                dataSource: this._dataSource,
                selectable: "multiple, row",
                persistSelection: true,
                change: this.processChange.bind(this),
                columns: [
                    {
                        selectable: true,
                        width: "50px",
                    },
                    {
                        field: "CreatedOn",
                        title: "Date",
                        width: "100px",
                        template: (data: HandpieceRequiredPartReallocateReadModel) => {
                            return kendo.toString(data.CreatedOn, "d");
                        },
                    },
                    {
                        field: "QuantityAbsolute",
                        title: "QTY",
                        width: "100px",
                    },
                    {
                        field: "HandpieceNumber",
                        title: "Handpiece",
                        width: "150px",
                        template: (data: HandpieceRequiredPartReallocateReadModel) => {
                            if (!(data.HandpieceId && data.HandpieceNumber)) {
                                return ``;
                            }

                            const link = document.createElement("a");
                            link.href = `/Handpieces/Edit/${data.HandpieceId}`;
                            link.appendChild(document.createTextNode(data.HandpieceNumber));
                            return link.outerHTML;
                        },
                    },
                    {
                        field: "HandpieceStatus",
                        title: "Status",
                        width: "150px",
                        template: (data: HandpieceRequiredPartReallocateReadModel) => {
                            if (data.HandpieceStatus === undefined || data.HandpieceStatus === null) {
                                return ``;
                            }

                            return HandpieceStatusHelper.toDisplayString(data.HandpieceStatus);
                        },
                    }
                ]
            }).data("kendoGrid");
        }

        private createDataSource(): kendo.data.DataSource {
            const dataSource = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: `/HandpieceRequiredParts/AllocateToRead?handpiece=${this._options.handpieceId}&sku=${this._options.skuId}`
                    },
                },
                schema: {
                    data: "Data",
                    total: "Total",
                    errors: "Errors",
                    model: {
                        id: "Id",
                        fields: {
                            "CreatedOn": { type: "Date" },
                            "QuantityAbsolute": { type: "number" },
                            "HandpieceId": { type: "string" },
                            "HandpieceNumber": { type: "string" }, 
                            "HandpieceStatus": { type: "number" },
                        },
                    },
                }
            });

            return dataSource;
        }
    }
}