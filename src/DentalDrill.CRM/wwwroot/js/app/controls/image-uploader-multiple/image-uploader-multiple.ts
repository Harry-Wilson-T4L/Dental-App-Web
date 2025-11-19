namespace DentalDrill.CRM.Controls {
    $(() => {
        $(document).on("click", ".image-uploader-multiple__previews__preview__remove", (e) => {
            const target = $(e.target);
            const previewNode = target.closest(".image-uploader-multiple__previews__preview");
            if (previewNode.length === 0) {
                return;
            }

            const id = previewNode.attr("data-id");
            if (!id) {
                return;
            }

            const containerNode = target.closest(".image-uploader-multiple");
            if (containerNode.length === 0) {
                return;
            }

            const idNode = containerNode.find(`.image-uploader-multiple__ids input.image-uploader-multiple__ids__id[value='${id}']`);
            if (idNode.length === 0) {
                return;
            }

            previewNode.remove();
            idNode.remove();
        });
    });
}