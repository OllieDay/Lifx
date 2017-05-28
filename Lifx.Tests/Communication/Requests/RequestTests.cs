using System;
using FluentAssertions;
using Lifx.Communication.Requests.Payloads;
using Lifx.Communication.Responses;
using Moq;
using Xunit;

namespace Lifx.Communication.Requests.Tests
{
	public sealed class RequestTests
	{
		private const int RequestLength = 36;
		private const int FrameAddressFragmentOffset = 22;
		private const int TaggedFlag = 0x2000;
		private const int ResRequiredFlag = 0x1;
		private const int AckRequiredFlag = 0x2;

		[Fact]
		public void GetDataShouldReturnDataOfCorrectLengthWhenPayloadIsEmpty()
		{
			CreateRequestData().Length.Should().Be(RequestLength);
		}

		[Fact]
		public void GetDataShouldReturnDataOfCorrectLengthWhenPayloadIsNotEmpty()
		{
			var payload = new SetPowerRequestPayload(Power.Off, durationInMilliseconds: 0);
			var length = RequestLength + payload.GetData().Length;

			CreateRequestData(payload: payload).Length.Should().Be(length);
		}

		[Fact]
		public void GetDataShouldReturnDataWithCorrectSizeWhenPayloadIsEmpty()
		{
			GetSizeFromData(CreateRequestData()).Should().Be(RequestLength);
		}

		[Fact]
		public void GetDataShouldReturnDataWithCorrectSizeWhenPayloadIsNotEmpty()
		{
			var payload = new SetPowerRequestPayload(Power.Off, durationInMilliseconds: 0);
			var length = (ushort)(RequestLength + payload.GetData().Length);

			GetSizeFromData(CreateRequestData(payload: payload)).Should().Be(length);
		}

		[Fact]
		public void GetDataShouldReturnDataWithCorrectProtocolFlag()
		{
			const int protocolFlag = 0x400;

			var frameFragment = GetFrameFragmentFromData(CreateRequestData());

			(frameFragment & protocolFlag).Should().Be(protocolFlag);
		}

		[Fact]
		public void GetDataShouldReturnDataWithCorrectAddressableFlag()
		{
			const int addressableFlag = 0x1000;

			var frameFragment = GetFrameFragmentFromData(CreateRequestData());

			(frameFragment & addressableFlag).Should().Be(addressableFlag);
		}

		[Fact]
		public void GetDataShouldReturnDataWithTaggedFlagSetWhenTargetIs0()
		{
			const ulong target = 0;

			var frameFragment = GetFrameFragmentFromData(CreateRequestData(target: target));

			(frameFragment & TaggedFlag).Should().Be(TaggedFlag);
		}

		[Fact]
		public void GetDataShouldReturnDataWithoutTaggedFlagSetWhenTargetIsNot0()
		{
			const ulong target = ulong.MaxValue;

			var frameFragment = GetFrameFragmentFromData(CreateRequestData(target: target));

			(frameFragment & TaggedFlag).Should().NotBe(TaggedFlag);
		}

		[Fact]
		public void GetDataShouldReturnDataWithCorrectSource()
		{
			const int sourceOffset = 4;
			const uint source = uint.MaxValue;

			CreateRequestData(source: source).ToUInt32(startIndex: sourceOffset).Should().Be(source);
		}

		[Fact]
		public void GetDataShouldReturnDataWithCorrectTarget()
		{
			const int targetOffset = 8;
			const ulong target = ulong.MaxValue;

			CreateRequestData(target: target).ToUInt64(startIndex: targetOffset).Should().Be(target);
		}

		[Fact]
		public void GetDataShouldReturnDataWithResRequiredFlagSetWhenResRequiredIsTrue()
		{
			var frameAddressFragment = CreateRequestData(resRequired: true)[FrameAddressFragmentOffset];

			(frameAddressFragment & ResRequiredFlag).Should().Be(ResRequiredFlag);
		}

		[Fact]
		public void GetDataShouldReturnDataWithoutResRequiredFlagSetWhenResRequiredIsFalse()
		{
			var frameAddressFragment = CreateRequestData(resRequired: false)[FrameAddressFragmentOffset];

			(frameAddressFragment & ResRequiredFlag).Should().NotBe(ResRequiredFlag);
		}

		[Fact]
		public void GetDataShouldReturnDataWithAckRequiredFlagSetWhenAckRequiredIsTrue()
		{
			var frameAddressFragment = CreateRequestData(ackRequired: true)[FrameAddressFragmentOffset];

			(frameAddressFragment & AckRequiredFlag).Should().Be(AckRequiredFlag);
		}

		[Fact]
		public void GetDataShouldReturnDataWithoutAckRequiredFlagSetWhenAckRequiredIsFalse()
		{
			var frameAddressFragment = CreateRequestData(ackRequired: false)[FrameAddressFragmentOffset];

			(frameAddressFragment & AckRequiredFlag).Should().NotBe(AckRequiredFlag);
		}

		[Fact]
		public void GetDataShouldReturnDataWithCorrectSequence()
		{
			const int sequenceOffset = 23;
			const byte sequence = byte.MaxValue;

			CreateRequestData(sequence: sequence)[sequenceOffset].Should().Be(sequence);
		}

		[Fact]
		public void GetDataShouldReturnDataWithCorrectCommand()
		{
			const int commandOffset = 32;
			const Command command = Command.DeviceEchoRequest;

			((Command)CreateRequestData(command: command).ToUInt16(startIndex: commandOffset)).Should().Be(command);
		}

		private static byte[] CreateRequestData(
			Command command = Command.DeviceEchoRequest,
			bool ackRequired = false,
			bool resRequired = false,
			byte sequence = 0,
			uint source = 0,
			ulong target = 0,
			RequestPayload payload = null
		)
		{
			payload = payload ?? RequestPayload.Empty;

			return new Request(command, ackRequired, resRequired, sequence, source, target, payload).GetData();
		}

		private static ushort GetSizeFromData(byte[] data)
		{
			const int sizeOffset = 0;

			return data.ToUInt16(startIndex: sizeOffset);
		}

		private static ushort GetFrameFragmentFromData(byte[] data)
		{
			const int frameFragmentOffset = 2;

			return data.ToUInt16(startIndex: frameFragmentOffset);
		}
	}
}
