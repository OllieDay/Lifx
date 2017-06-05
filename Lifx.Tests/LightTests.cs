using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Lifx.Communication;
using Lifx.Communication.Requests;
using Lifx.Communication.Responses.Payloads;
using Moq;
using Xunit;

namespace Lifx.Tests
{
	public sealed class LightTests
	{
		[Fact]
		public async Task SetBrightnessAsyncShouldUseCurrentColorAndTemperature()
		{
			var color = Color.Cyan;
			var temperature = Temperature.Neutral;

			var communicator = CreateCommunicator(
				new StateResponsePayload(
					color,
					It.IsAny<Percentage>(),
					temperature,
					It.IsAny<Power>(),
					It.IsAny<Label>()
				)
			);

			var requestFactoryMock = new Mock<IRequestFactory>();

			using (var light = CreateLight(communicator: communicator, requestFactory: requestFactoryMock.Object))
			{
				await light.SetBrightnessAsync(It.IsAny<Percentage>());
			}

			requestFactoryMock.Verify(
				requestFactory => requestFactory.CreateSetColorRequest(
					color,
					It.IsAny<Percentage>(),
					temperature,
					It.IsAny<uint>()
				)
			);
		}

		[Fact]
		public async Task SetTemperatureAsyncShouldUseColorNone()
		{
			var communicator = CreateCommunicator(
				new StateResponsePayload(
					It.IsAny<Color>(),
					It.IsAny<Percentage>(),
					It.IsAny<Temperature>(),
					It.IsAny<Power>(),
					It.IsAny<Label>()
				)
			);
			var requestFactoryMock = new Mock<IRequestFactory>();

			using (var light = CreateLight(communicator: communicator, requestFactory: requestFactoryMock.Object))
			{
				await light.SetTemperatureAsync(It.IsAny<Temperature>());
			}

			requestFactoryMock.Verify(
				requestFactory => requestFactory.CreateSetColorRequest(
					Color.None,
					It.IsAny<Percentage>(),
					It.IsAny<Temperature>(),
					It.IsAny<uint>()
				)
			);
		}

		[Fact]
		public async Task SetTemperatureAsyncShouldUseCurrentBrightness()
		{
			var brightness = Percentage.MaxValue;

			var communicator = CreateCommunicator(
				new StateResponsePayload(
					It.IsAny<Color>(),
					brightness,
					It.IsAny<Temperature>(),
					It.IsAny<Power>(),
					It.IsAny<Label>()
				)
			);

			var requestFactoryMock = new Mock<IRequestFactory>();

			using (var light = CreateLight(communicator: communicator, requestFactory: requestFactoryMock.Object))
			{
				await light.SetTemperatureAsync(It.IsAny<Temperature>());
			}

			requestFactoryMock.Verify(
				requestFactory => requestFactory.CreateSetColorRequest(
					It.IsAny<Color>(),
					brightness,
					It.IsAny<Temperature>(),
					It.IsAny<uint>()
				)
			);
		}

		[Fact]
		public async Task SetColorAsyncShouldUseCurrentBrightnessAndTemperature()
		{
			var brightness = Percentage.MaxValue;
			var temperature = Temperature.Neutral;

			var communicator = CreateCommunicator(
				new StateResponsePayload(
					It.IsAny<Color>(),
					brightness,
					temperature,
					It.IsAny<Power>(),
					It.IsAny<Label>()
				)
			);
			var requestFactoryMock = new Mock<IRequestFactory>();

			using (var light = CreateLight(communicator: communicator, requestFactory: requestFactoryMock.Object))
			{
				await light.SetColorAsync(It.IsAny<Color>());
			}

			requestFactoryMock.Verify(
				requestFactory => requestFactory.CreateSetColorRequest(
					It.IsAny<Color>(),
					brightness,
					temperature,
					It.IsAny<uint>()
				)
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
					await light.SetColorAsync(It.IsAny<Color>());
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
			var mock = new Mock<ICommunicator>();

			mock.Setup(
				communicator => communicator.CommunicateAsync<StateResponsePayload>(
					It.IsAny<Request>(),
					CancellationToken.None
				)
			).ReturnsAsync(payload);

			return mock.Object;
		}

		private static ILight CreateLight(
			IPAddress address = null,
			Product product = Product.Unknown,
			uint version = 0,
			ICommunicator communicator = null,
			IRequestFactory requestFactory = null
		)
		{
			communicator =  communicator ?? Mock.Of<ICommunicator>();
			requestFactory = requestFactory ?? Mock.Of<IRequestFactory>();
			address = address ?? IPAddress.Any;

			return new Light(address, product, version, communicator, requestFactory);
		}
	}
}
