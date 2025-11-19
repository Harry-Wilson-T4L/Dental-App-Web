declare namespace kendo.ui {
    interface Grid {
        dataItem<T>(row: string): kendo.data.ObservableObject & T;
        dataItem<T>(row: Element): kendo.data.ObservableObject & T;
        dataItem<T>(row: JQuery): kendo.data.ObservableObject & T;
    }

    interface ListView {
        dataItem<T>(row: string): kendo.data.ObservableObject & T;
        dataItem<T>(row: Element): kendo.data.ObservableObject & T;
        dataItem<T>(row: JQuery): kendo.data.ObservableObject & T;
    }
}

declare namespace kendo.data {
    interface DataSource {
        view<T>(): GenericObservableArray<T>;
        data<T>(): GenericObservableArray<T>;
    }

    interface GenericObservableArray<T> {
        length: number;
        [index: number]: T & ObservableObject;

        constructor(array: T[]);
        init(array: T[]): void;
        empty(): void;
        every(callback: (item: T, index: number, source: GenericObservableArray<T>) => boolean): boolean;
        filter(callback: (item: T, index: number, source: GenericObservableArray<T>) => boolean): any[];
        find(callback: (item: T, index: number, source: GenericObservableArray<T>) => boolean): T;
        forEach(callback: (item: T, index: number, source: GenericObservableArray<T>) => void): void;
        indexOf(item: any): number;
        join(separator: string): string;
        map<TTarget>(callback: (item: T, index: number, source: GenericObservableArray<T>) => TTarget): TTarget[];
        parent(): ObservableObject;
        pop(): ObservableObject;
        push(...items: T[]): number;
        remove(item: T): void;
        shift(): any;
        slice(begin: number, end?: number): T[];
        some(callback: (item: T, index: number, source: GenericObservableArray<T>) => boolean): boolean;
        splice(start: number): T[];
        splice(start: number, deleteCount: number, ...items: T[]): T[];
        toJSON(): any[];
        unshift(...items: T[]): number;
        wrap(object: Object, parent: Object): any;
        wrapAll(source: Object, target: Object): any;
    }
}