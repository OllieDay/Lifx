using System;
using System.Net;
using System.Threading.Tasks;
using Lifx.Communication;
using Lifx.Communication.Requests;
using Lifx.Communication.Responses.Payloads;

namespace Lifx
{
	internal sealed class Light : ILight
	{
		private const uint DefaultDurationInMilliseconds = 0;

		private readonly ICommunicator _communicator;
		private readonly IRequestFactory _requestFactory;

		public Light(
			IPAddress address,
			Product product,
			uint version,
			ICommunicator communicator,
			IRequestFactory requestFactory
		)
		{
			Address = address;
			Product = product;
			Version = version;
			_communicator = communicator;
			_requestFactory = requestFactory;
		}

		public IPAddress Address { get; }
		public Product Product { get; }
		public uint Version { get; }

		public async Task<LightState> GetStateAsync()
		{
			var request = _requestFactory.CreateGetRequest();
			var payload = await _communicator.CommunicateAsync<StateResponsePayload>(request);

			return new LightState(
				payload.Label,
				payload.Power,
				payload.Brightness,
				payload.Temperature,
				payload.Color
			);
		}

		public async Task SetLabelAsync(Label label)
		{
			var request = _requestFactory.CreateSetLabelRequest(label);

			await _communicator.CommunicateAsync(request);
		}

		public async Task SetPowerAsync(Power power)
		{
			await SetPowerAsync(power, DefaultDurationInMilliseconds);
		}

		public async Task SetPowerAsync(Power power, uint durationInMilliseconds)
		{
			var request = _requestFactory.CreateSetPowerRequest(power, durationInMilliseconds);

			await _communicator.CommunicateAsync(request);
		}

		public async Task OffAsync()
		{
			await OffAsync(DefaultDurationInMilliseconds);
		}

		public async Task OffAsync(uint durationInMilliseconds)
		{
			await SetPowerAsync(Power.Off, DefaultDurationInMilliseconds);
		}

		public async Task OnAsync()
		{
			await OnAsync(DefaultDurationInMilliseconds);
		}

		public async Task OnAsync(uint durationInMilliseconds)
		{
			await SetPowerAsync(Power.On, DefaultDurationInMilliseconds);
		}

		public async Task SetBrightnessAsync(Percentage brightness)
		{
			await SetBrightnessAsync(brightness, DefaultDurationInMilliseconds);
		}

		public async Task SetBrightnessAsync(Percentage brightness, uint durationInMilliseconds)
		{
			var state = await GetStateAsync();

			await SetPropertiesAsync(state.Color, brightness, state.Temperature, durationInMilliseconds);
		}

		public async Task SetTemperatureAsync(Temperature temperature)
		{
			await SetTemperatureAsync(temperature, DefaultDurationInMilliseconds);
		}

		public async Task SetTemperatureAsync(Temperature temperature, uint durationInMilliseconds)
		{
			var state = await GetStateAsync();

			await SetPropertiesAsync(Color.None, state.Brightness, temperature, durationInMilliseconds);
		}

		public async Task SetColorAsync(Color color)
		{
			await SetColorAsync(color, DefaultDurationInMilliseconds);
		}

		public async Task SetColorAsync(Color color, uint durationInMilliseconds)
		{
			if (!Product.SupportsColor())
			{
				throw new InvalidOperationException($"{Product} does not support color.");
			}

			var state = await GetStateAsync();

			await SetPropertiesAsync(color, state.Brightness, state.Temperature, durationInMilliseconds);
		}

		public override string ToString()
		{
			return $"[Address: {Address}; Product: {Product}; Version: {Version}]";
		}

		public void Dispose()
		{
			_communicator.Dispose();
		}

		private async Task SetPropertiesAsync(
			Color color,
			Percentage brightness,
			Temperature temperature,
			uint durationInMilliseconds
		)
		{
			var request = _requestFactory.CreateSetColorRequest(color, brightness, temperature, durationInMilliseconds);

			await _communicator.CommunicateAsync(request);
		}
	}
}
