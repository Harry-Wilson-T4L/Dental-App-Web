namespace DentalDrill.CRM.Pages.Users.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    export class UsersGrid {
        static get instance(): kendo.ui.Grid {
            return $("#UsersGrid").data("kendoGrid");
        }

        static handleDetails = GridHandlers.createButtonClickPopupHandler<{ Id: string, UserName: string }>(
            item => routes.users.details(item.Id),
            item => ({
                title: `User ${item.UserName}`,
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.center();
                },
            }));

        static handleEdit = GridHandlers.createButtonClickPopupHandler<{ Id: string, UserName: string }>(
            item => routes.users.edit(item.Id),
            item => ({
                title: `Edit user ${item.UserName}`,
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
                    await AjaxFormsManager.waitFor("UsersEdit");
                    await UsersGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static handleDelete = GridHandlers.createButtonClickPopupHandler<{ Id: string, UserName: string }>(
            item => routes.users.delete(item.Id),
            item => ({
                title: `Delete user ${item.UserName}`,
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
                    await AjaxFormsManager.waitFor("UsersDelete");
                    await UsersGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static handleCreate = GridHandlers.createGridButtonClickPopupHandler(
            "#UsersGrid .k-grid-CustomCreate",
            target => routes.users.create(),
            target => ({
                title: `Create user`,
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
                    await AjaxFormsManager.waitFor("UsersCreate");
                    await UsersGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            })
        );
    }
}