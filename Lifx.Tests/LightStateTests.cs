using FluentAssertions;
using Xunit;

namespace Lifx.Tests
{
	public sealed class LightStateTests
	{
		[Fact]
		public void ToStringShouldReturnFormattedProperties()
		{
			var state = new LightState(Label.None, Power.Off, Percentage.MaxValue, Temperature.None, Color.None);

			state.ToString().Should().Be(
				string.Format(
					"[Label: {0}; Power: {1}; Brightness: {2}; Temperature: {3}; Color: {4}]",
					state.Label,
					state.Power,
					state.Brightness,
					state.Temperature,
					state.Color
				)
			);
		}
	}
}
