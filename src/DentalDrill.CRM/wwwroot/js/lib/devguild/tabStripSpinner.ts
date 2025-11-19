namespace DevGuild.UI.TabStrip {

    export class TabStripSpinner {
        wrapper: JQuery<Element>;
        spinnerClass: string;
        tabstripWrapper: JQuery<Element>;
        tabStrip: kendo.ui.TabStrip;
        spiner: JQuery<Element>

        constructor(wrapper: JQuery<HTMLElement>, tabstripWrapper: JQuery<HTMLElement> = null) {
            this.wrapper = wrapper;
            this.spinnerClass = "tabstrip-spinner-class-" + Math.random().toString(36).substring(2, 6);

            if (tabstripWrapper == null) {
                this.tabstripWrapper = wrapper.find(".k-tabstrip-wrapper").eq(0);
                this.tabStrip = this.tabstripWrapper.find(".k-widget.k-tabstrip").eq(0).data("kendoTabStrip");
            } else {
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

        select = () => {
            this.spiner.remove();
            this.spiner = $("<div class='k-loading-image " + this.spinnerClass + "'></div>");
            this.tabstripWrapper.append(this.spiner);
        }

        show = () => {
            this.spiner.remove();
        }

        activate = () => {
            this.spiner.remove();
        }

        contentLoaded = () => {
            this.spiner.remove();
        }

        error = () => {
            this.spiner.remove();
        }

        static asyncInit() {
            initialiseSpiner("tab-strip-spinner-async");
        }

    }

    function initialiseSpiner(baseClassName: string) {
        $("." + baseClassName).each(function () {
            const el = $(this);
            new TabStripSpinner(el);
        });

        $(`.${baseClassName}-side`).each(function () {
            const el = $(this).find(".k-widget.k-tabstrip");
            new TabStripSpinner(el, el);
        });
    }

    $(() => {
        initialiseSpiner("tab-strip-spinner");
    });
}