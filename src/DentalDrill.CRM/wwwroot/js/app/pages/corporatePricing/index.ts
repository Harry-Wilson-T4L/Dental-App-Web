namespace DentalDrill.CRM.Pages.CorporatePricing.Index {
    interface CorporatePricingCategory {
        Id: string,
        Name: string,
    }

    export class CorporatePricingPage {
        private readonly _root: HTMLElement;
        private readonly _canEdit: boolean;
        private _categories: CorporatePricingCategory[];
        private _grid: kendo.ui.Grid;

        constructor(root: HTMLElement, canEdit: boolean) {
            this._root = root;
            this._canEdit = canEdit;
        }

        setCategories(categories: CorporatePricingCategory[]) {
            this._categories = categories;
        }

        init() {
            this._grid = this.createGrid();

            window.addEventListener("resize", e => {
                this._grid.setOptions({ height: "100px" });
                this._grid.resize(true);

                this._grid.setOptions({ height: "100%" });
                this._grid.resize(true);
            });
        }

        private createGrid(): kendo.ui.Grid {
            const dataSource = this.createDataSource();
            const gridContainer = $(this._root).find(".grid-container");
            gridContainer.kendoGrid({
                dataSource: dataSource,
                columns: this.initializeColumns(),
                toolbar: this._canEdit ? ["save"] : undefined,
                editable: this._canEdit ? {
                    mode: "incell"
                } : undefined,
                pageable: true,
            });

            const grid = gridContainer.data("kendoGrid");
            return grid;
        }

        private createDataSource(): kendo.data.DataSource {
            const dataSource = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: `/CorporatePricingServiceLevels/Read`
                    },
                    update: this._canEdit ? {
                        url: `/CorporatePricingServiceLevels/Update`
                    } : undefined,
                },
                pageSize: 20,
                schema: {
                    data: "Data",
                    total: "Total",
                    errors: "Errors",
                    model: {
                        id: "Id",
                        fields: this.initializeSchemaModel()
                    }
                }
            });

            return dataSource;
        }

        private initializeColumns(): kendo.ui.GridColumn[] {
            const columns: kendo.ui.GridColumn[] = [];
            columns.push({
                field: "ServiceLevelName",
                title: "Service Level"
            });
            for (let category of this._categories) {
                const fieldName = `CategoriesPricing_${category.Id.replace(/\-/g, "")}`;
                columns.push({
                    field: fieldName,
                    title: category.Name,
                });
            }

            return columns;
        }


        private initializeSchemaModel(): object {
            const fields = {
                ServiceLevelName: { type: "string", editable: "false" }
            };

            for (let category of this._categories) {
                const fieldName = `CategoriesPricing_${category.Id.replace(/\-/g, "")}`;
                const fieldSource = `CategoriesPricing["${category.Id}"]`;
                fields[fieldName] = {
                    type: "number",
                    from: fieldSource
                };
            }

            return fields;
        }
    }
}