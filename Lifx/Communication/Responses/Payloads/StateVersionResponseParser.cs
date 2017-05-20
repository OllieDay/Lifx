using System;

namespace Lifx.Communication.Responses.Payloads
{
	internal sealed class StateVersionResponsePayloadParser : IResponsePayloadParser<StateVersionResponsePayload>
	{
		public StateVersionResponsePayload Parse(byte[] data)
		{
			var vendor = ParseVendor(data);
			var product = ParseProduct(data);
			var version = ParseVersion(data);

			return new StateVersionResponsePayload(vendor, product, version);
		}

		private static uint ParseVendor(byte[] data)
		{
			return data.ToUInt32(startIndex: 0);
		}

		private static Product ParseProduct(byte[] data)
		{
			var value = data.ToUInt32(startIndex: 4);

			if (!Enum.IsDefined(typeof(Product), value))
			{
				value = (uint)Product.Unknown;
			}

			return (Product)value;
		}

		private static uint ParseVersion(byte[] data)
		{
			return data.ToUInt32(startIndex: 8);
		}
	}
}
