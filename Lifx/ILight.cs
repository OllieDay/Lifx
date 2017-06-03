using System;
using System.Net;
using System.Threading.Tasks;

namespace Lifx
{
	public interface ILight : IDisposable
	{
		IPAddress Address { get; }
		Product Product { get; }
		uint Version { get; }

		Task<LightState> GetStateAsync();
		Task SetLabelAsync(Label label);
		Task SetPowerAsync(Power power);
		Task SetPowerAsync(Power power, uint durationInMilliseconds);
		Task OffAsync();
		Task OffAsync(uint durationInMilliseconds);
		Task OnAsync();
		Task OnAsync(uint durationInMilliseconds);
		Task SetBrightnessAsync(Percentage brightness);
		Task SetBrightnessAsync(Percentage brightness, uint durationInMilliseconds);
		Task SetTemperatureAsync(Temperature temperature);
		Task SetTemperatureAsync(Temperature temperature, uint durationInMilliseconds);
		Task SetColorAsync(Color color);
		Task SetColorAsync(Color color, uint durationInMilliseconds);
	}
}
