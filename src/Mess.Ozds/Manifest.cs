using OrchardCore.DisplayManagement.Manifest;

[assembly: Theme(
  Name = "Mess.Ozds",
  Author = "Altibiz",
  Website = "https://altibiz.com",
  Version = "0.0.1",
  Description = "Mess.Ozds",
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
    "OrchardCore.Localization",
    "OrchardCore.ContentLocalization",
    "OrchardCore.ContentLocalization.ContentCulturePicker",
    "OrchardCore.ContentFields",
    "OrchardCore.ContentPreview",
    "OrchardCore.Contents",
    "OrchardCore.ContentTypes",
    "OrchardCore.Deployment",
    "OrchardCore.Placements",
    "OrchardCore.Queries",
    "OrchardCore.BackgroundTasks",
    "Mess.EventStore",
    "Mess.Timeseries"
  }
)]
