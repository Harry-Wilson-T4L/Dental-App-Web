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
            var HandpieceImages;
            (function (HandpieceImages) {
                var Index;
                (function (Index) {
                    var GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
                    var HandpieceImagesGrid = /** @class */ (function () {
                        function HandpieceImagesGrid() {
                        }
                        Object.defineProperty(HandpieceImagesGrid, "instance", {
                            get: function () {
                                return $("#HandpieceImagesGrid").data("kendoGrid");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        HandpieceImagesGrid.handleCreate = GridHandlers.createGridButtonClickPopupHandler("#HandpieceImagesGrid .k-grid-CustomCreate", function (target) { return new DevGuild.AspNet.Routing.Uri(target.attr("href")); }, function (target) { return ({
                            title: "Attach image",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.center();
                            },
                            open: function () {
                                return __awaiter(this, void 0, void 0, function () {
                                    return __generator(this, function (_a) {
                                        switch (_a.label) {
                                            case 0: return [4 /*yield*/, DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("CreateImage")];
                                            case 1:
                                                _a.sent();
                                                return [4 /*yield*/, HandpieceImagesGrid.instance.dataSource.read()];
                                            case 2:
                                                _a.sent();
                                                this.close();
                                                this.destroy();
                                                return [2 /*return*/];
                                        }
                                    });
                                });
                            }
                        }); });
                        HandpieceImagesGrid.handleCreateVideo = GridHandlers.createGridButtonClickPopupHandler("#HandpieceImagesGrid .k-grid-CustomCreateVideo", function (target) { return new DevGuild.AspNet.Routing.Uri(target.attr("href")); }, function (target) { return ({
                            title: "Attach video",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.center();
                            },
                            open: function () {
                                return __awaiter(this, void 0, void 0, function () {
                                    return __generator(this, function (_a) {
                                        switch (_a.label) {
                                            case 0: return [4 /*yield*/, DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("CreateVideo")];
                                            case 1:
                                                _a.sent();
                                                return [4 /*yield*/, HandpieceImagesGrid.instance.dataSource.read()];
                                            case 2:
                                                _a.sent();
                                                this.close();
                                                this.destroy();
                                                return [2 /*return*/];
                                        }
                                    });
                                });
                            }
                        }); });
                        HandpieceImagesGrid.handleBatchCreate = GridHandlers.createGridButtonClickPopupHandler("#HandpieceImagesGrid .k-grid-CustomBatchCreate", function (target) { return new DevGuild.AspNet.Routing.Uri(target.attr("href")); }, function (target) { return ({
                            title: "Attach multiple images",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.center();
                                var uploader = e.sender.wrapper.find("input.image-uploader-multiple__upload").data("kendoUpload");
                                uploader.bind("success", function (success) {
                                    e.sender.center();
                                });
                            },
                            open: function () {
                                return __awaiter(this, void 0, void 0, function () {
                                    return __generator(this, function (_a) {
                                        switch (_a.label) {
                                            case 0: return [4 /*yield*/, DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("HandpieceImageBatchCreate")];
                                            case 1:
                                                _a.sent();
                                                return [4 /*yield*/, HandpieceImagesGrid.instance.dataSource.read()];
                                            case 2:
                                                _a.sent();
                                                this.close();
                                                this.destroy();
                                                return [2 /*return*/];
                                        }
                                    });
                                });
                            }
                        }); });
                        return HandpieceImagesGrid;
                    }());
                    Index.HandpieceImagesGrid = HandpieceImagesGrid;
                    var HandpieceImagesPreviewManager = /** @class */ (function () {
                        function HandpieceImagesPreviewManager() {
                        }
                        HandpieceImagesPreviewManager.prototype.handleImageClick = function (handpieceId, imageId) {
                            if (this.initWindow(handpieceId, imageId)) {
                                this._window.open();
                                this._window.center();
                                this.selectPage(imageId);
                            }
                            else {
                                this._window.open();
                                this._window.center();
                            }
                        };
                        HandpieceImagesPreviewManager.prototype.initWindow = function (handpieceId, imageId) {
                            if (this._window) {
                                if (this.updateViewPortSize()) {
                                    this._window.setOptions({
                                        width: Math.round(this._viewPortSize.width * 0.9) + "px",
                                        height: Math.round(this._viewPortSize.height * 0.9) + "px",
                                    });
                                }
                                return true;
                            }
                            this.updateViewPortSize();
                            var wrapper = this.getWindowWrapper();
                            wrapper.kendoWindow({
                                width: Math.round(this._viewPortSize.width * 0.9) + "px",
                                height: Math.round(this._viewPortSize.height * 0.9) + "px",
                                title: "Handpiece Images",
                                actions: ["close"],
                                modal: true,
                                visible: false,
                                content: "/HandpieceImages/Preview?parentId=" + handpieceId + "&selected=" + imageId,
                                refresh: function (e) {
                                    e.sender.center();
                                }
                            });
                            this._window = wrapper.data("kendoWindow");
                            return false;
                        };
                        HandpieceImagesPreviewManager.prototype.updateViewPortSize = function () {
                            var viewPortSize = {
                                width: $(window).width(),
                                height: $(window).height(),
                            };
                            if (!this._viewPortSize) {
                                this._viewPortSize = viewPortSize;
                                return true;
                            }
                            if (this._viewPortSize.width === viewPortSize.width && this._viewPortSize.height == viewPortSize.height) {
                                return false;
                            }
                            this._viewPortSize = viewPortSize;
                            return true;
                        };
                        HandpieceImagesPreviewManager.prototype.getWindowWrapper = function () {
                            if (!this._windowWrapper) {
                                this._windowWrapper = $("<div></div>").addClass("handpiece-images-preview-window");
                            }
                            return this._windowWrapper;
                        };
                        HandpieceImagesPreviewManager.prototype.selectPage = function (pageId) {
                            var scrollViewWrapper = this._window.wrapper.find(".handpiece-images-scrollview");
                            if (scrollViewWrapper.length === 0) {
                                return;
                            }
                            var page = scrollViewWrapper.find(".handpiece-images-preview-page[data-id='" + pageId + "']");
                            if (page.length === 0) {
                                return;
                            }
                            try {
                                var pageNo = parseInt(page.attr("data-index"));
                                var scrollView = scrollViewWrapper.data("kendoScrollView");
                                scrollView.scrollTo(pageNo, false);
                            }
                            catch (exception) { }
                        };
                        return HandpieceImagesPreviewManager;
                    }());
                    Index.HandpieceImagesPreviewManager = HandpieceImagesPreviewManager;
                    $(function () {
                        var previewManager = new HandpieceImagesPreviewManager();
                        $(document).on("click", ".handpiece-thumbnail", function (e) {
                            e.preventDefault();
                            var target = $(e.target);
                            var handpieceId = target.attr("data-handpiece-id");
                            var imageId = target.attr("data-image-id");
                            previewManager.handleImageClick(handpieceId, imageId);
                        });
                    });
                })(Index = HandpieceImages.Index || (HandpieceImages.Index = {}));
            })(HandpieceImages = Pages.HandpieceImages || (Pages.HandpieceImages = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=index.js.map