using System.Linq;
using Lifx.Communication.Responses.Payloads;

namespace Lifx.Communication.Responses
{
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

		public bool TryParseResponse(byte[] data, out Response response)
		{
			response = null;

			if (data.Length < ResponseLength)
			{
				return false;
			}

			if (!TryParsePayload(data, out var payload))
			{
				return false;
			}

			var sequence = data[23];
			response = new Response(sequence, payload);

			return true;
		}

		private bool TryParsePayload(byte[] data, out ResponsePayload payload)
		{
			payload = null;

			var command = ParseCommand(data);
			var payloadData = ParsePayloadData(data);

			switch (command)
			{
				case Command.DeviceAcknowledgement:
					payload = ResponsePayload.Empty;
					break;
				case Command.DeviceStateVersion:
					payload = _stateVersionResponsePayloadParser.Parse(payloadData);
					break;
				case Command.LightState:
					payload = _stateResponsePayloadParser.Parse(payloadData);
					break;
				default:
					return false;
			}

			return true;
		}

		private static Command ParseCommand(byte[] data)
			=> (Command)data.ToUInt16(startIndex: 32);

		private static byte[] ParsePayloadData(byte[] data)
		{
			int payloadLength = data.Length - ResponseLength;

			return data.Skip(ResponseLength).Take(payloadLength).ToArray();
		}
	}
}
