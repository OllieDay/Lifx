using System;
using System.Linq;
using FluentAssertions;
using Lifx.Communication.Requests;
using Lifx.Communication.Responses.Payloads;
using Moq;
using Xunit;

namespace Lifx.Communication.Responses.Tests
{
	public sealed class ResponseParserTests
	{
		private const int ResponseLength = 36;
		private const int CommandOffset = 32;

		private static IResponsePayloadParser<StateVersionResponsePayload> StateVersionResponsePayloadParser
		{
			get
			{
				var mock = new Mock<IResponsePayloadParser<StateVersionResponsePayload>>();

				mock.Setup(parser => parser.Parse(It.IsAny<byte[]>()))
					.Returns(new StateVersionResponsePayload(
						It.IsAny<uint>(),
						It.IsAny<Product>(),
						It.IsAny<uint>()
					)
				);

				return mock.Object;
			}
		}

		private static IResponsePayloadParser<StateResponsePayload> StateResponsePayloadParser
		{
			get
			{
				var mock = new Mock<IResponsePayloadParser<StateResponsePayload>>();

				mock.Setup(parser => parser.Parse(It.IsAny<byte[]>()))
					.Returns(new StateResponsePayload(
						It.IsAny<Color>(),
						It.IsAny<Percentage>(),
						It.IsAny<Temperature>(),
						It.IsAny<Power>(),
						It.IsAny<Label>()
					)
				);

				return mock.Object;
			}
		}

		private static ResponseParser ResponseParser
			=> new ResponseParser(StateVersionResponsePayloadParser, StateResponsePayloadParser);

		[Fact]
		public void TryParseResponseShouldReturnFalseWhenDataLengthIsLessThanResponseLength()
		{
			var data = new byte[ResponseLength - 1];

			ResponseParser.TryParseResponse(data, out var response).Should().BeFalse();
		}

		[Fact]
		public void TryParseResponseShouldReturnFalseWhenCommandIsInvalid()
		{
			var invalidCommand = (Command)0;
			var data = CreateResponseDataWithCommand(invalidCommand);

			ResponseParser.TryParseResponse(data, out var response).Should().BeFalse();
		}

		[Fact]
		public void TryParseResponseShouldInitializeResponsePayloadWhenCommandIsDeviceAcknowledgement()
			=> TryParseResponseShouldInitializeResponsePayloadToSpecifiedType<ResponsePayload>(Command.DeviceAcknowledgement);

		[Fact]
		public void TryParseResponseShouldInitializeResponsePayloadWhenCommandIsDeviceStateVersion()
			=> TryParseResponseShouldInitializeResponsePayloadToSpecifiedType<StateVersionResponsePayload>(Command.DeviceStateVersion);

		[Fact]
		public void TryParseResponseShouldInitializeResponsePayloadWhenCommandIsLightState()
			=> TryParseResponseShouldInitializeResponsePayloadToSpecifiedType<StateResponsePayload>(Command.LightState);

		private void TryParseResponseShouldInitializeResponsePayloadToSpecifiedType<TResponsePayload>(Command command)
		{
			var data = CreateResponseDataWithCommand(command);

			ResponseParser.TryParseResponse(data, out var response);

			response.Payload.Should().BeOfType<TResponsePayload>();
		}

		private static byte[] CreateResponseDataWithCommand(Command command)
		{
			var data = new byte[ResponseLength];
			var commandData = ((ushort)command).GetBytes();

			Array.Copy(commandData, 0, data, CommandOffset, commandData.Length);

			return data;
		}
	}
}
