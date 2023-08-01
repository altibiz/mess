using OrchardCore.Logging;
using Mess.OrchardCore.Extensions.Newtonsoft;
using Mess.OrchardCore.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNLogHost();
builder.Services
  .AddOrchardCms()
  .ConfigureServices(services =>
  {
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

    services.AddScoped<IActionContextService, ActionContextService>();
  })
  .AddSetupFeatures("OrchardCore.AutoSetup");

var app = builder.Build();
app.UseOrchardCore();
app.Run();
