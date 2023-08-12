using System.Text;

namespace Lifx.Communication.Requests.Payloads.Tests;

public sealed class SetLabelRequestPayloadTests
{
	[Theory]
	[InlineData("")]
	[InlineData("00000000000000000000000000000000")]
	[InlineData("零零零零零零零零零零")]
	public void GetDataShouldReturnValidLabelData(string label)
	{
		var payload = new SetLabelRequestPayload(label);

		Encoding.UTF8.GetString(payload.GetData()).Should().Be(label);
	}
}
