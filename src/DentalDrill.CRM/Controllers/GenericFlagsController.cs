using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels.GenericFlags;
using DentalDrill.CRM.Services.GenericFlags;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Administrator)]
    public class GenericFlagsController : Controller
    {
        private readonly GenericFlagsService genericFlagsService;
        private readonly IRepository repository;

        public GenericFlagsController(GenericFlagsService genericFlagsService, IRepository repository)
        {
            this.genericFlagsService = genericFlagsService;
            this.repository = repository;
        }

        [AjaxPost]
        public async Task<IActionResult> GetValue([FromQuery] String flagId)
        {
            var state = await this.genericFlagsService.GetFlagStateAsync(flagId);
            return this.Json(new GenericFlagResponseModel
            {
                Flag = flagId,
                State = state,
            });
        }

        [AjaxPost]
        public async Task<IActionResult> SetValue([FromQuery] String flagId, [FromQuery] GenericFlagState state)
        {
            await this.genericFlagsService.SetFlagStateAsync(flagId, state);
            await this.repository.SaveChangesAsync();
            return this.Json(new GenericFlagResponseModel
            {
                Flag = flagId,
                State = state,
            });
        }

        [AjaxPost]
        public async Task<IActionResult> BulkGetValue([FromBody] GenericFlagBulkGetRequestModel model)
        {
            var response = new GenericFlagBulkGetResponseModel
            {
                States = new Dictionary<String, GenericFlagState>(),
            };

            if (model?.Flags != null)
            {
                foreach (var flag in model.Flags)
                {
                    response.States[flag] = await this.genericFlagsService.GetFlagStateAsync(flag);
                }
            }

            return this.Json(response);
        }
    }
}
