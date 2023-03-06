using Xunit;
using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;
using OrchardContentItem = OrchardCore.ContentManagement.ContentItem;

namespace Mess.OrchardCore.Test;

public record class ContentItemTest
{
  [Fact]
  public void OnePartTest()
  {
    var item = new OrchardContentItem();
    item.ContentType = "OnePart";
    item.Weld(
      nameof(OnePart.StringPart),
      new StringPart { Value = "TestValue" }
    );

    var type = item.AsContent<OnePart>();
    Assert.NotNull(type);

    Assert.NotNull(type.StringPart);
    Assert.NotNull(type.StringPart.Value);
    Assert.Equal("TestValue", type.StringPart.Value.Value);
  }

  private class OnePart : ContentItem
  {
    public Lazy<StringPart> StringPart { get; init; } = default!;

    private OnePart(OrchardContentItem inner) : base(inner) { }
  }

  [Fact]
  public void TwoPartTest()
  {
    var item = new OrchardContentItem();
    item.ContentType = "TwoPart";
    item.Weld(
      nameof(TwoPart.FirstStringPart),
      new StringPart { Value = "FirstTestValue" }
    );
    item.Weld(
      nameof(TwoPart.SecondStringPart),
      new StringPart { Value = "SecondTestValue" }
    );

    var type = item.AsContent<TwoPart>();
    Assert.NotNull(type);

    Assert.NotNull(type.FirstStringPart);
    Assert.NotNull(type.FirstStringPart.Value);
    Assert.Equal("FirstTestValue", type.FirstStringPart.Value.Value);

    Assert.NotNull(type.SecondStringPart);
    Assert.NotNull(type.SecondStringPart.Value);
    Assert.Equal("SecondTestValue", type.SecondStringPart.Value.Value);
  }

  private class TwoPart : ContentItem
  {
    public Lazy<StringPart> FirstStringPart { get; init; } = default!;
    public Lazy<StringPart> SecondStringPart { get; init; } = default!;

    private TwoPart(OrchardContentItem inner) : base(inner) { }
  }

  [Fact]
  public void ContainedTest()
  {
    var item = new OrchardContentItem();
    item.ContentType = "Contained";
    item.Weld(
      nameof(Contained.StringPart),
      new StringPart { Value = "TestValue" }
    );

    item.Weld(
      nameof(Contained.ContainedPart),
      new ContainedPart { ListContentItemId = "TestId", Order = 10 }
    );

    var type = item.AsContent<Contained>();
    Assert.NotNull(type);

    Assert.NotNull(type.StringPart);
    Assert.NotNull(type.StringPart.Value);
    Assert.Equal("TestValue", type.StringPart.Value.Value);

    Assert.NotNull(type.ContainedPart);
    Assert.NotNull(type.ContainedPart.Value);
    Assert.Equal("TestId", type.ContainedPart.Value.ListContentItemId);
    Assert.Equal(10, type.ContainedPart.Value.Order);
  }

  private class Contained : ContentItem
  {
    public Lazy<StringPart> StringPart { get; init; } = default!;

    private Contained(OrchardContentItem inner) : base(inner) { }
  }

  [Fact(Skip = "Not working yet")]
  public void DerivedTest()
  {
    var item = new OrchardContentItem();
    item.ContentType = "Derived";
    item.Weld(
      nameof(Derived.StringPart),
      new StringPart { Value = "TestValue" }
    );

    var type = item.AsContent<Derived>();
    Assert.NotNull(type);

    Assert.NotNull(type.StringPart);
    Assert.NotNull(type.StringPart.Value);
    Assert.Equal("TestValue", type.StringPart.Value.Value);
  }

  private class Derived : ContentItem<Derived>
  {
    public Lazy<StringPart> StringPart { get; init; } = default!;

    public Derived(OrchardContentItem inner) : base(inner) { }
  }

  private class StringPart : ContentPart
  {
    public string Value { get; set; } = default!;
  }
}
