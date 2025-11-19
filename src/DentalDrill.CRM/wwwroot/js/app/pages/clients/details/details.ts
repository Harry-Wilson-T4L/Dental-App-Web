namespace DentalDrill.CRM.Pages.Clients.Details {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;

    export class ClientAttachmentsGrid {
        static get instance(): kendo.ui.Grid {
            return $("#ClientAttachmentsGrid").data("kendoGrid");
        }

        static handleDownload = GridHandlers.createButtonClickNavigationHandler<{ Id: string, FileName: string }>(x => routes.clientAttachments.download(x.Id));

        static handleEdit = GridHandlers.createButtonClickPopupHandler<{ Id: string, FileName: string }>(
            item => routes.clientAttachments.edit(item.Id),
            item => ({
                title: `Edit Attachment ${item.FileName}`,
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click",
                        ".editor__submit__cancel",
                        clickEvent => {
                            clickEvent.preventDefault();
                            e.sender.close();
                            e.sender.destroy();
                        });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("ClientAttachmentsEdit");
                    await ClientAttachmentsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static handleDelete = GridHandlers.createButtonClickPopupHandler<{ Id: string, FileName: string }>(
            item => routes.clientAttachments.delete(item.Id),
            item => ({
                title: `Delete Attachment ${item.FileName}`,
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click",
                        ".editor__submit__cancel",
                        clickEvent => {
                            clickEvent.preventDefault();
                            e.sender.close();
                            e.sender.destroy();
                        });
                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("ClientAttachmentsDelete");
                    await ClientAttachmentsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static handleCreate = GridHandlers.createGridButtonClickPopupHandler(
            "#ClientAttachmentsGrid .k-grid-CustomCreate2",
            target => {
                const gridElement = (target as JQuery<HTMLElement>).closest(".client-attachments-grid");
                return routes.clientAttachments.create(gridElement.attr("data-client-id"));
            },
            target => ({
                title: "Create Attachment",
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click",
                        ".editor__submit__cancel",
                        clickEvent => {
                            clickEvent.preventDefault();
                            e.sender.close();
                            e.sender.destroy();
                        });
                    e.sender.center();
                },
                open: async function(this: kendo.ui.Window) {
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("ClientAttachmentsCreate");
                    await ClientAttachmentsGrid.instance.dataSource.read();
                    this.close();
                    this.destroy();
                }
            }));
    }
}