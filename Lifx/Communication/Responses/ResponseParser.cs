using Lifx.Communication.Responses.Payloads;

namespace Lifx.Communication.Responses;

// A response consists of a 36 byte header and (optional) payload.
internal sealed class ResponseParser : IResponseParser
{
	private const int ResponseLength = 36;

	private readonly IResponsePayloadParser<StateVersionResponsePayload> _stateVersionResponsePayloadParser;
	private readonly IResponsePayloadParser<StateResponsePayload> _stateResponsePayloadParser;

	public ResponseParser(
		IResponsePayloadParser<StateVersionResponsePayload> stateVersionResponsePayloadParser,
		IResponsePayloadParser<StateResponsePayload> stateResponsePayloadParser
	)
	{
		_stateVersionResponsePayloadParser = stateVersionResponsePayloadParser;
		_stateResponsePayloadParser = stateResponsePayloadParser;
	}

	public Response? TryParseResponse(byte[] data)
	{
		if (data.Length < ResponseLength)
		{
			return null;
		}

		var payload = TryParsePayload(data);

		if (payload == null)
		{
			return null;
		}

		var sequence = data[23];

		return new Response(sequence, payload);
	}

	private ResponsePayload? TryParsePayload(byte[] data)
	{
		var command = ParseCommand(data);
		var payloadData = ParsePayloadData(data);

		return command switch
		{
			Command.DeviceAcknowledgement => ResponsePayload.Empty,
			Command.DeviceStateVersion => _stateVersionResponsePayloadParser.Parse(payloadData),
			Command.LightState => _stateResponsePayloadParser.Parse(payloadData),
			_ => null,
		};
	}

	private static Command ParseCommand(byte[] data)
		=> (Command)data.ToUInt16(startIndex: 32);

	private static byte[] ParsePayloadData(byte[] data)
	{
		int payloadLength = data.Length - ResponseLength;

		return data.Skip(ResponseLength).Take(payloadLength).ToArray();
	}
}
