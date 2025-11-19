namespace DentalDrill.CRM.Pages.Corporates.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    export class CorporatesGrid {
        static get instance(): kendo.ui.Grid {
            return $("#CorporatesGrid").data("kendoGrid");
        }

        static handleDetails = GridHandlers.createButtonClickNavigationHandler<{ Id: string, Name: string, UserName: string }>(item => routes.corporates.details(item.Id));

        static handleEdit = GridHandlers.createButtonClickPopupHandler<{ Id: string, Name: string, UserName: string }>(
            item => routes.corporates.edit(item.Id),
            item => ({
                title: `Edit corporate ${item.Name}`,
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
                    await AjaxFormsManager.waitFor("CorporatesEdit");
                    await CorporatesGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static handleDelete = GridHandlers.createButtonClickPopupHandler<{ Id: string, Name: string, UserName: string }>(
            item => routes.corporates.delete(item.Id),
            item => ({
                title: `Delete corporate ${item.Name}`,
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
                    await AjaxFormsManager.waitFor("CorporatesDelete");
                    await CorporatesGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static handleCreate = GridHandlers.createGridButtonClickPopupHandler(
            "#CorporatesGrid .k-grid-CustomCreate",
            target => routes.corporates.create(),
            target => ({
                title: `Create corporate`,
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
                    await AjaxFormsManager.waitFor("CorporatesCreate");
                    await CorporatesGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            })
        );
    }
}