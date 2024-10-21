namespace Lifx.Tests;

public sealed class TemperatureTests
{
	[Theory]
	[InlineData(1499)]
	[InlineData(9001)]
	public void ConstructorShouldThrowArgumentOutOfRangeExceptionWhenValueNotInRange(int value)
		=> Assert.Throws<ArgumentOutOfRangeException>(() => new Temperature(value));
}
