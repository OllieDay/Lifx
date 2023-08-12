using System;
using System.Linq;

namespace Lifx.Communication.Requests.Payloads;

// Contains information pertaining to a request.
internal class RequestPayload
{
	public static RequestPayload Empty { get; } = new RequestPayload();

	public virtual byte[] GetData()
		=> Array.Empty<byte>();

	protected static byte[] CombineArrays(params byte[][] arrays)
		=> arrays.SelectMany(array => array).ToArray();
}
