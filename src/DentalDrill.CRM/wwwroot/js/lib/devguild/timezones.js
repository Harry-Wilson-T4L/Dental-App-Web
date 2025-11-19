var DevGuild;
(function (DevGuild) {
    var Utilities;
    (function (Utilities) {
        var TimeZones = /** @class */ (function () {
            function TimeZones() {
            }
            TimeZones.treatAsUtc = function (date) {
                var utcDate = new Date();
                utcDate.setUTCFullYear(date.getFullYear(), date.getMonth(), date.getDate());
                utcDate.setUTCHours(date.getHours(), date.getMinutes(), date.getSeconds(), date.getMilliseconds());
                return utcDate;
            };
            return TimeZones;
        }());
        Utilities.TimeZones = TimeZones;
    })(Utilities = DevGuild.Utilities || (DevGuild.Utilities = {}));
})(DevGuild || (DevGuild = {}));
//# sourceMappingURL=timezones.js.map