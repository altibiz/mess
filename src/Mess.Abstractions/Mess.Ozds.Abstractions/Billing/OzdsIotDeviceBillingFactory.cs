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
      usageCatalogueItem
    );
    var supplyExpenditure = CreateExpenditureData(
      billingData,
      supplyCatalogueItem
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
    OperatorCatalogueItem catalogueItem
  )
  {
    var highTariffEnergyPrice =
      catalogueItem.OperatorCataloguePart.Value.HighEnergyPrice.Value ?? 0.0M;
    var highTariffEnergyAmount =
      billingData.EndHighTariffEnergy_kWh
      - billingData.StartHighTariffEnergy_kWh;
    var highTariffEnergyTotal = highTariffEnergyAmount * highTariffEnergyPrice;
    var highTariffEnergyItem =
      highTariffEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          billingData.StartHighTariffEnergy_kWh,
          billingData.EndHighTariffEnergy_kWh,
          highTariffEnergyAmount,
          highTariffEnergyPrice,
          highTariffEnergyTotal
        );

    var lowTariffEnergyPrice =
      catalogueItem.OperatorCataloguePart.Value.LowEnergyPrice.Value ?? 0.0M;
    var lowTariffEnergyAmount =
      billingData.EndLowTariffEnergy_kWh - billingData.StartLowTariffEnergy_kWh;
    var lowTariffEnergyTotal = lowTariffEnergyAmount * lowTariffEnergyPrice;
    var lowTariffEnergyItem =
      lowTariffEnergyPrice == 0.0M
        ? null
        : new OzdsExpenditureItemData(
          billingData.StartLowTariffEnergy_kWh,
          billingData.EndLowTariffEnergy_kWh,
          lowTariffEnergyAmount,
          lowTariffEnergyPrice,
          lowTariffEnergyTotal
        );

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

    return new OzdsExpenditureData(
      highTariffEnergyItem,
      lowTariffEnergyItem,
      energyItem,
      maxPowerItem,
      iotDeviceFee,
      (highTariffEnergyItem?.Total ?? 0.0M)
      + (lowTariffEnergyItem?.Total ?? 0.0M)
      + (energyItem?.Total ?? 0.0M)
      + (maxPowerItem?.Total ?? 0.0M)
      + (iotDeviceFee?.Total ?? 0.0M)
    );
  }
}
