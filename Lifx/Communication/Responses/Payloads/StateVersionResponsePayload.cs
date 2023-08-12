namespace Lifx.Communication.Responses.Payloads;

// Contains device information comprised of vendor, product and version.
internal sealed class StateVersionResponsePayload : ResponsePayload
{
	public StateVersionResponsePayload(uint vendor, Product product, uint version)
	{
		Vendor = vendor;
		Product = product;
		Version = version;
	}

	public uint Vendor { get; }
	public Product Product { get; }
	public uint Version { get; }
}
