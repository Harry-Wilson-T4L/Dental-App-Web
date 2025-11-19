/// <reference path="query-string-item.ts" />

namespace DevGuild.Utilities {
    export class QueryString {
        private readonly _items: QueryStringItem[];

        constructor() {
            this._items = [];
        }

        static parse(query: string): QueryString {
            const result = new QueryString();
            if (!query || query.length <= 1) {
                return result;
            }

            const params = query.substring(1).split("&");
            for (let i = 0; i < params.length; i++) {
                const pair = params[i].split("=");
                const item = new QueryStringItem(decodeURIComponent(pair[0]), decodeURIComponent(pair[1]));
                result._items.push(item);
            }

            return result;
        }

        build(): string {
            let result = "";
            for (let item of this._items) {
                if (item.key && item.value) {
                    result += `&${encodeURIComponent(item.key)}=${encodeURIComponent(item.value)}`;
                }
            }

            return result === "" ? result : `?${result.substring(1)}`;
        }

        get(key: string): string {
            const item = this.find(key);
            return item ? item.value : undefined;
        }

        set(key: string, value: string): QueryString {
            let item = this.find(key);
            if (item) {
                item.value = value;
            } else {
                item = new QueryStringItem(key, value);
                this._items.push(item);
            }

            return this;
        }

        private find(key: string): QueryStringItem {
            for (let item of this._items) {
                if (item.key.toLowerCase() === key.toLowerCase()) {
                    return item;
                }
            }

            return undefined;
        }
    }
}