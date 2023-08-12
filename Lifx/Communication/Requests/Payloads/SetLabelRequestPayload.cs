using System.Text;

namespace Lifx.Communication.Requests.Payloads;

// Contains information used to set a light's label.
internal sealed class SetLabelRequestPayload : RequestPayload
{
	private readonly Label _label;

	public SetLabelRequestPayload(Label label)
		=> _label = label;

	public override byte[] GetData()
		=> Encoding.UTF8.GetBytes(_label);
}
