namespace DentalDrill.CRM.Controls {
    export class ClientSelectorControl {
        static async valueMapper(options: any): Promise<void> {
            const ids: string[] = [];
            if (typeof options.value === "string") {
                ids.push(options.value);
            } else if (typeof options.value === "object" && Array.isArray(options.value)) {
                for (let i = 0; i < (options.value as string[]).length; i++) {
                    ids.push(options.value[i]);
                }
            }

            const formData = new FormData();
            for (let id of ids) {
                formData.append("ids", id);
            }

            const response = await $.ajax({
                url: "/Jobs/ClientsResolve",
                method: "POST",
                data: { "ids": ids } as JQuery.PlainObject
            });

            options.success(response);
        }
    }
}