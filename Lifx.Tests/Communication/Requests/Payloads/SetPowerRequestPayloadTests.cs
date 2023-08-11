using Lifx.Communication.Responses;

namespace Lifx.Communication.Requests.Payloads.Tests
{
	public sealed class SetPowerRequestPayloadTests
	{
		[Fact]
		public void GetDataShouldReturnValidPowerData()
		{
			const int powerOffset = 0;

			var power = Power.On;
			var payload = new SetPowerRequestPayload(power, powerOffset);

			payload.GetData().ToUInt16(powerOffset).Should().Be((ushort)power);
		}

		[Fact]
		public void GetDataShouldReturnValidDurationInMillisecondsData()
		{
			const int durationInMillisecondsOffset = 2;

			var durationInMilliseconds = uint.MaxValue;
			var payload = new SetPowerRequestPayload(Power.Off, durationInMilliseconds);

			payload.GetData().ToUInt32(durationInMillisecondsOffset).Should().Be(durationInMilliseconds);
		}
	}
}
