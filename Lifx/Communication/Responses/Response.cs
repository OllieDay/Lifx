using System;
using Lifx.Communication.Responses.Payloads;

namespace Lifx.Communication.Responses
{
	// Sent by a light in response to a request.
	// The sequence is a wrap around value sent in a request and included in the response used for message tracking.
	internal sealed class Response
	{
		public Response(byte sequence, ResponsePayload payload)
		{
			Sequence = sequence;
			Payload = payload;
		}

		public byte Sequence { get; }
		public ResponsePayload Payload { get; }
		public DateTime CreationDate { get; } = DateTime.UtcNow;
	}
}
