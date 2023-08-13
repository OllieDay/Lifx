using Lifx.Communication.Responses.Payloads;

namespace Lifx.Communication.Responses;

// Sent by a light in response to a request.
// The sequence is a wrap around value sent in a request and included in the response used for message tracking.
internal sealed record Response(byte Sequence, ResponsePayload Payload)
{
	public DateTime CreationDate { get; } = DateTime.UtcNow;
}
