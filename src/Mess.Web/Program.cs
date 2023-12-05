using OrchardCore.Logging;
using Mess.Cms.Extensions.Newtonsoft;

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
              pretty: builder.Environment.IsDevelopment()
            )
            .AddMessNewtonsoftJsonConverters()
      );
  })
  .AddSetupFeatures("OrchardCore.AutoSetup");

var app = builder.Build();
app.UseOrchardCore();
app.Run();
