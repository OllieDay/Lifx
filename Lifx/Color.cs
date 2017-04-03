namespace Lifx
{
	// Represents a color comprised of hue and saturation.
	public struct Color
	{
		public Color(Hue hue, Percentage saturation)
		{
			Hue = hue;
			Saturation = saturation;
		}

		public Hue Hue { get; }
		public Percentage Saturation { get; }

		internal static Color None { get; } = new Color(Hue.MinValue, Percentage.MinValue);

		public override string ToString()
		{
			return $"[Hue: {Hue}; Saturation: {Saturation}]";
		}
	}
}
