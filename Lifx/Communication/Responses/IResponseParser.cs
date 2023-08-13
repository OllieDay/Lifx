namespace Lifx.Communication.Responses;

// Provides functionality for parsing responses from an array of bytes.
internal interface IResponseParser
{
	Response? TryParseResponse(byte[] data);
}
