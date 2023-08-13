namespace Lifx.Communication.Requests;

// Convert ushort, uint and ulong to byte array.
internal static class IntExtensions
{
	public static byte[] GetBytes(this ushort @this)
		=> BitConverter.GetBytes(@this);

	public static byte[] GetBytes(this uint @this)
		=> BitConverter.GetBytes(@this);

	public static byte[] GetBytes(this ulong @this)
		=> BitConverter.GetBytes(@this);
}
