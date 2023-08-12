using System;

namespace Lifx;

// Represents a hue value from 0 to 360.
public struct Hue
{
	public Hue(int value)
	{
		if (!value.IsBetween(MinValue, MaxValue))
		{
			throw new ArgumentOutOfRangeException(nameof(value), value, $"Must be {MinValue} to {MaxValue}.");
		}

		Value = value;
	}

	public static int MinValue { get; } = 0;
	public static int MaxValue { get; } = 360;

	public int Value { get; }

	public static implicit operator int(Hue hue)
		=> hue.Value;

	public static implicit operator Hue(int value)
		=> new Hue(value);

	public override string ToString()
		=> Value.ToString();
}
