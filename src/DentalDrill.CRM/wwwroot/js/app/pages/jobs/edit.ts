namespace DentalDrill.CRM.Pages.Jobs.Edit {
    $(document as any).on("change", "#ApprovedBy", (e: JQueryEventObject) => {
        const target = e.target as HTMLInputElement;
        if (target.value && target.value !== "") {
            const approvedOnPicker = $("#ApprovedOn").data("kendoDatePicker");
            if (!approvedOnPicker.value()) {
                approvedOnPicker.value(new Date());
            }
        };
    });

    $(document as any).on("change", "#HasWarning", (e: JQueryEventObject) => {
        const target = e.target as HTMLInputElement;
        $(target).closest(".job-warning").toggleClass("job-warning--has-warning", target.checked);
    });
}