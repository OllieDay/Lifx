using System;

namespace Lifx
{
	internal static class ComparableExtensions
	{
		// Check if value is within specified range (inclusive).
		public static bool InRange<T>(this T @this, T from, T to) where T : IComparable<T>
		{
			return @this.CompareTo(from) >= 0 && @this.CompareTo(to) <= 0;
		}
	}
}
