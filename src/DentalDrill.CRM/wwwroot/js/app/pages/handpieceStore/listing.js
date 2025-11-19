var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var HandpieceStore;
            (function (HandpieceStore) {
                var Listing;
                (function (Listing) {
                    var HandpieceStoreListingImagesCarousel = /** @class */ (function () {
                        function HandpieceStoreListingImagesCarousel(id, firstImage) {
                            this._id = id;
                            this.updateViewPortSize();
                            this._windowWrapper = $("<div></div>");
                            this._windowWrapper.kendoWindow({
                                width: Math.round(this._viewPortSize.width * 0.9) + "px",
                                height: Math.round(this._viewPortSize.height * 0.9) + "px",
                                title: "Handpiece Images",
                                actions: ["close"],
                                modal: true,
                                visible: false,
                                content: "/HandpieceStore/ListingImages/" + id + (firstImage ? "?image=" + firstImage : ""),
                                refresh: function (e) {
                                    e.sender.center();
                                }
                            });
                            this._window = this._windowWrapper.data("kendoWindow");
                            this._firstTime = true;
                        }
                        HandpieceStoreListingImagesCarousel.prototype.show = function (image) {
                            if (!this._firstTime && this.updateViewPortSize()) {
                                this._window.setOptions({
                                    width: Math.round(this._viewPortSize.width * 0.9) + "px",
                                    height: Math.round(this._viewPortSize.height * 0.9) + "px",
                                });
                            }
                            this._window.open();
                            this._window.center();
                            if (this._firstTime) {
                                this._firstTime = false;
                            }
                            else {
                                if (image) {
                                    this.selectPage(image);
                                }
                            }
                        };
                        HandpieceStoreListingImagesCarousel.prototype.handleResize = function () {
                            if (this._window.element.closest("html").length === 0 || this._window.element.is(":hidden")) {
                                return;
                            }
                            if (!this._viewPortSize) {
                                return;
                            }
                            var viewPortSize = {
                                width: $(window).width(),
                                height: $(window).height(),
                            };
                            if (this._viewPortSize.width === viewPortSize.width && this._viewPortSize.height == viewPortSize.height) {
                                return;
                            }
                            this._viewPortSize = viewPortSize;
                            this._window.setOptions({
                                width: Math.round(this._viewPortSize.width * 0.9) + "px",
                                height: Math.round(this._viewPortSize.height * 0.9) + "px",
                            });
                            this._window.center();
                            var scrollView = this._window.element.find(".k-scrollview[data-role='scrollview']").data("kendoScrollView");
                            if (scrollView) {
                                scrollView.resize();
                            }
                        };
                        HandpieceStoreListingImagesCarousel.prototype.updateViewPortSize = function () {
                            var viewPortSize = {
                                width: $(window).width(),
                                height: $(window).height(),
                            };
                            if (!this._viewPortSize) {
                                this._viewPortSize = viewPortSize;
                                return true;
                            }
                            if (this._viewPortSize.width === viewPortSize.width && this._viewPortSize.height === viewPortSize.height) {
                                return false;
                            }
                            this._viewPortSize = viewPortSize;
                            return true;
                        };
                        HandpieceStoreListingImagesCarousel.prototype.selectPage = function (id) {
                            var scrollViewWrapper = this._window.wrapper.find(".handpiece-image-carousel");
                            if (scrollViewWrapper.length === 0) {
                                return;
                            }
                            var page = scrollViewWrapper.find(".handpiece-image-carousel__page[data-id='" + id + "']");
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
                        return HandpieceStoreListingImagesCarousel;
                    }());
                    Listing.HandpieceStoreListingImagesCarousel = HandpieceStoreListingImagesCarousel;
                    var HandpieceStoreListingImagesCarouselManager = /** @class */ (function () {
                        function HandpieceStoreListingImagesCarouselManager() {
                            this._carousels = new Map();
                        }
                        HandpieceStoreListingImagesCarouselManager.prototype.showCarousel = function (id, image) {
                            var carousel;
                            if (this._carousels.has(id)) {
                                carousel = this._carousels.get(id);
                            }
                            else {
                                carousel = new HandpieceStoreListingImagesCarousel(id, image);
                                this._carousels.set(id, carousel);
                            }
                            carousel.show(image);
                        };
                        HandpieceStoreListingImagesCarouselManager.prototype.handleResize = function () {
                            this._carousels.forEach(function (value, key) {
                                value.handleResize();
                            });
                        };
                        return HandpieceStoreListingImagesCarouselManager;
                    }());
                    Listing.HandpieceStoreListingImagesCarouselManager = HandpieceStoreListingImagesCarouselManager;
                    $(function () {
                        var carouselManager = new HandpieceStoreListingImagesCarouselManager();
                        $(document).on("click", ".handpiece-image-show-carousel", function (e) {
                            var target = $(e.target);
                            var id = target.attr("data-id");
                            var image = target.attr("data-image");
                            carouselManager.showCarousel(id, image);
                        });
                        $(window).on("resize", function (e) {
                            carouselManager.handleResize();
                        });
                    });
                })(Listing = HandpieceStore.Listing || (HandpieceStore.Listing = {}));
            })(HandpieceStore = Pages.HandpieceStore || (Pages.HandpieceStore = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=listing.js.map