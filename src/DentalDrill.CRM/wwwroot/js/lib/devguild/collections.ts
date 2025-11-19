namespace DevGuild.Utilities {
    export interface IEqualityComparer<T> {
        areEquals(first: T, second: T): boolean;
    }

    export class EqualityComparer<T> implements IEqualityComparer <T> {
        private readonly _comparer: (first: T, second: T) => boolean;

        constructor(comparer: (first: T, second: T) => boolean) {
            this._comparer = comparer;
        }

        areEquals(first: T, second: T): boolean {
            return this._comparer(first, second);
        }
    }

    export interface ICollection<T> {
        add(item: T): void;
        insert(index: number, item: T): void;
        remove(item: T): boolean;
        removeAt(index: number): void;
        toArray(): T[];
        select<U>(converter: (value: T, index: number) => U): ICollection<U>;
        count(): number;
        count(predicate: (value: T, index: number) => boolean): number;
        sum(): number;
        sum(field: (value: T, index: number) => number): number;
        where(predicate: (value: T, index: number) => boolean): ICollection<T>;

    }

    export class Collection<T> implements ICollection<T> {
        private readonly _items: T[];

        constructor(items?: T[]) {
            this._items = items ? items : [];
        }

        protected get items(): T[] {
            return this._items;
        }

        add(item: T): void {
            this._items.push(item);
        }

        insert(index: number, item: T): void {
            this._items.splice(index, 0, item);
        }

        remove(item: T): boolean {
            const index = this._items.indexOf(item);
            if (index < 0) {
                return false;
            }

            this._items.splice(index, 1);
            return true;
        }

        removeAt(index: number) {
            if (index >= 0 && this._items.length > index) {
                this._items.splice(index, 1);
            }
        }

        toArray(): T[] {
            return this._items;
        }

        select<U>(converter: (value: T, index: number) => U): ICollection<U> {
            return new Collection<U>(this._items.map((value, index) => converter(value, index)));
        }

        count(): number;
        count(predicate: (value: T, index: number) => boolean): number;
        count(predicate?: (value: T, index: number) => boolean): number {
            if (predicate) {
                return this._items.filter((value, index) => predicate(value, index)).length;
            } else {
                return this._items.length;
            }
        }

        sum(): number;
        sum(field: (value: T, index: number) => number): number;
        sum(field?: (value: T, index: number) => number): number {
            if (field) {
                return this._items.reduce((acc, x, index) => acc + field(x, index), 0);
            } else {
                return this._items.reduce((acc, x) => acc + (x as any), 0);
            }
        }

        where(predicate: (value: T, index: number) => boolean): ICollection<T> {
            return new Collection<T>(this._items.filter((value, index) => predicate(value, index)));
        }

        groupBy<TKey>(groupKey: (value: T, index: number) => TKey): GroupedCollection<TKey, T> {
            const groupedCollection = new GroupedCollection<TKey, T>();
            for (let i = 0; i < this._items.length; i++) {
                groupedCollection.getOrCreateGroup(groupKey(this._items[i], i)).items.add(this._items[i]);
            }

            return groupedCollection;
        }
    }

    export interface GroupedCollectionItem<TKey, TItem> {
        key: TKey;
        items: ICollection<TItem>;
    }

    export class GroupedCollection<TKey, TItem> extends Collection<GroupedCollectionItem<TKey, TItem>> {
        private readonly _keyComparer: IEqualityComparer<TKey>;

        constructor(items?: GroupedCollectionItem<TKey, TItem>[], keyComparer?: IEqualityComparer<TKey>) {
            super(items);
            this._keyComparer = keyComparer ? keyComparer : new DefaultGroupKeyComparer<TKey>();
        }

        getGroup(key: TKey): GroupedCollectionItem<TKey, TItem> {
            for (let group of this.items) {
                if (this._keyComparer.areEquals(group.key, key)) {
                    return group;
                }
            }

            return undefined;
        }

        getOrCreateGroup(key: TKey): GroupedCollectionItem<TKey, TItem> {
            for (let group of this.items) {
                if (this._keyComparer.areEquals(group.key, key)) {
                    return group;
                }
            }

            const newGroup: GroupedCollectionItem<TKey, TItem> = {
                key: key,
                items: new Collection<TItem>()
            };

            this.items.push(newGroup);
            return newGroup;
        }
    }

    export class DefaultGroupKeyComparer<TKey> implements IEqualityComparer<TKey> {
        areEquals(first: TKey, second: TKey): boolean {
            return this.objectPropertyEquals(first, second);
        }

        private objectEquals(first: object, second: object): boolean {
            const firstKeys = Object.keys(first);
            const secondKeys = Object.keys(second);

            if (firstKeys.length !== secondKeys.length) {
                return false;
            }

            for (let key of firstKeys) {
                const secondIndex = secondKeys.indexOf(key);
                if (secondIndex < 0) {
                    return false;
                }

                secondKeys.splice(secondIndex, 1);
            }

            if (secondKeys.length > 0) {
                return false;
            }

            for (let key of firstKeys) {
                if (!this.objectPropertyEquals(first[key], second[key])) {
                    return false;
                }
            }

            return true;
        }

        private objectPropertyEquals(first: any, second: any): boolean {
            if (typeof first !== typeof second) {
                return false;
            }

            if (typeof first === "object" && typeof second === "object") {
                if (first === null && second === null) {
                    return true;
                }

                if (first === null || second === null) {
                    return false;
                }

                return this.objectEquals(first as object, second as object);
            }

            return first === second;
        }
    }
}