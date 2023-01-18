using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using YesSql.Indexes;
using Mess.Chart.Models;
using OrchardCore.ContentFields.Indexing.SQL;

namespace Mess.Chart.Indexing.SQL;

public class ChartFieldIndex : ContentFieldIndex
{
  public string Provider { get; set; } = default!;
}

public class ChartFieldIndexProvider : ContentFieldIndexProvider
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<ChartFieldIndex>()
      .Map(contentItem =>
      {
        // Remove index records of soft deleted items.
        if (!contentItem.Published && !contentItem.Latest)
        {
          return null!;
        }

        // Can we safely ignore this content item?
        if (_ignoredTypes.Contains(contentItem.ContentType))
        {
          return null!;
        }

        // Lazy initialization because of ISession cyclic dependency
        _contentDefinitionManager ??=
          _serviceProvider.GetRequiredService<IContentDefinitionManager>();

        // Search for Html fields
        var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(
          contentItem.ContentType
        );

        // This can occur when content items become orphaned, particularly layer widgets when a layer is removed, before its widgets have been unpublished.
        if (contentTypeDefinition == null)
        {
          _ignoredTypes.Add(contentItem.ContentType);
          return null!;
        }

        var fieldDefinitions = contentTypeDefinition.Parts
          .SelectMany(
            x =>
              x.PartDefinition.Fields.Where(
                f => f.FieldDefinition.Name == nameof(ChartField)
              )
          )
          .ToArray();

        // This type doesn't have any HtmlField, ignore it
        if (fieldDefinitions.Length == 0)
        {
          _ignoredTypes.Add(contentItem.ContentType);
          return null!;
        }

        return fieldDefinitions
          .GetContentFields<ChartField>(contentItem)
          .Select(
            pair =>
              new ChartFieldIndex
              {
                Latest = contentItem.Latest,
                Published = contentItem.Published,
                ContentItemId = contentItem.ContentItemId,
                ContentItemVersionId = contentItem.ContentItemVersionId,
                ContentType = contentItem.ContentType,
                ContentPart = pair.Definition.PartDefinition.Name,
                ContentField = pair.Definition.Name,
                Provider = pair.Field.Parameters.Provider,
              }
          );
      });
  }

  public ChartFieldIndexProvider(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  private readonly IServiceProvider _serviceProvider;
  private readonly HashSet<string> _ignoredTypes = new();
  private IContentDefinitionManager _contentDefinitionManager = default!;
}
