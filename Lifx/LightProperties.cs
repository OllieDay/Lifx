namespace Lifx
{
	public sealed class LightProperties
	{
		internal LightProperties(Label label, Power power, Percentage brightness, Temperature temperature, Color color)
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
	}
}
