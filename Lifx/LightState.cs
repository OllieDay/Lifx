namespace Lifx;

// Represents the state of a light.
public readonly record struct LightState
(
	Label Label,
	Power Power,
	Percentage Brightness,
	Temperature Temperature,
	Color Color
)
{
	public override string ToString()
	{
		return string.Format(
			"[Label: {0}; Power: {1}; Brightness: {2}; Temperature: {3}; Color: {4}]",
			Label,
			Power,
			Brightness,
			Temperature,
			Color
		);
	}
}
