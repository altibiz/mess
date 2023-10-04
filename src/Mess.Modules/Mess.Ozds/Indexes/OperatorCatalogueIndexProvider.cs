using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Ozds.Indexes;

public class OperatorCatalogueIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<OperatorCatalogueIndex>()
      .When(contentItem => contentItem.Has<OperatorCataloguePart>())
      .Map(contentItem =>
      {
        var operatorCataloguePart = contentItem.As<OperatorCataloguePart>();
        return new OperatorCatalogueIndex
        {
          Model = operatorCataloguePart.Model.Text,
          Voltage = operatorCataloguePart.Voltage.Text
        };
      });
  }
}
