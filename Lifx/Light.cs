using System;
using System.Net;
using System.Threading;
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
			return await GetStateAsync(CancellationToken.None);
		}

		public async Task<LightState> GetStateAsync(CancellationToken cancellationToken)
		{
			var request = _requestFactory.CreateGetRequest();
			var payload = await _communicator.CommunicateAsync<StateResponsePayload>(request, cancellationToken);

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
			await SetLabelAsync(label, CancellationToken.None);
		}

		public async Task SetLabelAsync(Label label, CancellationToken cancellationToken)
		{
			var request = _requestFactory.CreateSetLabelRequest(label);

			await _communicator.CommunicateAsync(request, cancellationToken);
		}

		public async Task SetPowerAsync(Power power)
		{
			await SetPowerAsync(power, CancellationToken.None);
		}

		public async Task SetPowerAsync(Power power, CancellationToken cancellationToken)
		{
			await SetPowerAsync(power, DefaultDurationInMilliseconds, cancellationToken);
		}

		public async Task SetPowerAsync(Power power, uint durationInMilliseconds)
		{
			await SetPowerAsync(power, durationInMilliseconds, CancellationToken.None);
		}

		public async Task SetPowerAsync(Power power, uint durationInMilliseconds, CancellationToken cancellationToken)
		{
			var request = _requestFactory.CreateSetPowerRequest(power, durationInMilliseconds);

			await _communicator.CommunicateAsync(request, cancellationToken);
		}

		public async Task OffAsync()
		{
			await OffAsync(CancellationToken.None);
		}

		public async Task OffAsync(CancellationToken cancellationToken)
		{
			await OffAsync(DefaultDurationInMilliseconds, cancellationToken);
		}

		public async Task OffAsync(uint durationInMilliseconds)
		{
			await OffAsync(durationInMilliseconds, CancellationToken.None);
		}

		public async Task OffAsync(uint durationInMilliseconds, CancellationToken cancellationToken)
		{
			await SetPowerAsync(Power.Off, DefaultDurationInMilliseconds, cancellationToken);
		}

		public async Task OnAsync()
		{
			await OnAsync(CancellationToken.None);
		}

		public async Task OnAsync(CancellationToken cancellationToken)
		{
			await OnAsync(DefaultDurationInMilliseconds, cancellationToken);
		}

		public async Task OnAsync(uint durationInMilliseconds)
		{
			await OnAsync(durationInMilliseconds, CancellationToken.None);
		}

		public async Task OnAsync(uint durationInMilliseconds, CancellationToken cancellationToken)
		{
			await SetPowerAsync(Power.On, DefaultDurationInMilliseconds, cancellationToken);
		}

		public async Task SetBrightnessAsync(Percentage brightness)
		{
			await SetBrightnessAsync(brightness, CancellationToken.None);
		}

		public async Task SetBrightnessAsync(Percentage brightness, CancellationToken cancellationToken)
		{
			await SetBrightnessAsync(brightness, DefaultDurationInMilliseconds, cancellationToken);
		}

		public async Task SetBrightnessAsync(Percentage brightness, uint durationInMilliseconds)
		{
			await SetBrightnessAsync(brightness, durationInMilliseconds, CancellationToken.None);
		}

		public async Task SetBrightnessAsync(
			Percentage brightness,
			uint durationInMilliseconds,
			CancellationToken cancellationToken
		)
		{
			var state = await GetStateAsync();

			await SetPropertiesAsync(
				state.Color,
				brightness,
				state.Temperature,
				durationInMilliseconds,
				cancellationToken
			);
		}

		public async Task SetTemperatureAsync(Temperature temperature)
		{
			await SetTemperatureAsync(temperature, CancellationToken.None);
		}

		public async Task SetTemperatureAsync(Temperature temperature, CancellationToken cancellationToken)
		{
			await SetTemperatureAsync(temperature, DefaultDurationInMilliseconds, cancellationToken);
		}

		public async Task SetTemperatureAsync(Temperature temperature, uint durationInMilliseconds)
		{
			await SetTemperatureAsync(temperature, durationInMilliseconds, CancellationToken.None);
		}

		public async Task SetTemperatureAsync(
			Temperature temperature,
			uint durationInMilliseconds,
			CancellationToken cancellationToken
		)
		{
			var state = await GetStateAsync();

			await SetPropertiesAsync(
				Color.None,
				state.Brightness,
				temperature,
				durationInMilliseconds,
				cancellationToken
			);
		}

		public async Task SetColorAsync(Color color)
		{
			await SetColorAsync(color, CancellationToken.None);
		}

		public async Task SetColorAsync(Color color, CancellationToken cancellationToken)
		{
			await SetColorAsync(color, DefaultDurationInMilliseconds, cancellationToken);
		}

		public async Task SetColorAsync(Color color, uint durationInMilliseconds)
		{
			await SetColorAsync(color, durationInMilliseconds, CancellationToken.None);
		}

		public async Task SetColorAsync(Color color, uint durationInMilliseconds, CancellationToken cancellationToken)
		{
			if (!Product.SupportsColor())
			{
				throw new InvalidOperationException($"{Product} does not support color.");
			}

			var state = await GetStateAsync();

			await SetPropertiesAsync(
				color,
				state.Brightness,
				state.Temperature,
				durationInMilliseconds,
				cancellationToken
			);
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
			uint durationInMilliseconds,
			CancellationToken cancellationToken
		)
		{
			var request = _requestFactory.CreateSetColorRequest(color, brightness, temperature, durationInMilliseconds);

			await _communicator.CommunicateAsync(request, cancellationToken);
		}
	}
}
