namespace DevGuild.AspNet.Controls.Grids.Errors {
    export class GridErrorHandler {
        static handleRequestStart(this: kendo.data.DataSource, e: kendo.data.DataSourceRequestStartEvent) {
        }

        static handleGridError(this: kendo.data.DataSource, e: kendo.data.DataSourceErrorEvent) {
            if (e.errors) {
                var message = "Errors:\n";
                Object.keys(e.errors).forEach(key => {
                    var entry = e.errors[key];
                    if ("errors" in entry) {
                        entry.errors.forEach(value => {
                            if (key) {
                                message += key + ": " + value + "\n";
                            } else {
                                message += value + "\n";
                            }
                        });
                    }
                });

                alert(message);
                e.sender.cancelChanges();
            } else if (e.xhr.status === 403) {
                alert("Access denied");
                e.sender.cancelChanges();
            }
        }
    }
}