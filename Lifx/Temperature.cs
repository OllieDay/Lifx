using System;

namespace Lifx
{
	// Represents light temperature in kelvin from 2500 to 9000.
	public struct Temperature
	{
		public Temperature(int value)
		{
			if (!value.IsBetween(MinValue, MaxValue))
			{
				throw new ArgumentOutOfRangeException(nameof(value), value, $"Must be {MinValue} to {MaxValue}.");
			}

			Value = value;
		}

		public static int MinValue { get; } = 2500;
		public static int MaxValue { get; } = 9000;

		public static Temperature None { get; } = MinValue;
		
		public static Temperature BlueIce { get; } = 9000;
		public static Temperature BlueWater { get; } = 8500;
		public static Temperature BlueOvercast { get; } = 8000;
		public static Temperature BlueDaylight { get; } = 7500;
		public static Temperature CloudyDaylight { get; } = 7000;
		public static Temperature BrightDaylight { get; } = 6500;
		public static Temperature NoonDaylight { get; } = 6000;
		public static Temperature Daylight { get; } = 5500;
		public static Temperature SoftDaylight { get; } = 5000;
		public static Temperature CoolDaylight { get; } = 4500;
		public static Temperature Cool { get; } = 4000;
		public static Temperature Neutral { get; } = 3500;
		public static Temperature NeutralWarm { get; } = 3200;
		public static Temperature Warm { get; } = 3000;
		public static Temperature Incandescent { get; } = 2750;
		public static Temperature UltraWarm { get; } = 2500;

		public int Value { get; }

		public static implicit operator int(Temperature temperature)
		{
			return temperature.Value;
		}

		public static implicit operator Temperature(int value)
		{
			return new Temperature(value);
		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
