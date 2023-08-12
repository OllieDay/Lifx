using System.Text;
using Lifx.Communication.Requests;

namespace Lifx.Communication.Responses.Payloads.Tests;

public class StateResponsePayloadParserTests
{
	private static readonly StateResponsePayloadParser Parser = new StateResponsePayloadParser();

	[Fact]
	public void ParseShouldReturnPayloadWithValidColor()
	{
		var color = Color.Cyan;

		var data = CreatePayloadData(color, Percentage.MinValue, Temperature.MinValue, Power.Off, Label.None);

		Parser.Parse(data).Color.Should().Be(color);
	}

	[Fact]
	public void ParseShouldReturnPayloadWithValidTemperature()
	{
		var temperature = new Temperature(Temperature.MaxValue);

		var data = CreatePayloadData(Color.None, Percentage.MinValue, temperature, Power.Off, Label.None);

		Parser.Parse(data).Temperature.Should().Be(temperature);
	}

	[Fact]
	public void ParseShouldReturnPayloadWithValidBrightness()
	{
		var brightness = new Percentage(Percentage.MaxValue);

		var data = CreatePayloadData(Color.None, brightness, Temperature.MinValue, Power.Off, Label.None);

		Parser.Parse(data).Brightness.Should().Be(brightness);
	}

	[Fact]
	public void ParseShouldReturnPayloadWithValidPower()
	{
		var power = Power.On;

		var data = CreatePayloadData(Color.None, Percentage.MinValue, Temperature.MinValue, power, Label.None);

		Parser.Parse(data).Power.Should().Be(power);
	}

	[Fact]
	public void ParseShouldReturnPayloadWithValidLabel()
	{
		var label = new Label("Label");

		var data = CreatePayloadData(Color.None, Percentage.MinValue, Temperature.MinValue, Power.Off, label);

		Parser.Parse(data).Label.Should().Be(label);
	}

	private static byte[] CreatePayloadData(
		Color color,
		Percentage brightness,
		Temperature Temperature,
		Power power,
		Label label
	)
	{
		var hueData = HueConverter.ConvertHueToUInt16(color.Hue).GetBytes();
		var saturationData = PercentageConverter.ConvertPercentageToUInt16(color.Saturation).GetBytes();
		var brightnessData = PercentageConverter.ConvertPercentageToUInt16(brightness).GetBytes();
		var temperatureData = ((ushort)Temperature).GetBytes();
		var reserved1 = new byte[2];
		var powerData = ((ushort)power).GetBytes();
		var labelData = Encoding.UTF8.GetBytes(label);
		var reserved2 = new byte[8];

		return CombineArrays(
			hueData,
			saturationData,
			brightnessData,
			temperatureData,
			reserved1,
			powerData,
			labelData,
			reserved2
		);
	}

	private static byte[] CombineArrays(params byte[][] arrays)
		=> arrays.SelectMany(array => array).ToArray();
}
