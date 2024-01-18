using Mess.Billing.Abstractions.Services;
using Mess.Cms;
using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql;
using ISession = YesSql.ISession;

namespace Mess.Ozds.Abstractions.Billing;

// TODO: shorten the hell out of this thing

public abstract class OzdsClosedDistributionUnitBillingFactory : IBillingFactory
{
  private readonly IContentManager _contnetManager;

  private readonly ISession _session;

  private readonly List<IOzdsIotDeviceBillingFactory> _iotDeviceBillingFactories;

  protected OzdsClosedDistributionUnitBillingFactory(
    IContentManager contentManager,
    ISession session,
    IEnumerable<IOzdsIotDeviceBillingFactory> iotDeviceBillingFactories
  )
  {
    _contnetManager = contentManager;
    _session = session;
    _iotDeviceBillingFactories = iotDeviceBillingFactories.ToList();
  }

  public bool IsApplicable(ContentItem contentItem)
  {
    return contentItem.ContentType == "DistributionSystemUnit";
  }

  public ContentItem CreateInvoice(
    ContentItem contentItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
  }

  public async Task<ContentItem> CreateInvoiceAsync(
    ContentItem contentItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    var iotDevices = await _session
      .Query<ContentItem, OzdsIotDeviceIndex>()
      .Where(index => index.DistributionSystemUnitContentItemId == contentItem.ContentItemId)
      .ListAsync();

    var calculations = new List<OzdsCalculationData>();
    foreach (var iotDevice in iotDevices)
    {
      var iotDeviceBillingFactory = _iotDeviceBillingFactories
        .FirstOrDefault(iotDeviceBillingFactory => iotDeviceBillingFactory.IsApplicable(iotDevice))
          ?? throw new InvalidOperationException($"Billing factory not found for {iotDevice}");

      var calculation = await iotDeviceBillingFactory
        .CreateCalculationAsync(
          contentItem,
          iotDevice,
          fromDate,
          toDate
        );

      calculations.Add(calculation);
    }


  }

  public ContentItem CreateReceipt(
    ContentItem contentItem,
    ContentItem invoiceContentItem
  )
  {
  }

  public async Task<ContentItem> CreateReceiptAsync(
    ContentItem contentItem,
    ContentItem invoiceContentItem
  )
  {
  }
}
