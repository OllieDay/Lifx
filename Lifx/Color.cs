﻿namespace Lifx;

// Represents a color comprised of hue and saturation.
public readonly record struct Color(Hue Hue, Percentage Saturation)
{
	public static Color None { get; } = new Color(0, 0);

	public static Color White { get; } = new Color(0, 0);
	public static Color Red { get; } = new Color(0, 1);
	public static Color Orange { get; } = new Color(36, 1);
	public static Color Yellow { get; } = new Color(60, 1);
	public static Color Green { get; } = new Color(120, 1);
	public static Color Cyan { get; } = new Color(180, 1);
	public static Color Blue { get; } = new Color(250, 1);
	public static Color Purple { get; } = new Color(280, 1);
	public static Color Pink { get; } = new Color(325, 1);

	public override string ToString()
		=> $"[Hue: {Hue}; Saturation: {Saturation}]";
}
