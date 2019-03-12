﻿namespace RestaurantBooking.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Bot.Solutions.Skills;

    public class UrlResolver : IUrlResolver
    {
        public UrlResolver(IHttpContextAccessor httpContextAccessor, SkillConfigurationBase services)
        {
            if (httpContextAccessor != null)
            {
                var httpContext = httpContextAccessor.HttpContext;
                ServerUrl = httpContext.Request.Scheme + "://" + httpContext.Request.Host.Value;
            }
            else
            {
                // In skill-mode we don't have HttpContext and require skills to provide their own storage for assets
                services.Properties.TryGetValue("ImageAssetLocation", out var imageUri);

                var imageUriStr = (string)imageUri;
                if (string.IsNullOrWhiteSpace(imageUriStr))
                {
                    throw new Exception("ImageAssetLocation Uri not configured on the skill.");
                }
                else
                {
                    ServerUrl = imageUriStr;
                }
            }
        }

        public string ServerUrl { get; }

        public string GetImageUrl(string imagePath)
        {
            return GetImageByCulture(imagePath);
        }

        private string GetImageByCulture(string imagePath)
        {
            var currentCulture = CultureInfo.CurrentUICulture.Name.Split("-");
            var neutralCulture = currentCulture[0].ToLower();
            string specificCulture = null;

            if (currentCulture.ElementAtOrDefault(1) != null)
            {
                specificCulture = currentCulture[1];
            }

            return GetImagePath(imagePath, neutralCulture, specificCulture);
        }

        private string GetImagePath(string imagePath, string neutralCulture, string specificCulture)
        {
            return $"{ServerUrl}/assets/en/images/{imagePath}";
        }
    }
}
