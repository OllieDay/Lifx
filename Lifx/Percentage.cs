using System;

namespace Lifx;

// Represents a percentage value from 0 to 1.
public struct Percentage
{
	public Percentage(double value)
	{
		if (!value.IsBetween(MinValue, MaxValue))
		{
			throw new ArgumentOutOfRangeException(nameof(value), value, $"Must be {MinValue} to {MaxValue}.");
		}

		Value = value;
	}

	public static double MinValue { get; } = 0;
	public static double MaxValue { get; } = 1;

	public double Value { get; }

	public static implicit operator double(Percentage percentage)
		=> percentage.Value;

	public static implicit operator Percentage(double value)
		=> new Percentage(value);

	public override string ToString()
		=> Value.ToString();
}
