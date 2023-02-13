using OrchardCore.Modules.Manifest;

[assembly: Module(
  Name = "Mess.Default",
  Author = "Altibiz",
  Website = "https://altibiz.com",
  Version = "0.0.1",
  Description = "Mess.Default",
  Category = "Content Management",
  Dependencies = new[]
  {
    "OrchardCore.Features",
    "OrchardCore.Recipes",
    "OrchardCore.Resources",
    "OrchardCore.Settings",
    "OrchardCore.Themes",
    "OrchardCore.Roles",
    "OrchardCore.Users",
    "OrchardCore.Users.Registration",
    "OrchardCore.Users.ResetPassword",
    "OrchardCore.Admin",
    "OrchardCore.AdminMenu",
    "OrchardCore.Navigation",
    "OrchardCore.ContentFields",
    "OrchardCore.ContentPreview",
    "OrchardCore.Contents",
    "OrchardCore.ContentTypes",
    "OrchardCore.Deployment",
    "OrchardCore.Placements",
    "OrchardCore.Queries",
    "OrchardCore.BackgroundTasks",
    "OrchardCore.Liquid",
    "Mess.EventStore",
    "Mess.Timeseries",
    "Mess.Chart",
    "Mess.MeasurementDevice"
  }
)]
