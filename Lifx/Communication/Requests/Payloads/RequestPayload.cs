namespace Lifx.Communication.Requests.Payloads;

// Contains information pertaining to a request.
internal record RequestPayload
{
	public static RequestPayload Empty { get; } = new RequestPayload();

	public virtual byte[] GetData()
		=> Array.Empty<byte>();

	protected static byte[] CombineArrays(params byte[][] arrays)
		=> arrays.SelectMany(array => array).ToArray();
}
