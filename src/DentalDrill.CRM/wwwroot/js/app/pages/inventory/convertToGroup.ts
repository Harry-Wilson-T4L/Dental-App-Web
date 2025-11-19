namespace DentalDrill.CRM.Pages.Inventory.ConvertToGroup {
    export class InventoryConvertToGroupEditor {
        private readonly _root: HTMLElement;
        private readonly _rootNode: JQuery<HTMLElement>;

        private readonly _createNewGroup: JQuery<HTMLElement>;
        private readonly _groupName: JQuery<HTMLElement>;
        private readonly _leafName: JQuery<HTMLElement>;
        private readonly _groupNameWrapper: JQuery<HTMLElement>;
        private readonly _leafNameWrapper: JQuery<HTMLElement>;


        constructor(root: HTMLElement) {
            this._root = root;
            this._rootNode = $(root);

            this._createNewGroup = this._rootNode.find("input[type=checkbox][name=CreateNewGroup]");
            this._groupName = this._rootNode.find("input[name=GroupName]");
            this._leafName = this._rootNode.find("input[name=LeafName]");
            this._groupNameWrapper = this._groupName.closest(".row");
            this._leafNameWrapper = this._leafName.closest(".row");
        }

        init() {
            this._createNewGroup.on("change", (e: JQueryEventObject) => {
                this.update();
            });

            this.update();
        }

        private update() {
            if (this._createNewGroup.prop("checked")) {
                this._groupName.prop("readonly", false);
                this._leafName.prop("readonly", false);
                this._groupNameWrapper.removeClass("k-state-disabled");
                this._leafNameWrapper.removeClass("k-state-disabled");
            } else {
                this._groupName.prop("readonly", true);
                this._leafName.prop("readonly", true);
                this._groupNameWrapper.addClass("k-state-disabled");
                this._leafNameWrapper.addClass("k-state-disabled");
            }
        }
    }
}