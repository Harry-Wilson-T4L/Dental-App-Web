namespace DentalDrill.CRM.Pages.Jobs.Details {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    export class HandpiecesGrid {
        static getInstance(jobId: string): kendo.ui.Grid {
            return $(`#JobHandpiecesGrid_${jobId}`).data("kendoGrid");
        }

        static statusTemplate(dataItem: { Id: string; StatusId: number; Status: string }, options: { source: number; destinations: number[] }[], statuses: { value: number; text: string }[]): string {
            const option = options.filter(x => x.source === dataItem.StatusId)[0];
            if (!option) {
                return dataItem.Status;
            }

            const id = `Item_${dataItem.Id.replace(/\-/g, "",)}_Status`;
            let html = `<select id="${id}" class="handpiece-status-change__select" data-item-id="${dataItem.Id}">`;
            for (let destination of option.destinations) {
                const status = statuses.filter(x => x.value === destination)[0];
                if (destination === dataItem.StatusId) {
                    html += `<option value="${destination}" selected="selected">${status.text}</option>`;
                } else {
                    html += `<option value="${destination}">${status.text}</option>`;
                }
            }
            html += `</select>`;
            
            setTimeout(() => {
                $(`#${id}`).kendoDropDownList();
            });

            return `${html}`;
        }

        static handleStatusSubmit(event: Event) {
            const inputsRoot = $(".handpiece-status-change__inputs");
            inputsRoot.empty();

            $("select.handpiece-status-change__select").each((index, element) => {
                const node = $(element);
                const dropDown = node.data("kendoDropDownList");
                const value = dropDown.value();

                inputsRoot.append($("<input type=\"hidden\" />").attr("name", `Items[${index}].Id`).attr("value", node.attr("data-item-id")));
                inputsRoot.append($("<input type=\"hidden\" />").attr("name", `Items[${index}].Status`).attr("value", value));
            });

            return true;
        }


        static handleRead() {
            const jobNumber = $("#JobNumberFilter").val();
            const client = $("#ClientFilter").val();
            const receivedFrom = $("#ReceivedFromFilter").data("kendoDatePicker").value();
            const receivedTo = $("#ReceivedToFilter").data("kendoDatePicker").value();
            const serial = $("#SerialFilter").val();
            const makeAndModel = $("#MakeAndModelFilter").val();
            const type = $("#TypeFilter").data("kendoDropDownList").dataItem();

            return {
                jobNumber,
                client: client !== "None" ? client : null,
                receivedFrom: receivedFrom ? receivedFrom.toISOString() : null,
                receivedTo: receivedTo ? receivedTo.toISOString() : null,
                serial,
                makeAndModel: makeAndModel !== "Any" ? makeAndModel : null,
                type: type ? type.Value : null,
            }
        }

        static handleDetails = GridHandlers.createButtonClickNavigationHandler<{ Id: string, JobId: string, Serial: string }>(
            item => routes.handpieces.details(item.Id));

        static handleEdit = GridHandlers.createButtonClickNavigationHandler<{ Id: string, JobId: string, Serial: string }>(
            item => routes.handpieces.edit(item.Id));

        static handleDelete = GridHandlers.createButtonClickPopupHandler<{ Id: string, JobId: string, Serial: string }>(
            item => routes.handpieces.delete(item.Id),
            item => ({
                title: `Delete Handpiece ${item.Serial}`,
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
                    await AjaxFormsManager.waitFor("HandpiecesDelete");
                    await HandpiecesGrid.getInstance(item.JobId).dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static handleCreate = GridHandlers.createGridButtonClickPopupHandler(
            ".job-handpieces-grid .k-grid-CustomCreate2",
            target => {
                const gridElement = (target as JQuery<HTMLElement>).closest(".job-handpieces-grid");
                return routes.handpieces.create(gridElement.attr("data-job-id"));
            },
            target => {
                const gridElement = (target as JQuery<HTMLElement>).closest(".job-handpieces-grid");
                const grid = gridElement.data("kendoGrid");

                return {
                    title: `Add Handpiece`,
                    width: "1000px",
                    height: "auto",
                    refresh: (e: kendo.ui.WindowEvent) => {
                        e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                            clickEvent.preventDefault();
                            e.sender.close();
                            e.sender.destroy();
                        });

                        const form = document.querySelector("#HandpiecesCreate") as HTMLFormElement;
                        if (form) {
                            DentalDrill.CRM.Pages.Handpieces.Edit.HandpiecesEditForm.initialize(form);
                        }

                        e.sender.center();
                    },
                    open: async (e: kendo.ui.WindowEvent) => {
                        await AjaxFormsManager.waitFor("HandpiecesCreate");
                        await grid.dataSource.read();
                        e.sender.close();
                        e.sender.destroy();
                    }
                };
            }
        );

        static handleDataBound(e: kendo.ui.GridDataBoundEvent) {
            e.sender.element.find("[data-toggle='tooltip']").tooltip();
        }
    }

    export class JobInvoicesGrid {
        static getInstance(jobId: string): kendo.ui.Grid {
            return $(`#JobInvoicesGrid_${jobId}`).data("kendoGrid");
        }

        static handleDownload = GridHandlers.createButtonClickNavigationHandler<{ Id: string, JobId: string, FullInvoiceNumber: string }>(item => routes.jobInvoices.download(item.Id));

        static handleDelete = GridHandlers.createButtonClickPopupHandler<{ Id: string, JobId: string, FullInvoiceNumber: string }>(
            item => routes.jobInvoices.delete(item.Id),
            item => ({
                title: `Delete Invoice ${item.FullInvoiceNumber}`,
                width: "800px",
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
                    await AjaxFormsManager.waitFor("JobInvoicesDelete");
                    await JobInvoicesGrid.getInstance(item.JobId).dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static handleCreate = GridHandlers.createGridButtonClickPopupHandler(
            ".job-invoices-grid .k-grid-CustomCreate2",
            target => {
                const gridElement = (target as JQuery<HTMLElement>).closest(".job-invoices-grid");
                return routes.jobInvoices.create(gridElement.attr("data-job-id"));
            },
            target => {
                const gridElement = (target as JQuery<HTMLElement>).closest(".job-invoices-grid");
                const grid = gridElement.data("kendoGrid");

                return {
                    title: `Upload Invoice`,
                    width: "800px",
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
                        await AjaxFormsManager.waitFor("JobInvoicesCreate");
                        await grid.dataSource.read();
                        e.sender.close();
                        e.sender.destroy();
                    }
                };
            });
    }

    $(() => {
        $("[data-toggle='tooltip']").tooltip();
        //$("[data-toggle='tooltip']").tooltip({ trigger: 'manual' }).tooltip('show');
        //$("[data-toggle='tooltip']")
        //    .on("mouseenter", function () {
        //        var _this = this;
        //        $(this).tooltip("show");
        //        $(".tooltip").on("mouseleave", function () {
        //            $(_this).tooltip('hide');
        //        });
        //    }).on("mouseleave", function () {
        //        var _this = this;
        //        setTimeout(function () {
        //            if (!$(".tooltip:hover").length) {
        //                $(_this).tooltip("hide");
        //            }
        //        }, 300);
        //    });
    });
}