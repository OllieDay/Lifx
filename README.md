# Lifx
Library for controlling LIFX Wi-Fi Smart LED devices over the LAN.

## Overview
This library allows you to communicate with LIFX devices over the LAN without relying on the LIFX HTTP API.
All communication is done over UDP for low-latency device control using the LIFX LAN Protocol.

See [https://lan.developer.lifx.com/docs](https://lan.developer.lifx.com/docs) for more information about the LIFX LAN
Protocol.

## Device creation
The `LightFactory` class provides functionality for creating `ILight` objects from a given IP address. The `ILight`
object contains the IP address, product (i.e. model) and version of the device and is used for all control
operations.

```csharp
var lightFactory = new LightFactory();

using (var light = await lightFactory.CreateLightAsync(IPAddress.Parse("192.168.0.100")))
{
	// ...
}
```

The `LightFactory` class has an overloaded constructor that takes an `int` parameter used to specify the port number
used when communicating with lights. The default port number used with the parameterless constructor is 56700.

```csharp
var lightFactory = new LightFactory(1000);
```

## Device info
Use the overridden `ToString()` method of the `ILight` object to obtain a string representation of the device.

```csharp
Console.WriteLine(light);
```
Output: `[Address: 192.168.0.100; Product: White800HightVoltage; Version: 0]`

## Device state
Use the `GetStateAsync()` method to retrieve the current state of the device, and the overridden `ToString()` method of
the returned `LightState` object to obtain a string representation of the device containing the label, power,
brightness, temperature and color.

```csharp
var state = await light.GetStateAsync();

Console.WriteLine(state);
```
Output: `[Label: Office; Power: On; Brightness: 1; Temperature: 3000; Color: [Hue: 0; Saturation: 0]]`

## Device label
Use the `SetLabelAsync()` method of the `ILight` object to set the device label. A label should consist of no more than
32 bytes with UTF8 encoding.

```csharp
await light.SetLabelAsync("Office");
```

## Device power
Light power can be set using `SetPowerAsync(Power)` where the `Power` parameter is an enum with values `Off` and `On`.

```csharp
await light.SetPowerAsync(Power.Off);
await light.SetPowerAsync(Power.On);
```

The methods `OffAsync()` and `OnAsync()` provide the same functionality.

```csharp
await light.OffAsync();
await light.OnAsync();
```

Each of these methods have an override that takes a `uint` parameter used to specify the duration in milliseconds for
the power change.

```csharp
// Change power over a 10 second duration
await light.SetPowerAsync(Power.Off, 10000);
await light.SetPowerAsync(Power.On, 10000);
await light.OffAsync(10000);
await light.OnAsync(10000);
```

## Device brightness
Light brightness can be set using `SetBrightnessAsync(Percentage)` where the `Percentage` parameter is a `float` between
0 and 1.

```csharp
await light.SetBrightnessAsync(0.5);
```

This method has an override that takes a `uint` parameter used to specify the duration in milliseconds for the
brightness change.

```csharp
// Change brightness over a 10 second duration
await light.SetBrightnessAsync(0.5, 10000);
```

## Device temperature
Light temperature can be set using `SetTemperatureAsync(Temperature)` where the `Temperature` parameter is an `int`
between 2500 and 9000 representing kelvin value. The `Temperature` struct contains named temperature values accessible
via static properties.

```csharp
// Set temperature to explicit value
await light.SetTemperatureAsync(3500);

// Set temperature to named value
await light.SetTemperatureAsync(Temperature.Warm);
```

Named temperature values:

```csharp
public static Temperature BlueIce { get; } = 9000;
public static Temperature BlueWater { get; } = 8500;
public static Temperature BlueOvercast { get; } = 8000;
public static Temperature BlueDaylight { get; } = 7500;
public static Temperature CloudyDaylight { get; } = 7000;
public static Temperature BrightDaylight { get; } = 6500;
public static Temperature NoonDaylight { get; } = 6000;
public static Temperature Daylight { get; } = 5500;
public static Temperature SoftDaylight { get; } = 5000;
public static Temperature CoolDaylight { get; } = 4500;
public static Temperature Cool { get; } = 4000;
public static Temperature Neutral { get; } = 3500;
public static Temperature NeutralWarm { get; } = 3200;
public static Temperature Warm { get; } = 3000;
public static Temperature Incandescent { get; } = 2750;
public static Temperature UltraWarm { get; } = 2500;
```

This method has an override that takes a `uint` parameter used to specify the duration in milliseconds for the
temperature change.

```csharp
// Change temperature over a 10 second duration
await light.SetTemperatureAsync(3500, 10000);
```

## Device color
Light color can be set using `SetColorAsync(Color)` where the `Color` parameter is a struct comprised of hue
(`int` between 0 and 360) and saturation (`float` between 0 and 1).

```csharp
await light.SetColorAsync(new Color(180, 0.5));
```

This method has an override that takes a `uint` parameter used to specify the duration in milliseconds for the
color change.

```csharp
// Change color over a 10 second duration
await light.SetColorAsync(new Color(180, 0.5), 10000);
```

Note that this method is only applicable to devices that support color. Calling this method on an unsupported device
will result in an `InvalidOperationException` being thrown.
