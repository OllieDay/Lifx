using Lifx.Communication.Responses;

namespace Lifx.Communication.Requests.Payloads.Tests;

public sealed class SetColorRequestPayloadTests
{
	[Fact]
	public void GetDataShouldReturnValidHueData()
	{
		const int hueOffset = 1;

		var color = Color.Cyan;
		var data = CreatePayloadData(color, Percentage.MaxValue, Temperature.None, 0);

		HueConverter.ConvertUInt16ToHue(data.ToUInt16(hueOffset)).Should().Be(color.Hue);
	}

	[Fact]
	public void GetDataShouldReturnValidSaturationData()
	{
		const int saturationOffset = 3;

		var color = Color.Cyan;
		var data = CreatePayloadData(color, Percentage.MaxValue, Temperature.None, 0);

		PercentageConverter.ConvertUInt16ToPercentage(
			data.ToUInt16(saturationOffset)
		).Should().Be(color.Saturation);
	}

	[Fact]
	public void GetDataShouldReturnValidBrightnessData()
	{
		const int brightnessOffset = 5;

		var brightness = new Percentage(Percentage.MaxValue);
		var data = CreatePayloadData(Color.None, brightness, Temperature.None, 0);

		PercentageConverter.ConvertUInt16ToPercentage(data.ToUInt16(brightnessOffset)).Should().Be(brightness);
	}

	[Fact]
	public void GetDataShouldReturnValidTemperatureData()
	{
		const int temperatureOffset = 7;

		var temperature = Temperature.Neutral;
		var data = CreatePayloadData(Color.None, Percentage.MinValue, temperature, 0);

		data.ToUInt16(temperatureOffset).Should().Be((ushort)temperature);
	}

	[Fact]
	public void GetDataShouldReturnValidDurationInMillisecondsData()
	{
		const int durationInMillisecondsOffset = 9;

		var durationInMilliseconds = ushort.MaxValue;

		var data = CreatePayloadData(
			Color.None,
			Percentage.MinValue,
			Temperature.None,
			durationInMilliseconds
		);

		data.ToUInt32(durationInMillisecondsOffset).Should().Be(durationInMilliseconds);
	}

	private static byte[] CreatePayloadData(
		Color color,
		Percentage brightness,
		Temperature temperature,
		uint durationInMilliseconds
	)
	{
		return new SetColorRequestPayload(color, brightness, temperature, durationInMilliseconds).GetData();
	}
}
