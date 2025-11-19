var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            $(function () {
                $(document).on("click", ".image-uploader-multiple__previews__preview__remove", function (e) {
                    var target = $(e.target);
                    var previewNode = target.closest(".image-uploader-multiple__previews__preview");
                    if (previewNode.length === 0) {
                        return;
                    }
                    var id = previewNode.attr("data-id");
                    if (!id) {
                        return;
                    }
                    var containerNode = target.closest(".image-uploader-multiple");
                    if (containerNode.length === 0) {
                        return;
                    }
                    var idNode = containerNode.find(".image-uploader-multiple__ids input.image-uploader-multiple__ids__id[value='" + id + "']");
                    if (idNode.length === 0) {
                        return;
                    }
                    previewNode.remove();
                    idNode.remove();
                });
            });
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=image-uploader-multiple.js.map