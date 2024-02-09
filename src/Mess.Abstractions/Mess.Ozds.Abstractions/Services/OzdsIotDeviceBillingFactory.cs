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
    ContentItem iotDeviceItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return CreateCalculationAsync(
      distributionSystemUnitItem,
      closedDistributionSystemItem,
      distributionSystemOperatorItem,
      iotDeviceItem,
      fromDate,
      toDate
    ).Result;
  }

  public async Task<OzdsCalculationData> CreateCalculationAsync(
    DistributionSystemUnitItem distributionSystemUnitItem,
    ClosedDistributionSystemItem closedDistributionSystemItem,
    DistributionSystemOperatorItem distributionSystemOperatorItem,
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
      .GetContentAsync<RegulatoryAgencyCatalogueItem>(ozdsIotDevicePart.SupplyCatalogue.ContentItemIds.First())
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
    OperatorCatalogueItem usageCatalogueItem,
    RegulatoryAgencyCatalogueItem supplyCatalogueItem
  )
  {
    var usageExpenditure = CreateUsageExpenditureData(
      billingData,
      usageCatalogueItem
    );
    var supplyExpenditure = CreateSupplyExpenditureData(
      billingData,
      supplyCatalogueItem
    );

    return new OzdsCalculationData(
      iotDevice,
      usageCatalogueItem,
      supplyCatalogueItem,
      fromDate,
      toDate,
      usageExpenditure,
      supplyExpenditure,
      Math.Round(supplyExpenditure.Total + usageExpenditure.Total, 2)
    );
  }

  private static OzdsUsageExpenditureData CreateUsageExpenditureData(
    OzdsIotDeviceBillingData billingData,
    OperatorCatalogueItem catalogueItem
  )
  {
    var energyPrice =
      catalogueItem.OperatorCataloguePart.Value.EnergyPrice.Value ?? 0.0M;
    var energyStart = Math.Round(billingData.StartEnergyImportTotal_kWh, 2);
    var energyEnd = Math.Round(billingData.EndEnergyImportTotal_kWh, 2);
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
    var highEnergyStart = Math.Round(billingData.HighStartEnergyImportTotal_kWh, 2);
    var highEnergyEnd = Math.Round(billingData.HighEndEnergyImportTotal_kWh, 2);
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
    var lowEnergyStart = Math.Round(billingData.LowStartEnergyImportTotal_kWh, 2);
    var lowEnergyEnd = Math.Round(billingData.LowEndEnergyImportTotal_kWh, 2);
    var lowEnergyAmount = Math.Round(lowEnergyEnd - lowEnergyStart, 2);
    var lowEnergyTotal = Math.Round(lowEnergyAmount * lowEnergyPrice, 2);
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
    var reactiveEnergyThreshold = (billingData.EndEnergyImportTotal_kWh - billingData.StartEnergyImportTotal_kWh) * 0.33M;
    var reactiveEnergyInductive = Math.Abs(billingData.EndReactiveEnergyImportTotal_kVARh - billingData.StartReactiveEnergyImportTotal_kVARh);
    var reactiveEnergyCapacitive = Math.Abs(billingData.EndReactiveEnergyExportTotal_kVARh - billingData.StartReactiveEnergyExportTotal_kVARh);
    var reactiveEnergyAmount = Math.Round(Math.Max(reactiveEnergyInductive + reactiveEnergyCapacitive - reactiveEnergyThreshold, 0.0M), 2);
    var reactiveEnergyTotal = Math.Round(reactiveEnergyAmount * reactiveEnergyPrice, 2);
    var reactiveEnergyItem =
      reactiveEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          null,
          null,
          reactiveEnergyAmount,
          reactiveEnergyPrice,
          reactiveEnergyTotal
        );

    var maxPowerPrice =
      catalogueItem.OperatorCataloguePart.Value.MaxPowerPrice.Value ?? 0.0M;
    var maxPowerAmount = Math.Round(billingData.PeakPower_kW, 2);
    var maxPowerTotal = Math.Round(maxPowerAmount * maxPowerPrice, 2);
    var maxPowerItem =
      maxPowerPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          null,
          null,
          maxPowerAmount,
          maxPowerPrice,
          maxPowerTotal
        );

    var iotDeviceFeePrice =
      catalogueItem.OperatorCataloguePart.Value.IotDeviceFee.Value ?? 0.0M;
    var iotDeviceFeeAmount = 1.0M;
    var iotDeviceFeeTotal = Math.Round(iotDeviceFeeAmount * iotDeviceFeePrice, 2);
    var iotDeviceFee =
      iotDeviceFeePrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          null,
          null,
          iotDeviceFeeAmount,
          iotDeviceFeePrice,
          iotDeviceFeeTotal
        );

    return new(
      highEnergyItem,
      lowEnergyItem,
      energyItem,
      reactiveEnergyItem,
      maxPowerItem,
      iotDeviceFee,
      (energyItem?.Total ?? 0.0M)
      + (highEnergyItem?.Total ?? 0.0M)
      + (lowEnergyItem?.Total ?? 0.0M)
      + (reactiveEnergyItem?.Total ?? 0.0M)
      + (maxPowerItem?.Total ?? 0.0M)
      + (iotDeviceFee?.Total ?? 0.0M)
    );
  }

  private static OzdsSupplyExpenditureData CreateSupplyExpenditureData(
    OzdsIotDeviceBillingData billingData,
    RegulatoryAgencyCatalogueItem catalogueItem
  )
  {
    var highEnergyPrice =
      catalogueItem.RegulatoryAgencyCataloguePart.Value.HighEnergyPrice.Value ?? 0.0M;
    var highEnergyStart = Math.Round(billingData.HighStartEnergyImportTotal_kWh, 2);
    var highEnergyEnd = Math.Round(billingData.HighEndEnergyImportTotal_kWh, 2);
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
      catalogueItem.RegulatoryAgencyCataloguePart.Value.LowEnergyPrice.Value ?? 0.0M;
    var lowEnergyStart = Math.Round(billingData.LowStartEnergyImportTotal_kWh, 2);
    var lowEnergyEnd = Math.Round(billingData.LowEndEnergyImportTotal_kWh, 2);
    var lowEnergyAmount = Math.Round(lowEnergyEnd - lowEnergyStart, 2);
    var lowEnergyTotal = Math.Round(lowEnergyAmount * lowEnergyPrice, 2);
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

    var renewableEnergyFeePrice =
      catalogueItem.RegulatoryAgencyCataloguePart.Value.RenewableEnergyFee.Value ?? 0.0M;
    var renewableEnergyFeeStart = Math.Round(billingData.StartEnergyImportTotal_kWh, 2);
    var renewableEnergyFeeEnd = Math.Round(billingData.EndEnergyImportTotal_kWh, 2);
    var renewableEnergyFeeAmount = Math.Round(renewableEnergyFeeEnd - renewableEnergyFeeStart, 2);
    var renewableEnergyFeeTotal = Math.Round(renewableEnergyFeeAmount * renewableEnergyFeePrice, 2);
    var renewableEnergyFee =
      renewableEnergyFeePrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          renewableEnergyFeeStart,
          renewableEnergyFeeEnd,
          renewableEnergyFeeAmount,
          renewableEnergyFeePrice,
          renewableEnergyFeeTotal
        );

    var businessUsageFeePrice =
      catalogueItem.RegulatoryAgencyCataloguePart.Value.BusinessUsageFee.Value ?? 0.0M;
    var businessUsageFeeStart = Math.Round(billingData.StartEnergyImportTotal_kWh, 2);
    var businessUsageFeeEnd = Math.Round(billingData.EndEnergyImportTotal_kWh, 2);
    var businessUsageFeeAmount = Math.Round(businessUsageFeeEnd - businessUsageFeeStart, 2);
    var businessUsageFeeTotal = Math.Round(businessUsageFeeAmount * businessUsageFeePrice, 2);
    var businessUsageFee =
      businessUsageFeePrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          businessUsageFeeStart,
          businessUsageFeeEnd,
          businessUsageFeeAmount,
          businessUsageFeePrice,
          businessUsageFeeTotal
        );

    return new(
      highEnergyItem,
      lowEnergyItem,
      renewableEnergyFee,
      businessUsageFee,
      (highEnergyItem?.Total ?? 0.0M)
      + (lowEnergyItem?.Total ?? 0.0M)
      + (renewableEnergyFee?.Total ?? 0.0M)
      + (businessUsageFee?.Total ?? 0.0M)
    );
  }
}
