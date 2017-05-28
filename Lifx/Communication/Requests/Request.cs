using System.Linq;
using Lifx.Communication.Requests.Payloads;

namespace Lifx.Communication.Requests
{
	// Sent to a light to perform an action or retrieve information.
	//
	// ackRequired: if true an acknowledgement will be sent by the device upon receiving the request
	// resRequired: if true a response will be sent by the device
	// sequence: Wrap-around value included in the response. Used to track which request the response is for
	// target: 6 byte device address (MAC) or zero for all devices
	//
	// C header declaration:
	//
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
	internal sealed class Request
	{
		private readonly Command _command;
		private readonly bool _ackRequired;
		private readonly bool _resRequired;
		private readonly byte _sequence;
		private readonly uint _source;
		private readonly ulong _target;
		private readonly RequestPayload _payload;

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
			_command = command;
			_ackRequired = ackRequired;
			_resRequired = resRequired;
			_sequence = sequence;
			_source = source;
			_target = target;
			_payload = payload;
		}

		public byte[] GetData()
		{
			var frameData = GetFrameData();
			var frameAddressData = GetFrameAddressData();
			var protocolHeaderData = GetProtocolHeaderData();
			var payloadData = _payload.GetData();

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
			var sourceData = _source.GetBytes();

			return CombineArrays(sizeData, frameFragmentData, sourceData);
		}

		private byte[] GetFrameAddressData()
		{
			var targetData = _target.GetBytes();
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

			var payloadLength = _payload.GetData().Length;
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
			if (_target == 0)
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

			if (_resRequired)
			{
				fragment |= resRequiredFlag;
			}

			if (_ackRequired)
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
				_sequence
			};
		}

		private byte[] GetCommandData()
		{
			return ((ushort)_command).GetBytes();
		}
	}
}
