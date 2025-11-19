namespace DevGuild.Utilities {
    export class QueryStringItem {
        key: string;
        value: string;

        constructor(key: string, value: string) {
            this.key = key;
            this.value = value;
        }
    }
}