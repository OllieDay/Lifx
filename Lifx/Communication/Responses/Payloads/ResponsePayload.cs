namespace Lifx.Communication.Responses.Payloads;

// Contains information pertaining to a response.
internal record ResponsePayload
{
	public static ResponsePayload Empty { get; } = new ResponsePayload();
}
