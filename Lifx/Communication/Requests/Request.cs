using System.Linq;
using Lifx.Communication.Requests.Payloads;

namespace Lifx.Communication.Requests
{
	// ## Overview
	//
	// Each LIFX Protocol message has the following format ...
	//
	// | Frame | Frame Address | Protocol header | Payload |
	// |-------|---------------|-----------------|---------|
	//
	// The header is composed of the Frame, Frame Address and Protocol header.
	// The Payload is covered separately in the documentation for the various
	// message types.
	//
	// ## Frame
	//
	// The Frame section contains information about the ...
	//
	// * Size of the entire message
	// * LIFX Protocol number: must be 1024 (decimal)
	// * Use of the Frame Address `target` field
	// * Source identifier
	//
	// | Field       | Bits | Type     | Description                                                         |
	// |-------------|------|----------|---------------------------------------------------------------------|
	// | size        | 16   | uint16_t | Size of entire message in bytes including this field                 |
	// | origin      | 2    | uint8_t  | Message origin indicator: must be zero (0)                          |
	// | tagged      | 1    | bool     | Determines usage of Frame Address `target` field                     |
	// | addressable | 1    | bool     | Message includes a target address: must be one (1)                  |
	// | protocol    | 12   | uint16_t | Protocol number: must be 1024 (decimal)                             |
	// | source      | 32   | uint32_t | Source identifier: unique value set by the client, used by responses |
	//
	// The `tagged` field is a boolean flag that indicates whether the Frame Address
	// target field is being used to address an individual device or all devices.
	// For device discovery using Device::GetService
	// the `tagged` field should be set to one (1) and the `target` should be all
	// zeroes.  In all other messages, the `tagged` field should be set to zero (0)
	// and the `target` field should contain the device MAC address. The device will
	// then respond with a Device::StateService
	// message, which will include its own MAC address in the target field. In all
	// subsequent messages that the client sends to the device, the target field
	// should be set to the device MAC address, and the tagged field should be set
	// to zero (0).
	//
	// The `source` identifier allows each client to provide an unique value,
	// which will be included by the LIFX device in any message that is sent in
	// response to a message sent by the client.
	// If the `source` identifier is a non-zero value, then the LIFX device will
	// send a unicast message to the source IP address and port that the client
	// used to send the originating message.
	// If the `source` identifier is a zero value, then the LIFX device may send
	// a broadcast message that can be received by all clients on the same sub-net.
	// See `ack_required` and `res_required` fields in the Frame Address.
	//
	// ## Frame Address
	//
	// The Frame Address section contains routing information about the ...
	//
	// * Target device address
	// * Acknowledgement message is required flag
	// * State response message is required flag
	// * Message sequence number
	//
	// | Field        | Bits | Type       | Description                                                       |
	// |--------------|------|------------|-------------------------------------------------------------------|
	// | target       | 64   | uint64_t   | 6 byte device address (MAC address) or zero (0) means all devices |
	// | reserved     | 48   | uint8_t[6] | Must all be zero (0)                                              |
	// | reserved     | 6    |            | Reserved                                                          |
	// | ack_required | 1    | bool       | Acknowledgement message required                                  |
	// | res_required | 1    | bool       | Response message required                                         |
	// | sequence     | 8    | uint8_t    | Wrap around message sequence number                               |
	//
	// The `target` device address is 8 bytes long, when using the 6 byte MAC address
	// then left-justify the value and zero-fill the last two bytes.
	// A `target` device address of all zeroes effectively addresses all devices
	// on the local network.  The Frame `tagged` field must be set accordingly.
	//
	// There are two flags that cause a LIFX device to send a message in response.
	// In these cases, the `source` identifier in the response message will be set
	// to the same value as that in the requesting message sent by the client.
	//
	// * `ack_required` set to one (1) will cause the device to send an
	// Device::Acknowledgement messafe
	// * `res_required` set to one (1) within a Set message,
	// e.g Light::SetPower
	// will cause the device to send the corresponding State message,
	// e.g Light::StatePower
	//
	// The client can use acknowledgements to determine that the LIFX device
	// has received a message.  However, when using acknowledgements to ensure
	// reliability in an over-burdened lossy network ... causing additional
	// network packets may make the problem worse.
	//
	// Client that don't need to track the updated state of a LIFX device can
	// choose not to request a response, which will reduce the network burden
	// and may provide some performance advantage.  In some cases, a device
	// may choose to send a state update response independent of whether
	// `res_required` is set.
	//
	// The `sequence` number allows the client to provide a unique value,
	// which will be included by the LIFX device in any message that is sent in
	// response to a message sent by the client.
	// This allows the client to distinguish between different messages sent with
	// the same `source` identifier in the Frame.
	// See `ack_required` and `res_required` fields in the Frame Address.
	//
	// ## Protocol header
	//
	// The Protocol header contains information about the message ...
	//
	// * Message type which determines what action to take (based on the Payload)
	//
	// | Field    | Bits | Type     | Description                                    |
	// |----------|------|----------|------------------------------------------------|
	// | reerved  | 64   | uint64_t | Reserved                                       |
	// | type     | 16   | uint16_t | Message type determines the payload being used |
	// | reserved | 16   |          | Reserved                                       |
	//
	// Reserved fields must be set to zero by the client..
	//
	// ## C header declaration
	//
	// #pragma pack(push, 1)
	// typedef struct {
	//     /* frame */
	//     uint16_t size;
	//     uint16_t protocol:12;
	//     uint8_t  addressable:1;
	//     uint8_t  tagged:1;
	//     uint8_t  origin:2;
	//     uint32_t source;
	//     /* frame address */
	//     uint8_t  target[8];
	//     uint8_t  reserved[6];
	//     uint8_t  res_required:1;
	//     uint8_t  ack_required:1;
	//     uint8_t  :6;
	//     uint8_t  sequence;
	//     /* protocol header */
	//     uint64_t :64;
	//     uint16_t type;
	//     uint16_t :16;
	//     /* variable length payload follows */
	// } lx_protocol_header_t;
	// #pragma pack(pop)
	//
	// Numeric data-type byte-order is
	// little-endian,
	// which means that dumping serialized data structures or viewing network
	// packet sniffing may show the content of numeric fields in reversed
	// byte-order.
	internal sealed class Request
	{
		public Request(
			Command command,
			bool ackRequired,
			bool resRequired,
			byte sequence,
			uint source,
			ulong target,
			RequestPayload payload
		)
		{
			Command = command;
			AckRequired = ackRequired;
			ResRequired = resRequired;
			Sequence = sequence;
			Source = source;
			Target = target;
			Payload = payload;
		}

		public Command Command { get; }
		public bool AckRequired { get; }
		public bool ResRequired { get; }
		public byte Sequence { get; }
		public uint Source { get; }
		public ulong Target { get; }
		public RequestPayload Payload { get; }

		public byte[] GetData()
		{
			var frameData = GetFrameData();
			var frameAddressData = GetFrameAddressData();
			var protocolHeaderData = GetProtocolHeaderData();
			var payloadData = Payload.GetData();

			return CombineArrays(frameData, frameAddressData, protocolHeaderData, payloadData);
		}

		private static byte[] CombineArrays(params byte[][] arrays)
		{
			return arrays.SelectMany(array => array).ToArray();
		}

		private byte[] GetFrameData()
		{
			var sizeData = GetSizeData();
			var frameFragmentData = GetFrameFragmentData();
			var sourceData = Source.GetBytes();

			return CombineArrays(sizeData, frameFragmentData, sourceData);
		}

		private byte[] GetFrameAddressData()
		{
			var targetData = Target.GetBytes();
			var reservedData = new byte[6];
			var frameAddressFragmentData = GetFrameAddressFragmentData();
			var sequenceData = GetSequenceData();

			return CombineArrays(targetData, reservedData, frameAddressFragmentData, sequenceData);
		}

		private byte[] GetProtocolHeaderData()
		{
			var reserved1Data = new byte[8];
			var commandData = GetCommandData();
			var reserved2Data = new byte[2];

			return CombineArrays(reserved1Data, commandData, reserved2Data);
		}

		private byte[] GetSizeData()
		{
			const int headerLength = 36;

			var payloadLength = Payload.GetData().Length;
			var size = (ushort)(headerLength + payloadLength);

			return size.GetBytes();
		}

		private byte[] GetFrameFragmentData()
		{
			const int protocolFlag = 0x400;
			const int addressableFlag = 0x1000;
			const int taggedFlag = 0x2000;

			// protocolFlag and addressableFlag must always be set
			ushort fragment = protocolFlag | addressableFlag;

			// A target device address of all zeroes addresses all devices on the local network
			if (Target == 0)
			{
				fragment |= taggedFlag;
			}

			return fragment.GetBytes();
		}

		private byte[] GetFrameAddressFragmentData()
		{
			const int resRequiredFlag = 0x1;
			const int ackRequiredFlag = 0x2;

			byte fragment = 0;

			if (ResRequired)
			{
				fragment |= resRequiredFlag;
			}

			if (AckRequired)
			{
				fragment |= ackRequiredFlag;
			}

			return new[]
			{
				fragment
			};
		}

		private byte[] GetSequenceData()
		{
			return new[]
			{
				Sequence
			};
		}

		private byte[] GetCommandData()
		{
			return ((ushort)Command).GetBytes();
		}
	}
}
