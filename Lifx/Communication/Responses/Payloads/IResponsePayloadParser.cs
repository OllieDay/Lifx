namespace Lifx.Communication.Responses.Payloads
{
	internal interface IResponsePayloadParser<TResponsePayload> where TResponsePayload : ResponsePayload
	{
		TResponsePayload Parse(byte[] data);
	}
}
