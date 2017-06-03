using System;
using System.Linq;
using System.Net;
using System.Threading;
using Lifx.Communication.Requests.Payloads;
using Lifx.Communication.Responses;

namespace Lifx.Communication.Requests
{
	internal sealed class RequestFactory
	{
		// The `source` identifier allows each client to provide an unique value,
		// which will be included by the LIFX device in any message that is sent in
		// response to a message sent by the client.
		// If the `source` identifier is a non-zero value, then the LIFX device will
		// send a unicast message to the source IP address and port that the client
		// used to send the originating message.
		// If the `source` identifier is a zero value, then the LIFX device may send
		// a broadcast message that can be received by all clients on the same sub-net.
		// See `ack_required` and `res`_required` fields in the Frame Address.
		private const uint Source = 1;

		// The `target` device address is 8 bytes long, when using the 6 byte MAC address
		// then left-justify the value and zero-fill the last two bytes.
		// A `target` device address of all zeroes effectively addresses all devices
		// on the local network.  The Frame `tagged` field must be set accordingly.
		private const ulong Target = 0;

		// The `sequence` number allows the client to provide a unique value,
		// which will be included by the LIFX device in any message that is sent in
		// response to a message sent by the client.
		// This allows the client to distinguish between different messages sent with
		// the same `source` identifier in the Frame.
		// See `ack_required` and `res_required` fields in the Frame Address.
		private int _sequence;

		public Request CreateGetVersionRequest()
		{
			return CreateRequest(
				Command.DeviceGetVersion,
				ackRequired: false,
				resRequired: true,
				payload: RequestPayload.Empty
			);
		}

		public Request CreateGetRequest()
		{
			return CreateRequest(
				Command.LightGet,
				ackRequired: false,
				resRequired: true,
				payload: RequestPayload.Empty
			);
		}

		public Request CreateSetLabelRequest(Label label)
		{
			return CreateRequest(
				Command.DeviceSetLabel,
				ackRequired: true,
				resRequired: false,
				payload: new SetLabelRequestPayload(label)
			);
		}

		public Request CreateSetPowerRequest(Power power, uint durationInMilliseconds)
		{
			return CreateRequest(
				Command.DeviceSetPower,
				ackRequired: true,
				resRequired: false,
				payload: new SetPowerRequestPayload(power, durationInMilliseconds)
			);
		}

		public Request CreateSetColorRequest(
			Color color,
			Percentage brightness,
			Temperature temperature,
			uint durationInMilliseconds
		)
		{
			return CreateRequest(
				Command.LightSetColor,
				ackRequired: true,
				resRequired: false,
				payload: new SetColorRequestPayload(color, brightness, temperature, durationInMilliseconds)
			);
		}

		private Request CreateRequest(Command command, bool ackRequired, bool resRequired, RequestPayload payload)
		{
			// Response will contain the same sequence so it can be identified.
			var sequence = (byte)Interlocked.Increment(ref _sequence);

			return new Request(
				command: command,
				ackRequired: ackRequired,
				resRequired: resRequired,
				sequence: sequence,
				source: Source,
				target: Target,
				payload: payload
			);
		}
	}
}
