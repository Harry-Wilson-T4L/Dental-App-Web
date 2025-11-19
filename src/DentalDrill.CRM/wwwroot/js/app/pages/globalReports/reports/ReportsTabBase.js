var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var GlobalReports;
            (function (GlobalReports) {
                var Reports;
                (function (Reports) {
                    var ReportsTabBase = /** @class */ (function () {
                        function ReportsTabBase() {
                        }
                        ReportsTabBase.prototype.generateDateRange = function (from, to, type) {
                            function addMonth(date, numberOfMonths) {
                                var copy = new Date(date.getTime());
                                copy.setMonth(copy.getMonth() + numberOfMonths);
                                return copy;
                            }
                            function getWeekStart(date) {
                                var copy = new Date(date.getTime());
                                copy.setDate(copy.getDate() - copy.getDay());
                                return copy;
                            }
                            function getWeek(date) {
                                var jan1 = new Date(date.getFullYear(), 0, 1);
                                var jan1WeekStart = getWeekStart(jan1);
                                var dateWeekStart = getWeekStart(date);
                                var passedTime = dateWeekStart.getTime() - jan1WeekStart.getTime();
                                var passedWeeks = Math.floor(passedTime / (7 * 24 * 60 * 60 * 1000));
                                return passedWeeks + 1;
                            }
                            function addWeek(date, numberOfWeeks) {
                                var copy = new Date(date.getTime());
                                copy.setDate(copy.getDate() + 7 * numberOfWeeks);
                                return copy;
                            }
                            function addDay(date, numberOfDays) {
                                var copy = new Date(date.getTime());
                                copy.setDate(copy.getDate() + numberOfDays);
                                return copy;
                            }
                            function numPad(val, digits) {
                                var result = val.toString();
                                while (result.length < digits) {
                                    result = "0" + result;
                                }
                                return result;
                            }
                            switch (type) {
                                case "Yearly":
                                    {
                                        var start = from.getFullYear();
                                        var end = to.getFullYear();
                                        var result = [];
                                        for (var current = start; current <= end; current++) {
                                            result.push(current.toString());
                                        }
                                        return result;
                                    }
                                case "Quarterly":
                                    {
                                        var start = new Date(from.getFullYear(), Math.floor(from.getMonth() / 3) * 3, 1);
                                        var end = to;
                                        var result = [];
                                        for (var current = start; current <= end; current = addMonth(current, 3)) {
                                            result.push(current.getFullYear() + "-Q" + (Math.floor(current.getMonth() / 3) + 1));
                                        }
                                        return result;
                                    }
                                case "Monthly":
                                    {
                                        var start = new Date(from.getFullYear(), from.getMonth(), 1);
                                        var end = to;
                                        var result = [];
                                        for (var current = start; current <= end; current = addMonth(current, 1)) {
                                            result.push(current.getFullYear() + "-" + (current.getMonth() + 1));
                                        }
                                        return result;
                                    }
                                case "Weekly":
                                    {
                                        var start = getWeekStart(from);
                                        var end = to;
                                        var result = [];
                                        for (var current = start, week = getWeek(start), previous = current; current <= end; current = addWeek(current, 1), week++) {
                                            if (previous.getFullYear() !== current.getFullYear()) {
                                                week = 1;
                                                previous = current;
                                            }
                                            result.push(current.getFullYear() + "-W" + week);
                                        }
                                        return result;
                                    }
                                case "Daily":
                                    {
                                        var start = from;
                                        var end = to;
                                        var result = [];
                                        for (var current = start; current <= end; current = addDay(current, 1)) {
                                            result.push(current.getFullYear() + "-" + numPad(current.getMonth() + 1, 2) + "-" + numPad(current.getDate(), 2));
                                        }
                                        return result;
                                    }
                            }
                        };
                        return ReportsTabBase;
                    }());
                    Reports.ReportsTabBase = ReportsTabBase;
                })(Reports = GlobalReports.Reports || (GlobalReports.Reports = {}));
            })(GlobalReports = Pages.GlobalReports || (Pages.GlobalReports = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=ReportsTabBase.js.map