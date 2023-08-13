namespace Lifx;

internal static class ComparableExtensions
{
	// Check if value is within specified range (inclusive).
	public static bool IsBetween<T>(this T @this, T from, T to) where T : IComparable<T>
		=> @this.CompareTo(from) >= 0 && @this.CompareTo(to) <= 0;
}
