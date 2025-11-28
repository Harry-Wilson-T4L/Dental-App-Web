(() => {
    // Kendo culture file is already loaded in the bundle before this file
    if (typeof kendo !== 'undefined') {
        kendo.culture("en-AU");
    }
})();