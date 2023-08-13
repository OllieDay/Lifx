namespace Lifx.Communication.Responses.Payloads;

// Contains light state comprised of color, brightness, temperature, power and label.
internal sealed record StateResponsePayload(
	Color Color,
	Percentage Brightness,
	Temperature Temperature,
	Power Power,
	Label Label
) : ResponsePayload
{

}
