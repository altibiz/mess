var builder = WebAssemblyHostBuilder.CreateDefault(args);

var services = builder.Services;
builder.RootComponents.Add<App>("#app");

await builder.Build().RunAsync();
