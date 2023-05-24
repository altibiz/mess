using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;
using OrchardContentItem = OrchardCore.ContentManagement.ContentItem;

namespace Mess.OrchardCore.Test;

public record class ContentItemBaseTest
{
  [Fact]
  public void OnePartTest()
  {
    var item = new OrchardContentItem();
    item.ContentType = "OnePart";
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

  private class OnePartItem : ContentItemBase
  {
    public Lazy<StringPart> StringPart { get; init; } = default!;

    private OnePartItem(OrchardContentItem inner)
      : base(inner) { }
  }

  [Fact]
  public void TwoPartTest()
  {
    var item = new OrchardContentItem();
    item.ContentType = "TwoPart";
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

  private class TwoPartItem : ContentItemBase
  {
    public Lazy<StringPart> FirstStringPart { get; init; } = default!;
    public Lazy<StringPart> SecondStringPart { get; init; } = default!;

    private TwoPartItem(OrchardContentItem inner)
      : base(inner) { }
  }

  [Fact]
  public void ContainedTest()
  {
    var item = new OrchardContentItem();
    item.ContentType = "Contained";
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

  private class ContainedItem : ContentItemBase
  {
    public Lazy<StringPart> StringPart { get; init; } = default!;

    private ContainedItem(OrchardContentItem inner)
      : base(inner) { }
  }

  [Fact(Skip = "Not working yet")]
  public void DerivedTest()
  {
    var item = new OrchardContentItem();
    item.ContentType = "Derived";
    item.Weld(
      nameof(DerivedItem.StringPart),
      new StringPart { Value = "TestValue" }
    );

    var type = item.AsContent<DerivedItem>();
    Assert.NotNull(type);

    Assert.NotNull(type.StringPart);
    Assert.NotNull(type.StringPart.Value);
    Assert.Equal("TestValue", type.StringPart.Value.Value);
  }

  private class DerivedItem : ContentItemBase<DerivedItem>
  {
    public Lazy<StringPart> StringPart { get; init; } = default!;

    public DerivedItem(OrchardContentItem inner)
      : base(inner) { }
  }

  private class StringPart : ContentPart
  {
    public string Value { get; set; } = default!;
  }
}
