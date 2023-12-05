using OrchardCore.ContentManagement;

namespace Mess.Cms.Test.Snapshots;

public record SnapshotTests(ISnapshotFixture SnapshotFixture)
{
  public record SnapshotTestRecord(string Property);

  public class SnapshotTestClass
  {
    public string Property { get; set; } = default!;
  }

  public struct SnapshotTestStruct
  {
    public SnapshotTestStruct() { }

    public string Property { get; set; } = default!;
  }

  public static readonly object[][] MatchesSnapshotData = new[]
  {
    new object[] { new object() },
    new object[] { new { Property = "Value" } },
    new object[] { new SnapshotTestRecord("Value") },
    new object[] { new SnapshotTestClass() { Property = "Value" } },
    new object[] { new SnapshotTestStruct() { Property = "Value" } },
  };

  [Theory]
  [StaticData(nameof(MatchesSnapshotData))]
  public async Task MatchesSnapshot(object snapshot)
  {
    var verificationHash = await SnapshotFixture.MakeVerificationHash(snapshot);
    await SnapshotFixture.Verify(snapshot, verificationHash);
  }

  [Theory]
  [NewtonsoftJsonAssetData]
  public async Task MatchesSnapshotFromJson(SnapshotTestRecord snapshot)
  {
    var verificationHash = await SnapshotFixture.MakeVerificationHash(snapshot);
    await SnapshotFixture.Verify(snapshot, verificationHash);
  }

  [Theory]
  [NewtonsoftJsonAssetData]
  public async Task MatchesContentItem(ContentItem contentItem)
  {
    var verificationHash = await SnapshotFixture.MakeVerificationHash(
      contentItem
    );
    await SnapshotFixture.Verify(contentItem, verificationHash);
  }
}
