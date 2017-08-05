using System;
using System.Text;

namespace Lifx.Communication.Responses.Payloads
{
	internal sealed class StateResponsePayloadParser : IResponsePayloadParser<StateResponsePayload>
	{
		public StateResponsePayload Parse(byte[] data)
		{
			var color = ParseColor(data);
			var brightness = ParseBrightness(data);
			var temperature = ParseTemperature(data);
			var power = ParsePower(data);
			var label = ParseLabel(data);

			return new StateResponsePayload(color, brightness, temperature, power, label);
		}

		private static Color ParseColor(byte[] data)
		{
			var hue = ParseHue(data);
			var saturation = ParseSaturation(data);

			return new Color(hue, saturation);
		}

		private static Hue ParseHue(byte[] data)
		{
			var value = data.ToUInt16(startIndex: 0);

			return HueConverter.ConvertUInt16ToHue(value);
		}

		private static Percentage ParseSaturation(byte[] data)
		{
			var value = data.ToUInt16(startIndex: 2);

			return PercentageConverter.ConvertUInt16ToPercentage(value);
		}

		private static Percentage ParseBrightness(byte[] data)
		{
			var value = data.ToUInt16(startIndex: 4);

			return PercentageConverter.ConvertUInt16ToPercentage(value);
		}

		private static Temperature ParseTemperature(byte[] data)
			=> data.ToUInt16(startIndex: 6);

		private static Power ParsePower(byte[] data)
			=> (Power)data.ToUInt16(startIndex: 10);

		private static Label ParseLabel(byte[] data)
		{
			const int offset = 12;
			const int reserved = 8;

			var labelLength = data.Length - offset - reserved;

			return Encoding.UTF8.GetString(data, offset, labelLength).TrimEnd('\0');
		}
	}
}
