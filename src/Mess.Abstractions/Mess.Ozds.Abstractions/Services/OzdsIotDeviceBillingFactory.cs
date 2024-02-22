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
      Math.Round(catalogueItem.OperatorCataloguePart.Value.EnergyPrice.Value ?? 0.0M, 6);
    var energyMin = Math.Round(billingData.MinEnergyImportTotal_kWh, 2);
    var energyMax = Math.Round(billingData.MaxEnergyImportTotal_kWh, 2);
    var energyAmount = Math.Round(energyMax - energyMin, 0);
    var energyTotal = Math.Round(energyAmount * energyPrice, 2);
    var energyItem =
      energyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          energyMin,
          energyMax,
          null,
          null,
          null,
          null,
          energyAmount,
          energyPrice,
          energyTotal
        );

    var highEnergyPrice =
      Math.Round(catalogueItem.OperatorCataloguePart.Value.HighEnergyPrice.Value ?? 0.0M, 6);
    var highEnergyMin = Math.Round(billingData.HighTariffMinEnergyImportTotal_kWh, 2);
    var highEnergyMax = Math.Round(billingData.HighTariffMaxEnergyImportTotal_kWh, 2);
    var highEnergyAmount = Math.Round(highEnergyMax - highEnergyMin, 0);
    var highEnergyTotal = Math.Round(highEnergyAmount * highEnergyPrice, 2);
    var highEnergyItem =
      highEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          highEnergyMin,
          highEnergyMax,
          null,
          null,
          null,
          null,
          highEnergyAmount,
          highEnergyPrice,
          highEnergyTotal
        );

    var lowEnergyPrice =
      Math.Round(catalogueItem.OperatorCataloguePart.Value.LowEnergyPrice.Value ?? 0.0M, 6);
    var lowEnergyMin = Math.Round(billingData.LowTariffMinEnergyImportTotal_kWh, 2);
    var lowEnergyMax = Math.Round(billingData.LowTariffMaxEnergyImportTotal_kWh, 2);
    var lowEnergyAmount = Math.Round(lowEnergyMax - lowEnergyMin, 0);
    var lowEnergyTotal = Math.Round(lowEnergyAmount * lowEnergyPrice, 2);
    var lowEnergyItem =
      lowEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          lowEnergyMin,
          lowEnergyMax,
          null,
          null,
          null,
          null,
          lowEnergyAmount,
          lowEnergyPrice,
          lowEnergyTotal
        );

    var reactiveEnergyPrice =
      Math.Round(catalogueItem.OperatorCataloguePart.Value.ReactiveEnergyPrice.Value ?? 0.0M, 6);
    var reactiveEnergyThreshold = (billingData.MaxEnergyImportTotal_kWh - billingData.MinEnergyImportTotal_kWh) * 0.33M;
    var reactiveEnergyInductiveMin = Math.Round(billingData.MinReactiveEnergyImportTotal_kVARh, 2);
    var reactiveEnergyInductiveMax = Math.Round(billingData.MaxReactiveEnergyImportTotal_kVARh, 2);
    var reactiveEnergyCapacitativeMin = Math.Round(billingData.MinReactiveEnergyExportTotal_kVARh, 2);
    var reactiveEnergyCapacitativeMax = Math.Round(billingData.MaxReactiveEnergyExportTotal_kVARh, 2);
    var reactiveEnergyInductive = Math.Abs(reactiveEnergyInductiveMax - reactiveEnergyInductiveMin);
    var reactiveEnergyCapacitive = Math.Abs(reactiveEnergyCapacitativeMax - reactiveEnergyCapacitativeMin);
    var reactiveEnergyAmount = Math.Round(Math.Max(reactiveEnergyInductive + reactiveEnergyCapacitive - reactiveEnergyThreshold, 0.0M), 0);
    var reactiveEnergyTotal = Math.Round(reactiveEnergyAmount * reactiveEnergyPrice, 2);
    var reactiveEnergyItem =
      reactiveEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          null,
          null,
          reactiveEnergyInductiveMin,
          reactiveEnergyInductiveMax,
          reactiveEnergyCapacitativeMin,
          reactiveEnergyCapacitativeMax,
          reactiveEnergyAmount,
          reactiveEnergyPrice,
          reactiveEnergyTotal
        );

    var maxPowerPrice =
      Math.Round(catalogueItem.OperatorCataloguePart.Value.MaxPowerPrice.Value ?? 0.0M, 3);
    var maxPowerAmount = Math.Round(billingData.PeakPower_kW, 0);
    var maxPowerTotal = Math.Round(maxPowerAmount * maxPowerPrice, 2);
    var maxPowerItem =
      maxPowerPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          null,
          null,
          null,
          null,
          null,
          null,
          maxPowerAmount,
          maxPowerPrice,
          maxPowerTotal
        );

    var iotDeviceFeePrice =
      Math.Round(catalogueItem.OperatorCataloguePart.Value.IotDeviceFee.Value ?? 0.0M, 6);
    var iotDeviceFeeAmount = 1M;
    var iotDeviceFeeTotal = Math.Round(iotDeviceFeeAmount * iotDeviceFeePrice, 2);
    var iotDeviceFee =
      iotDeviceFeePrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          null,
          null,
          null,
          null,
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
      Math.Round(catalogueItem.RegulatoryAgencyCataloguePart.Value.HighEnergyPrice.Value ?? 0.0M, 6);
    var highEnergyMin = Math.Round(billingData.HighTariffMinEnergyImportTotal_kWh, 2);
    var highEnergyMax = Math.Round(billingData.HighTariffMaxEnergyImportTotal_kWh, 2);
    var highEnergyAmount = Math.Round(highEnergyMax - highEnergyMin, 0);
    var highEnergyTotal = Math.Round(highEnergyAmount * highEnergyPrice, 2);
    var highEnergyItem =
      highEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          highEnergyMin,
          highEnergyMax,
          null,
          null,
          null,
          null,
          highEnergyAmount,
          highEnergyPrice,
          highEnergyTotal
        );

    var lowEnergyPrice =
      Math.Round(catalogueItem.RegulatoryAgencyCataloguePart.Value.LowEnergyPrice.Value ?? 0.0M, 6);
    var lowEnergyMin = Math.Round(billingData.LowTariffMinEnergyImportTotal_kWh, 2);
    var lowEnergyMax = Math.Round(billingData.LowTariffMaxEnergyImportTotal_kWh, 2);
    var lowEnergyAmount = Math.Round(lowEnergyMax - lowEnergyMin, 0);
    var lowEnergyTotal = Math.Round(lowEnergyAmount * lowEnergyPrice, 2);
    var lowEnergyItem =
      lowEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          lowEnergyMin,
          lowEnergyMax,
          null,
          null,
          null,
          null,
          lowEnergyAmount,
          lowEnergyPrice,
          lowEnergyTotal
        );

    var renewableEnergyFeePrice =
      Math.Round(catalogueItem.RegulatoryAgencyCataloguePart.Value.RenewableEnergyFee.Value ?? 0.0M, 6);
    var renewableEnergyFeeMin = Math.Round(billingData.MinEnergyImportTotal_kWh, 2);
    var renewableEnergyFeeMax = Math.Round(billingData.MaxEnergyImportTotal_kWh, 2);
    var renewableEnergyFeeAmount = Math.Round(renewableEnergyFeeMax - renewableEnergyFeeMin, 0);
    var renewableEnergyFeeTotal = Math.Round(renewableEnergyFeeAmount * renewableEnergyFeePrice, 2);
    var renewableEnergyFee =
      renewableEnergyFeePrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          renewableEnergyFeeMin,
          renewableEnergyFeeMax,
          null,
          null,
          null,
          null,
          renewableEnergyFeeAmount,
          renewableEnergyFeePrice,
          renewableEnergyFeeTotal
        );

    var businessUsageFeePrice =
      Math.Round(catalogueItem.RegulatoryAgencyCataloguePart.Value.BusinessUsageFee.Value ?? 0.0M, 6);
    var businessUsageFeeMin = Math.Round(billingData.MinEnergyImportTotal_kWh, 2);
    var businessUsageFeeMax = Math.Round(billingData.MaxEnergyImportTotal_kWh, 2);
    var businessUsageFeeAmount = Math.Round(businessUsageFeeMax - businessUsageFeeMin, 0);
    var businessUsageFeeTotal = Math.Round(businessUsageFeeAmount * businessUsageFeePrice, 2);
    var businessUsageFee =
      businessUsageFeePrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          businessUsageFeeMin,
          businessUsageFeeMax,
          null,
          null,
          null,
          null,
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
