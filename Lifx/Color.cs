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

		public static Color None { get; } = White;

		public static Color White { get; } = new Color(0, 0);
		public static Color Red { get; } = new Color(0, 1);
		public static Color Orange { get; } = new Color(36, 1);
		public static Color Yellow { get; } = new Color(60, 1);
		public static Color Green { get; } = new Color(120, 1);
		public static Color Cyan { get; } = new Color(180, 1);
		public static Color Blue { get; } = new Color(250, 1);
		public static Color Purple { get; } = new Color(280, 1);
		public static Color Pink { get; } = new Color(325, 1);

		public Hue Hue { get; }
		public Percentage Saturation { get; }

		public override string ToString()
		{
			return $"[Hue: {Hue}; Saturation: {Saturation}]";
		}
	}
}
