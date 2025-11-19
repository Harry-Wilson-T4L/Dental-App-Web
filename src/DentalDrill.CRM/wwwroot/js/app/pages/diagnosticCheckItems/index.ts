namespace DentalDrill.CRM.Pages.DiagnosticCheckItems.Index {
    interface DiagnosticCheckType {
        Id: string;
        Name: string;
    }

    interface DiagnosticCheckItem {
        Id: string;
        Types: string[];
        Name: string;
    }

    export class DiagnosticCheckItemsGrid {
        private static _types: DiagnosticCheckType[];

        static setTypes(types: DiagnosticCheckType[]) {
            DiagnosticCheckItemsGrid._types = [];
            for (let type of types) {
                DiagnosticCheckItemsGrid._types.push({ Id: type.Id, Name: type.Name });
            }
        }

        private static resolveType(id: string): DiagnosticCheckType {
            return DiagnosticCheckItemsGrid._types.filter(x => x.Id === id)[0];
        }

        private static resolveTypeName(id: string): string {
            const type = DiagnosticCheckItemsGrid.resolveType(id);
            return type ? type.Name : "Unknown";
        }

        static renderTypesColumn(data: DiagnosticCheckItem): string {
            if (!DiagnosticCheckItemsGrid._types) {
                return `N/A`;
            }

            if (data.Types) {
                return data.Types.map(x => DiagnosticCheckItemsGrid.resolveTypeName(x)).join(", ");
            } else {
                return ``;
            }
        }

        static handleBeforeEdit(e: kendo.ui.GridBeforeEditEvent) {
            if (e.model.isNew()) {
                const types = e.model["Types"];
                if (types === undefined || types === null || types === "") {
                    e.model["Types"] = [];
                }
            }
        }
    }
}
