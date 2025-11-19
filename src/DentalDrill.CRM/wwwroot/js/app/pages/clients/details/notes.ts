namespace DentalDrill.CRM.Pages.Clients.Details {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;

    export class ClientNotesGrid {
        static get instance(): kendo.ui.Grid {
            return $("#ClientNotesGrid").data("kendoGrid");
        }

        static handleDetailsClick = GridHandlers.createButtonClickPopupHandler<ViewModels.ClientNoteViewModel>(
            item => routes.clientNotes.details(item.Id),
            target => ({
                title: "Note",
                width: "800px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.center();
                }
            }));

        static handleEditClick = GridHandlers.createButtonClickPopupHandler<ViewModels.ClientNoteEditViewModel>(
            item => routes.clientNotes.edit(item.Id),
            target => ({
                title: "Edit note",
                width: "800px",
                height: "auto",
                open: async function (this: kendo.ui.Window) {
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("EditNote");
                    await ClientNotesGrid.instance.dataSource.read();
                    this.close();
                    this.destroy();
                },
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                }

            }));

        static handleDeleteClick = GridHandlers.createButtonClickPopupHandler<ViewModels.ClientNoteViewModel>(
            item => routes.clientNotes.delete(item.Id),
            target => ({
                title: "Delete note",
                width: "800px",
                height: "auto",
                open: async function (this: kendo.ui.Window) {
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("DeleteNote");
                    await ClientNotesGrid.instance.dataSource.read();
                    this.close();
                    this.destroy();
                },
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                }
            }));

        static handleCreate = GridHandlers.createGridButtonClickPopupHandler(
            "#ClientNotesGrid .k-grid-CustomCreate",
            target => new DevGuild.AspNet.Routing.Uri(target.attr("href")),
            target => ({
                title: "Create Note",
                width: "800px",
                height: "auto",
                open: async function (this: kendo.ui.Window) {
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("CreateNote");
                    await ClientNotesGrid.instance.dataSource.read();
                    this.close();
                    this.destroy();
                },
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                }
            }));
    }
}