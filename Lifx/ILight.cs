using System;
using System.Net;
using System.Threading.Tasks;

namespace Lifx
{
	public interface ILight : IDisposable
	{
		IPAddress Address { get; }

		Task<LightState> GetStateAsync();
		Task SetLabelAsync(Label label);
		Task SetPowerAsync(Power power);
		Task SetPowerAsync(Power power, uint duration);
		Task OffAsync();
		Task OffAsync(uint duration);
		Task OnAsync();
		Task OnAsync(uint duration);
		Task SetBrightnessAsync(Percentage brightness);
		Task SetBrightnessAsync(Percentage brightness, uint duration);
		Task SetTemperatureAsync(Temperature temperature);
		Task SetTemperatureAsync(Temperature temperature, uint duration);
		Task SetColorAsync(Color color);
		Task SetColorAsync(Color color, uint duration);
	}
}
