namespace Lifx.Communication.Requests;

internal interface IRequestFactory
{
	Request CreateGetVersionRequest();
	Request CreateGetRequest();
	Request CreateSetLabelRequest(Label label);
	Request CreateSetPowerRequest(Power power, uint durationInMilliseconds);
	Request CreateSetColorRequest(
		Color color,
		Percentage brightness,
		Temperature temperature,
		uint durationInMilliseconds
	);
}
