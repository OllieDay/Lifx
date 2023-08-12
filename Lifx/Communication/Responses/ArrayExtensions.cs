using System;

namespace Lifx.Communication.Responses;

// Convert byte array to ushort, uint and ulong at the specified start index.
internal static class ArrayExtensions
{
	public static ushort ToUInt16(this byte[] @this, int startIndex)
		=> BitConverter.ToUInt16(@this, startIndex);

	public static uint ToUInt32(this byte[] @this, int startIndex)
		=> BitConverter.ToUInt32(@this, startIndex);

	public static ulong ToUInt64(this byte[] @this, int startIndex)
		=> BitConverter.ToUInt64(@this, startIndex);
}
