using System.Text;

namespace Lifx.Communication.Requests.Payloads;

// Contains information used to set a light's label.
internal sealed record SetLabelRequestPayload(Label Label) : RequestPayload
{
	public override byte[] GetData()
		=> Encoding.UTF8.GetBytes(Label);
}
