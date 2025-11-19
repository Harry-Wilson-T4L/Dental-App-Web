namespace DentalDrill.CRM.Controls {
    $(() => {
        $(document).on("click", ".dropdown-submenu > a", e => {
            e.preventDefault();

            const toggle = $(e.target);
            const menu = toggle.next(".dropdown-menu");

            if (menu.hasClass("show")) {
                $(".dropdown-submenu .dropdown-menu").removeClass("show");
            } else {
                $(".dropdown-submenu .dropdown-menu").removeClass("show");
                toggle.next(".dropdown-menu").addClass("show");
            }

            e.stopPropagation();
        });

        $(document).on("hidden.bs.dropdown", e => {
            $(".dropdown-menu.show").removeClass("show");
        });

        $(".dropdown-submenu").each((index, element) => {
            const dropdownNode = $(element).closest(".dropdown");
            if (dropdownNode.length > 0) {
                const dropdown = new Dropdown(dropdownNode[0]);
                dropdown.preventContentClickHide();
            }
        });
    });
}