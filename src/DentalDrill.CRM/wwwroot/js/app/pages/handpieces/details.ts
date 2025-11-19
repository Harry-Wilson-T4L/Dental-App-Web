namespace DentalDrill.CRM.Pages.Handpieces.Details {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    export class HandpiecesDetailsForm {
        static initializeOtherDiagnostics() {
            ($(document as any) as JQuery<HTMLElement>).on("change", ".diagnostic-check-other__selected", (e: JQueryEventObject) => {
                const value = $(".diagnostic-check-other__selected").prop("checked");
                $(".diagnostic-check-other__text").toggleClass("d-none", !value);
            });
        }

        static handleSetStatus = GridHandlers.createGridButtonClickPopupHandler(
            ".handpiece-status-change-button",
            target => new DevGuild.AspNet.Routing.Uri(target.attr("href")),
            target => {
                return {
                    title: `Set Status`,
                    width: "600px",
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
                        const result = await AjaxFormsManager.waitFor("HandpiecesSetStatus");

                        if (result.status !== undefined && result.visualisation !== undefined && result.description !== undefined) {
                            const parent = (target as JQuery<HTMLElement>).closest(".details");
                            parent.find(".handpiece-status-indicator")
                                .attr("data-value", `${result.visualisation > 0 ? result.visualisation : 6}`)
                                .attr("data-danger", result.visualisation === 0 ? "True" : "False");
                            parent.find(".handpiece-status-description").text(result.description);
                        }

                        e.sender.close();
                        e.sender.destroy();
                    }
                };
            }
        );
    }

    $(() => {
        HandpiecesDetailsForm.initializeOtherDiagnostics();
    });

    export class HandpieceHistoryGrid {
        static handleDetails = GridHandlers.createButtonClickNavigationHandler<{ Id: string }>(item => routes.handpieces.details(item.Id));
    }
}