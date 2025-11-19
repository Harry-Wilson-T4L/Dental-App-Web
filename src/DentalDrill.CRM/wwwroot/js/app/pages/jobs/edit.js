var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var Jobs;
            (function (Jobs) {
                var Edit;
                (function (Edit) {
                    $(document).on("change", "#ApprovedBy", function (e) {
                        var target = e.target;
                        if (target.value && target.value !== "") {
                            var approvedOnPicker = $("#ApprovedOn").data("kendoDatePicker");
                            if (!approvedOnPicker.value()) {
                                approvedOnPicker.value(new Date());
                            }
                        }
                        ;
                    });
                    $(document).on("change", "#HasWarning", function (e) {
                        var target = e.target;
                        $(target).closest(".job-warning").toggleClass("job-warning--has-warning", target.checked);
                    });
                })(Edit = Jobs.Edit || (Jobs.Edit = {}));
            })(Jobs = Pages.Jobs || (Pages.Jobs = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=edit.js.map