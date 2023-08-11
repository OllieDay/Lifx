using System.Net;
using Lifx.Communication;
using Lifx.Communication.Requests;
using Lifx.Communication.Responses.Payloads;

namespace Lifx.Tests
{
	public sealed class LightTests
	{
		[Fact]
		public async Task SetBrightnessAsyncShouldUseCurrentColorAndTemperature()
		{
			var color = Color.Cyan;
			var brightness = Percentage.MaxValue;
			var temperature = Temperature.Neutral;

			var communicator = CreateCommunicator(
				new StateResponsePayload(
					color,
					brightness,
					temperature,
					Power.On,
					Label.None
				)
			);

			var requestFactoryMock = Substitute.For<IRequestFactory>();

			using (var light = CreateLight(communicator: communicator, requestFactory: requestFactoryMock))
			{
				await light.SetBrightnessAsync(brightness);
			}

			requestFactoryMock
				.Received()
				.CreateSetColorRequest(
					color,
					Arg.Any<Percentage>(),
					temperature,
					Arg.Any<uint>()
				);
		}

		[Fact]
		public async Task SetTemperatureAsyncShouldUseColorNone()
		{
			var temperature = Temperature.Neutral;

			var communicator = CreateCommunicator(
				new StateResponsePayload(
					Color.Cyan,
					Percentage.MaxValue,
					temperature,
					Power.On,
					Label.None
				)
			);

			var requestFactoryMock = Substitute.For<IRequestFactory>();

			using (var light = CreateLight(communicator: communicator, requestFactory: requestFactoryMock))
			{
				await light.SetTemperatureAsync(temperature);
			}

			requestFactoryMock
				.Received()
				.CreateSetColorRequest(
					Color.None,
					Arg.Any<Percentage>(),
					Arg.Any<Temperature>(),
					Arg.Any<uint>()
				);
		}

		[Fact]
		public async Task SetTemperatureAsyncShouldUseCurrentBrightness()
		{
			var brightness = Percentage.MaxValue;
			var temperature = Temperature.Neutral;

			var communicator = CreateCommunicator(
				new StateResponsePayload(
					Color.Cyan,
					brightness,
					temperature,
					Power.On,
					Label.None
				)
			);

			var requestFactoryMock = Substitute.For<IRequestFactory>();

			using (var light = CreateLight(communicator: communicator, requestFactory: requestFactoryMock))
			{
				await light.SetTemperatureAsync(temperature);
			}

			requestFactoryMock
				.Received()
				.CreateSetColorRequest(
					Arg.Any<Color>(),
					brightness,
					Arg.Any<Temperature>(),
					Arg.Any<uint>()
				);
		}

		[Fact]
		public async Task SetColorAsyncShouldUseCurrentBrightnessAndTemperature()
		{
			var color = Color.Cyan;
			var brightness = Percentage.MaxValue;
			var temperature = Temperature.Neutral;

			var communicator = CreateCommunicator(
				new StateResponsePayload(
					color,
					brightness,
					temperature,
					Power.On,
					Label.None
				)
			);

			var requestFactoryMock = Substitute.For<IRequestFactory>();

			using (var light = CreateLight(communicator: communicator, requestFactory: requestFactoryMock))
			{
				await light.SetColorAsync(color);
			}


			requestFactoryMock
				.Received()
				.CreateSetColorRequest(
					color,
					brightness,
					temperature,
					Arg.Any<uint>()
				);
		}

		[Theory]
		[InlineData(Product.White800LowVoltage)]
		[InlineData(Product.White800HightVoltage)]
		[InlineData(Product.White900BR30)]
		public void SetColorAsyncShouldThrowInvalidOperationExceptionWhenProductDoesNotSupportColor(Product product)
		{
			using (var light = CreateLight())
			{
				Assert.ThrowsAnyAsync<InvalidOperationException>(async () =>
				{
					await light.SetColorAsync(Color.Cyan);
				});
			}
		}

		[Fact]
		public void ToStringShouldReturnFormattedProperties()
		{
			var address = IPAddress.Any;
			var product = Product.Unknown;
			var version = uint.MinValue;
			var light = CreateLight(address, product, version);

			light.ToString().Should().Be($"[Address: {address}; Product: {product}; Version: {version}]");
		}

		private static ICommunicator CreateCommunicator(StateResponsePayload payload)
		{
			var mock = Substitute.For<ICommunicator>();
			
			mock.CommunicateAsync<StateResponsePayload>(
				Arg.Any<Request>(),
				Arg.Any<CancellationToken>()
			).Returns(Task.FromResult(payload));

			return mock;
		}

		private static ILight CreateLight(
			IPAddress address = null,
			Product product = Product.Unknown,
			uint version = 0,
			ICommunicator communicator = null,
			IRequestFactory requestFactory = null
		)
		{
			communicator = communicator ?? Substitute.For<ICommunicator>();
			requestFactory = requestFactory ?? Substitute.For<IRequestFactory>();
			address = address ?? IPAddress.Any;

			return new Light(address, product, version, communicator, requestFactory);
		}
	}
}
