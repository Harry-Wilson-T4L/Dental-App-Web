(() => {
    ($ as any).validator.setDefaults({
        ignore: ":hidden:not(.validate-hidden):not(.file-uploader__id)"
    })
})();