var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var HandpieceModels;
            (function (HandpieceModels) {
                var Edit;
                (function (Edit) {
                    var GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
                    var AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;
                    var HandpieceModelSchematicType;
                    (function (HandpieceModelSchematicType) {
                        HandpieceModelSchematicType[HandpieceModelSchematicType["Text"] = 0] = "Text";
                        HandpieceModelSchematicType[HandpieceModelSchematicType["Attachment"] = 1] = "Attachment";
                        HandpieceModelSchematicType[HandpieceModelSchematicType["Image"] = 2] = "Image";
                    })(HandpieceModelSchematicType || (HandpieceModelSchematicType = {}));
                    var HandpieceStoreListingStatus;
                    (function (HandpieceStoreListingStatus) {
                        HandpieceStoreListingStatus[HandpieceStoreListingStatus["Listed"] = 0] = "Listed";
                        HandpieceStoreListingStatus[HandpieceStoreListingStatus["Unlisted"] = 1] = "Unlisted";
                        HandpieceStoreListingStatus[HandpieceStoreListingStatus["Deleted"] = 2] = "Deleted";
                        HandpieceStoreListingStatus[HandpieceStoreListingStatus["Requested"] = 3] = "Requested";
                        HandpieceStoreListingStatus[HandpieceStoreListingStatus["Sold"] = 4] = "Sold";
                    })(HandpieceStoreListingStatus || (HandpieceStoreListingStatus = {}));
                    var HandpieceModelSchematicsGrid = /** @class */ (function () {
                        function HandpieceModelSchematicsGrid() {
                        }
                        Object.defineProperty(HandpieceModelSchematicsGrid, "instance", {
                            get: function () {
                                return $("#HandpieceModelSchematicsGrid").data("kendoGrid");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(HandpieceModelSchematicsGrid, "parentId", {
                            get: function () {
                                return HandpieceModelSchematicsGrid.instance.wrapper.attr("data-parent-id");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        HandpieceModelSchematicsGrid.handleMoveUp = function (e) {
                            return __awaiter(this, void 0, void 0, function () {
                                var dataItem;
                                return __generator(this, function (_b) {
                                    switch (_b.label) {
                                        case 0:
                                            e.preventDefault();
                                            dataItem = this.dataItem(e.currentTarget.closest("tr"));
                                            console.log(dataItem);
                                            return [4 /*yield*/, fetch(routes.handpieceModelSchematics.moveUp(dataItem.Id).value, {
                                                    method: "POST",
                                                    credentials: "same-origin",
                                                    headers: {
                                                        "X-Requested-With": "XMLHttpRequest"
                                                    },
                                                    body: ""
                                                })];
                                        case 1:
                                            _b.sent();
                                            return [4 /*yield*/, this.dataSource.read()];
                                        case 2:
                                            _b.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        HandpieceModelSchematicsGrid.handleMoveDown = function (e) {
                            return __awaiter(this, void 0, void 0, function () {
                                var dataItem;
                                return __generator(this, function (_b) {
                                    switch (_b.label) {
                                        case 0:
                                            e.preventDefault();
                                            dataItem = this.dataItem(e.currentTarget.closest("tr"));
                                            console.log(dataItem);
                                            return [4 /*yield*/, fetch(routes.handpieceModelSchematics.moveDown(dataItem.Id).value, {
                                                    method: "POST",
                                                    credentials: "same-origin",
                                                    headers: {
                                                        "X-Requested-With": "XMLHttpRequest"
                                                    },
                                                    body: ""
                                                })];
                                        case 1:
                                            _b.sent();
                                            return [4 /*yield*/, this.dataSource.read()];
                                        case 2:
                                            _b.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        var _a;
                        _a = HandpieceModelSchematicsGrid;
                        HandpieceModelSchematicsGrid.handleCreateText = GridHandlers.createGridButtonClickPopupHandler("#HandpieceModelSchematicsGrid .k-grid-CustomCreateText", function (target) { return routes.handpieceModelSchematics.createText(HandpieceModelSchematicsGrid.parentId); }, function (target) { return ({
                            title: "Add text schematic",
                            width: "1000px",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                e.sender.center();
                            },
                            open: function (e) { return __awaiter(_a, void 0, void 0, function () {
                                return __generator(_a, function (_b) {
                                    switch (_b.label) {
                                        case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("HandpieceModelSchematicCreateText")];
                                        case 1:
                                            _b.sent();
                                            return [4 /*yield*/, HandpieceModelSchematicsGrid.instance.dataSource.read()];
                                        case 2:
                                            _b.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        HandpieceModelSchematicsGrid.handleCreateAttachment = GridHandlers.createGridButtonClickPopupHandler("#HandpieceModelSchematicsGrid .k-grid-CustomCreateAttachment", function (target) { return routes.handpieceModelSchematics.createAttachment(HandpieceModelSchematicsGrid.parentId); }, function (target) { return ({
                            title: "Add pdf schematic",
                            width: "1000px",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                e.sender.center();
                            },
                            open: function (e) { return __awaiter(_a, void 0, void 0, function () {
                                return __generator(_a, function (_b) {
                                    switch (_b.label) {
                                        case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("HandpieceModelSchematicCreateAttachment")];
                                        case 1:
                                            _b.sent();
                                            return [4 /*yield*/, HandpieceModelSchematicsGrid.instance.dataSource.read()];
                                        case 2:
                                            _b.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        HandpieceModelSchematicsGrid.handleCreateImage = GridHandlers.createGridButtonClickPopupHandler("#HandpieceModelSchematicsGrid .k-grid-CustomCreateImage", function (target) { return routes.handpieceModelSchematics.createImage(HandpieceModelSchematicsGrid.parentId); }, function (target) { return ({
                            title: "Add image schematic",
                            width: "1000px",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                e.sender.center();
                            },
                            open: function (e) { return __awaiter(_a, void 0, void 0, function () {
                                return __generator(_a, function (_b) {
                                    switch (_b.label) {
                                        case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("HandpieceModelSchematicCreateImage")];
                                        case 1:
                                            _b.sent();
                                            return [4 /*yield*/, HandpieceModelSchematicsGrid.instance.dataSource.read()];
                                        case 2:
                                            _b.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        HandpieceModelSchematicsGrid.handleDetails = GridHandlers.createButtonClickPopupHandler(function (item) { return routes.handpieceModelSchematics.details(item.Id); }, function (item) { return ({
                            title: "Schematic " + (item.Title ? item.Title : ""),
                            width: "1000px",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                switch (item.Type) {
                                    case HandpieceModelSchematicType.Text:
                                        e.sender.center();
                                        break;
                                    case HandpieceModelSchematicType.Attachment:
                                        e.sender.center();
                                        break;
                                    case HandpieceModelSchematicType.Image:
                                        var images = e.sender.wrapper.find("img");
                                        images.on("load", function (loaded) {
                                            e.sender.center();
                                        });
                                        break;
                                }
                            }
                        }); });
                        HandpieceModelSchematicsGrid.handleEdit = GridHandlers.createButtonClickPopupHandler(function (item) {
                            switch (item.Type) {
                                case HandpieceModelSchematicType.Text:
                                    return routes.handpieceModelSchematics.editText(item.Id);
                                case HandpieceModelSchematicType.Attachment:
                                    return routes.handpieceModelSchematics.editAttachment(item.Id);
                                case HandpieceModelSchematicType.Image:
                                    return routes.handpieceModelSchematics.editImage(item.Id);
                                default:
                                    alert("Invalid schematic type");
                                    throw new Error("Invalid schematic type");
                            }
                        }, function (item) { return ({
                            title: "Edit schematic " + item.Title,
                            width: "1000px",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                e.sender.center();
                            },
                            open: function (e) { return __awaiter(_a, void 0, void 0, function () {
                                var _b;
                                return __generator(_a, function (_c) {
                                    switch (_c.label) {
                                        case 0:
                                            _b = item.Type;
                                            switch (_b) {
                                                case HandpieceModelSchematicType.Text: return [3 /*break*/, 1];
                                                case HandpieceModelSchematicType.Attachment: return [3 /*break*/, 3];
                                                case HandpieceModelSchematicType.Image: return [3 /*break*/, 5];
                                            }
                                            return [3 /*break*/, 7];
                                        case 1: return [4 /*yield*/, AjaxFormsManager.waitFor("HandpieceModelSchematicEditText")];
                                        case 2:
                                            _c.sent();
                                            return [3 /*break*/, 7];
                                        case 3: return [4 /*yield*/, AjaxFormsManager.waitFor("HandpieceModelSchematicEditAttachment")];
                                        case 4:
                                            _c.sent();
                                            return [3 /*break*/, 7];
                                        case 5: return [4 /*yield*/, AjaxFormsManager.waitFor("HandpieceModelSchematicEditImage")];
                                        case 6:
                                            _c.sent();
                                            return [3 /*break*/, 7];
                                        case 7: return [4 /*yield*/, HandpieceModelSchematicsGrid.instance.dataSource.read()];
                                        case 8:
                                            _c.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        HandpieceModelSchematicsGrid.handleDelete = GridHandlers.createButtonClickPopupHandler(function (item) { return routes.handpieceModelSchematics.delete(item.Id); }, function (item) { return ({
                            title: "Delete schematic " + item.Title,
                            width: "1000px",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                e.sender.center();
                            },
                            open: function (e) { return __awaiter(_a, void 0, void 0, function () {
                                return __generator(_a, function (_b) {
                                    switch (_b.label) {
                                        case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("HandpieceModelSchematicDelete")];
                                        case 1:
                                            _b.sent();
                                            return [4 /*yield*/, HandpieceModelSchematicsGrid.instance.dataSource.read()];
                                        case 2:
                                            _b.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        return HandpieceModelSchematicsGrid;
                    }());
                    Edit.HandpieceModelSchematicsGrid = HandpieceModelSchematicsGrid;
                    var HandpieceModelListingsGrid = /** @class */ (function () {
                        function HandpieceModelListingsGrid() {
                        }
                        Object.defineProperty(HandpieceModelListingsGrid, "instance", {
                            get: function () {
                                return $("#HandpieceModelListingsGrid").data("kendoGrid");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(HandpieceModelListingsGrid, "parentId", {
                            get: function () {
                                return HandpieceModelListingsGrid.instance.wrapper.attr("data-parent-id");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        HandpieceModelListingsGrid.formatStatus = function (status) {
                            if (status === undefined || status === null) {
                                return undefined;
                            }
                            switch (status) {
                                case HandpieceStoreListingStatus.Listed: return "Listed";
                                case HandpieceStoreListingStatus.Unlisted: return "Unlisted";
                                case HandpieceStoreListingStatus.Deleted: return "Deleted";
                                case HandpieceStoreListingStatus.Requested: return "Requested";
                                case HandpieceStoreListingStatus.Sold: return "Sold";
                            }
                            return undefined;
                        };
                        var _b;
                        _b = HandpieceModelListingsGrid;
                        HandpieceModelListingsGrid.handleCreate = GridHandlers.createGridButtonClickPopupHandler("#HandpieceModelListingsGrid .k-grid-CustomCreate", function (target) { return routes.handpieceModelListings.create(HandpieceModelListingsGrid.parentId); }, function (target) { return ({
                            title: "Add listing",
                            width: "1000px",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                e.sender.center();
                            },
                            open: function (e) { return __awaiter(_b, void 0, void 0, function () {
                                return __generator(_b, function (_c) {
                                    switch (_c.label) {
                                        case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("HandpieceModelListingCreate")];
                                        case 1:
                                            _c.sent();
                                            return [4 /*yield*/, HandpieceModelListingsGrid.instance.dataSource.read()];
                                        case 2:
                                            _c.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        HandpieceModelListingsGrid.handleEdit = GridHandlers.createButtonClickPopupHandler(function (item) { return routes.handpieceModelListings.edit(item.Id); }, function (item) { return ({
                            title: "Edit listing",
                            width: "1000px",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                e.sender.center();
                            },
                            open: function (e) { return __awaiter(_b, void 0, void 0, function () {
                                return __generator(_b, function (_c) {
                                    switch (_c.label) {
                                        case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("HandpieceModelListingEdit")];
                                        case 1:
                                            _c.sent();
                                            return [4 /*yield*/, HandpieceModelListingsGrid.instance.dataSource.read()];
                                        case 2:
                                            _c.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        HandpieceModelListingsGrid.handleDelete = GridHandlers.createButtonClickPopupHandler(function (item) { return routes.handpieceModelListings.delete(item.Id); }, function (item) { return ({
                            title: "Delete listing",
                            width: "1000px",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                e.sender.center();
                            },
                            open: function (e) { return __awaiter(_b, void 0, void 0, function () {
                                return __generator(_b, function (_c) {
                                    switch (_c.label) {
                                        case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("HandpieceModelListingDelete")];
                                        case 1:
                                            _c.sent();
                                            return [4 /*yield*/, HandpieceModelListingsGrid.instance.dataSource.read()];
                                        case 2:
                                            _c.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        return HandpieceModelListingsGrid;
                    }());
                    Edit.HandpieceModelListingsGrid = HandpieceModelListingsGrid;
                })(Edit = HandpieceModels.Edit || (HandpieceModels.Edit = {}));
            })(HandpieceModels = Pages.HandpieceModels || (Pages.HandpieceModels = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=edit.js.map