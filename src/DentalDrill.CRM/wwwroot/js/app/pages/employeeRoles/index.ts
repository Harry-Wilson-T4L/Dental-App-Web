namespace DentalDrill.CRM.Pages.EmployeeRoles.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    interface EmployeeRole {
        Id: string;
        Name: string;
    }

    export class EmployeeRolesGrid {
        static get instance(): kendo.ui.Grid {
            return $("#EmployeeRolesGrid").data("kendoGrid");
        }

        static handleDetails = GridHandlers.createButtonClickNavigationHandler<EmployeeRole>(
            item => routes.employeeRoles.details(item.Id));

        static handleEdit = GridHandlers.createButtonClickPopupHandler<EmployeeRole>(
            item => routes.employeeRoles.edit(item.Id),
            item => ({
                title: `Edit employee role ${item.Name}`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("EmployeeRolesEdit");
                    await EmployeeRolesGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                },
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
            }));

        static handleDelete = GridHandlers.createButtonClickPopupHandler<EmployeeRole>(
            item => routes.employeeRoles.delete(item.Id),
            item => ({
                title: `Delete employee role ${item.Name}`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("EmployeeRolesDelete");
                    await EmployeeRolesGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                },
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
            }));

        static handleCreate = GridHandlers.createGridButtonClickPopupHandler(
            "#EmployeeRolesGrid .k-grid-CustomCreate",
            target => routes.employeeRoles.create(),
            target => ({
                title: `Create employee role`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("EmployeeRolesCreate");
                    await EmployeeRolesGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                },
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
            })
        );
    }
}