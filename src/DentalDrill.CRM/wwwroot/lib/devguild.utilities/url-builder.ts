namespace DevGuild.Utilities {
    export class UriBuilder {
        private _path: string;
        private _query: QueryString;

        constructor(uri: string) {
            const regex = /^([^?]+)\??(.*)$/;
            const match = regex.exec(uri);
            if (!match) {
                throw new Error("Invalid uri");
            }

            this._path = match[1];
            this._query = match[2] ? QueryString.parse(match[2]) : new QueryString();
        }

        get path(): string {
            return this._path;
        }

        set path(value: string) {
            this._path = value;
        }

        get query(): QueryString {
            return this._query;
        }

        get value(): string {
            return this._path + this._query.build();
        }
    }
}