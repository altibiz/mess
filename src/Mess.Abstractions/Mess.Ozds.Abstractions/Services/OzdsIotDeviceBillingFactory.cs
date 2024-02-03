using Mess.Billing.Abstractions.Services;
using Mess.Cms;
using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Billing;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql;
using ISession = YesSql.ISession;

namespace Mess.Ozds.Abstractions.Services;

public abstract class OzdsIotDeviceBillingFactory<T> : IOzdsIotDeviceBillingFactory
  where T : ContentItemBase
{
  private readonly IContentManager _contentManager;

  protected OzdsIotDeviceBillingFactory(
    IContentManager contentManager
  )
  {
    _contentManager = contentManager;
  }

  public bool IsApplicable(ContentItem iotDeviceItem)
  {
    return iotDeviceItem.ContentType == typeof(T).ContentTypeName();
  }

  public OzdsCalculationData CreateCalculation(
    DistributionSystemUnitItem distributionSystemUnitItem,
    ClosedDistributionSystemItem closedDistributionSystemItem,
    DistributionSystemOperatorItem distributionSystemOperatorItem,
    RegulatoryAgencyCatalogueItem regulatoryAgencyCatalogueItem,
    ContentItem iotDeviceItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return CreateCalculationAsync(
      distributionSystemUnitItem,
      closedDistributionSystemItem,
      distributionSystemOperatorItem,
      regulatoryAgencyCatalogueItem,
      iotDeviceItem,
      fromDate,
      toDate
    ).Result;
  }

  public async Task<OzdsCalculationData> CreateCalculationAsync(
    DistributionSystemUnitItem distributionSystemUnitItem,
    ClosedDistributionSystemItem closedDistributionSystemItem,
    DistributionSystemOperatorItem distributionSystemOperatorItem,
    RegulatoryAgencyCatalogueItem regulatoryAgencyCatalogueItem,
    ContentItem iotDeviceItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    var measurementDeviceItem = iotDeviceItem.AsContent<T>();

    var ozdsIotDevicePart = iotDeviceItem.As<OzdsIotDevicePart>()
      ?? throw new InvalidOperationException($"Item is not OZDS IOT item {iotDeviceItem.ContentItemId}");

    var usageCatalogueItem = await _contentManager
      .GetContentAsync<OperatorCatalogueItem>(ozdsIotDevicePart.UsageCatalogue.ContentItemIds.First())
        ?? throw new InvalidOperationException($"Usage catalogue {ozdsIotDevicePart.UsageCatalogue.ContentItemIds.First()} not found");

    var supplyCatalogueItem = await _contentManager
      .GetContentAsync<OperatorCatalogueItem>(ozdsIotDevicePart.SupplyCatalogue.ContentItemIds.First())
        ?? throw new InvalidOperationException($"Supply catalogue {ozdsIotDevicePart.SupplyCatalogue.ContentItemIds.First()} not found");

    var billingData = await FetchBillingDataAsync(
      measurementDeviceItem,
      fromDate,
      toDate
    ) ?? throw new InvalidOperationException($"Billing data for {measurementDeviceItem.ContentItemId} is null");

    var calculation = CreateCalculationData(
      fromDate,
      toDate,
      billingData,
      iotDeviceItem,
      regulatoryAgencyCatalogueItem,
      usageCatalogueItem,
      supplyCatalogueItem
    );

    return calculation;
  }

  protected abstract OzdsIotDeviceBillingData? FetchBillingData(
    T measurementDeviceItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  protected abstract Task<OzdsIotDeviceBillingData?> FetchBillingDataAsync(
    T measurementDeviceItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  private static OzdsCalculationData CreateCalculationData(
    DateTimeOffset fromDate,
    DateTimeOffset toDate,
    OzdsIotDeviceBillingData billingData,
    ContentItem iotDevice,
    RegulatoryAgencyCatalogueItem regulatoryAgencyCatalogue,
    OperatorCatalogueItem usageCatalogueItem,
    OperatorCatalogueItem supplyCatalogueItem
  )
  {
    var usageExpenditure = CreateExpenditureData(
      billingData,
      usageCatalogueItem,
      null
    );
    var supplyExpenditure = CreateExpenditureData(
      billingData,
      supplyCatalogueItem,
      regulatoryAgencyCatalogue
    );

    return new OzdsCalculationData(
      iotDevice,
      regulatoryAgencyCatalogue,
      usageCatalogueItem,
      supplyCatalogueItem,
      fromDate,
      toDate,
      usageExpenditure,
      supplyExpenditure
    );
  }

  private static OzdsExpenditureData CreateExpenditureData(
    OzdsIotDeviceBillingData billingData,
    OperatorCatalogueItem catalogueItem,
    RegulatoryAgencyCatalogueItem? regulatoryAgencyCatalogueItem
  )
  {
    var energyPrice =
      catalogueItem.OperatorCataloguePart.Value.EnergyPrice.Value ?? 0.0M;
    var energyStart = Math.Round(billingData.StartEnergyTotal_kWh, 2);
    var energyEnd = Math.Round(billingData.EndEnergyTotal_kWh, 2);
    var energyAmount = Math.Round(energyEnd - energyStart, 2);
    var energyTotal = Math.Round(energyAmount * energyPrice, 2);
    var energyItem =
      energyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          energyStart,
          energyEnd,
          energyAmount,
          energyPrice,
          energyTotal
        );

    var highEnergyPrice =
      catalogueItem.OperatorCataloguePart.Value.HighEnergyPrice.Value ?? 0.0M;
    var highEnergyStart = Math.Round(billingData.HighStartEnergyTotal_kWh, 2);
    var highEnergyEnd = Math.Round(billingData.HighEndEnergyTotal_kWh, 2);
    var highEnergyAmount = Math.Round(highEnergyEnd - highEnergyStart, 2);
    var highEnergyTotal = Math.Round(highEnergyAmount * highEnergyPrice, 2);
    var highEnergyItem =
      highEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          highEnergyStart,
          highEnergyEnd,
          highEnergyAmount,
          highEnergyPrice,
          highEnergyTotal
        );

    var lowEnergyPrice =
      catalogueItem.OperatorCataloguePart.Value.LowEnergyPrice.Value ?? 0.0M;
    var lowEnergyStart = Math.Round(billingData.LowStartEnergyTotal_kWh, 2);
    var lowEnergyEnd = Math.Round(billingData.LowEndEnergyTotal_kWh, 2);
    var lowEnergyAmount = Math.Round(lowEnergyEnd - lowEnergyStart, 2);
    var lowEnergyTotal = lowEnergyAmount * lowEnergyPrice;
    var lowEnergyItem =
      lowEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          lowEnergyStart,
          lowEnergyEnd,
          lowEnergyAmount,
          lowEnergyPrice,
          lowEnergyTotal
        );

    var reactiveEnergyPrice =
      catalogueItem.OperatorCataloguePart.Value.ReactiveEnergyPrice.Value ?? 0.0M;
    var reactiveEnergyStart = Math.Round(billingData.StartReactiveEnergyTotal_kWh, 2);
    var reactiveEnergyEnd = Math.Round(billingData.EndReactiveEnergyTotal_kWh, 2);
    var reactiveEnergyAmount = Math.Round(reactiveEnergyEnd - reactiveEnergyStart, 2);
    var reactiveEnergyTotal = Math.Round(reactiveEnergyAmount * reactiveEnergyPrice, 2);
    var reactiveEnergyItem =
      reactiveEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          reactiveEnergyStart,
          reactiveEnergyEnd,
          reactiveEnergyAmount,
          reactiveEnergyPrice,
          reactiveEnergyTotal
        );

    var highReactiveEnergyPrice =
      catalogueItem.OperatorCataloguePart.Value.HighReactiveEnergyPrice.Value ?? 0.0M;
    var highReactiveEnergyStart = Math.Round(billingData.HighStartReactiveEnergyTotal_kWh, 2);
    var highReactiveEnergyEnd = Math.Round(billingData.HighEndReactiveEnergyTotal_kWh, 2);
    var highReactiveEnergyAmount = Math.Round(highReactiveEnergyEnd - highReactiveEnergyStart, 2);
    var highReactiveEnergyTotal = Math.Round(highReactiveEnergyAmount * highReactiveEnergyPrice, 2);
    var highReactiveEnergyItem =
      highReactiveEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          highReactiveEnergyStart,
          highReactiveEnergyEnd,
          highReactiveEnergyAmount,
          highReactiveEnergyPrice,
          highReactiveEnergyTotal
        );

    var lowReactiveEnergyPrice =
      catalogueItem.OperatorCataloguePart.Value.LowReactiveEnergyPrice.Value ?? 0.0M;
    var lowReactiveEnergyStart = Math.Round(billingData.LowStartReactiveEnergyTotal_kWh, 2);
    var lowReactiveEnergyEnd = Math.Round(billingData.LowEndReactiveEnergyTotal_kWh, 2);
    var lowReactiveEnergyAmount = Math.Round(lowReactiveEnergyEnd - lowReactiveEnergyStart, 2);
    var lowReactiveEnergyTotal = Math.Round(lowReactiveEnergyAmount * lowReactiveEnergyPrice, 2);
    var lowReactiveEnergyItem =
      lowReactiveEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          lowReactiveEnergyStart,
          lowReactiveEnergyEnd,
          lowReactiveEnergyAmount,
          lowReactiveEnergyPrice,
          lowReactiveEnergyTotal
        );

    var maxPowerPrice =
      catalogueItem.OperatorCataloguePart.Value.MaxPowerPrice.Value ?? 0.0M;
    var maxPowerAmount = billingData.PeakPowerTotal_kW;
    var maxPowerTotal = maxPowerAmount * maxPowerPrice;
    var maxPowerItem =
      maxPowerPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          0.0M,
          0.0M,
          maxPowerAmount,
          maxPowerPrice,
          maxPowerTotal
        );

    var iotDeviceFeePrice =
      catalogueItem.OperatorCataloguePart.Value.IotDeviceFee.Value ?? 0.0M;
    var iotDeviceFeeAmount = 1.0M;
    var iotDeviceFeeTotal = iotDeviceFeeAmount * iotDeviceFeePrice;
    var iotDeviceFee =
      iotDeviceFeePrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          0.0M,
          0.0M,
          iotDeviceFeeAmount,
          iotDeviceFeePrice,
          iotDeviceFeeTotal
        );

    var renewableEnergyFeePrice =
      regulatoryAgencyCatalogueItem?.RegulatoryAgencyCataloguePart.Value.RenewableEnergyFee.Value ?? 0.0M;
    var renewableEnergyFeeAmount =
      billingData.EndEnergyTotal_kWh - billingData.StartEnergyTotal_kWh;
    var renewableEnergyFeeTotal = renewableEnergyFeeAmount * renewableEnergyFeePrice;
    var renewableEnergyFee =
      renewableEnergyFeePrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          billingData.StartEnergyTotal_kWh,
          billingData.EndEnergyTotal_kWh,
          renewableEnergyFeeAmount,
          renewableEnergyFeePrice,
          renewableEnergyFeeTotal
        );

    var businessUsageFeePrice =
      regulatoryAgencyCatalogueItem?.RegulatoryAgencyCataloguePart.Value.BusinessUsageFee.Value ?? 0.0M;
    var businessUsageFeeAmount =
      billingData.EndEnergyTotal_kWh - billingData.StartEnergyTotal_kWh;
    var businessUsageFeeTotal = businessUsageFeeAmount * businessUsageFeePrice;
    var businessUsageFee =
      businessUsageFeePrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          billingData.StartEnergyTotal_kWh,
          billingData.EndEnergyTotal_kWh,
          businessUsageFeeAmount,
          businessUsageFeePrice,
          businessUsageFeeTotal
        );

    return new OzdsExpenditureData(
      highEnergyItem,
      lowEnergyItem,
      energyItem,
      highReactiveEnergyItem,
      lowReactiveEnergyItem,
      reactiveEnergyItem,
      maxPowerItem,
      iotDeviceFee,
      renewableEnergyFee,
      businessUsageFee,
      (energyItem?.Total ?? 0.0M)
      + (highEnergyItem?.Total ?? 0.0M)
      + (lowEnergyItem?.Total ?? 0.0M)
      + (reactiveEnergyItem?.Total ?? 0.0M)
      + (highReactiveEnergyItem?.Total ?? 0.0M)
      + (lowReactiveEnergyItem?.Total ?? 0.0M)
      + (maxPowerItem?.Total ?? 0.0M)
      + (iotDeviceFee?.Total ?? 0.0M)
      + (renewableEnergyFee?.Total ?? 0.0M)
      + (businessUsageFee?.Total ?? 0.0M)
    );
  }
}
