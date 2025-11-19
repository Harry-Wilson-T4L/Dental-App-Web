var DevGuild;
(function (DevGuild) {
    var AspNet;
    (function (AspNet) {
        var Controls;
        (function (Controls) {
            var Grids;
            (function (Grids) {
                var Errors;
                (function (Errors) {
                    var GridErrorHandler = /** @class */ (function () {
                        function GridErrorHandler() {
                        }
                        GridErrorHandler.handleRequestStart = function (e) {
                        };
                        GridErrorHandler.handleGridError = function (e) {
                            if (e.errors) {
                                var message = "Errors:\n";
                                Object.keys(e.errors).forEach(function (key) {
                                    var entry = e.errors[key];
                                    if ("errors" in entry) {
                                        entry.errors.forEach(function (value) {
                                            if (key) {
                                                message += key + ": " + value + "\n";
                                            }
                                            else {
                                                message += value + "\n";
                                            }
                                        });
                                    }
                                });
                                alert(message);
                                e.sender.cancelChanges();
                            }
                            else if (e.xhr.status === 403) {
                                alert("Access denied");
                                e.sender.cancelChanges();
                            }
                        };
                        return GridErrorHandler;
                    }());
                    Errors.GridErrorHandler = GridErrorHandler;
                })(Errors = Grids.Errors || (Grids.Errors = {}));
            })(Grids = Controls.Grids || (Controls.Grids = {}));
        })(Controls = AspNet.Controls || (AspNet.Controls = {}));
    })(AspNet = DevGuild.AspNet || (DevGuild.AspNet = {}));
})(DevGuild || (DevGuild = {}));
//# sourceMappingURL=grid-errors.js.map