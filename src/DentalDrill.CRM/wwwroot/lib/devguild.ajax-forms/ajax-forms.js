var DevGuild;
(function (DevGuild) {
    var AspNet;
    (function (AspNet) {
        var Forms;
        (function (Forms) {
            var AjaxForms;
            (function (AjaxForms) {
                var EventsHandler = /** @class */ (function () {
                    function EventsHandler() {
                        this._handlers = [];
                    }
                    EventsHandler.prototype.subscribe = function (handler) {
                        this._handlers.push(handler);
                    };
                    EventsHandler.prototype.fire = function (payload) {
                        for (var _i = 0, _a = this._handlers; _i < _a.length; _i++) {
                            var handler = _a[_i];
                            handler(payload);
                        }
                    };
                    return EventsHandler;
                }());
                var EventsManager = /** @class */ (function () {
                    function EventsManager() {
                        this._map = {};
                    }
                    EventsManager.prototype.subscribe = function (eventId, handler) {
                        var handlersCollection = this._map[eventId];
                        if (!handlersCollection) {
                            handlersCollection = new EventsHandler();
                            this._map[eventId] = handlersCollection;
                        }
                        handlersCollection.subscribe(handler);
                    };
                    EventsManager.prototype.fire = function (eventId, payload) {
                        var handlersCollection = this._map[eventId];
                        if (handlersCollection) {
                            handlersCollection.fire(payload);
                        }
                    };
                    return EventsManager;
                }());
                var AjaxFormsManager = /** @class */ (function () {
                    function AjaxFormsManager() {
                    }
                    Object.defineProperty(AjaxFormsManager, "events", {
                        get: function () {
                            return AjaxFormsManager._events;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(AjaxFormsManager, "htmlEvents", {
                        get: function () {
                            return AjaxFormsManager._htmlEvents;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    AjaxFormsManager.handleComplete = function (xhr, status) {
                        if (status === "success") {
                            var contentType = xhr.getResponseHeader("content-type");
                            if (contentType.match("^application/json")) {
                                var deserialized = JSON.parse(xhr.responseText);
                                if (deserialized && deserialized.formId && deserialized.result) {
                                    AjaxFormsManager.events.fire(deserialized.formId, deserialized.result);
                                }
                                // TODO: Process response
                            }
                            else if (contentType.match("^text/html")) {
                                // TODO: Handle HTML response
                                var html = $(xhr.responseText);
                                var formId = html.eq(0).attr("id");
                                if (formId) {
                                    var responseText = html.eq(0).attr("data-inline-response");
                                    var response = responseText ? JSON.parse(responseText) : undefined;
                                    AjaxFormsManager.htmlEvents.fire(formId, response);
                                }
                            }
                        }
                    };
                    AjaxFormsManager.waitFor = function (formId) {
                        return new Promise(function (resolve, reject) {
                            AjaxFormsManager.events.subscribe(formId, function (e) {
                                resolve(e);
                            });
                        });
                    };
                    AjaxFormsManager.waitForHtml = function (formId) {
                        return new Promise(function (resolve, reject) {
                            AjaxFormsManager.htmlEvents.subscribe(formId, function (e) {
                                resolve(e);
                            });
                        });
                    };
                    AjaxFormsManager._events = new EventsManager();
                    AjaxFormsManager._htmlEvents = new EventsManager();
                    return AjaxFormsManager;
                }());
                AjaxForms.AjaxFormsManager = AjaxFormsManager;
            })(AjaxForms = Forms.AjaxForms || (Forms.AjaxForms = {}));
        })(Forms = AspNet.Forms || (AspNet.Forms = {}));
    })(AspNet = DevGuild.AspNet || (DevGuild.AspNet = {}));
})(DevGuild || (DevGuild = {}));
//# sourceMappingURL=ajax-forms.js.map