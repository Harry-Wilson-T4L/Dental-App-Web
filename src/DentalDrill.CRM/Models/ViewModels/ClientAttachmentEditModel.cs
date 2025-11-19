using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.ObjectModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ClientAttachmentEditModel : IEditModelOriginalEntity<ClientAttachment>
    {
        [BindNever]
        public ClientAttachment Original { get; set; }

        public String Comment { get; set; }
    }
}
