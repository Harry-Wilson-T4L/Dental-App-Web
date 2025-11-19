using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Storage;
using DevGuild.AspNetCore.Services.Uploads.Files.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DentalDrill.CRM.Services.Theming
{
    public class ThemesService : IThemesService
    {
        private static readonly IDictionary<String, String> DefaultThemeResources = ThemesService.BuildDefaultThemeResources();
        private static readonly IDictionary<String, String> DarkThemeResources = ThemesService.BuildDarkThemeResources();
        private static readonly IDictionary<String, String> Light2ThemeResources = ThemesService.BuildLight2ThemeResources();

        private static readonly List<ThemeDescriptor> AvailableThemes = new List<ThemeDescriptor>
        {
            new ThemeDescriptor("light2", "New Light Theme"),
            new ThemeDescriptor("dark", "Dark Theme"),
            new ThemeDescriptor("light", "Default Light Theme"),
        };

        private readonly ThemesOptions options;
        private readonly UserEntityResolver userResolver;
        private readonly IRepository repository;
        private readonly IStorageHub storageHub;

        private String currentTheme = "Default";

        public ThemesService(IOptions<ThemesOptions> options, UserEntityResolver userResolver, IRepository repository, IStorageHub storageHub)
        {
            this.userResolver = userResolver;
            this.repository = repository;
            this.storageHub = storageHub;
            this.options = options.Value;
        }

        public IReadOnlyList<ThemeDescriptor> GetAvailableThemes()
        {
            return ThemesService.AvailableThemes;
        }

        public ThemeDescriptor FindTheme(String key)
        {
            return ThemesService.AvailableThemes.SingleOrDefault(x => x.Key == key)
                ?? ThemesService.AvailableThemes.SingleOrDefault(x => x.Key == "light");
        }

        public void SetCurrentTheme(String name)
        {
            this.currentTheme = name;
        }

        public async Task<String> ResolveResourcePath(String name)
        {
            switch (this.currentTheme)
            {
                case "Dark":
                    var currentUser = await this.userResolver.ResolveCurrentUserEntity();
                    if (currentUser is Employee employee)
                    {
                        switch (employee.AppearanceTheme)
                        {
                            case "light2":
                                return ThemesService.Light2ThemeResources[name];
                            case "dark":
                                return ThemesService.DarkThemeResources[name];
                            case "light":
                                return ThemesService.DefaultThemeResources[name];
                        }

                        switch (this.options.StaffTheme)
                        {
                            case "light2":
                                return ThemesService.Light2ThemeResources[name];
                            case "dark":
                                return ThemesService.DarkThemeResources[name];
                            case "light":
                                return ThemesService.DefaultThemeResources[name];
                        }
                    }

                    if (currentUser is Administrator)
                    {
                        switch (this.options.StaffTheme)
                        {
                            case "light2":
                                return ThemesService.Light2ThemeResources[name];
                            case "dark":
                                return ThemesService.DarkThemeResources[name];
                            case "light":
                                return ThemesService.DefaultThemeResources[name];
                        }
                    }

                    return ThemesService.DefaultThemeResources[name];

                case "Default":
                    return ThemesService.DefaultThemeResources[name];
                default:
                    throw new ArgumentException("Invalid theme");
            }
        }

        public async Task<IHtmlContent> RenderCustomStyles()
        {
            if (this.currentTheme == "Dark")
            {
                var currentUser = await this.userResolver.ResolveCurrentUserEntity();
                if (currentUser is Employee employee)
                {
                    var styles = new StringBuilder();

                    // Applying opacity
                    if (employee.AppearanceOpacity.HasValue && employee.AppearanceOpacity >= 0 && employee.AppearanceOpacity < 1)
                    {
                        switch (employee.AppearanceTheme)
                        {
                            case "light":
                            case "light2":
                                styles.Append($"main > .container {{ background-color: rgba(255, 255, 255, {(employee.AppearanceOpacity.Value).ToString("N", CultureInfo.InvariantCulture)}); }}");
                                break;
                            case "dark":
                                styles.Append($"main > .container {{ background-color: rgba(34, 34, 34, {(employee.AppearanceOpacity.Value).ToString("N", CultureInfo.InvariantCulture)}); }}");
                                break;
                        }
                    }

                    // Applying cover styles
                    if (employee.AppearanceBackgroundId.HasValue)
                    {
                        styles.Append($"main > .cover {{ object-fit: cover; position: fixed; top: 4.5rem; left: 0; width: 100%; height: 100%; z-index: 0;  }}");
                        styles.Append($"main > .container {{ z-index: 1;  }}");
                        styles.Append($"footer.footer {{ z-index: 1;  }}");
                    }

                    if (styles.Length > 0)
                    {
                        var tagBuilder = new TagBuilder("style");
                        tagBuilder.InnerHtml.AppendHtml(styles.ToString());
                        return tagBuilder;
                    }
                }
            }

            return new StringHtmlContent(String.Empty);
        }

        public async Task<IHtmlContent> RenderCustomCover()
        {
            if (this.currentTheme == "Dark")
            {
                var currentUser = await this.userResolver.ResolveCurrentUserEntity();
                if (currentUser is Employee employee)
                {
                    if (employee.AppearanceBackgroundId.HasValue)
                    {
                        var file = await this.repository.QueryWithoutTracking<UploadedFile>().SingleOrDefaultAsync(x => x.Id == employee.AppearanceBackgroundId.Value);
                        var filePath = await this.storageHub.GetContainer(file.Container).GetFileUrlAsync(file.GetContainerPath());

                        if (file.Extension == "jpg" || file.Extension == "jpeg" || file.Extension == "png")
                        {
                            var imageCover = new TagBuilder("img");
                            imageCover.Attributes.Add("src", filePath);
                            imageCover.AddCssClass("cover");
                            return imageCover;
                        }
                        else if (file.Extension == "mp24" || file.Extension == "webm")
                        {
                            var videoCover = new TagBuilder("video");
                            videoCover.Attributes.Add("src", filePath);
                            videoCover.Attributes.Add("autoplay", "autoplay");
                            videoCover.Attributes.Add("muted", "muted");
                            videoCover.Attributes.Add("loop", "loop");
                            videoCover.AddCssClass("cover");
                            return videoCover;
                        }
                    }
                }
            }

            return new StringHtmlContent(String.Empty);
        }

        private static IDictionary<String, String> BuildDefaultThemeResources()
        {
            return new Dictionary<String, String>
            {
                ["/vendor/kendo/kendo.min.css"] = "/vendor/kendo/kendo.min.css",
                ["/css/main.min.css"] = "/css/main.min.css",
                ["/img/hub-logo.png"] = "/img/hub-logo.png",
                ["/css/editor.min.css"] = "/css/editor.min.css",
            };
        }

        private static IDictionary<String, String> BuildDarkThemeResources()
        {
            var defaultCopy = ThemesService.BuildDefaultThemeResources().ToDictionary(x => x.Key, x => x.Value);
            defaultCopy["/vendor/kendo/kendo.min.css"] = "/vendor/kendo/kendo-dark.min.css";
            defaultCopy["/css/main.min.css"] = "/css/main-dark.min.css";
            defaultCopy["/img/hub-logo.png"] = "/img/hub-logo-dark.png";
            defaultCopy["/css/editor.min.css"] = "/css/editor-dark.min.css";
            return defaultCopy;
        }

        private static IDictionary<String, String> BuildLight2ThemeResources()
        {
            var defaultCopy = ThemesService.BuildDefaultThemeResources().ToDictionary(x => x.Key, x => x.Value);
            defaultCopy["/css/main.min.css"] = "/css/main-light2.min.css";
            return defaultCopy;
        }
    }
}
