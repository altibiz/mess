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
app.Use(async (context, next) =>
{
  if (context.Request.Path == "/")
  {
    context.Response.Redirect("/ozds", permanent: false); // You can set 'permanent' to true if it's a permanent redirection
  }
  else
  {
    await next();
  }
});
app.UseStaticFiles();
app.UseOrchardCore();
app.Run();
