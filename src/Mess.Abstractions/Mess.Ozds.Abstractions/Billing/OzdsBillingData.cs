namespace Mess.Ozds.Abstractions.Billing;

public record OzdsBillingData(
  decimal StartEnergy,
  decimal EndEnergy,
  decimal StartHighEnergy,
  decimal EndHighEnergy,
  decimal StartLowEnergy,
  decimal EndLowEnergy,
  decimal MaxPower
);
