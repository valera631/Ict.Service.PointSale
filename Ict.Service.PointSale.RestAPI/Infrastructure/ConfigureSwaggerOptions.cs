using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace Ict.Service.PointSale.RestAPI.Infrastructure
{
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            this.provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            // add swagger document for every API version discovered
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateVersionInfo(description));
            }

            //// Включаем XML-комментарии для аннотаций
            //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

            //// Настраиваем фильтр для поддержки версионирования
            //options.DocInclusionPredicate((docName, apiDesc) =>
            //{
            //    if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;

            //    var versions = apiDesc.ActionDescriptor.EndpointMetadata
            //        .OfType<ApiVersionAttribute>()
            //        .SelectMany(attr => attr.Versions)
            //        .Select(v => $"v{v.ToString()}");

            //    return versions.Any(v => v == docName);
            //});
        }

        public void Configure(string? name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "PointSale API",
                Description = "Сервис управления точками продаж",
                Version = description.ApiVersion.ToString(),
                Contact = new OpenApiContact
                {
                    Name = "ICT Ltd.",
                    Email = "support@ictcorp.biz",
                    Url = new Uri("http://www.ictcorp.biz")
                },
                License = new OpenApiLicense
                {
                    Name = "ООО \"АйСиТи\"",
                    Url = new Uri("http://www.ictcorp.biz")
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " Эта версия API устарела.";
            }

            return info;
        }
    }
}
