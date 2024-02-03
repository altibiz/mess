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
    var energyAmount =
      billingData.EndEnergyTotal_kWh - billingData.StartEnergyTotal_kWh;
    var energyTotal = energyAmount * energyPrice;
    var energyItem =
      energyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          billingData.StartEnergyTotal_kWh,
          billingData.EndEnergyTotal_kWh,
          energyAmount,
          energyPrice,
          energyTotal
        );

    var highEnergyPrice =
      catalogueItem.OperatorCataloguePart.Value.HighEnergyPrice.Value ?? 0.0M;
    var highEnergyAmount =
      billingData.HighEndEnergyTotal_kWh - billingData.HighStartEnergyTotal_kWh;
    var highEnergyTotal = highEnergyAmount * highEnergyPrice;
    var highEnergyItem =
      highEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          billingData.HighStartEnergyTotal_kWh,
          billingData.HighEndEnergyTotal_kWh,
          highEnergyAmount,
          highEnergyPrice,
          highEnergyTotal
        );

    var lowEnergyPrice =
      catalogueItem.OperatorCataloguePart.Value.LowEnergyPrice.Value ?? 0.0M;
    var lowEnergyAmount =
      billingData.LowEndEnergyTotal_kWh - billingData.LowStartEnergyTotal_kWh;
    var lowEnergyTotal = lowEnergyAmount * lowEnergyPrice;
    var lowEnergyItem =
      lowEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          billingData.LowStartEnergyTotal_kWh,
          billingData.LowEndEnergyTotal_kWh,
          lowEnergyAmount,
          lowEnergyPrice,
          lowEnergyTotal
        );

    var reactiveEnergyPrice =
      catalogueItem.OperatorCataloguePart.Value.ReactiveEnergyPrice.Value ?? 0.0M;
    var reactiveEnergyAmount =
      billingData.EndReactiveEnergyTotal_kWh - billingData.StartReactiveEnergyTotal_kWh;
    var reactiveEnergyTotal = reactiveEnergyAmount * reactiveEnergyPrice;
    var reactiveEnergyItem =
      reactiveEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          billingData.StartReactiveEnergyTotal_kWh,
          billingData.EndReactiveEnergyTotal_kWh,
          reactiveEnergyAmount,
          reactiveEnergyPrice,
          reactiveEnergyTotal
        );

    var highReactiveEnergyPrice =
      catalogueItem.OperatorCataloguePart.Value.HighReactiveEnergyPrice.Value ?? 0.0M;
    var highReactiveEnergyAmount =
      billingData.HighEndReactiveEnergyTotal_kWh - billingData.HighStartReactiveEnergyTotal_kWh;
    var highReactiveEnergyTotal = highReactiveEnergyAmount * highReactiveEnergyPrice;
    var highReactiveEnergyItem =
      highReactiveEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          billingData.HighStartReactiveEnergyTotal_kWh,
          billingData.HighEndReactiveEnergyTotal_kWh,
          highReactiveEnergyAmount,
          highReactiveEnergyPrice,
          highReactiveEnergyTotal
        );

    var lowReactiveEnergyPrice =
      catalogueItem.OperatorCataloguePart.Value.LowReactiveEnergyPrice.Value ?? 0.0M;
    var lowReactiveEnergyAmount =
      billingData.LowEndReactiveEnergyTotal_kWh - billingData.LowStartReactiveEnergyTotal_kWh;
    var lowReactiveEnergyTotal = lowReactiveEnergyAmount * lowReactiveEnergyPrice;
    var lowReactiveEnergyItem =
      lowReactiveEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          billingData.LowStartReactiveEnergyTotal_kWh,
          billingData.LowEndReactiveEnergyTotal_kWh,
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
          0.0M,
          0.0M,
          renewableEnergyFeeAmount,
          renewableEnergyFeePrice,
          renewableEnergyFeeTotal
        );

    var businessUsageFeePrice =
      regulatoryAgencyCatalogueItem?.RegulatoryAgencyCataloguePart.Value.RenewableEnergyFee.Value ?? 0.0M;
    var businessUsageFeeAmount =
      billingData.EndEnergyTotal_kWh - billingData.StartEnergyTotal_kWh;
    var businessUsageFeeTotal = businessUsageFeeAmount * businessUsageFeePrice;
    var businessUsageFee =
      businessUsageFeePrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          0.0M,
          0.0M,
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
      + (maxPowerItem?.Total ?? 0.0M)
      + (iotDeviceFee?.Total ?? 0.0M)
      + (renewableEnergyFee?.Total ?? 0.0M)
      + (businessUsageFee?.Total ?? 0.0M)
    );
  }
}
