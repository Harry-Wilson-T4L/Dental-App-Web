namespace DevGuild.AspNet.Forms.AjaxForms {
    class EventsHandler {
        private _handlers: ((e: any) => void)[] = [];

        subscribe(handler: (e: any) => void) {
            this._handlers.push(handler);
        }

        fire(payload: any): void {
            for (let handler of this._handlers) {
                handler(payload);
            }
        }
    }

    class EventsManager {
        private _map: object = {};

        subscribe(eventId: string, handler: (e: any) => void): void {
            let handlersCollection = this._map[eventId] as EventsHandler;
            if (!handlersCollection) {
                handlersCollection = new EventsHandler();
                this._map[eventId] = handlersCollection;
            }

            handlersCollection.subscribe(handler);
        }

        fire(eventId: string, payload: any): void {
            const handlersCollection = this._map[eventId] as EventsHandler;
            if (handlersCollection) {
                handlersCollection.fire(payload);
            }
        }
    }

    export class AjaxFormsManager {
        private static _events = new EventsManager();
        private static _htmlEvents = new EventsManager();

        public static get events(): EventsManager {
            return AjaxFormsManager._events;
        }

        public static get htmlEvents(): EventsManager {
            return AjaxFormsManager._htmlEvents;
        }

        static handleComplete(xhr: JQueryXHR, status: string): void {
            if (status === "success") {
                const contentType = xhr.getResponseHeader("content-type");
                if (contentType.match(`^application/json`)) {
                    const deserialized = JSON.parse(xhr.responseText);
                    if (deserialized && deserialized.formId && deserialized.result) {
                        AjaxFormsManager.events.fire(deserialized.formId, deserialized.result);
                    }
                    // TODO: Process response
                } else if (contentType.match(`^text/html`)) {
                    // TODO: Handle HTML response
                    const html = $(xhr.responseText);
                    const formId = html.eq(0).attr("id");
                    if (formId) {
                        const responseText = html.eq(0).attr("data-inline-response");
                        const response = responseText ? JSON.parse(responseText) : undefined;
                        
                        AjaxFormsManager.htmlEvents.fire(formId, response);
                    }
                }
            }
        }

        static waitFor(formId: string): Promise<any> {
            return new Promise<any>((resolve, reject) => {
                AjaxFormsManager.events.subscribe(formId, e => {
                    resolve(e);
                });
            });
        }

        static waitForHtml(formId: string): Promise<any> {
            return new Promise<any>((resolve, reject) => {
                AjaxFormsManager.htmlEvents.subscribe(formId, e => {
                    resolve(e);
                });
            })
        }
    }
}