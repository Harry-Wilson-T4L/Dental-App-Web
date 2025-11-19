var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var FeedbackForms;
            (function (FeedbackForms) {
                var Index;
                (function (Index) {
                    var GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
                    var FeedbackFormStatus;
                    (function (FeedbackFormStatus) {
                        FeedbackFormStatus[FeedbackFormStatus["New"] = 0] = "New";
                        FeedbackFormStatus[FeedbackFormStatus["Completed"] = 1] = "Completed";
                        FeedbackFormStatus[FeedbackFormStatus["Expired"] = 2] = "Expired";
                        FeedbackFormStatus[FeedbackFormStatus["Cancelled"] = 3] = "Cancelled";
                    })(FeedbackFormStatus || (FeedbackFormStatus = {}));
                    var FeedbackFormsGrid = /** @class */ (function () {
                        function FeedbackFormsGrid(root, questions) {
                            this._root = root;
                            this._questions = questions;
                        }
                        FeedbackFormsGrid.prototype.init = function () {
                            this._grid = this.createGrid();
                        };
                        FeedbackFormsGrid.initialize = function (root, questions) {
                            var obj = new FeedbackFormsGrid(root, questions);
                            obj.init();
                            FeedbackFormsGrid._instance = obj;
                            return obj;
                        };
                        Object.defineProperty(FeedbackFormsGrid, "instance", {
                            get: function () {
                                return FeedbackFormsGrid._instance;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        FeedbackFormsGrid.prototype.createGrid = function () {
                            var dataSource = this.createDataSource();
                            var gridContainer = $(this._root).find(".grid-container");
                            gridContainer.kendoGrid({
                                height: 630,
                                dataSource: dataSource,
                                columns: this.initializeColumns(),
                                pageable: true,
                                sortable: true,
                                filterable: {
                                    mode: "menu",
                                    extra: false,
                                    operators: {
                                        date: {
                                            gt: "After",
                                            lt: "Before",
                                        },
                                        string: {
                                            contains: "Contains",
                                        },
                                        number: {
                                            gt: ">",
                                            gte: ">=",
                                            lt: "<",
                                            lte: "<=",
                                        },
                                    },
                                },
                            });
                            var grid = gridContainer.data("kendoGrid");
                            return grid;
                        };
                        FeedbackFormsGrid.prototype.initializeColumns = function () {
                            var columns = [];
                            columns.push({
                                field: "CreatedOn",
                                title: "Created On",
                                template: "#if (data.CreatedOn) {# #:kendo.toString(data.CreatedOn, \"d\")#<br />#:kendo.toString(data.CreatedOn, \"t\")# #}#",
                                filterable: {
                                    extra: true,
                                    operators: {
                                        date: {
                                            gt: "After",
                                            lt: "Before",
                                        },
                                    },
                                },
                                width: 70,
                            });
                            columns.push({
                                field: "SentOn",
                                title: "Sent On",
                                template: "#if (data.SentOn) {# #:kendo.toString(data.SentOn, \"d\")#<br />#:kendo.toString(data.SentOn, \"t\")# #}#",
                                filterable: {
                                    extra: true,
                                    operators: {
                                        date: {
                                            gt: "After",
                                            lt: "Before",
                                        },
                                    },
                                },
                                width: 70,
                            });
                            columns.push({
                                field: "ClientFullName",
                                title: "Client",
                                template: "#:data.ClientName#<br/>#:data.ClientPrincipalDentist#<br/>#:data.ClientSuburb#",
                                width: 150,
                            });
                            columns.push({
                                field: "Status",
                                title: "Status",
                                template: function (data) {
                                    if (data === undefined || data === null) {
                                        return "";
                                    }
                                    switch (data.Status) {
                                        case FeedbackFormStatus.New:
                                            return "New";
                                        case FeedbackFormStatus.Completed:
                                            return "Completed";
                                        case FeedbackFormStatus.Expired:
                                            return "Expired";
                                        case FeedbackFormStatus.Cancelled:
                                            return "Cancelled";
                                    }
                                },
                                filterable: {
                                    multi: true,
                                    dataSource: [
                                        { Status: 0, StatusName: "New" },
                                        { Status: 1, StatusName: "Completed" },
                                        { Status: 2, StatusName: "Expired" },
                                        { Status: 3, StatusName: "Cancelled" },
                                    ],
                                    itemTemplate: function (e) { return "<label class=\"k-label\"><input type=\"checkbox\" class=\"\" value=\"#:(data.all ? '' : data.Status)#\"><span class=\"k-item-title\">#:(data.all ? data.all : data.StatusName)#</span></label>"; },
                                },
                                width: 80,
                            });
                            columns.push({
                                field: "TotalRating",
                                title: "Total Rating",
                                width: 50,
                            });
                            for (var _i = 0, _a = this._questions; _i < _a.length; _i++) {
                                var question = _a[_i];
                                var fieldName = "Answers_" + question.id.replace(/\-/g, "");
                                switch (question.type) {
                                    case 0:
                                        columns.push({
                                            field: fieldName,
                                            title: question.shortName,
                                            sortable: false,
                                            filterable: false,
                                            width: 50,
                                        });
                                        break;
                                    case 1:
                                        columns.push({
                                            field: fieldName,
                                            title: question.shortName,
                                            sortable: false,
                                            filterable: false,
                                            width: 100,
                                        });
                                        break;
                                }
                            }
                            columns.push({
                                command: [
                                    {
                                        name: "CustomDetails",
                                        text: "<span class=\"fas fa-fw fa-external-link-alt\"></span>",
                                        click: GridHandlers.createButtonClickPopupHandler(function (item) { return routes.feedbackForms.details(item.Id); }, function (item) { return ({
                                            title: "Feedback form",
                                            width: "1000px",
                                            height: "auto",
                                            refresh: function (e) {
                                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                                    clickEvent.preventDefault();
                                                    e.sender.close();
                                                    e.sender.destroy();
                                                });
                                                e.sender.center();
                                            }
                                        }); })
                                    }
                                ],
                                width: 50,
                            });
                            return columns;
                        };
                        FeedbackFormsGrid.prototype.createDataSource = function () {
                            var dataSource = new kendo.data.DataSource({
                                type: "aspnetmvc-ajax",
                                transport: {
                                    read: {
                                        url: routes.feedbackForms.read().value,
                                    }
                                },
                                pageSize: 20,
                                schema: {
                                    data: "Data",
                                    total: "Total",
                                    errors: "Errors",
                                    model: {
                                        id: "Id",
                                        fields: this.initializeSchemaModel()
                                    }
                                },
                                serverAggregates: true,
                                serverFiltering: true,
                                serverGrouping: true,
                                serverPaging: true,
                                serverSorting: true,
                                sort: [
                                    { field: "CreatedOn", dir: "desc" }
                                ]
                            });
                            return dataSource;
                        };
                        FeedbackFormsGrid.prototype.initializeSchemaModel = function () {
                            var fields = {
                                CreatedOn: { type: "date" },
                                SentOn: { type: "date" },
                                Status: { type: "number" },
                                TotalRating: { type: "number" },
                            };
                            for (var _i = 0, _a = this._questions; _i < _a.length; _i++) {
                                var question = _a[_i];
                                var fieldName = "Answers_" + question.id.replace(/\-/g, "");
                                var fieldSource = "Answers[\"" + question.id + "\"]";
                                switch (question.type) {
                                    case 0:
                                        fields[fieldName] = {
                                            type: "number",
                                            from: fieldSource + ".IntegerValue"
                                        };
                                        break;
                                    case 1:
                                        fields[fieldName] = {
                                            type: "string",
                                            from: fieldSource + ".StringValue"
                                        };
                                        break;
                                }
                            }
                            return fields;
                        };
                        return FeedbackFormsGrid;
                    }());
                    Index.FeedbackFormsGrid = FeedbackFormsGrid;
                })(Index = FeedbackForms.Index || (FeedbackForms.Index = {}));
            })(FeedbackForms = Pages.FeedbackForms || (Pages.FeedbackForms = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=index.js.map