namespace DentalDrill.CRM.Pages.EmployeeRoleWorkshops.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    interface EmployeeRoleWorkshop {
        Id: string;
    }

    export class EmployeeRoleWorkshopsGrid {
        static get instance(): kendo.ui.Grid {
            return $("#EmployeeRoleWorkshopsGrid").data("kendoGrid");
        }

        static handleDetails = GridHandlers.createButtonClickNavigationHandler<EmployeeRoleWorkshop>(
            item => routes.employeeRoleWorkshops.details(item.Id));

        static handleEdit = GridHandlers.createButtonClickPopupHandler<EmployeeRoleWorkshop>(
            item => routes.employeeRoleWorkshops.edit(item.Id),
            item => ({
                title: `Edit employee role workshop permission`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("EmployeeRoleWorkshopsEdit");
                    await EmployeeRoleWorkshopsGrid.instance.dataSource.read();
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

        static handleDelete = GridHandlers.createButtonClickPopupHandler<EmployeeRoleWorkshop>(
            item => routes.employeeRoleWorkshops.delete(item.Id),
            item => ({
                title: `Delete employee role workshop permission`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("EmployeeRoleWorkshopsDelete");
                    await EmployeeRoleWorkshopsGrid.instance.dataSource.read();
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
            "#EmployeeRoleWorkshopsGrid .k-grid-CustomCreate",
            target => routes.employeeRoleWorkshops.create($("#EmployeeRoleWorkshopsGrid").attr("data-parent-id")),
            target => ({
                title: `Create employee role workshop permission`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("EmployeeRoleWorkshopsCreate");
                    await EmployeeRoleWorkshopsGrid.instance.dataSource.read();
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