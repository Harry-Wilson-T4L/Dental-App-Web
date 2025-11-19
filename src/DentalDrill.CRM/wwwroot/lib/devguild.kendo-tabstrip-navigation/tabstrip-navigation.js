var DevGuild;
(function (DevGuild) {
    var Controls;
    (function (Controls) {
        var TabStrip;
        (function (TabStrip) {
            var Navigation;
            (function (Navigation) {
                var TabStripNavigator = /** @class */ (function () {
                    function TabStripNavigator() {
                    }
                    TabStripNavigator.enable = function (element) {
                        var tabStrip = $(element).data("kendoTabStrip");
                        tabStrip.bind("select", function (e) {
                            var tabName = $(e.item).attr("data-tabname");
                            var baseUrl = location.protocol + "//" + location.host + location.pathname;
                            var query = DevGuild.Utilities.QueryString.parse(location.search).set("Tab", tabName).build();
                            history.replaceState({}, document.title, "" + baseUrl + query);
                        });
                    };
                    return TabStripNavigator;
                }());
                Navigation.TabStripNavigator = TabStripNavigator;
                $(function () {
                    $(".k-tabstrip-autonavigation").each(function (index, element) { return TabStripNavigator.enable(element); });
                });
            })(Navigation = TabStrip.Navigation || (TabStrip.Navigation = {}));
        })(TabStrip = Controls.TabStrip || (Controls.TabStrip = {}));
    })(Controls = DevGuild.Controls || (DevGuild.Controls = {}));
})(DevGuild || (DevGuild = {}));
//# sourceMappingURL=tabstrip-navigation.js.map