var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            var DateRangeSelector = /** @class */ (function () {
                function DateRangeSelector(root) {
                    var _this = this;
                    this._root = root;
                    this._root.on("click", ".date-range-selector__item", function (e) {
                        var target = $(e.target);
                        var rangeFrom = _this.parseRangeDate(target.attr("data-range-from"));
                        var rangeTo = _this.parseRangeDate(target.attr("data-range-to"));
                        _this.applyRange(rangeFrom, rangeTo);
                    });
                    this._targetFrom = $(root.attr("data-range-target-from"));
                    this._targetTo = $(root.attr("data-range-target-to"));
                    this._defaultFrom = this.parseRangeDate(root.attr("data-range-default-from"));
                    this._defaultTo = this.parseRangeDate(root.attr("data-range-default-to"));
                    this._root.data("DateRangeSelector", this);
                }
                DateRangeSelector.prototype.reset = function () {
                    this.applyRange(this._defaultFrom, this._defaultTo);
                };
                DateRangeSelector.prototype.applyRange = function (from, to) {
                    this._targetFrom.each(function (index, element) {
                        var datePicker = $(element).data("kendoDatePicker");
                        datePicker.value(from);
                    });
                    this._targetTo.each(function (index, element) {
                        var datePicker = $(element).data("kendoDatePicker");
                        datePicker.value(to);
                    });
                };
                DateRangeSelector.prototype.parseRangeDate = function (dateString) {
                    var match = /^(\d{4})-(\d{2})-(\d{2})$/.exec(dateString);
                    if (match && match.length > 3) {
                        return new Date(parseInt(match[1]), parseInt(match[2]) - 1, parseInt(match[3]));
                    }
                    else {
                        throw new Error("Invalid date");
                    }
                };
                return DateRangeSelector;
            }());
            Controls.DateRangeSelector = DateRangeSelector;
            $(function () {
                $(".date-range-selector[data-init='true']").each(function (index, element) {
                    var selector = new DateRangeSelector($(element));
                });
            });
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=date-range-selector.js.map