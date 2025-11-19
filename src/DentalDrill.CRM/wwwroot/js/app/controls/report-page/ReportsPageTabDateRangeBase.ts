namespace DentalDrill.CRM.Controls.Reporting {
    export abstract class ReportsPageTabDateRangeBase<TGlobalFilters extends ReportsPageDateRangeGlobalFilters> extends ReportsPageTabBase<TGlobalFilters> {
        protected generateDateRange(from: Date, to: Date, type: string): string[] {
            function addMonth(date: Date, numberOfMonths: number): Date {
                const copy = new Date(date.getTime());
                copy.setMonth(copy.getMonth() + numberOfMonths);
                return copy;
            }

            function getWeekStart(date: Date): Date {
                const copy = new Date(date.getTime());
                copy.setDate(copy.getDate() - copy.getDay());
                return copy;
            }

            function getWeek(date: Date): number {
                const jan1 = new Date(date.getFullYear(), 0, 1);
                const jan1WeekStart = getWeekStart(jan1);
                const dateWeekStart = getWeekStart(date);

                const passedTime = dateWeekStart.getTime() - jan1WeekStart.getTime();
                const passedWeeks = Math.floor(passedTime / (7 * 24 * 60 * 60 * 1000));
                return passedWeeks + 1;
            }

            function addWeek(date: Date, numberOfWeeks: number): Date {
                const copy = new Date(date.getTime());
                copy.setDate(copy.getDate() + 7 * numberOfWeeks);
                return copy;
            }

            function addDay(date: Date, numberOfDays: number): Date {
                const copy = new Date(date.getTime());
                copy.setDate(copy.getDate() + numberOfDays);
                return copy;
            }

            function numPad(val: number, digits: number): string {
                let result = val.toString();
                while (result.length < digits) {
                    result = `0${result}`;
                }

                return result;
            }

            switch (type) {
                case "Yearly":
                    {
                        const start = from.getFullYear();
                        const end = to.getFullYear();
                        const result: string[] = [];

                        for (let current = start; current <= end; current++) {
                            result.push(current.toString());
                        }

                        return result;
                    }
                case "Quarterly":
                    {
                        const start = new Date(from.getFullYear(), Math.floor(from.getMonth() / 3) * 3, 1);
                        const end = to;
                        const result: string[] = [];

                        for (let current = start; current <= end; current = addMonth(current, 3)) {
                            result.push(`${current.getFullYear()}-Q${Math.floor(current.getMonth() / 3) + 1}`);
                        }

                        return result;
                    }
                case "Monthly":
                    {
                        const start = new Date(from.getFullYear(), from.getMonth(), 1);
                        const end = to;
                        const result: string[] = [];

                        for (let current = start; current <= end; current = addMonth(current, 1)) {
                            result.push(`${current.getFullYear()}-${current.getMonth() + 1}`);
                        }

                        return result;
                    }
                case "Weekly":
                    {
                        const start = getWeekStart(from);
                        const end = to;
                        const result: string[] = [];

                        for (let current = start, week = getWeek(start), previous = current; current <= end; current = addWeek(current, 1), week++) {
                            if (previous.getFullYear() !== current.getFullYear()) {
                                week = 1;
                                previous = current;
                            }
                            result.push(`${current.getFullYear()}-W${week}`);
                        }

                        return result;
                    }
                case "Daily":
                    {
                        const start = from;
                        const end = to;
                        const result: string[] = [];

                        for (let current = start; current <= end; current = addDay(current, 1)) {
                            result.push(`${current.getFullYear()}-${numPad(current.getMonth() + 1, 2)}-${numPad(current.getDate(), 2)}`);
                        }

                        return result;
                    }
            }
        }
    }
}