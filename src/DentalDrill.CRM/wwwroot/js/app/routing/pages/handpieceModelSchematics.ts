namespace DentalDrill.CRM.Routing.Pages {
    export class HandpieceModelSchematicsControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/HandpieceModelSchematics/");
        }
        
        createText(parentId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`CreateText?parentId=${parentId}`);
        }

        createAttachment(parentId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`CreateAttachment?parentId=${parentId}`);
        }

        createImage(parentId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`CreateImage?parentId=${parentId}`);
        }

        read(parentId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Read?parentId=${parentId}`);
        }

        details(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Details/${id}`);
        }

        editText(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`EditText/${id}`);
        }

        editAttachment(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`EditAttachment/${id}`);
        }

        editImage(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`EditImage/${id}`);
        }

        delete(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Delete/${id}`);
        }

        moveUp(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`MoveUp/${id}`);
        }

        moveDown(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`MoveDown/${id}`);
        }

        downloadAttachment(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`DownloadAttachment/${id}`);
        }
    }
}