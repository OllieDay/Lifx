using System;

namespace Lifx
{
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

		public static int MinValue => 0;
		public static int MaxValue => 360;

		public int Value { get; }

		public static implicit operator int(Hue hue)
		{
			return hue.Value;
		}

		public static implicit operator Hue(int value)
		{
			return new Hue(value);
		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
