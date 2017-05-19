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

		public static int MinValue => 2500;
		public static int MaxValue => 9000;

		public static Temperature BlueIce => 9000;
		public static Temperature BlueWater => 8500;
		public static Temperature BlueOvercast => 8000;
		public static Temperature BlueDaylight => 7500;
		public static Temperature CloudyDaylight => 7000;
		public static Temperature BrightDaylight => 6500;
		public static Temperature NoonDaylight => 6000;
		public static Temperature Daylight => 5500;
		public static Temperature SoftDaylight => 5000;
		public static Temperature CoolDaylight => 4500;
		public static Temperature Cool => 4000;
		public static Temperature Neutral => 3500;
		public static Temperature NeutralWarm => 3200;
		public static Temperature Warm => 3000;
		public static Temperature Incandescent => 2750;
		public static Temperature UltraWarm => 2500;

		public int Value { get; }

		internal static Temperature None => MinValue;

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
