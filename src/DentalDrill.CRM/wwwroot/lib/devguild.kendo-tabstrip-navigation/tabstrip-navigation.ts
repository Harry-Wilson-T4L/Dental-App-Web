namespace DevGuild.Controls.TabStrip.Navigation {
    export class TabStripNavigator {
        static enable(element: Element): void {
            const tabStrip = $(element).data("kendoTabStrip");
            tabStrip.bind("select", e => {
                const tabName = $(e.item).attr("data-tabname");
                const baseUrl = `${location.protocol}//${location.host}${location.pathname}`;
                const query = Utilities.QueryString.parse(location.search).set("Tab", tabName).build();

                history.replaceState({}, document.title, `${baseUrl}${query}`);
            });
        }
    }

    $(() => {
        $(".k-tabstrip-autonavigation").each((index, element) => TabStripNavigator.enable(element));
    });
}