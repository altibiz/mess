using OrchardCore.DisplayManagement.Descriptors.ShapeTemplateStrategy;

using Mess.Blazor.Abstractions;

namespace Mess.Blazor;

public class BasicShapeTemplateHarvester : IShapeComponentHarvester
{
    public IEnumerable<string> SubNamespaces()
    {
        return new[] { "Components", "Components.Items", "Components.Parts", "Components.Fields", "Components.Elements" };
    }

    public IEnumerable<HarvestShapeHit> HarvestShape(HarvestShapeComponentInfo info)
    {
        var lastDash = info.Type.Name.LastIndexOf('-');
        var lastDot = info.Type.Name.LastIndexOf('.');
        if (lastDot <= 0 || lastDot < lastDash)
        {
            yield return new HarvestShapeHit
            {
                ShapeType = Adjust(info.SubNamespace, info.Type.Name, null),
            };
        }
        else
        {
            var displayType = info.Type.Name[(lastDot + 1)..];
            yield return new HarvestShapeHit
            {
                ShapeType = Adjust(info.SubNamespace, info.Type.Name[..lastDot], displayType),
                DisplayType = displayType,
            };
        }
    }

    private static string Adjust(string subNamespace, string typeName, string? displayType)
    {
        var leader = "";
        if (subNamespace.StartsWith("Components.", StringComparison.Ordinal) && subNamespace != "Components.Items")
        {
            leader = String.Concat(subNamespace.AsSpan("Components.".Length), "_");
        }

        // canonical shape type names must not have - or . to be compatible
        // with display and shape api calls)))
        var shapeType = leader + typeName.Replace("--", "__").Replace("-", "__").Replace('.', '_');

        if (String.IsNullOrEmpty(displayType))
        {
            return shapeType.ToLowerInvariant();
        }

        var firstBreakingSeparator = shapeType.IndexOf("__", StringComparison.Ordinal);
        if (firstBreakingSeparator <= 0)
        {
            return (shapeType + "_" + displayType).ToLowerInvariant();
        }

        return String.Concat(
            shapeType.AsSpan(0, firstBreakingSeparator), "_", displayType, shapeType.AsSpan(firstBreakingSeparator))
            .ToLowerInvariant();
    }
}
