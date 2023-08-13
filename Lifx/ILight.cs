using System.Net;

namespace Lifx;

public interface ILight : IDisposable
{
	IPAddress Address { get; }
	Product Product { get; }
	uint Version { get; }

	Task<LightState> GetStateAsync();
	Task<LightState> GetStateAsync(CancellationToken cancellationToken);
	Task SetLabelAsync(Label label);
	Task SetLabelAsync(Label label, CancellationToken cancellationToken);
	Task SetPowerAsync(Power power);
	Task SetPowerAsync(Power power, CancellationToken cancellationToken);
	Task SetPowerAsync(Power power, uint durationInMilliseconds);
	Task SetPowerAsync(Power power, uint durationInMilliseconds, CancellationToken cancellationToken);
	Task OffAsync();
	Task OffAsync(CancellationToken cancellationToken);
	Task OffAsync(uint durationInMilliseconds);
	Task OffAsync(uint durationInMilliseconds, CancellationToken cancellationToken);
	Task OnAsync();
	Task OnAsync(CancellationToken cancellationToken);
	Task OnAsync(uint durationInMilliseconds);
	Task OnAsync(uint durationInMilliseconds, CancellationToken cancellationToken);
	Task SetBrightnessAsync(Percentage brightness);
	Task SetBrightnessAsync(Percentage brightness, CancellationToken cancellationToken);
	Task SetBrightnessAsync(Percentage brightness, uint durationInMilliseconds);
	Task SetBrightnessAsync(
		Percentage brightness,
		uint durationInMilliseconds,
		CancellationToken cancellationToken
	);
	Task SetTemperatureAsync(Temperature temperature);
	Task SetTemperatureAsync(Temperature temperature, CancellationToken cancellationToken);
	Task SetTemperatureAsync(Temperature temperature, uint durationInMilliseconds);
	Task SetTemperatureAsync(
		Temperature temperature,
		uint durationInMilliseconds,
		CancellationToken cancellationToken
	);
	Task SetColorAsync(Color color);
	Task SetColorAsync(Color color, CancellationToken cancellationToken);
	Task SetColorAsync(Color color, uint durationInMilliseconds);
	Task SetColorAsync(Color color, uint durationInMilliseconds, CancellationToken cancellationToken);
}
