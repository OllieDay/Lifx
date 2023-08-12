using System;

namespace Lifx.Communication;

// Convert to and from value used by light.
internal static class HueConverter
{
	// Convert from value used by light (0 to 65535).
	public static Hue ConvertUInt16ToHue(ushort value)
		=> (Hue)Math.Round((value / (double)ushort.MaxValue) * Hue.MaxValue);

	// Convert to value used by light (0 to 65535).
	public static ushort ConvertHueToUInt16(Hue hue)
		=> (ushort)Math.Round((hue * ushort.MaxValue) / (double)Hue.MaxValue);
}
