using System;

namespace Lifx
{
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

		public static double MinValue => 0;
		public static double MaxValue => 1;

		public double Value { get; }

		public static implicit operator double(Percentage percentage)
		{
			return percentage.Value;
		}

		public static implicit operator Percentage(double value)
		{
			return new Percentage(value);
		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
