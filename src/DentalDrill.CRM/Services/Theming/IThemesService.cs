using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace DentalDrill.CRM.Services.Theming
{
    public interface IThemesService
    {
        void SetCurrentTheme(String name);

        Task<String> ResolveResourcePath(String name);

        IReadOnlyList<ThemeDescriptor> GetAvailableThemes();

        ThemeDescriptor FindTheme(String key);

        Task<IHtmlContent> RenderCustomStyles();

        Task<IHtmlContent> RenderCustomCover();
    }
}
