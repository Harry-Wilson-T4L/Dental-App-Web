var DevGuild;
(function (DevGuild) {
    var UI;
    (function (UI) {
        var TabStrip;
        (function (TabStrip) {
            var TabStripSpinner = /** @class */ (function () {
                function TabStripSpinner(wrapper, tabstripWrapper) {
                    var _this = this;
                    if (tabstripWrapper === void 0) { tabstripWrapper = null; }
                    this.select = function () {
                        _this.spiner.remove();
                        _this.spiner = $("<div class='k-loading-image " + _this.spinnerClass + "'></div>");
                        _this.tabstripWrapper.append(_this.spiner);
                    };
                    this.show = function () {
                        _this.spiner.remove();
                    };
                    this.activate = function () {
                        _this.spiner.remove();
                    };
                    this.contentLoaded = function () {
                        _this.spiner.remove();
                    };
                    this.error = function () {
                        _this.spiner.remove();
                    };
                    this.wrapper = wrapper;
                    this.spinnerClass = "tabstrip-spinner-class-" + Math.random().toString(36).substring(2, 6);
                    if (tabstripWrapper == null) {
                        this.tabstripWrapper = wrapper.find(".k-tabstrip-wrapper").eq(0);
                        this.tabStrip = this.tabstripWrapper.find(".k-widget.k-tabstrip").eq(0).data("kendoTabStrip");
                    }
                    else {
                        this.tabstripWrapper = tabstripWrapper;
                        this.tabStrip = this.tabstripWrapper.data("kendoTabStrip");
                    }
                    this.spiner = $("<div class='k-loading-image " + this.spinnerClass + "'></div>");
                    this.wrapper.append(this.spiner);
                    this.tabStrip.bind("select", this.select);
                    this.tabStrip.bind("show", this.show);
                    this.tabStrip.bind("activate", this.activate);
                    this.tabStrip.bind("contentLoaded", this.contentLoaded);
                    this.tabStrip.bind("error", this.error);
                }
                TabStripSpinner.asyncInit = function () {
                    initialiseSpiner("tab-strip-spinner-async");
                };
                return TabStripSpinner;
            }());
            TabStrip.TabStripSpinner = TabStripSpinner;
            function initialiseSpiner(baseClassName) {
                $("." + baseClassName).each(function () {
                    var el = $(this);
                    new TabStripSpinner(el);
                });
                $("." + baseClassName + "-side").each(function () {
                    var el = $(this).find(".k-widget.k-tabstrip");
                    new TabStripSpinner(el, el);
                });
            }
            $(function () {
                initialiseSpiner("tab-strip-spinner");
            });
        })(TabStrip = UI.TabStrip || (UI.TabStrip = {}));
    })(UI = DevGuild.UI || (DevGuild.UI = {}));
})(DevGuild || (DevGuild = {}));
//# sourceMappingURL=tabStripSpinner.js.map