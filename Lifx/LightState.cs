namespace Lifx
{
	// Represents the state of a light.
	public struct LightState
	{
		internal LightState(Label label, Power power, Percentage brightness, Temperature temperature, Color color)
		{
			Label = label;
			Power = power;
			Brightness = brightness;
			Temperature = temperature;
			Color = color;
		}

		public Label Label { get; }
		public Power Power { get; }
		public Percentage Brightness { get; }
		public Temperature Temperature { get; }
		public Color Color { get; }

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
}
