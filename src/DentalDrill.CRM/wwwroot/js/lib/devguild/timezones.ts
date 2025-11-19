namespace DevGuild.Utilities {
    export class TimeZones {
        static treatAsUtc(date: Date): Date {
            const utcDate = new Date();
            utcDate.setUTCFullYear(date.getFullYear(), date.getMonth(), date.getDate());
            utcDate.setUTCHours(date.getHours(), date.getMinutes(), date.getSeconds(), date.getMilliseconds());
            return utcDate;
        }
    }
}