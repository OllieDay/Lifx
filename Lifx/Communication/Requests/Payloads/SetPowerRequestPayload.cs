namespace Lifx.Communication.Requests.Payloads;

// Contains information used to set a light's power over a duration in milliseconds.
internal sealed record SetPowerRequestPayload(
	Power Power,
	uint DurationInMilliseconds
) : RequestPayload
{
	public override byte[] GetData()
	{
		var powerData = ((ushort)Power).GetBytes();
		var durationInMillisecondsData = DurationInMilliseconds.GetBytes();

		return CombineArrays(powerData, durationInMillisecondsData);
	}
}
