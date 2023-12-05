using Mess.Cms.Extensions.Newtonsoft;
using OrchardCore.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNLogHost();
builder.Services
  .AddOrchardCms()
  .ConfigureServices(services =>
  {
    // TODO: in a module??
    services
      .AddControllers()
      .AddNewtonsoftJson(
        options =>
          options.SerializerSettings
            .AddMessNewtonsoftJsonSettings(
              builder.Environment.IsDevelopment()
            )
            .AddMessNewtonsoftJsonConverters()
      );
  })
  .AddSetupFeatures("OrchardCore.AutoSetup");

var app = builder.Build();
app.UseOrchardCore();
app.Run();
