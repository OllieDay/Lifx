namespace Lifx.Communication.Requests.Payloads
{
	// Contains information used to set a light's power over a duration in milliseconds.
	internal sealed class SetPowerRequestPayload : RequestPayload
	{
		private readonly Power _power;
		private readonly uint _durationInMilliseconds;

		public SetPowerRequestPayload(Power power, uint durationInMilliseconds)
		{
			_power = power;
			_durationInMilliseconds = durationInMilliseconds;
		}

		public override byte[] GetData()
		{
			var powerData = ((ushort)_power).GetBytes();
			var durationInMillisecondsData = _durationInMilliseconds.GetBytes();

			return CombineArrays(powerData, durationInMillisecondsData);
		}
	}
}
