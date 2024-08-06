namespace Lifx.Communication.Requests.Payloads;

// Contains information used to set a light's color, brightness, and temperature over a duration in milliseconds.
internal sealed record SetColorRequestPayload(
	Color Color,
	Percentage Brightness,
	Temperature Temperature,
	// ReSharper disable once BuiltInTypeReferenceStyle
	// Required to be UInt32 per protocol docs, type uint is not honored
	UInt32 DurationInMilliseconds
) : RequestPayload
{
	public override byte[] GetData()
	{
		var reservedData = new byte[1];
		var hueData = HueConverter.ConvertHueToUInt16(Color.Hue).GetBytes();
		var saturationData = PercentageConverter.ConvertPercentageToUInt16(Color.Saturation).GetBytes();
		var brightnessData = PercentageConverter.ConvertPercentageToUInt16(Brightness).GetBytes();
		var temperatureData = ((ushort)Temperature).GetBytes();
		var durationData = DurationInMilliseconds.GetBytes();

		return CombineArrays(reservedData, hueData, saturationData, brightnessData, temperatureData, durationData);
	}
}
