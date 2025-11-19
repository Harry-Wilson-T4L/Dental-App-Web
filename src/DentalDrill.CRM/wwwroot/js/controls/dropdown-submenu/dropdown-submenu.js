var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            $(function () {
                $(document).on("click", ".dropdown-submenu > a", function (e) {
                    e.preventDefault();
                    var toggle = $(e.target);
                    var menu = toggle.next(".dropdown-menu");
                    if (menu.hasClass("show")) {
                        $(".dropdown-submenu .dropdown-menu").removeClass("show");
                    }
                    else {
                        $(".dropdown-submenu .dropdown-menu").removeClass("show");
                        toggle.next(".dropdown-menu").addClass("show");
                    }
                    e.stopPropagation();
                });
                $(document).on("hidden.bs.dropdown", function (e) {
                    $(".dropdown-menu.show").removeClass("show");
                });
                $(".dropdown-submenu").each(function (index, element) {
                    var dropdownNode = $(element).closest(".dropdown");
                    if (dropdownNode.length > 0) {
                        var dropdown = new Controls.Dropdown(dropdownNode[0]);
                        dropdown.preventContentClickHide();
                    }
                });
            });
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=dropdown-submenu.js.map