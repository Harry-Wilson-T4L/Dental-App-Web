namespace DentalDrill.CRM.Pages.Employees.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    export class EmployeesGrid {
        static get instance(): kendo.ui.Grid {
            return $("#EmployeesGrid").data("kendoGrid");
        }

        static handleDetails = GridHandlers.createButtonClickPopupHandler<{ Id: string, FirstName: string, LastName: string, UserName: string }>(
            item => routes.employees.details(item.Id),
            item => ({
                title: `Employee ${item.FirstName} ${item.LastName}`,
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.center();
                },
            }));

        static handleEdit = GridHandlers.createButtonClickPopupHandler<{ Id: string, FirstName: string, LastName: string, UserName: string }>(
            item => routes.employees.edit(item.Id),
            item => ({
                title: `Edit employee ${item.FirstName} ${item.LastName}`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("EmployeesEdit");
                    await EmployeesGrid.instance.dataSource.read();
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

        static handleDelete = GridHandlers.createButtonClickPopupHandler<{ Id: string, FirstName: string, LastName: string, UserName: string }>(
            item => routes.employees.delete(item.Id),
            item => ({
                title: `Delete employee ${item.FirstName} ${item.LastName}`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("EmployeesDelete");
                    await EmployeesGrid.instance.dataSource.read();
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
            "#EmployeesGrid .k-grid-CustomCreate",
            target => routes.employees.create(),
            target => ({
                title: `Create employee`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("EmployeesCreate");
                    await EmployeesGrid.instance.dataSource.read();
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

        static handleShowDeleted = GridHandlers.createGridButtonClickNavigationHandler(
            "#EmployeesGrid .k-grid-CustomShowDeleted",
            target => routes.employees.indexWithDeleted());
    }
}