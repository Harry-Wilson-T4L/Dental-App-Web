namespace DentalDrill.CRM.Controls.Reporting {
    export interface ReportsPageGridColumn {
        field: string;
        title: string;
        format: string;
        groupHeaderColumnTemplate?: (dateSuffix: string) => (group: any) => string;
    }

    export class ReportsPageGridColumnHeaderGroupings {
        static sum(fieldName: string): (dateSuffix: string) => (group: any) => string {
            return (dateSuffix: string) => {
                return (group: any) => {
                    const valueField = `${fieldName}${dateSuffix}`;
                    const groupItems = ReportsPageDataSourceGroupHelper.getAllItemsFromGroup(group);
                    if (groupItems.length === 0) {
                        return ``;
                    }

                    let sumValue = 0;
                    for (let i = 0; i < groupItems.length; i++) {
                        const item = groupItems[i];
                        if (item[valueField] !== undefined && item[valueField] !== null) {
                            sumValue += item[valueField];
                        }
                    }

                    if (sumValue === 0) {
                        return ``;
                    }

                    return `${sumValue.toFixed(0)}`;
                };
            };
        }

        static average(valueFieldName: string, countFieldName: string): (dateSuffix: string) => (group: any) => string {
            return (dateSuffix: string) => {
                return (group: any) => {
                    const valueField = `${valueFieldName}${dateSuffix}`;
                    const countField = `${countFieldName}${dateSuffix}`;
                    const groupItems = ReportsPageDataSourceGroupHelper.getAllItemsFromGroup(group);
                    if (groupItems.length === 0) {
                        return ``;
                    }

                    let sumCount = 0;
                    let sumValue = 0;
                    for (let i = 0; i < groupItems.length; i++) {
                        const item = groupItems[i];
                        if (item[valueField] !== undefined && item[valueField] !== null && item[countField] !== undefined && item[countField] !== null) {
                            sumCount += item[countField];
                            sumValue += item[valueField] * item[countField];
                        }
                    }

                    if (sumCount === 0 || sumValue === 0) {
                        return ``;
                    }

                    return `${(sumValue / sumCount).toFixed(2)}`;
                };
            };
        }

        static averageFromSum(valueFieldName: string, countFieldName: string): (dateSuffix: string) => (group: any) => string {
            return (dateSuffix: string) => {
                return (group: any) => {
                    const valueField = `${valueFieldName}${dateSuffix}`;
                    const countField = `${countFieldName}${dateSuffix}`;
                    const groupItems = ReportsPageDataSourceGroupHelper.getAllItemsFromGroup(group);
                    if (groupItems.length === 0) {
                        return ``;
                    }

                    let sumCount = 0;
                    let sumValue = 0;
                    for (let i = 0; i < groupItems.length; i++) {
                        const item = groupItems[i];
                        if (item[valueField] !== undefined && item[valueField] !== null && item[countField] !== undefined && item[countField] !== null) {
                            sumCount += item[countField];
                            sumValue += item[valueField];
                        }
                    }

                    if (sumCount === 0 || sumValue === 0) {
                        return ``;
                    }

                    return `${(sumValue / sumCount).toFixed(2)}`;
                };
            };
        }
        
        static percent(valueFieldName: string, countFieldName: string): (dateSuffix: string) => (group: any) => string {
            return (dateSuffix: string) => {
                return (group: any) => {
                    const valueField = `${valueFieldName}${dateSuffix}`;
                    const countField = `${countFieldName}${dateSuffix}`;
                    const groupItems = ReportsPageDataSourceGroupHelper.getAllItemsFromGroup(group);
                    if (groupItems.length === 0) {
                        return ``;
                    }

                    let sumValue = 0;
                    let sumRating = 0;
                    for (let i = 0; i < groupItems.length; i++) {
                        const item = groupItems[i];
                        if (item[valueField] !== undefined && item[valueField] !== null && item[countField] !== undefined && item[countField] !== null) {
                            sumValue += item[countField];
                            sumRating += item[valueField] * item[countField];
                        }
                    }

                    if (sumValue === 0 || sumRating === 0) {
                        return ``;
                    }

                    return `${(100 * sumRating / sumValue).toFixed(2)}%`;
                };
            };
        }
    }

    export class ReportsPageGridHandpiecesColumns {
        handpiecesCount(options?: any): ReportsPageGridColumn {
            const column: ReportsPageGridColumn = {
                field: "HandpiecesCount",
                title: "Repairs",
                format: "{0:n0}",
                groupHeaderColumnTemplate: ReportsPageGridColumnHeaderGroupings.sum("HandpiecesCount")
            };

            if (options && typeof options === "object") {
                $.extend(column, options);
            }

            return column;
        }

        ratingAverage(options?: any): ReportsPageGridColumn {
            const column = {
                field: "RatingAverage",
                title: "Avg. Rating",
                format: "{0:n}",
                groupHeaderColumnTemplate: ReportsPageGridColumnHeaderGroupings.average("RatingAverage", "HandpiecesCount")
            };

            if (options && typeof options === "object") {
                $.extend(column, options);
            }

            return column;
        }

        unrepairablePercent(options?: any): ReportsPageGridColumn {
            const column = {
                field: "UnrepairedPercent",
                title: "Unrep.",
                format: "{0:p2}",
                groupHeaderColumnTemplate: ReportsPageGridColumnHeaderGroupings.percent("UnrepairedPercent", "HandpiecesCount")
            };

            if (options && typeof options === "object") {
                $.extend(column, options);
            }

            return column;
        }

        returnUnrepairedPercent(options?: any): ReportsPageGridColumn {
            const column = {
                field: "ReturnUnrepairedPercent",
                title: "Ret. Unrep.",
                format: "{0:p2}",
                groupHeaderColumnTemplate: ReportsPageGridColumnHeaderGroupings.percent("ReturnUnrepairedPercent", "HandpiecesCount")
            };

            if (options && typeof options === "object") {
                $.extend(column, options);
            }

            return column;
        }

        turnaroundAverage(options?: any): ReportsPageGridColumn {
            const column = {
                field: "TurnaroundAverage",
                title: "Avg. Turnaround",
                format: "{0:2}",
                groupHeaderColumnTemplate: ReportsPageGridColumnHeaderGroupings.average("TurnaroundAverage", "HandpiecesCount")
            };

            if (options && typeof options === "object") {
                $.extend(column, options);
            }

            return column;
        }

        warrantyCount(options?: any): ReportsPageGridColumn {
            const column = {
                field: "WarrantyCount",
                title: "Warranty",
                format: "{0:2}",
                groupHeaderColumnTemplate: ReportsPageGridColumnHeaderGroupings.sum("WarrantyCount")
            };

            if (options && typeof options === "object") {
                $.extend(column, options);
            }

            return column;
        }

        costSum(options?: any): ReportsPageGridColumn {
            const column = {
                field: "CostSum",
                title: "Cost",
                format: "{0:c0}",
                groupHeaderColumnTemplate: ReportsPageGridColumnHeaderGroupings.sum("CostSum")
            };

            if (options && typeof options === "object") {
                $.extend(column, options);
            }

            return column;
        }

        costAverage(options?: any): ReportsPageGridColumn {
            const column = {
                field: "CostAverage",
                title: "Avg. Cost",
                format: "{0:c0}",
                groupHeaderColumnTemplate: ReportsPageGridColumnHeaderGroupings.averageFromSum("CostSum", "HandpiecesCount")
            };

            if (options && typeof options === "object") {
                $.extend(column, options);
            }

            return column;
        }
    }

    export class ReportsPageGridColumns {
        private static readonly _handpieces: ReportsPageGridHandpiecesColumns = new ReportsPageGridHandpiecesColumns();

        static get handpieces(): ReportsPageGridHandpiecesColumns {
            return ReportsPageGridColumns._handpieces;
        }
    }
}