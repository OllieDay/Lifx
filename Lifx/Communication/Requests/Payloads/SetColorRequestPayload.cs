namespace Lifx.Communication.Requests.Payloads;

// Contains information used to set a light's color, brightness, and temperature over a duration in milliseconds.
internal sealed class SetColorRequestPayload : RequestPayload
{
	private readonly Color _color;
	private readonly Percentage _brightness;
	private readonly Temperature _temperature;
	private readonly uint _durationInMilliseconds;

	public SetColorRequestPayload(
		Color color,
		Percentage brightness,
		Temperature temperature,
		uint durationInMilliseconds
	)
	{
		_color = color;
		_brightness = brightness;
		_temperature = temperature;
		_durationInMilliseconds = durationInMilliseconds;
	}

	public override byte[] GetData()
	{
		var reservedData = new byte[1];
		var hueData = HueConverter.ConvertHueToUInt16(_color.Hue).GetBytes();
		var saturationData = PercentageConverter.ConvertPercentageToUInt16(_color.Saturation).GetBytes();
		var brightnessData = PercentageConverter.ConvertPercentageToUInt16(_brightness).GetBytes();
		var temperatureData = ((ushort)_temperature).GetBytes();
		var durationData = _durationInMilliseconds.GetBytes();

		return CombineArrays(reservedData, hueData, saturationData, brightnessData, temperatureData, durationData);
	}
}
