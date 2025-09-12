using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Ict.ApiProvider;
using Ict.Provider.Service.File;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
using Ict.Service.PointSale.Core.Interfaces;
using Ict.Service.PointSale.Core.Mapper;
using Ict.Service.PointSale.Core.Services;
using Ict.Service.PointSale.DataBase;
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;
using Ict.Service.PointSale.Repository.Action;
using Ict.Service.PointSale.RestAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

var nameAssembly = Assembly.GetExecutingAssembly().GetName().Name;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();



builder.Services.AddDbContext<PointSaleDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PointSaleConnection"),
        b => b.MigrationsAssembly("Ict.Service.PointSale.DataBase"))
    .EnableSensitiveDataLogging() // ѕоказывает параметры запросов (будьте осторожны в продакшене)
           .LogTo(message => Log.Information(message),
                  new[] { DbLoggerCategory.Database.Command.Name },
                  LogLevel.Information);


});

builder.Services.AddFileService(options =>
{
    options.ServiceFileUrl = builder.Configuration.GetSection("Service.File:Url").Value ?? "";
});

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddApiProvider();
builder.Services.AddScoped<IPointSaleService, PointSaleService>();
builder.Services.AddScoped<IDescriptionService, DescriptionService>();
builder.Services.AddScoped<IDescriptionRepository, DescriptionRepository>();
builder.Services.AddScoped<IChiefService, ChiefService>();
builder.Services.AddScoped<IChiefRepository, ChiefRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IPointSaleSearch, PointSaleSearch>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();
builder.Services.AddScoped<IReferencesService, ReferencesService>();
builder.Services.AddScoped<IReferencesRepository, ReferencesRepository>();
builder.Services.AddScoped<IPointSaleRepository, PointSaleRepository>();

// Add services to the container.

builder.Services.AddControllers();


// версионность API
builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(1, 0);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
    setup.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("version"));
    //setup.ApiVersionReader = new QueryStringApiVersionReader
    //{
    //    ParameterNames = { "TEst" }
    //};
})
.AddApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
}); ;

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();

    //// рисует кнопку у swagger дл€ аутентификации
    //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    //{
    //    Type = SecuritySchemeType.OAuth2,
    //    //BearerFormat = "JWT",
    //    Flows = new OpenApiOAuthFlows
    //    {
    //        Password = new OpenApiOAuthFlow
    //        {
    //            TokenUrl = new Uri($"{builder.Configuration["OIDC:Authority"]}/connect/token"), //  "https://localhost:10001/connect/token"
    //            Scopes = new Dictionary<string, string>()
    //            {
    //                { "VacancyAPI", "Vacancy API Service" },
    //            },
    //        },
    //    },
    //});

    //// получение токена дл€ доступа к API по имени
    //options.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "oauth2"
    //            },
    //            Scheme = "oauth2",
    //            Name = "Bearer",
    //            In = ParameterLocation.Header,
    //        },
    //        new List<string>()
    //    }
    //});
});

// конфигураци€ дл€ многоверсионности API в Swagger
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

// добавить описани€ методов в swagger
builder.Services.ConfigureSwaggerGen(options =>
{
    //базовый путь.
    //var basePath = "";
    var basePath = AppContext.BaseDirectory;
    var xmlPath = Path.Combine(basePath, $"{nameAssembly}.xml");

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});



var app = builder.Build();


if (app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("ShowSwagger", false))
{
    app.UseSwagger(c =>
    {
        // c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
    });
    app.UseSwaggerUI(
        options =>
        {
            options.InjectStylesheet("/swagger-ui/custom.css"); // свой дизайн
            options.DocumentTitle = "Swagger Law Converter";
            options.RoutePrefix = "swagger";
            //options.OAuthClientId(builder.Configuration["OIDC:ClientId"]);
            //options.OAuthClientSecret(builder.Configuration["OIDC:ClientSecret"]);

            //  options.DocExpansion(DocExpansion.List); // развернуть всЄ дерево
            options.DocExpansion(DocExpansion.None); // развернуть всЄ дерево
            options.DefaultModelRendering(ModelRendering.Model); // параметры показать как Shema
            options.DefaultModelExpandDepth(0); //
            options.DefaultModelsExpandDepth(-1); // убрать Schemas со страницы

            // ƒинамически добавл€ем endpoints дл€ каждой версии
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

app.Run();
