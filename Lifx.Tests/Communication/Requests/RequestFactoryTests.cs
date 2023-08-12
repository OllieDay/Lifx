using Lifx.Communication.Requests.Payloads;

namespace Lifx.Communication.Requests.Tests;

public sealed class RequestFactoryTests
{
	private static readonly IRequestFactory RequestFactory = new RequestFactory();

	[Fact]
	public void CreateGetVersionRequestShouldReturnRequestWithCorrectProperties()
	{
		var request = RequestFactory.CreateGetVersionRequest();

		CheckRequestProperties(
			request,
			command: Command.DeviceGetVersion,
			ackRequired: false,
			resRequired: true,
			payloadType: typeof(RequestPayload)
		);
	}

	[Fact]
	public void CreateGetRequestShouldReturnRequestWithCorrectProperties()
	{
		var request = RequestFactory.CreateGetRequest();

		CheckRequestProperties(
			request,
			command: Command.LightGet,
			ackRequired: false,
			resRequired: true,
			payloadType: typeof(RequestPayload)
		);
	}

	[Fact]
	public void CreateSetLabelRequestShouldReturnRequestWithCorrectProperties()
	{
		var request = RequestFactory.CreateSetLabelRequest(Label.None);

		CheckRequestProperties(
			request,
			command: Command.DeviceSetLabel,
			ackRequired: true,
			resRequired: false,
			payloadType: typeof(SetLabelRequestPayload)
		);
	}

	[Fact]
	public void CreateSetPowerRequestShouldReturnRequestWithCorrectProperties()
	{
		var request = RequestFactory.CreateSetPowerRequest(Power.Off, 0);

		CheckRequestProperties(
			request,
			command: Command.DeviceSetPower,
			ackRequired: true,
			resRequired: false,
			payloadType: typeof(SetPowerRequestPayload)
		);
	}

	[Fact]
	public void CreateSetColorRequestShouldReturnRequestWithCorrectProperties()
	{
		var request = RequestFactory.CreateSetColorRequest(Color.None, Percentage.MinValue, Temperature.None, 0);

		CheckRequestProperties(
			request,
			command: Command.LightSetColor,
			ackRequired: true,
			resRequired: false,
			payloadType: typeof(SetColorRequestPayload)
		);
	}

	[Fact]
	public void CreateRequestShouldReturnRequestWithIncrementingSequence()
	{
		var sequence1 = RequestFactory.CreateGetRequest().Sequence;
		var sequence2 = RequestFactory.CreateGetRequest().Sequence;

		sequence2.Should().Be((byte)(sequence1 + 1));
	}

	private static void CheckRequestProperties(
		Request request,
		Command command,
		bool ackRequired,
		bool resRequired,
		Type payloadType
	)
	{
		request.Command.Should().Be(command);
		request.AckRequired.Should().Be(ackRequired);
		request.ResRequired.Should().Be(resRequired);
		request.Payload.Should().BeOfType(payloadType);
	}
}
