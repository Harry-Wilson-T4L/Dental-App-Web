namespace DentalDrill.CRM.Pages.HandpieceBrands.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    interface HandpieceBrandReadModel {
        Id: string;
        Name: string;
        ImageId: string;
        ImageUrl: string;
    }

    interface HandpieceModelReadModel {
        Id: string;
        BrandId: string;
        BrandName: string;
        Name: string;
        Description: string;
        ImageId: string;
        ImageUrl: string;
    }

    export class HandpieceBrandsGrid {
        private static _initialized: Map<string, HandpieceModelsGrid> = new Map<string, HandpieceModelsGrid>();

        static get instance(): kendo.ui.Grid {
            return $("#HandpieceBrandsGrid").data("kendoGrid");
        }

        static handleCreateModel = GridHandlers.createButtonClickPopupHandler<HandpieceBrandReadModel>(
            item => routes.handpieceModels.create(item.Id),
            item => ({
                title: `Add model to ${item.Name}`,
                width: "1000px",
                height: "600px",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("HandpieceModelCreate");

                    if (HandpieceBrandsGrid._initialized.has(item.Id)) {
                        const childGrid = HandpieceBrandsGrid._initialized.get(item.Id);
                        await childGrid.refresh();
                    }
                    
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static handleEdit = GridHandlers.createButtonClickPopupHandler<HandpieceBrandReadModel>(
            item => routes.handpieceBrands.edit(item.Id),
            item => ({
                title: `Edit brand ${item.Name}`,
                width: "1000px",
                height: "600px",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("HandpieceBrandEdit");
                    await HandpieceBrandsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static handleDelete = GridHandlers.createButtonClickPopupHandler<HandpieceBrandReadModel>(
            item => routes.handpieceBrands.delete(item.Id),
            item => ({
                title: `Delete brand ${item.Name}`,
                width: "1000px",
                height: "600px",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("HandpieceBrandDelete");
                    await HandpieceBrandsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static handleCreate = GridHandlers.createGridButtonClickPopupHandler(
            "#HandpieceBrandsGrid-Create",
            target => routes.handpieceBrands.create(),
            target => ({
                title: `Add brand`,
                width: "1000px",
                height: "600px",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("HandpieceBrandCreate");
                    await HandpieceBrandsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            })
        );

        static handleDetailsInit(e: kendo.ui.GridDetailInitEvent) {
            const isReadonly = HandpieceBrandsGrid.instance.wrapper.attr("data-readonly") == "true";
            const item = e.data as kendo.data.ObservableObject & HandpieceBrandReadModel;
            const wrapper = $("<div></div>").appendTo(e.detailCell);
            const grid = new HandpieceModelsGrid(wrapper, item, !isReadonly);

            HandpieceBrandsGrid._initialized.set(item.Id, grid);
        }
    }

    export class HandpieceModelsGrid {
        private readonly _parent: HandpieceBrandReadModel;
        private readonly _editable: boolean;
        private readonly _wrapper: JQuery;
        private readonly _instance: kendo.ui.Grid;

        constructor(wrapper: JQuery, parent: HandpieceBrandReadModel, editable: boolean) {
            this._parent = parent;
            this._editable = editable;
            this._wrapper = wrapper;
            this._wrapper.kendoGrid({
                dataSource: {
                    type: "aspnetmvc-ajax",
                    transport: {
                        read: routes.handpieceModels.read(parent.Id).value
                    },
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    schema: {
                        data: "Data",
                        total: "Total",
                        errors: "Errors",
                        model: {
                            fields: {
                                Id: { type: "string" },
                                BrandId: { type: "string" },
                                BrandName: { type: "string" },
                                Name: { type: "string" },
                                Description: { type: "string" },
                                ImageId: { type: "string" },
                                ImageUrl: { type: "string" },
                            }
                        }
                    }
                },
                scrollable: false,
                sortable: true,
                columns: [
                    { field: "ImageId", title: "Image", width: "50px", template: item => this.renderImage(item), sortable: false },
                    { field: "Name", title: "Name", width: "150px" },
                    { field: "Description", title: "Description", width: "300px" },
                    {
                        title: "",
                        width: "200px",
                        command: this._editable ? [
                            {
                                name: "CustomDetails",
                                text: `<span class="fas fa-fw fa-pencil-alt"></span> Edit`,
                                click: this.handleEdit,
                            },
                            {
                                name: "CustomDeleteModel",
                                text: `<span class="fas fa-fw fa-trash-alt"></span> Delete`,
                                click: this.handleDelete,
                            }
                        ] : [
                            {
                                name: "CustomDetails",
                                text: `<span class="fas fa-fw fa-external-link-alt"></span> Details`,
                                click: this.handleEdit,
                            }
                        ]
                    }
                ]
            });

            this._instance = this._wrapper.data("kendoGrid");
        }

        refresh(): Promise<void> {
            return this._instance.dataSource.read();
        }

        private renderImage(item: HandpieceModelReadModel): string {
            const img = document.createElement("img");
            img.src = item.ImageUrl;
            img.alt = item.Name;
            return img.outerHTML;
        }

        private handleEdit = GridHandlers.createButtonClickNavigationHandler<HandpieceModelReadModel>(item => routes.handpieceModels.details(item.Id));

        private handleDelete = GridHandlers.createButtonClickPopupHandler<HandpieceModelReadModel>(
            item => routes.handpieceModels.delete(item.Id),
            item => ({
                title: `Delete model ${item.BrandName} ${item.Name}`,
                width: "1000px",
                height: "400px",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("HandpieceModelDelete");
                    await this._instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));
    }
}