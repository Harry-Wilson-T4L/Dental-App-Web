namespace DentalDrill.CRM.Pages.HandpieceImages.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;

    export class HandpieceImagesGrid {
        static get instance(): kendo.ui.Grid {
            return $("#HandpieceImagesGrid").data("kendoGrid");
        }

        static handleCreate = GridHandlers.createGridButtonClickPopupHandler(
            "#HandpieceImagesGrid .k-grid-CustomCreate",
            target => new DevGuild.AspNet.Routing.Uri(target.attr("href")),
            target => ({
                title: "Attach image",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.center();
                },
                open: async function (this: kendo.ui.Window) {
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("CreateImage");
                    await HandpieceImagesGrid.instance.dataSource.read();
                    this.close();
                    this.destroy();
                }
            }));

        static handleCreateVideo = GridHandlers.createGridButtonClickPopupHandler(
            "#HandpieceImagesGrid .k-grid-CustomCreateVideo",
            target => new DevGuild.AspNet.Routing.Uri(target.attr("href")),
            target => ({
                title: "Attach video",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.center();
                },
                open: async function (this: kendo.ui.Window) {
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("CreateVideo");
                    await HandpieceImagesGrid.instance.dataSource.read();
                    this.close();
                    this.destroy();
                }
            }));

        static handleBatchCreate = GridHandlers.createGridButtonClickPopupHandler(
            "#HandpieceImagesGrid .k-grid-CustomBatchCreate",
            target => new DevGuild.AspNet.Routing.Uri(target.attr("href")),
            target => ({
                title: "Attach multiple images",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.center();
                    const uploader = e.sender.wrapper.find("input.image-uploader-multiple__upload").data("kendoUpload");
                    uploader.bind("success", (success: kendo.ui.UploadSuccessEvent) => {
                        e.sender.center();
                    });
                },
                open: async function (this: kendo.ui.Window) {
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("HandpieceImageBatchCreate");
                    await HandpieceImagesGrid.instance.dataSource.read();
                    this.close();
                    this.destroy();
                }
            }));
    }

    export class HandpieceImagesPreviewManager {
        private _windowWrapper: JQuery;
        private _window: kendo.ui.Window;
        private _viewPortSize: { width: number, height: number };

        handleImageClick(handpieceId: string, imageId: string) {
            if (this.initWindow(handpieceId, imageId)) {
                this._window.open();
                this._window.center();
                this.selectPage(imageId);
            } else {
                this._window.open();
                this._window.center();
            }
        }

        private initWindow(handpieceId: string, imageId: string) {
            if (this._window) {
                if (this.updateViewPortSize()) {
                    this._window.setOptions({
                        width: `${Math.round(this._viewPortSize.width * 0.9)}px`,
                        height: `${Math.round(this._viewPortSize.height * 0.9)}px`,
                    });
                }

                return true;
            }

            this.updateViewPortSize();
            const wrapper = this.getWindowWrapper();
            wrapper.kendoWindow({
                width: `${Math.round(this._viewPortSize.width * 0.9)}px`,
                height: `${Math.round(this._viewPortSize.height * 0.9)}px`,
                title: "Handpiece Images",
                actions: ["close"],
                modal: true,
                visible: false,
                content: `/HandpieceImages/Preview?parentId=${handpieceId}&selected=${imageId}`,
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.center();
                }
            });

            this._window = wrapper.data("kendoWindow");
            return false;
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

            if (this._viewPortSize.width === viewPortSize.width && this._viewPortSize.height == viewPortSize.height) {
                return false;
            }

            this._viewPortSize = viewPortSize;
            return true;
        }

        private getWindowWrapper(): JQuery {
            if (!this._windowWrapper) {
                this._windowWrapper = $("<div></div>").addClass("handpiece-images-preview-window");
            }

            return this._windowWrapper;
        }

        private selectPage(pageId: string) {
            const scrollViewWrapper = this._window.wrapper.find(".handpiece-images-scrollview");
            if (scrollViewWrapper.length === 0) {
                return;
            }

            const page = scrollViewWrapper.find(`.handpiece-images-preview-page[data-id='${pageId}']`);
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

    $(() => {
        const previewManager = new HandpieceImagesPreviewManager();
        $(document).on("click", ".handpiece-thumbnail", e => {
            e.preventDefault();
            const target = $(e.target);
            const handpieceId = target.attr("data-handpiece-id");
            const imageId = target.attr("data-image-id");
            previewManager.handleImageClick(handpieceId, imageId);
        });
    });
}