namespace Lifx.Communication.Responses.Payloads;

// Contains device information comprised of vendor, product and version.
internal sealed record StateVersionResponsePayload(
	uint Vendor,
	Product Product,
	uint Version
) : ResponsePayload
{

}
