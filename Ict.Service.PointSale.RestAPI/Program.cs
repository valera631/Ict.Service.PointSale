using Ict.ApiProvider;
using Ict.Provider.Service.File;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
using Ict.Service.PointSale.Core.Interfaces;
using Ict.Service.PointSale.Core.Mapper;
using Ict.Service.PointSale.Core.Services;
using Ict.Service.PointSale.DataBase;
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;
using Ict.Service.PointSale.Repository.Action;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
    .EnableSensitiveDataLogging() // Показывает параметры запросов (будьте осторожны в продакшене)
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
builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(1, 0);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

app.Run();
