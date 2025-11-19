namespace DevGuild.Filters.Grids {

    export abstract class GridFilterField {
        abstract apply(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void;
        abstract reset(): void;
    }

    export class IntSelectGridFilterField extends GridFilterField {
        private readonly _root: JQuery;
        private readonly _fieldName: string;

        constructor(root: JQuery, fieldName: string) {
            super();
            this._root = root;
            this._fieldName = fieldName;
        }

        apply(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void {
            var value = this._root.val();
            if (Array.isArray(value)) {
                throw new Error('Value cannot be array');
            }

            if (typeof value === 'string') {
                value = parseInt(value);
            }
            if ((!exceptions || exceptions.every(x => x !== this._fieldName)) && value) {
                filters.push({
                    field: this._fieldName,
                    operator: "eq",
                    value
                });
            }
        }

        reset(): void {
            this._root.val("").trigger("change");
        }

        get control(): JQuery {
            return this._root;
        }
    }

    export class IdDropDownListGridFilterField extends GridFilterField {
        private readonly _root: JQuery;
        private readonly _fieldName: string;
        private readonly _operator: string;

        constructor(root: JQuery, fieldName: string, operator: string = "eq") {
            super();
            this._root = root;
            this._fieldName = fieldName;
            this._operator = operator;
        }

        apply(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void {
            var value = this._root.data("kendoDropDownList").dataItem().Id;

            if ((!exceptions || exceptions.every(x => x !== this._fieldName)) && value && value !== "00000000-0000-0000-0000-000000000000") {
                filters.push({
                    field: this._fieldName,
                    operator: this._operator,
                    value
                });
            }
        }

        reset(): void {
            this._root.data('kendoDropDownList').trigger("change");
        }

        get control(): JQuery {
            return this._root;
        }
    }

    export class DropDownListGridFilterField extends GridFilterField {
        private readonly _root: JQuery;
        private readonly _fieldName: string;
        private readonly _operator: string;
        private readonly _defaultValue: string;
        private readonly _getValue: (any) => any;

        private _applyValueDelegate: (filters: kendo.data.DataSourceFilter[], value: any) => void;
        private _resetValueDelegate: () => string;

        constructor(root: JQuery, fieldName: string, { defaultValue = null, operator = "eq", value = a => a }: { defaultValue?: any, operator?: string, value?: (any) => any} = {}) {
            super();
            this._root = root;
            this._fieldName = fieldName;
            this._operator = operator;
            this._defaultValue = defaultValue;
            this._getValue = value;
        }

        apply(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void {
            const value = this._getValue(this._root.data("kendoDropDownList").dataItem());

            if ((!exceptions || exceptions.every(x => x !== this._fieldName)) && value && (!this._defaultValue || value !== this._defaultValue)) {

                if (this._applyValueDelegate) {
                    this._applyValueDelegate(filters, value);
                } else {
                    filters.push({
                        field: this._fieldName,
                        operator: this._operator,
                        value
                    });
                }
            }
        }

        reset(): void {
            const dropDown = this._root.data("kendoDropDownList");
            if (this._resetValueDelegate) {
                dropDown.value(this._resetValueDelegate());
            } else {
                dropDown.value(this._defaultValue);
            }
            dropDown.trigger("change");
        }

        get control(): JQuery {
            return this._root;
        }

        get kendoControl(): kendo.ui.DropDownList {
            return this._root.data("kendoDropDownList");
        }

        get applyValueDelegate(): (filters: kendo.data.DataSourceFilter[], value: any) => void {
            return this._applyValueDelegate;
        }

        set applyValueDelegate(val: (filters: kendo.data.DataSourceFilter[], value: any) => void) {
            this._applyValueDelegate = val;
        }

        get resetValueDelegate(): () => string {
            return this._resetValueDelegate;
        }

        set resetValueDelegate(val: () => string) {
            this._resetValueDelegate = val;
        }
    }

    export class ComplexDropDownListGridFilterField extends GridFilterField {
        private readonly _root: JQuery;
        private readonly _fieldName: string;
        private readonly _operator: string;
        private readonly _defaultValue: string;
        private readonly _getValue: (any) => any;

        constructor(root: JQuery, fieldName: string, { defaultValue = null, operator = "eq", value = a => a }: { defaultValue?: any, operator?: string, value?: (any) => any } = {}) {
            super();
            this._root = root;
            this._fieldName = fieldName;
            this._operator = operator;
            this._defaultValue = defaultValue;
            this._getValue = value;
        }

        apply(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void {
            const value = this._getValue(this._root.data("kendoDropDownList").dataItem());

            if ((!exceptions || exceptions.every(x => x !== this._fieldName)) && value && (this._defaultValue === undefined || value.Value !== this._defaultValue)) {
                filters.push({
                    field: this._fieldName,
                    operator: this._operator,
                    value: value.Value
                });
            }
        }

        reset(): void {
            this._root.data('kendoDropDownList').trigger("change");
        }

        get control(): JQuery {
            return this._root;
        }
    }

    export class MultiSelectGridFilterField extends GridFilterField {
        private readonly _root: JQuery;
        private readonly _fieldName: string;

        constructor(root: JQuery, fieldName: string) {
            super();
            this._root = root;
            this._fieldName = fieldName;
        }

        apply(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void {
            var values = this._root.data('kendoMultiSelect').value();

            var innerFilters: kendo.data.DataSourceFilterItem[] = [];
            for (var i = 0; i < values.length; i++) {
                const value = values[i];
                if ((!exceptions || exceptions.every(x => x !== this._fieldName)) && value) {
                    innerFilters.push({
                        field: this._fieldName,
                        operator: "eq",
                        value
                    });
                }
            }

            if (innerFilters.length) {
                filters.push({
                    logic: "or",
                    filters: innerFilters
                });
            }    
        }

        reset(): void {
            this._root.data('kendoMultiSelect').trigger("change");
        }

        get control(): JQuery {
            return this._root;
        }
    }

    export class BooleanSelectGridFilterField extends GridFilterField {
        private readonly _root: JQuery;
        private readonly _fieldName: string;

        constructor(root: JQuery, fieldName: string) {
            super();
            this._root = root;
            this._fieldName = fieldName;
        }

        apply(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void {
            const value = this._root.val();
            if ((!exceptions || exceptions.every(x => x !== this._fieldName)) && value) {
                switch (value) {
                    case "true":
                        filters.push({
                            field: this._fieldName,
                            operator: "eq",
                            value: "true"
                        });
                        break;
                    case "false":
                        filters.push({
                            field: this._fieldName,
                            operator: "eq",
                            value: "false"
                        });
                        break;
                }
            }
        }

        reset(): void {
            this._root.val("").trigger("change");
        }

        get control(): JQuery {
            return this._root;
        }
    }

    export class StringInputGridFilterField extends GridFilterField {
        private readonly _root: JQuery;
        private readonly _fieldName: string;
        private readonly _operator: string;

        constructor(root: JQuery, fieldName: string, operator: string = "contains") {
            super();
            this._root = root;
            this._fieldName = fieldName;
            this._operator = operator;
        }

        apply(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void {
            const value = this._root.val();
            if ((!exceptions || exceptions.every(x => x !== this._fieldName)) && value) {
                filters.push({
                    field: this._fieldName,
                    operator: this._operator,
                    value: value
                });
            }
        }

        reset(): void {
            this._root.val("").trigger("change");
        }

        get control(): JQuery {
            return this._root;
        }
    }


    export class DatePickerFilterField extends GridFilterField {
        private readonly _root: JQuery;
        private readonly _fieldName: string;
        private readonly _operator: string;

        constructor(root: JQuery, fieldName: string, operator: string = "eq") {
            super();
            this._root = root;
            this._fieldName = fieldName;
            this._operator = operator;
        }

        apply(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void {
            const value = this._root.data("kendoDatePicker").value();
            if ((!exceptions || exceptions.every(x => x !== this._fieldName)) && value) {
                filters.push({
                    field: this._fieldName,
                    operator: this._operator,
                    value: value
                });
            }
        }

        reset(): void {
            this._root.data("kendoDatePicker").value(null);
            this._root.data("kendoDatePicker").trigger("change");
        }

        get control(): JQuery {
            return this._root;
        }

        get kendoControl(): kendo.ui.DatePicker {
            return this._root.data("kendoDatePicker");
        }
    }

    export class StringInputGridFilterMultiField extends GridFilterField {
        private readonly _root: JQuery;
        private readonly _fieldNames: string[];

        constructor(root: JQuery, fieldNames: string[]) {
            super();
            this._root = root;
            this._fieldNames = fieldNames;
        }

        apply(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void {
            const value = this._root.val();

            var innerFilters: kendo.data.DataSourceFilterItem[] = [];
            for (var i = 0; i < this._fieldNames.length; i++) {
                const fieldName = this._fieldNames[i];
                if ((!exceptions || exceptions.every(x => x !== fieldName)) && value) {
                    innerFilters.push({
                        field: fieldName,
                        operator: "contains",
                        value: value
                    });
                }
            }

            if (innerFilters.length) {
                filters.push({
                    logic: "or",
                    filters: innerFilters
                });
            }            
        }

        reset(): void {
            this._root.val("").trigger("change");
        }

        get control(): JQuery {
            return this._root;
        }
    }

    export class IntInputGridFilterField extends GridFilterField {
        private readonly _root: JQuery;
        private readonly _operator: string;
        private readonly _fieldName: string;

        constructor(root: JQuery, fieldName: string, operator: string = 'eq') {
            super();
            this._root = root;
            this._operator = operator;
            this._fieldName = fieldName;
        }

        apply(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void {
            const value = this._root.val();
            const operator = this._operator;

            if ((!exceptions || exceptions.every(x => x !== this._fieldName)) && value) {
                filters.push({
                    field: this._fieldName,
                    operator,
                    value
                });
            }
        }

        reset(): void {
            this._root.val("").trigger("change");         
        }
    }

    export class IntInputGridFilterFieldWithOperator extends GridFilterField {
        private readonly _root: JQuery;
        private readonly _operator: JQuery;
        private readonly _fieldName: string;

        constructor(root: JQuery, operator: JQuery, fieldName: string) {
            super();
            this._root = root;
            this._operator = operator;
            this._fieldName = fieldName;
        }

        apply(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void {
            const value = this._root.val();
            var operator = this._operator.val();
            if (typeof operator !== 'string') {
                throw new Error('Operator must be function');
            }
            operator = this.convertOperatorToFilter(operator);
            if ((!exceptions || exceptions.every(x => x !== this._fieldName)) && value && operator) {
                filters.push({
                    field: this._fieldName,
                    operator,
                    value
                });
            }
        }

        reset(): void {
            this._root.val("").trigger("change");
            this._operator.val("EqualsTo").trigger("change");
        }

        private convertOperatorToFilter(operator: string): string {
            if (operator) {
                switch (operator) {
                    case "EqualsTo":
                        return "eq";
                    case "NotEqualsTo":
                        return "neq";
                    case "GreaterThan":
                        return "gt";
                    case "GreaterThanOrEqualsTo":
                        return "gte";
                    case "LessThan":
                        return "lt";
                    case "LessThanOrEqualsTo":
                        return "lte";
                }
            }

            return undefined;
        }
    }

    export class NumberInputGridFilterField extends GridFilterField {
        private readonly _root: JQuery;
        private readonly _operator: string;
        private readonly _fieldName: string;

        constructor(root: JQuery, fieldName: string, operator: string = "eq") {
            super();
            this._root = root;
            this._operator = operator;
            this._fieldName = fieldName;
        }

        apply(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void {
            const value = this._root.val();
            var operator = this._operator;
            if ((!exceptions || exceptions.every(x => x !== this._fieldName)) && value && operator) {
                filters.push({
                    field: this._fieldName,
                    operator,
                    value
                });
            }
        }

        reset(): void {
            this._root.val("").trigger("change");
        }
    }

    export abstract class GridFilterFieldsCollection {
        private readonly _root: JQuery;

        protected constructor(root: JQuery) {
            this._root = root;
        }

        abstract applyAll(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void;

        abstract resetAll(): void;

        protected get root(): JQuery {
            return this._root;
        }
    }

    export abstract class DynamicGridFilter<TFields extends GridFilterFieldsCollection> {
        private readonly _root: JQuery;
        private _fields: TFields;

        constructor(root: JQuery) {
            this._root = root;
        }

        get root(): JQuery {
            return this._root;
        }

        get fields(): TFields {
            return this._fields;
        }

        initialize(): void {
            this._fields = this.createFields(this.root);

            this.additionalInitialization();
        }

        getFilters(exceptions?: string[]): kendo.data.DataSourceFilterItem[] {
            const filters: kendo.data.DataSourceFilterItem[] = [];
            this._fields.applyAll(filters, exceptions);

            return filters;
        }

        protected abstract createFields(root: JQuery): TFields;

        protected additionalInitialization(): void {
        }

        public apply(grid: kendo.ui.Grid, exceptions?: string[]): void {
            grid.dataSource.filter({ logic: "and", filters: this.getFilters(exceptions) });
            grid.dataSource.read();
        }

        public reset(grid: kendo.ui.Grid): void {
            this._fields.resetAll();
            this.apply(grid);
        }
    }

    export abstract class GridFilterCore<TFields extends GridFilterFieldsCollection> {
        private readonly _root: JQuery;
        private _fields: TFields;

        constructor(root: JQuery) {
            this._root = root;
        }

        get root(): JQuery {
            return this._root;
        }

        get fields(): TFields {
            return this._fields;
        }

        initialize(): void {
            this._fields = this.createFields(this.root);

            this.additionalInitialization();
        }

        getFilters(exceptions?: string[]): kendo.data.DataSourceFilterItem[] {
            if (!this._fields) {
                this.initialize();
            }
            const filters: kendo.data.DataSourceFilterItem[] = [];
            this._fields.applyAll(filters, exceptions);

            return filters;
        }

        protected abstract createFields(root: JQuery): TFields;

        protected additionalInitialization(): void {
        }

        public apply(): void {
            this.applyFilter({ logic: "and", filters: this.getFilters() });
        }

        protected abstract applyFilter(filters: Object);

        public reset(): void {
            this._fields.resetAll();
            this.apply();
        }
    }

    export abstract class GridFilter<TFields extends GridFilterFieldsCollection> {
        private readonly _root: JQuery;
        private _grid: kendo.ui.Grid;
        private _fields: TFields;

        constructor(root: JQuery, grid: kendo.ui.Grid) {
            this._root = root;
            this._grid = grid;
        }

        get root(): JQuery {
            return this._root;
        }

        get grid(): kendo.ui.Grid {
            return this._grid;
        }

        set grid(newGrid: kendo.ui.Grid) {
            this._grid = newGrid;
        }

        get fields(): TFields {
            return this._fields;
        }

        initialize(): void {
            this._fields = this.createFields(this.root);

            this.additionalInitialization();
        }

        getFilters(exceptions?: string[]): kendo.data.DataSourceFilterItem[] {
            const filters: kendo.data.DataSourceFilterItem[] = [];
            this._fields.applyAll(filters, exceptions);

            return filters;
        }

        protected abstract createFields(root: JQuery): TFields;

        protected additionalInitialization(): void {
        }

        public apply(): void {
            this.grid.dataSource.filter({ logic: "and", filters: this.getFilters() });
        }

        public reset(): void {
            this._fields.resetAll();
            this.apply();
        }
    }

    export abstract class GridsFilter<TFields extends GridFilterFieldsCollection, SFields extends GridFilterFieldsCollection> {
        private readonly _root: JQuery;
        private readonly _grids: kendo.ui.Grid[];
        private _fields: (TFields | SFields)[];

        constructor(root: JQuery, grids: kendo.ui.Grid[]) {
            this._root = root;
            this._grids = grids;
        }

        get root(): JQuery {
            return this._root;
        }

        get grids(): kendo.ui.Grid[] {
            return this._grids;
        }

        get fields(): (TFields | SFields)[] {
            return this._fields;
        }

        initialize(): void {
            this._fields = this.createFields(this.root);

            this.additionalInitialization();
        }

        getFilters(n: number, exceptions?: string[]): kendo.data.DataSourceFilterItem[] {
            const filters: kendo.data.DataSourceFilterItem[] = [];
            this._fields[n].applyAll(filters, exceptions);

            return filters;
        }

        protected abstract createFields(root: JQuery): (TFields | SFields)[];

        protected additionalInitialization(): void {
        }

        public apply(): void {
            var grids = this.grids;
            for (var i = 0; i < grids.length; i++) {
                const filters = this.getFilters(i);
                const result = { logic: "and", filters };
                grids[i].dataSource.filter(result);
                this.additionalApply(result, i);
            }            
        }

        protected abstract additionalApply(filters: kendo.data.DataSourceFilters, n: number): void;

        public reset(): void {
            this._fields.forEach(field => field.resetAll());
            this.apply();
        }
    }

}