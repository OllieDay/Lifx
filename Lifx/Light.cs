using System.Net;
using Lifx.Communication;
using Lifx.Communication.Requests;
using Lifx.Communication.Responses.Payloads;

namespace Lifx;

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
		=> await GetStateAsync(CancellationToken.None).ConfigureAwait(false);

	public async Task<LightState> GetStateAsync(CancellationToken cancellationToken)
	{
		var request = _requestFactory.CreateGetRequest();
		var payload = await _communicator.CommunicateAsync<StateResponsePayload>(request, cancellationToken)
			.ConfigureAwait(false);

		return new LightState(
			payload.Label,
			payload.Power,
			payload.Brightness,
			payload.Temperature,
			payload.Color
		);
	}

	public async Task SetLabelAsync(Label label)
		=> await SetLabelAsync(label, CancellationToken.None).ConfigureAwait(false);

	public async Task SetLabelAsync(Label label, CancellationToken cancellationToken)
	{
		var request = _requestFactory.CreateSetLabelRequest(label);

		await _communicator.CommunicateAsync(request, cancellationToken).ConfigureAwait(false);
	}

	public async Task SetPowerAsync(Power power)
		=> await SetPowerAsync(power, CancellationToken.None).ConfigureAwait(false);

	public async Task SetPowerAsync(Power power, CancellationToken cancellationToken)
		=> await SetPowerAsync(power, DefaultDurationInMilliseconds, cancellationToken).ConfigureAwait(false);

	public async Task SetPowerAsync(Power power, uint durationInMilliseconds)
		=> await SetPowerAsync(power, durationInMilliseconds, CancellationToken.None).ConfigureAwait(false);

	public async Task SetPowerAsync(Power power, uint durationInMilliseconds, CancellationToken cancellationToken)
	{
		var request = _requestFactory.CreateSetPowerRequest(power, durationInMilliseconds);

		await _communicator.CommunicateAsync(request, cancellationToken).ConfigureAwait(false);
	}

	public async Task OffAsync()
		=> await OffAsync(CancellationToken.None).ConfigureAwait(false);

	public async Task OffAsync(CancellationToken cancellationToken)
		=> await OffAsync(DefaultDurationInMilliseconds, cancellationToken).ConfigureAwait(false);

	public async Task OffAsync(uint durationInMilliseconds)
		=> await OffAsync(durationInMilliseconds, CancellationToken.None).ConfigureAwait(false);

	public async Task OffAsync(uint durationInMilliseconds, CancellationToken cancellationToken)
		=> await SetPowerAsync(Power.Off, DefaultDurationInMilliseconds, cancellationToken).ConfigureAwait(false);

	public async Task OnAsync()
		=> await OnAsync(CancellationToken.None).ConfigureAwait(false);

	public async Task OnAsync(CancellationToken cancellationToken)
		=> await OnAsync(DefaultDurationInMilliseconds, cancellationToken).ConfigureAwait(false);

	public async Task OnAsync(uint durationInMilliseconds)
		=> await OnAsync(durationInMilliseconds, CancellationToken.None).ConfigureAwait(false);

	public async Task OnAsync(uint durationInMilliseconds, CancellationToken cancellationToken)
		=> await SetPowerAsync(Power.On, DefaultDurationInMilliseconds, cancellationToken).ConfigureAwait(false);

	public async Task SetBrightnessAsync(Percentage brightness)
		=> await SetBrightnessAsync(brightness, CancellationToken.None).ConfigureAwait(false);

	public async Task SetBrightnessAsync(Percentage brightness, CancellationToken cancellationToken)
		=> await SetBrightnessAsync(brightness, DefaultDurationInMilliseconds, cancellationToken)
			.ConfigureAwait(false);

	public async Task SetBrightnessAsync(Percentage brightness, uint durationInMilliseconds)
		=> await SetBrightnessAsync(brightness, durationInMilliseconds, CancellationToken.None)
			.ConfigureAwait(false);

	public async Task SetBrightnessAsync(
		Percentage brightness,
		uint durationInMilliseconds,
		CancellationToken cancellationToken
	)
	{
		var state = await GetStateAsync().ConfigureAwait(false);

		await SetPropertiesAsync(
			state.Color,
			brightness,
			state.Temperature,
			durationInMilliseconds,
			cancellationToken
		).ConfigureAwait(false);
	}

	public async Task SetTemperatureAsync(Temperature temperature)
		=> await SetTemperatureAsync(temperature, CancellationToken.None).ConfigureAwait(false);

	public async Task SetTemperatureAsync(Temperature temperature, CancellationToken cancellationToken)
		=> await SetTemperatureAsync(temperature, DefaultDurationInMilliseconds, cancellationToken)
			.ConfigureAwait(false);

	public async Task SetTemperatureAsync(Temperature temperature, uint durationInMilliseconds)
		=> await SetTemperatureAsync(temperature, durationInMilliseconds, CancellationToken.None)
			.ConfigureAwait(false);

	public async Task SetTemperatureAsync(
		Temperature temperature,
		uint durationInMilliseconds,
		CancellationToken cancellationToken
	)
	{
		var state = await GetStateAsync().ConfigureAwait(false);

		await SetPropertiesAsync(
			Color.None,
			state.Brightness,
			temperature,
			durationInMilliseconds,
			cancellationToken
		).ConfigureAwait(false);
	}

	public async Task SetColorAsync(Color color)
		=> await SetColorAsync(color, CancellationToken.None).ConfigureAwait(false);

	public async Task SetColorAsync(Color color, CancellationToken cancellationToken)
		=> await SetColorAsync(color, DefaultDurationInMilliseconds, cancellationToken).ConfigureAwait(false);

	public async Task SetColorAsync(Color color, uint durationInMilliseconds)
		=> await SetColorAsync(color, durationInMilliseconds, CancellationToken.None).ConfigureAwait(false);

	public async Task SetColorAsync(Color color, uint durationInMilliseconds, CancellationToken cancellationToken)
	{
		if (!Product.SupportsColor())
		{
			throw new InvalidOperationException($"{Product} does not support color.");
		}

		var state = await GetStateAsync().ConfigureAwait(false);

		await SetPropertiesAsync(
			color,
			state.Brightness,
			state.Temperature,
			durationInMilliseconds,
			cancellationToken
		).ConfigureAwait(false);
	}

	public override string ToString()
		=> $"[Address: {Address}; Product: {Product}; Version: {Version}]";

	public void Dispose()
		=> _communicator.Dispose();

	private async Task SetPropertiesAsync(
		Color color,
		Percentage brightness,
		Temperature temperature,
		uint durationInMilliseconds,
		CancellationToken cancellationToken
	)
	{
		var request = _requestFactory.CreateSetColorRequest(color, brightness, temperature, durationInMilliseconds);

		await _communicator.CommunicateAsync(request, cancellationToken).ConfigureAwait(false);
	}
}
