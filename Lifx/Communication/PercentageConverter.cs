using System;

namespace Lifx.Communication
{
	// Convert to and from value used by light.
	internal static class PercentageConverter
	{
		// Convert from value used by light (0 to 65535).
		public static Percentage ConvertUInt16ToPercentage(ushort value)
		{
			return value / (double)ushort.MaxValue;
		}

		// Convert to value used by light (0 to 65535).
		public static ushort ConvertPercentageToUInt16(Percentage percentage)
		{
			return (ushort)Math.Round(percentage * ushort.MaxValue);
		}
	}
}
