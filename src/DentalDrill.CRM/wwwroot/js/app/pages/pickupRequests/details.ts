namespace DentalDrill.CRM.Pages.PickupRequests.Details {
    $(() => {
        const $document = $(document as any) as JQuery;
        $document.on("click",
            ".pickup-request-actions__complete",
            async (e) => {
                const id = e.target.getAttribute("data-id");
                const url = routes.pickupRequests.complete(id);

                const response = await $.ajax({
                    url: url.value,
                    method: "POST",
                    async: true,
                    cache: false,
                    data: { }
                });

                $(e.target).parents(".pickup-request-actions").remove();
            });

        $document.on("click",
            ".pickup-request-actions__cancel",
            async (e) => {
                const id = e.target.getAttribute("data-id");
                const url = routes.pickupRequests.cancel(id);

                const response = await $.ajax({
                    url: url.value,
                    method: "POST",
                    async: true,
                    cache: false,
                    data: { }
                });

                $(e.target).parents(".pickup-request-actions").remove();
            });
    });
}