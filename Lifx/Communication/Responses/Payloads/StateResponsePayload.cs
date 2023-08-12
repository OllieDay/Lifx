namespace Lifx.Communication.Responses.Payloads;

// Contains light state comprised of color, brightness, temperature, power and label.
internal sealed class StateResponsePayload : ResponsePayload
{
	public StateResponsePayload(
		Color color,
		Percentage brightness,
		Temperature temperature,
		Power power,
		Label label
	)
	{
		Color = color;
		Brightness = brightness;
		Temperature = temperature;
		Power = power;
		Label = label;
	}

	public Color Color { get; }
	public Percentage Brightness { get; }
	public Temperature Temperature { get; }
	public Power Power { get; }
	public Label Label { get; }
}
