using Lifx.Communication.Requests;
using Lifx.Communication.Responses.Payloads;

namespace Lifx.Communication.Responses.Tests;

public sealed class ResponseParserTests
{
	private const int ResponseLength = 36;
	private const int CommandOffset = 32;

	private static IResponsePayloadParser<StateVersionResponsePayload> StateVersionResponsePayloadParser
	{
		get
		{
			var mock = Substitute.For<IResponsePayloadParser<StateVersionResponsePayload>>();

			mock.Parse(Arg.Any<byte[]>())
				.Returns(new StateVersionResponsePayload(0, Product.Unknown, 0));

			return mock;
		}
	}

	private static IResponsePayloadParser<StateResponsePayload> StateResponsePayloadParser
	{
		get
		{
			var mock = Substitute.For<IResponsePayloadParser<StateResponsePayload>>();

			mock.Parse(Arg.Any<byte[]>())
				.Returns(new StateResponsePayload(
					Color.None,
					Percentage.MinValue,
					Temperature.None,
					Power.Off,
					Label.None
				));

			return mock;
		}
	}

	private static ResponseParser ResponseParser
		=> new ResponseParser(StateVersionResponsePayloadParser, StateResponsePayloadParser);

	[Fact]
	public void TryParseResponseShouldReturnNullWhenDataLengthIsLessThanResponseLength()
	{
		var data = new byte[ResponseLength - 1];

		ResponseParser.TryParseResponse(data).Should().BeNull();
	}

	[Fact]
	public void TryParseResponseShouldReturnNullWhenCommandIsInvalid()
	{
		var invalidCommand = (Command)0;
		var data = CreateResponseDataWithCommand(invalidCommand);

		ResponseParser.TryParseResponse(data).Should().BeNull();
	}

	[Fact]
	public void TryParseResponseShouldInitializeResponsePayloadWhenCommandIsDeviceAcknowledgement()
		=> TryParseResponseShouldInitializeResponsePayloadToSpecifiedType<ResponsePayload>(
			Command.DeviceAcknowledgement
		);

	[Fact]
	public void TryParseResponseShouldInitializeResponsePayloadWhenCommandIsDeviceStateVersion()
		=> TryParseResponseShouldInitializeResponsePayloadToSpecifiedType<StateVersionResponsePayload>(
			Command.DeviceStateVersion
		);

	[Fact]
	public void TryParseResponseShouldInitializeResponsePayloadWhenCommandIsLightState()
		=> TryParseResponseShouldInitializeResponsePayloadToSpecifiedType<StateResponsePayload>(Command.LightState);

	private void TryParseResponseShouldInitializeResponsePayloadToSpecifiedType<TResponsePayload>(Command command)
	{
		var data = CreateResponseDataWithCommand(command);

		var response = ResponseParser.TryParseResponse(data);

		response!.Payload.Should().BeOfType<TResponsePayload>();
	}

	private static byte[] CreateResponseDataWithCommand(Command command)
	{
		var data = new byte[ResponseLength];
		var commandData = ((ushort)command).GetBytes();

		Array.Copy(commandData, 0, data, CommandOffset, commandData.Length);

		return data;
	}
}
