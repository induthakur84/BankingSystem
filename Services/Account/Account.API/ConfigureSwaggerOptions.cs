using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProjectCommonCode
{

    //choose the version (e.g V1, v2, v3)
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "User Management API",
                Version = description.ApiVersion.ToString(),
                Description = "A simple CRUD operations API configured with Repository/Service Pattern, Dependency Injection, and API Versioning.",
                Contact = new OpenApiContact
                {
                    Name = "Web API Instructor"
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " **[DEPRECATED] This API version has been deprecated. Please upgrade to the latest version.**";
            }

            return info;
        }
    }
}
