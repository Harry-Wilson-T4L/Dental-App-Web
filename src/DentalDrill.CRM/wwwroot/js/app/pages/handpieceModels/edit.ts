namespace DentalDrill.CRM.Pages.HandpieceModels.Edit {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    enum HandpieceModelSchematicType {
        Text,
        Attachment,
        Image
    }

    interface HandpieceModelSchematicReadModel {
        Id: string;
        Type: HandpieceModelSchematicType;
        Title: string;
        Display: boolean;
        ThumbnailUrl: string;
    }

    enum HandpieceStoreListingStatus {
        Listed,
        Unlisted,
        Deleted,
        Requested,
        Sold,
    }

    interface HandpieceModelListingReadModel {
        Id: string;
        Status: HandpieceStoreListingStatus;
        SerialNumber: string;
        CreatedOn: Date;
        Price: number;
        Warranty: string;
        Notes: string;
        Coupling: string;
        CosmeticCondition: string;
        FiberOptionBrightness: string;
    }

    export class HandpieceModelSchematicsGrid {
        static get instance(): kendo.ui.Grid {
            return $("#HandpieceModelSchematicsGrid").data("kendoGrid");
        }

        static get parentId(): string {
            return HandpieceModelSchematicsGrid.instance.wrapper.attr("data-parent-id");
        }

        static handleCreateText = GridHandlers.createGridButtonClickPopupHandler(
            "#HandpieceModelSchematicsGrid .k-grid-CustomCreateText",
            target => routes.handpieceModelSchematics.createText(HandpieceModelSchematicsGrid.parentId),
            target => ({
                title: `Add text schematic`,
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("HandpieceModelSchematicCreateText");
                    await HandpieceModelSchematicsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            })
        );

        static handleCreateAttachment = GridHandlers.createGridButtonClickPopupHandler(
            "#HandpieceModelSchematicsGrid .k-grid-CustomCreateAttachment",
            target => routes.handpieceModelSchematics.createAttachment(HandpieceModelSchematicsGrid.parentId),
            target => ({
                title: `Add pdf schematic`,
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("HandpieceModelSchematicCreateAttachment");
                    await HandpieceModelSchematicsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            })
        );

        static handleCreateImage = GridHandlers.createGridButtonClickPopupHandler(
            "#HandpieceModelSchematicsGrid .k-grid-CustomCreateImage",
            target => routes.handpieceModelSchematics.createImage(HandpieceModelSchematicsGrid.parentId),
            target => ({
                title: `Add image schematic`,
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("HandpieceModelSchematicCreateImage");
                    await HandpieceModelSchematicsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            })
        );
        
        static handleDetails = GridHandlers.createButtonClickPopupHandler<HandpieceModelSchematicReadModel>(
            item => routes.handpieceModelSchematics.details(item.Id),
            item => ({
                title: `Schematic ${item.Title ? item.Title : ""}`,
                width: `1000px`,
                height: `auto`,
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });

                    switch (item.Type) {
                        case HandpieceModelSchematicType.Text:
                            e.sender.center();
                            break;
                        case HandpieceModelSchematicType.Attachment:
                            e.sender.center();
                            break;
                        case HandpieceModelSchematicType.Image:
                            const images = e.sender.wrapper.find("img");
                            images.on("load", loaded => {
                                e.sender.center();
                            });
                            break;
                    }
                }
            }));

        static handleEdit = GridHandlers.createButtonClickPopupHandler<HandpieceModelSchematicReadModel>(
            item => {
                switch (item.Type) {
                case HandpieceModelSchematicType.Text:
                    return routes.handpieceModelSchematics.editText(item.Id);
                case HandpieceModelSchematicType.Attachment:
                    return routes.handpieceModelSchematics.editAttachment(item.Id);
                case HandpieceModelSchematicType.Image:
                    return routes.handpieceModelSchematics.editImage(item.Id);
                default:
                    alert("Invalid schematic type");
                    throw new Error("Invalid schematic type");
                }
            },
            item => ({
                title: `Edit schematic ${item.Title}`,
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    switch (item.Type) {
                        case HandpieceModelSchematicType.Text:
                            await AjaxFormsManager.waitFor("HandpieceModelSchematicEditText");
                            break;
                        case HandpieceModelSchematicType.Attachment:
                            await AjaxFormsManager.waitFor("HandpieceModelSchematicEditAttachment");
                            break;
                        case HandpieceModelSchematicType.Image:
                            await AjaxFormsManager.waitFor("HandpieceModelSchematicEditImage");
                            break;
                    }
                    
                    await HandpieceModelSchematicsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static handleDelete = GridHandlers.createButtonClickPopupHandler<HandpieceModelSchematicReadModel>(
            item => routes.handpieceModelSchematics.delete(item.Id),
            item => ({
                title: `Delete schematic ${item.Title}`,
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("HandpieceModelSchematicDelete");
                    await HandpieceModelSchematicsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static async handleMoveUp(this: kendo.ui.Grid, e: JQueryEventObject) {
            e.preventDefault();
            const dataItem = this.dataItem<HandpieceModelSchematicReadModel>(e.currentTarget.closest("tr"));
            console.log(dataItem);

            await fetch(routes.handpieceModelSchematics.moveUp(dataItem.Id).value, {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    "X-Requested-With": "XMLHttpRequest"
                },
                body: ""
            });
            await this.dataSource.read();
        }

        static async handleMoveDown(this: kendo.ui.Grid, e: JQueryEventObject) {
            e.preventDefault();
            const dataItem = this.dataItem<HandpieceModelSchematicReadModel>(e.currentTarget.closest("tr"));
            console.log(dataItem);

            await fetch(routes.handpieceModelSchematics.moveDown(dataItem.Id).value, {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    "X-Requested-With": "XMLHttpRequest"
                },
                body: ""
            });
            await this.dataSource.read();
        }
    }

    export class HandpieceModelListingsGrid {
        static get instance(): kendo.ui.Grid {
            return $("#HandpieceModelListingsGrid").data("kendoGrid");
        }

        static get parentId(): string {
            return HandpieceModelListingsGrid.instance.wrapper.attr("data-parent-id");
        }

        static formatStatus(status: HandpieceStoreListingStatus): string {
            if (status === undefined || status === null) {
                return undefined;
            }

            switch (status) {
                case HandpieceStoreListingStatus.Listed: return "Listed";
                case HandpieceStoreListingStatus.Unlisted: return "Unlisted";
                case HandpieceStoreListingStatus.Deleted: return "Deleted";
                case HandpieceStoreListingStatus.Requested: return "Requested";
                case HandpieceStoreListingStatus.Sold: return "Sold";
            }

            return undefined;
        }

        static handleCreate = GridHandlers.createGridButtonClickPopupHandler(
            "#HandpieceModelListingsGrid .k-grid-CustomCreate",
            target => routes.handpieceModelListings.create(HandpieceModelListingsGrid.parentId),
            target => ({
                title: `Add listing`,
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("HandpieceModelListingCreate");
                    await HandpieceModelListingsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            })
        );

        static handleEdit = GridHandlers.createButtonClickPopupHandler<HandpieceModelListingReadModel>(
            item => routes.handpieceModelListings.edit(item.Id),
            item => ({
                title: `Edit listing`,
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("HandpieceModelListingEdit");
                    await HandpieceModelListingsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static handleDelete = GridHandlers.createButtonClickPopupHandler<HandpieceModelListingReadModel>(
            item => routes.handpieceModelListings.delete(item.Id),
            item => ({
                title: `Delete listing`,
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("HandpieceModelListingDelete");
                    await HandpieceModelListingsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));
    }
}