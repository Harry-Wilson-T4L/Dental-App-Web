namespace DentalDrill.CRM.Controls.Reporting {
    export class ReportsPageDataSourceGroupHelper {
        public static getFirstItemFromGroup(group: kendo.data.DataSourceGroup, field: string) {
            if (group && group.items && group.items.length > 0) {
                const firstItem = group.items[0] as any;
                if (firstItem[field]) {
                    return firstItem;
                }

                if (firstItem["aggregates"] && firstItem["items"]) {
                    return this.getFirstItemFromGroup(firstItem, field);
                }
            }

            return undefined;
        }

        public static getAllItemsFromGroup(group: kendo.data.DataSourceGroup): any[] {
            const result = [];

            function processGroup(processedGroup: kendo.data.DataSourceGroup) {
                for (let i = 0; i < processedGroup.items.length; i++) {
                    const processedItem = processedGroup.items[i] as any;
                    if (processedItem["aggregates"] && processedItem["items"]) {
                        processGroup(processedItem);
                    } else {
                        result.push(processedItem);
                    }
                }
            }

            processGroup(group);
            return result;
        }
    }
}