namespace DentalDrill.CRM.Pages.HandpieceStore.Listing {
    export class HandpieceStoreListingImagesCarousel {
        private readonly _id: string;
        private readonly _windowWrapper: JQuery;
        private readonly _window: kendo.ui.Window;
        private _firstTime: boolean;
        private _viewPortSize: { width: number, height: number };

        constructor(id: string, firstImage?: string) {
            this._id = id;

            this.updateViewPortSize();
            this._windowWrapper = $("<div></div>");
            this._windowWrapper.kendoWindow({
                width: `${Math.round(this._viewPortSize.width * 0.9)}px`,
                height: `${Math.round(this._viewPortSize.height * 0.9)}px`,
                title: "Handpiece Images",
                actions: ["close"],
                modal: true,
                visible: false,
                content: `/HandpieceStore/ListingImages/${id}${firstImage ? `?image=${firstImage}` : ""}`,
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.center();
                }
            });

            this._window = this._windowWrapper.data("kendoWindow");
            this._firstTime = true;
        }

        show(image?: string): void {
            if (!this._firstTime && this.updateViewPortSize()) {
                this._window.setOptions({
                    width: `${Math.round(this._viewPortSize.width * 0.9)}px`,
                    height: `${Math.round(this._viewPortSize.height * 0.9)}px`,
                });
            }

            this._window.open();
            this._window.center();
            if (this._firstTime) {
                this._firstTime = false;
            } else {
                if (image) {
                    this.selectPage(image);
                }
            }
        }

        handleResize(): void {
            if (this._window.element.closest("html").length === 0 || this._window.element.is(":hidden")) {
                return;
            }

            if (!this._viewPortSize) {
                return;
            }

            const viewPortSize = {
                width: $(window).width(),
                height: $(window).height(),
            };

            if (this._viewPortSize.width === viewPortSize.width && this._viewPortSize.height == viewPortSize.height) {
                return;
            }

            this._viewPortSize = viewPortSize;
            this._window.setOptions({
                width: `${Math.round(this._viewPortSize.width * 0.9)}px`,
                height: `${Math.round(this._viewPortSize.height * 0.9)}px`,
            });
            this._window.center();
            const scrollView = this._window.element.find(".k-scrollview[data-role='scrollview']").data("kendoScrollView");
            if (scrollView) {
                scrollView.resize();
            }
        }

        private updateViewPortSize(): boolean {
            const viewPortSize = {
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
        }

        private selectPage(id: string) {
            const scrollViewWrapper = this._window.wrapper.find(".handpiece-image-carousel");
            if (scrollViewWrapper.length === 0) {
                return;
            }

            const page = scrollViewWrapper.find(`.handpiece-image-carousel__page[data-id='${id}']`);
            if (page.length === 0) {
                return;
            }

            try {
                const pageNo = parseInt(page.attr("data-index"));
                const scrollView = scrollViewWrapper.data("kendoScrollView");
                scrollView.scrollTo(pageNo, false);
            } catch (exception) { }
        }
    }

    export class HandpieceStoreListingImagesCarouselManager {
        private readonly _carousels: Map<string, HandpieceStoreListingImagesCarousel> = new Map<string, HandpieceStoreListingImagesCarousel>();
        
        constructor() {
        }

        showCarousel(id: string, image?: string): void {
            let carousel: HandpieceStoreListingImagesCarousel;
            if (this._carousels.has(id)) {
                carousel = this._carousels.get(id);
            } else {
                carousel = new HandpieceStoreListingImagesCarousel(id, image);
                this._carousels.set(id, carousel);
            }

            carousel.show(image);
        }

        handleResize(): void {
            this._carousels.forEach((value, key) => {
                value.handleResize();
            });
        }
    }

    $(() => {
        const carouselManager = new HandpieceStoreListingImagesCarouselManager();
        $(document).on("click", ".handpiece-image-show-carousel", e => {
            const target = $(e.target);
            const id = target.attr("data-id");
            const image = target.attr("data-image");
            carouselManager.showCarousel(id, image);
        });

        $(window).on("resize", e => {
            carouselManager.handleResize();
        });
    });
} 