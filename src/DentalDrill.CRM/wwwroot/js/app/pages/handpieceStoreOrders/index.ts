namespace DentalDrill.CRM.Pages.HandpieceStoreOrders.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    interface HandpieceStoreOrderReadModel {
        Id: string;
        OrderNumber: number;
        CreatedOn: Date;
        BrandModel: string;
        Status: string;
        Coupling: string;
        Notes: string;
        CosmeticCondition: string;
        FiberOpticBrightness: string;
        Warranty: string;
        Price: number;
    }

    export class HandpieceStoreOrdersGrid {
        static handleDetails = GridHandlers.createButtonClickPopupHandler<HandpieceStoreOrderReadModel>(
            item => routes.handpieceStoreOrders.details(item.Id),
            item => ({
                title: `Handpiece Order ${item.OrderNumber}`,
                width: `1000px`,
                height: `auto`,
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