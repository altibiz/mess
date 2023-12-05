using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;
using OrchardContentItem = OrchardCore.ContentManagement.ContentItem;

namespace Mess.Cms.Test;

public record class ContentItemBaseTest
{
  [Fact]
  public void OnePartTest()
  {
    var item = new OrchardContentItem
    {
      ContentType = "OnePart"
    };
    item.Weld(
      nameof(OnePartItem.StringPart),
      new StringPart { Value = "TestValue" }
    );

    var type = item.AsContent<OnePartItem>();
    Assert.NotNull(type);

    Assert.NotNull(type.StringPart);
    Assert.NotNull(type.StringPart.Value);
    Assert.Equal("TestValue", type.StringPart.Value.Value);
  }

  [Fact]
  public void TwoPartTest()
  {
    var item = new OrchardContentItem
    {
      ContentType = "TwoPart"
    };
    item.Weld(
      nameof(TwoPartItem.FirstStringPart),
      new StringPart { Value = "FirstTestValue" }
    );
    item.Weld(
      nameof(TwoPartItem.SecondStringPart),
      new StringPart { Value = "SecondTestValue" }
    );

    var type = item.AsContent<TwoPartItem>();
    Assert.NotNull(type);

    Assert.NotNull(type.FirstStringPart);
    Assert.NotNull(type.FirstStringPart.Value);
    Assert.Equal("FirstTestValue", type.FirstStringPart.Value.Value);

    Assert.NotNull(type.SecondStringPart);
    Assert.NotNull(type.SecondStringPart.Value);
    Assert.Equal("SecondTestValue", type.SecondStringPart.Value.Value);
  }

  [Fact]
  public void ContainedTest()
  {
    var item = new OrchardContentItem
    {
      ContentType = "Contaned"
    };
    item.Weld(
      nameof(ContainedItem.StringPart),
      new StringPart { Value = "TestValue" }
    );

    item.Weld(
      nameof(ContainedItem.ContainedPart),
      new ContainedPart { ListContentItemId = "TestId", Order = 10 }
    );

    var type = item.AsContent<ContainedItem>();
    Assert.NotNull(type);

    Assert.NotNull(type.StringPart);
    Assert.NotNull(type.StringPart.Value);
    Assert.Equal("TestValue", type.StringPart.Value.Value);

    Assert.NotNull(type.ContainedPart);
    Assert.NotNull(type.ContainedPart.Value);
    Assert.Equal("TestId", type.ContainedPart.Value.ListContentItemId);
    Assert.Equal(10, type.ContainedPart.Value.Order);
  }

  private class OnePartItem : ContentItemBase
  {
    private OnePartItem(OrchardContentItem inner)
      : base(inner)
    {
    }

    public Lazy<StringPart> StringPart { get; init; } = default!;
  }

  private class TwoPartItem : ContentItemBase
  {
    private TwoPartItem(OrchardContentItem inner)
      : base(inner)
    {
    }

    public Lazy<StringPart> FirstStringPart { get; init; } = default!;
    public Lazy<StringPart> SecondStringPart { get; init; } = default!;
  }

  private class ContainedItem : ContentItemBase
  {
    private ContainedItem(OrchardContentItem inner)
      : base(inner)
    {
    }

    public Lazy<StringPart> StringPart { get; init; } = default!;
  }

  // TODO: make this work
  // public void DerivedTest()
  // {
  //   var item = new OrchardContentItem() {
  //     ContentType = "Derived"
  //   };
  //   item.Weld(
  //     nameof(DerivedItem.StringPart),
  //     new StringPart { Value = "TestValue" }
  //   );

  //   var type = item.AsContent<DerivedItem>();
  //   Assert.NotNull(type);

  //   Assert.NotNull(type.StringPart);
  //   Assert.NotNull(type.StringPart.Value);
  //   Assert.Equal("TestValue", type.StringPart.Value.Value);
  // }

  private class DerivedItem : ContentItemBase<DerivedItem>
  {
    public DerivedItem(OrchardContentItem inner)
      : base(inner)
    {
    }

    public Lazy<StringPart> StringPart { get; init; } = default!;
  }

  private class StringPart : ContentPart
  {
    public string Value { get; set; } = default!;
  }
}
