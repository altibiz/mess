namespace Mess.Iot.Iot;

public readonly record struct EgaugeRegister(
  EgaugeRegisterType Type,
  EgaugeRegisterUnit Unit,
  decimal Value
);
